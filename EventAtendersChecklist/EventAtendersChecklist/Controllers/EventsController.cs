namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.DAL;
    using EventAtendersChecklist.Models;
    using EventAtendersChecklist.ModelsView;
    using EventAtendersChecklist.SignalR;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;


    /// <summary>
    /// Defines the <see cref="EventsController" />
    /// </summary>
    [Authorize]
    public class EventsController : Controller
    {
        /// <summary>
        /// Defines the db
        /// </summary>
        private EacContext db = new EacContext();

        /// <summary>
        /// Defines the EventId
        /// </summary>
        private static int EventId = new int();

        // GET: Events
        /// <summary>
        /// The Index
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// The GetCurrentEvents
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpGet]
        public ActionResult GetCurrentEvents()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlcom = new SqlCommand("SELECT [Id], [Name], [StartDate], [EndDate] FROM dbo.Events", sqlcon))
                {
                    sqlcon.Open();
                    sqlcom.CommandType = CommandType.Text;
                    sqlcom.Notification = null;
                    SqlDependency dependancy = new SqlDependency(sqlcom);
                    dependancy.OnChange += Dependancy_OnChange;
                    var reader = sqlcom.ExecuteReader();

                    var events = reader.Cast<IDataRecord>()
                       .Select(e => new Event()
                       {
                           Id = e.GetInt32(0),
                           Name = e.GetString(1),
                           StartDate = e.GetDateTime(2),
                           EndDate = e.GetDateTime(3)
                       }).ToList();

                    List<Event> currentevents = new List<Event>();
                    foreach (var i in events)
                    {
                        if (i.EndDate.CompareTo(DateTime.Now) > 0)
                        {
                            currentevents.Add(i);
                        }
                    }

                    return PartialView("_EventsList", currentevents.OrderBy(x => x.StartDate));
                }
            }
        }

        /// <summary>
        /// The History
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult History()
        {
            return View();
        }

        /// <summary>
        /// The GetHistoricalEvents
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpGet]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult GetHistoricalEvents()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlcom = new SqlCommand("SELECT [Id], [Name], [StartDate], [EndDate] FROM dbo.Events", sqlcon))
                {
                    sqlcon.Open();
                    sqlcom.CommandType = CommandType.Text;
                    sqlcom.Notification = null;
                    SqlDependency dependancy = new SqlDependency(sqlcom);
                    dependancy.OnChange += Dependancy_OnChange;
                    var reader = sqlcom.ExecuteReader();
                    var events = reader.Cast<IDataRecord>()
                       .Select(e => new Event()
                       {
                           Id = e.GetInt32(0),
                           Name = e.GetString(1),
                           StartDate = e.GetDateTime(2),
                           EndDate = e.GetDateTime(3)
                       }).ToList();

                    List<Event> historicalevents = new List<Event>();
                    foreach (var i in events)
                    {
                        if (i.EndDate.CompareTo(DateTime.Now) < 0)
                        {
                            historicalevents.Add(i);
                        }
                    }

                    return PartialView("_EventsHistoricalList", historicalevents.OrderBy(x => x.StartDate));
                }
            }
        }

        /// <summary>
        /// The dependancy_OnChange
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

        // GET: Events/Details/5
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
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Show/5
        /// <summary>
        /// The Show
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = (int)id;
            return View();
        }

        /// <summary>
        /// The ShowHistory
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult ShowHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = (int)id;
            return View();
        }

        /// <summary>
        /// The GetEventGrid
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpGet]
        public ActionResult GetEventGrid(int? id)
        {
            if (db.EmployeeEventAssignments.Include(x => x.Event)
                        .Where(x => x.EventId == id).Count() == 0)
            {
                var employ = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == id & x.ActionDictionaryId == 1).ToList();

                var actions = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                    .Where(x => x.EventId == id).ToList();

                var listOfActions = db.ActionGroups.Include(x => x.ActionDictionary).Include(x => x.Event)
                   .Where(x => x.EventId == id)
                   .Select(x => x.ActionDictionary).ToList();

                var list = new ListOfAttendeesWithActions()
                {
                    EventId = id,
                    ActionDictionaryList = listOfActions,
                    EventAttenderList = from e in employ
                                        select new EventAttender()
                                        {
                                            FirstName = e.Employee.FirstName,
                                            AttenderId = e.EmployeeId,
                                            LastName = e.Employee.LastName,
                                            Email = e.Employee.Email,
                                            Actions = from ea in actions.Where(x => x.EmployeeId == e.EmployeeId)
                                                      select new ActionValue()
                                                      {
                                                          ActionId = ea.ActionDictionaryId,
                                                          ActionName = ea.ActionDictionary.Name,
                                                          Value = ea.ActionValue
                                                      }
                                        }
                };
                return PartialView("_EventsGrid", list);
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlcom = new SqlCommand("SELECT [ActionDictionaryId], [ActionValue], [EmployeeId], [EventId] FROM dbo.EmployeeEventAssignments WHERE EventId = @ID", sqlcon))
                    {
                        sqlcon.Open();
                        sqlcom.CommandType = CommandType.Text;
                        sqlcom.Parameters.AddWithValue("@ID", id);
                        sqlcom.Notification = null;
                        SqlDependency dependancy = new SqlDependency(sqlcom);
                        dependancy.OnChange += Dependancy_OnChange;
                        var reader = sqlcom.ExecuteReader();
                        if (reader.HasRows)
                        {
                            var ourEvent = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                            .Where(x => x.EventId == id).ToList();

                            var events = new ListOfAttendeesWithActions()
                            {
                                ActionDictionaryList = ourEvent.Select(x => new ActionDictionary()
                                {
                                    Id = x.ActionDictionaryId,
                                    Name = x.ActionDictionary.Name
                                }).GroupBy(x => x.Id)
                                    .Select(x => x.First())
                                    .ToList(),
                                EventId = ourEvent.First().EventId,
                                EventAttenderList = ourEvent.Select(x => new EventAttender()
                                {
                                    AttenderId = x.Employee.Id,
                                    FirstName = x.Employee.FirstName,
                                    LastName = x.Employee.LastName,
                                    Email = x.Employee.Email,
                                    Actions = ourEvent.Where(z => z.EmployeeId == x.EmployeeId)
                                        .Select(y => new ActionValue()
                                        {
                                            ActionId = y.ActionDictionaryId,
                                            ActionName = y.ActionDictionary.Name,
                                            Value = y.ActionValue
                                        }).GroupBy(y => y.ActionId).Select(y => y.First()).ToList()
                                }).GroupBy(x=>x.AttenderId).Select(x=>x.First()).OrderBy(x=>x.LastName).ToList()
                            };
                            return PartialView("_EventsGrid", events);
                        }
                        return PartialView("_EventsGrid");
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult GetEventGrid2(int? id)
        {
            var ourEvent = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                        .Where(x => x.EventId == id).ToList();

            var events = new ListOfAttendeesWithActions()
            {
                ActionDictionaryList = ourEvent.Select(x => new ActionDictionary()
                {
                    Id = x.ActionDictionaryId,
                    Name = x.ActionDictionary.Name
                }).GroupBy(x => x.Id)
                    .Select(x => x.First())
                    .ToList(),
                EventId = ourEvent.First().EventId,
                EventAttenderList = ourEvent.Select(x => new EventAttender()
                {
                    AttenderId = x.Employee.Id,
                    FirstName = x.Employee.FirstName,
                    LastName = x.Employee.LastName,
                    Email = x.Employee.Email,
                    Actions = ourEvent.Where(z => z.EmployeeId == x.EmployeeId)
                        .Select(y => new ActionValue()
                        {
                            ActionId = y.ActionDictionaryId,
                            ActionName = y.ActionDictionary.Name,
                            Value = y.ActionValue
                        }).GroupBy(y => y.ActionId).Select(y => y.First()).ToList()
                }).GroupBy(x => x.AttenderId).Select(x => x.First()).OrderBy(x => x.LastName).ToList()
            };
            return PartialView("_EventsGrid", events);     
        }

        /// <summary>
        /// The GetEventHistoryGrid
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpGet]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult GetEventHistoryGrid(int? id)
        {
            if (db.EmployeeEventAssignments.Include(x => x.Event)
                        .Where(x => x.EventId == id).Count() == 0)
            {
                var employ = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == id & x.ActionDictionaryId == 1).ToList();

                var actions = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                    .Where(x => x.EventId == id).ToList();

                var listOfActions = db.ActionGroups.Include(x => x.ActionDictionary).Include(x => x.Event)
                   .Where(x => x.EventId == id)
                   .Select(x => x.ActionDictionary).ToList();

                var list = new ListOfAttendeesWithActions()
                {
                    EventId = id,
                    ActionDictionaryList = listOfActions,
                    EventAttenderList = from e in employ
                                        select new EventAttender()
                                        {
                                            FirstName = e.Employee.FirstName,
                                            AttenderId = e.EmployeeId,
                                            LastName = e.Employee.LastName,
                                            Email = e.Employee.Email,
                                            Actions = from ea in actions.Where(x => x.EmployeeId == e.EmployeeId)
                                                      select new ActionValue()
                                                      {
                                                          ActionId = ea.ActionDictionaryId,
                                                          ActionName = ea.ActionDictionary.Name,
                                                          Value = ea.ActionValue
                                                      }
                                        }
                };
                return PartialView("_EventsHistoryGrid", list);
            }
            else
            {
                var ourEvent = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                            .Where(x => x.EventId == id).ToList();

                var list = new ListOfAttendeesWithActions()
                {
                    ActionDictionaryList = ourEvent.Select(x => new ActionDictionary()
                    {
                        Id = x.ActionDictionaryId,
                        Name = x.ActionDictionary.Name
                    }).GroupBy(x => x.Id)
                        .Select(x => x.First())
                        .ToList(),
                    EventId = ourEvent.First().EventId,
                    EventAttenderList = ourEvent.Select(x => new EventAttender()
                    {
                        AttenderId = x.Employee.Id,
                        FirstName = x.Employee.FirstName,
                        LastName = x.Employee.LastName,
                        Email = x.Employee.Email,
                        Actions = ourEvent.Where(z => z.EmployeeId == x.EmployeeId)
                            .Select(y => new ActionValue()
                            {
                                ActionId = y.ActionDictionaryId,
                                ActionName = y.ActionDictionary.Name,
                                Value = y.ActionValue
                            }).GroupBy(y => y.ActionId).Select(y => y.First()).ToList()
                    }).GroupBy(x => x.AttenderId).Select(x => x.First()).OrderBy(x => x.LastName).ToList()
                };

                return PartialView("_EventsHistoryGrid", list);
            }
        }

        // GET: Events/Create
        /// <summary>
        /// The Create
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Create
        /// </summary>
        /// <param name="@event">The <see cref="Event"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Create([Bind(Include = "Id,Name,StartDate,EndDate")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events/Edit/5
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
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Edit
        /// </summary>
        /// <param name="@event">The <see cref="Event"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Edit([Bind(Include = "Id,Name,StartDate,EndDate")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
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
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
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
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// The DeleteEmployee
        /// </summary>
        /// <param name="EmployeeId">The <see cref="int"/></param>
        /// <param name="EventId">The <see cref="int"/></param>
        /// <returns>The <see cref="JsonResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public JsonResult DeleteEmployee(int EmployeeId, int EventId)
        {
            var listOfId = db.EmployeeEventAssignments
                .Where(x => x.EmployeeId == EmployeeId & x.EventId == EventId)
                .Select(x => x.Id).ToList();
            var result = false;
            foreach (var item in listOfId)
            {
                EmployeeEventAssignment employeeEventAssignment = db.EmployeeEventAssignments.Find(item);
                db.EmployeeEventAssignments.Remove(employeeEventAssignment);
                db.SaveChanges();
                result = true;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The DeleteEmployee
        /// </summary>
        /// <param name="EmployeeId">The <see cref="int"/></param>
        /// <param name="EventId">The <see cref="int"/></param>
        /// <returns>The <see cref="JsonResult"/></returns>
        public async Task<JsonResult> ChangeCheckBoxValue(int EventId, int EmployeeId, int ActionID, bool value = true)
        {
            var result = false;

            foreach (var i in db.EmployeeEventAssignments)
            {
                if (i.ActionDictionaryId == ActionID && i.EmployeeId == EmployeeId && i.EventId == EventId)
                {
                    i.ActionValue = value;
                }
            }
            await db.SaveChangesAsync();
            result = true;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The ImportExcelFile
        /// </summary>
        /// <param name="id">The <see cref="int"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [RoleAuthorize(Roles = "HR")]
        public ActionResult ImportExcelFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventId = (int)id;
            return View();
        }

        /// <summary>
        /// The ReadExcel
        /// </summary>
        /// <param name="upload">The <see cref="HttpPostedFileBase"/></param>
        /// <param name="id">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult ReadExcel(HttpPostedFileBase upload)
        {
            if (upload == null)
            {
                ViewBag.Message = "File could not be empty.";
                return RedirectToAction("Events", "ImportExcelFile", new { id = EventId });
            }
            else if (Path.GetExtension(upload.FileName) == ".xlsx" || Path.GetExtension(upload.FileName) == ".xls")
            {
                ExcelPackage package = new ExcelPackage(upload.InputStream);
                var excelDatabase = EmployeeToDatabase.ToDataBase(package);
                AddToDatabase(excelDatabase);
                return RedirectToAction("Show", "Events", new { id = EventId });
            }
            else
            {
                return RedirectToAction("Show", "Events", new { id = EventId });
            }
        }

        /// <summary>
        /// The AddToDatabase
        /// </summary>
        /// <param name="loadedDatabase">The <see cref="ListOfAttendeesFromExcel"/></param>
        /// <param name="id">The <see cref="int?"/></param>
        [RoleAuthorize(Roles = "HR")]
        private ActionResult AddToDatabase(ListOfAttendeesFromExcel loadedDatabase)
        {
            int eventId = EventId;

            // Add all actions to Attendees
            foreach (var item in loadedDatabase.ListOfActionDictionary)
            {
                var name = item.Name;

                //Add Action to Database if doesnt exist
                if (db.ActionDictionary.Where(x => x.Name == name).Count() == 0)
                {
                    db.ActionDictionary.Add(new ActionDictionary
                    {
                        Name = name
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = entityValidationError.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in entityValidationError.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                Debug.Fail(message);
                            }
                        }
                    }
                }

                //Get Action Id from Database
                var actionId = db.ActionDictionary.Where(x => x.Name == name)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                // Add correct actionGroup if doesnt exist
                if (db.ActionGroups.Where(x => x.EventId == eventId & x.ActionDictionaryId == actionId).Count() == 0)
                {
                    db.ActionGroups.Add(new ActionGroup
                    {
                        EventId = eventId,
                        ActionDictionaryId = actionId
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                        {
                            // Get entry

                            DbEntityEntry entry = entityValidationError.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in entityValidationError.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                Debug.Fail(message);
                            }
                        }
                    }
                }

                // Take all attendees from db in event
                var attendeesInEventList = db.EmployeeEventAssignments
                    .Where(x => x.EventId == eventId)
                    .GroupBy(x => x.EmployeeId)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                foreach (var attendee in attendeesInEventList)
                {
                    db.EmployeeEventAssignments.Add(new EmployeeEventAssignment
                    {
                        EventId = eventId,
                        EmployeeId = attendee.EmployeeId,
                        ActionDictionaryId = actionId,
                        ActionValue = false
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                        {
                            // Get entry
                            DbEntityEntry entry = entityValidationError.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in entityValidationError.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                Debug.Fail(message);
                            }
                        }
                    }
                }
            }

            //Add attendees to database
            foreach (var item in loadedDatabase.ListOfEmployee)
            {
                // for emp from excel check if there is no emp in db where db.email == emp.email
                //If not add to db.
                if (db.Employees.Where(x => x.Email == item.Email).Count() == 0)
                {
                    db.Employees.Add(new Employee
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email
                    });
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                        {
                            // Get entry
                            DbEntityEntry entry = entityValidationError.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Display or log error messages

                            foreach (DbValidationError subItem in entityValidationError.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                Debug.Fail(message);
                            }
                        }
                    }
                }

                // Take id of specified emp from db, there should be only 1 emp with that email
                var idEmployee = db.Employees.Where(x => x.Email == item.Email)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                // Take all actions from db in event with specified ID
                var actionsInEvent = db.ActionGroups.Where(x => x.EventId == eventId).ToList();

                // For each action in event add to database new employeeEventAsignment with proper action id if doesn't exist.
                foreach (var actionInEvent in actionsInEvent)
                {
                    if (db.EmployeeEventAssignments.Where(x => x.EmployeeId == idEmployee
                             & x.ActionDictionaryId == actionInEvent.ActionDictionaryId & x.EventId == eventId).Select(x => x.Id).Count() == 0)
                    {
                        db.EmployeeEventAssignments.Add(new EmployeeEventAssignment
                        {
                            EventId = eventId,
                            EmployeeId = idEmployee,
                            ActionDictionaryId = actionInEvent.ActionDictionaryId,
                            ActionValue = false
                        });
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                            {
                                // Get entry
                                DbEntityEntry entry = entityValidationError.Entry;
                                string entityTypeName = entry.Entity.GetType().Name;

                                // Display or log error messages

                                foreach (DbValidationError subItem in entityValidationError.ValidationErrors)
                                {
                                    string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                             subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                    Debug.Fail(message);
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Show", "Events", new { id = eventId });
        }

        /// <summary>
        /// The ExportToExcel
        /// </summary>
        /// <param name="id">The <see cref="int?"/></param>
        [RoleAuthorize(Roles = "HR")]
        public void ExportToExcel(int? id)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var employ = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == id).GroupBy(x => x.EmployeeId).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Employee.LastName).ToList();

            var actions = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == id).ToList();

            var listOfActions = db.ActionGroups.Include(x => x.ActionDictionary).Include(x => x.Event)
               .Where(x => x.EventId == id)
               .Select(x => x.ActionDictionary).ToList();

            var eventList = db.Events.Where(x => x.Id == id).ToList();
            var eventName = eventList[0].Name;

            var list = new ListOfAttendeesWithActions()
            {
                EventId = id,
                ActionDictionaryList = listOfActions,
                EventAttenderList = from e in employ
                                    select new EventAttender()
                                    {
                                        FirstName = e.Employee.FirstName,
                                        AttenderId = e.EmployeeId,
                                        LastName = e.Employee.LastName,
                                        Email = e.Employee.Email,
                                        Actions = from ea in actions.Where(x => x.EmployeeId == e.EmployeeId)
                                                  select new ActionValue()
                                                  {
                                                      ActionId = ea.ActionDictionaryId,
                                                      ActionName = ea.ActionDictionary.Name,
                                                      Value = ea.ActionValue
                                                  }
                                    }
            };

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = eventName;

            ws.Cells["A3"].Value = "Data";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["B6"].Value = "First Name";
            ws.Cells["C6"].Value = "Last Name";
            ws.Cells["D6"].Value = "Email";
            int alphabetIndex = 4;
            foreach (var item in list.ActionDictionaryList)
            {
                ws.Cells[string.Format("{0}6", alphabet[alphabetIndex])].Value = item.Name;
                alphabetIndex++;
            }

            if (list != null)
            {
                int rowStart = 7;
                foreach (var attendee in list.EventAttenderList)
                {
                    alphabetIndex = 4;
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("#fadce1")));
                    ws.Cells[string.Format("B{0}", rowStart)].Value = attendee.FirstName;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = attendee.LastName;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = attendee.Email;
                    foreach (var actionForAttendee in attendee.Actions)
                    {
                        if (actionForAttendee.Value == true)
                        {
                            ws.Cells[string.Format("{0}{1}", alphabet[alphabetIndex], rowStart)].Value = 1;
                        }
                        else
                        {
                            ws.Cells[string.Format("{0}{1}", alphabet[alphabetIndex], rowStart)].Value = 0;
                        }
                        alphabetIndex++;
                    }
                    rowStart++;
                }
                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + String.Format(eventName + ".xlsx"));
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
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
