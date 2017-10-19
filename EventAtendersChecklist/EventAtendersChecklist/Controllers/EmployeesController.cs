namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.DAL;
    using EventAtendersChecklist.Models;
    using EventAtendersChecklist.ModelsView;
    using EventAtendersChecklist.SignalR;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="EmployeesController" />
    /// </summary>
    [Authorize]
    public class EmployeesController : Controller
    {
        /// <summary>
        /// Defines the db
        /// </summary>
        private EacContext db = new EacContext();

        // GET: Employees
        /// <summary>
        /// The Index
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Index()
        {
            var ActionDictionary = db.ActionGroups.Include(x => x.ActionDictionary).Include(x => x.Event)
               .Where(x => x.EventId == 1)
               .Select(x => x.ActionDictionary).ToList();

            var extion = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == 1 & x.ActionDictionaryId == 1)
                .Select(x => x.Employee).ToList();

            var valueFor = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee).Include(x => x.ActionDictionary).Where(x => x.EventId == 1).ToList();
            var ValueForMarcin = valueFor.Where(x => x.Employee.Id == 3 & x.ActionDictionaryId == 1).Select(x => x.ActionValue);

            return View();
        }

        /// <summary>
        /// The GetEmployees
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpGet]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult GetEmployees()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlcom = new SqlCommand(@"SELECT [Id], [FirstName], [LastName], [Email] FROM dbo.Employees", sqlcon))
                {
                    sqlcon.Open();
                    sqlcom.CommandType = CommandType.Text;
                    sqlcom.Notification = null;
                    SqlDependency dependancy = new SqlDependency(sqlcom);
                    dependancy.OnChange += Dependancy_OnChange;
                    var reader = sqlcom.ExecuteReader();
                    var employees = reader.Cast<IDataRecord>()
                       .Select(e => new Employee()
                       {
                           Id = e.GetInt32(0),
                           FirstName = e.GetString(1),
                           LastName = e.GetString(2),
                           Email = e.GetString(3)
                       }).OrderBy(x=>x.LastName).ToList();
                    return PartialView("_EmployeesList", employees);
                }
            }
        }

        /// <summary>
        /// The Dependancy_OnChange
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="SqlNotificationEventArgs"/></param>
        internal void Dependancy_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SignalRHub.NotifyChanges();
            }
        }

        // GET: Employees/Details/5
        /// <summary>
        /// The Details
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/AddToEvent/5
        /// <summary>
        /// The AddToEvent
        /// </summary>
        /// <param name="id">The <see cref="int"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult AddToEvent(int id)
        {
            AddEmployeeToEventViewModel employee = new AddEmployeeToEventViewModel()
            {
                EventId = id,
                _employee = new Employee()
            };

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        /// <summary>
        /// The AddToEvent
        /// </summary>
        /// <param name="employee">The <see cref="AddEmployeeToEventViewModel"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToEvent(AddEmployeeToEventViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var eventId = employee.EventId;
                if (db.Employees.Where(x => x.Email.Equals(employee._employee.Email)).Count() == 0)
                {
                    db.Employees.Add(new Employee
                    {
                        FirstName = employee._employee.FirstName,
                        LastName = employee._employee.LastName,
                        Email = employee._employee.Email
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                Debug.Fail(message);
                            }
                        }
                    }
                }

                var id = db.Employees.Where(x => x.Email == employee._employee.Email)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                if (db.EmployeeEventAssignments.Where(x => x.EmployeeId == id & x.EventId == eventId).Count() == 0)
                {
                    var actionsInEvent = db.ActionGroups.Where(x => x.EventId == eventId).ToList();
                    foreach (var item in actionsInEvent)
                    {
                        db.EmployeeEventAssignments.Add(new EmployeeEventAssignment
                        {
                            EventId = eventId,
                            EmployeeId = id,
                            ActionDictionaryId = item.ActionDictionaryId,
                            ActionValue = false
                        });
                        
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item2 in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = item2.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in item2.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                Debug.Fail(message);
                            }
                        }
                    }
                }
                return RedirectToAction("Show", "Events", new { id = employee.EventId });
            }
            return View(employee);
        }

        // GET: Employees/Create
        /// <summary>
        /// The Create
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Create
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        /// <summary>
        /// The Edit
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Edit
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        /// <summary>
        /// The Delete
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        /// <summary>
        /// The DeleteConfirmed
        /// </summary>
        /// <param name="id">The <see cref="int"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            return View();
        }

        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult DeleteAllConfirmed()
        {
            var employees = db.Employees;
            foreach(var item in employees)
            {
                db.Employees.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        /// <param name="disposing">The <see cref="bool"/></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
