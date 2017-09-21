namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.DAL;
    using EventAtendersChecklist.Models;
    using EventAtendersChecklist.ModelsView;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="EmployeesController" />
    /// </summary>
    public class EmployeesController : Controller
    {
        /// <summary>
        /// Defines the db
        /// </summary>
        private eacContext db = new eacContext();

        // GET: Employees
        /// <summary>
        /// The Index
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
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

            return View(db.Employees.ToList());
        }

        // GET: Employees/Details/5
        /// <summary>
        /// The Details
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
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
                db.Employees.Add(new Employee
                {
                    FirstName = employee._employee.FirstName,
                    LastName = employee._employee.LastName,
                    Email = employee._employee.Email
                });
                db.SaveChanges();
                var id = db.Employees.Where(x => x.Email == employee._employee.Email)
                    .Select(x => x.Id)
                    .ToList()
                    .First();
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
                    db.SaveChanges();
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
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
