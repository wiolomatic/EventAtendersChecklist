namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.DAL;
    using EventAtendersChecklist.Models;
    using EventAtendersChecklist.ModelsView;
    using OfficeOpenXml;
    using System.Data;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="EventsController" />
    /// </summary>
    public class EventsController : Controller
    {
        /// <summary>
        /// Defines the db
        /// </summary>
        private eacContext db = new eacContext();

        // GET: Events
        /// <summary>
        /// The Index
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult Index()
        {
            return View(db.Events.ToList().OrderBy(x => x.StartDate));
        }

        // GET: Events/Details/5
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


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: Events/Create
        /// <summary>
        /// The Create
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
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
        /// The ImportExcelFile
        /// </summary>
        /// <param name="id">The <see cref="int"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult ImportExcelFile(int? id)
        {
            if(id == null)
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

        /// <summary>
        /// The ReadExcel
        /// </summary>
        /// <param name="upload">The <see cref="HttpPostedFileBase"/></param>
        /// <param name="eventId">The <see cref="int?"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        public ActionResult ReadExcel(HttpPostedFileBase upload, int id)
        {
            if (upload == null)
            {
                ViewBag.Message = "File could not be empty.";
                return View(string.Format("../Events/ImportExcelFile/{0}", id));
            }
            else if (Path.GetExtension(upload.FileName) == ".xlsx" || Path.GetExtension(upload.FileName) == ".xls")
            {
                ExcelPackage package = new ExcelPackage(upload.InputStream);
                var lol = EmployeeToDatabase.ToDataBase(package);
                AddToDatabase(lol, id);
                ViewBag.Message = "Success";
                return View(string.Format("../Events/Show/", id));
            }
            else
            {
                ViewBag.Message = "Wrong file extension";
                return View(string.Format("../Events/ImportExcelFile/{0}", id));
            }
        }

        /// <summary>
        /// The AddToDatabase
        /// </summary>
        /// <param name="loafe">The <see cref="ListOfAttendeesFromExcel"/></param>
        /// <param name="id">The <see cref="int?"/></param>
        private void AddToDatabase(ListOfAttendeesFromExcel loafe, int id)
        {
            int eventId = (int)id;

            //Add attendees to database
            foreach (var item in loafe.ListOfEmployee)
            {
                db.Employees.Add(new Employee
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email
                });
                db.SaveChanges();

                var idEmployee = db.Employees.Where(x => x.Email == item.Email)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                var actionsInEvent = db.ActionGroups.Where(x => x.EventId == eventId).ToList();
                foreach (var actionInEvent in actionsInEvent)
                {
                    db.EmployeeEventAssignments.Add(new EmployeeEventAssignment
                    {
                        EventId = eventId,
                        EmployeeId = idEmployee,
                        ActionDictionaryId = actionInEvent.ActionDictionaryId,
                        ActionValue = false
                    });
                    db.SaveChanges();
                }
            }

            // Add all actions to Attendees
            foreach (var item in loafe.ListOfActionDictionary)
            {
                var name = item.Name;

                //Add Action to Database if doesnt exist
                var exist = db.ActionDictionary.Where(x => x.Name == name).Count();
                if (exist == 0)
                {
                    db.ActionDictionary.Add(new ActionDictionary
                    {
                        Name = name
                    });
                    db.SaveChanges();
                }

                //Get Action Id from Database
                var actionId = db.ActionDictionary.Where(x => x.Name == name)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                // Add correct actionGroup if doesnt exist
                var exist2 = db.ActionGroups.Where(x => x.EventId == eventId & x.ActionDictionaryId == actionId).Count();
                if (exist2 == 0)
                {
                    db.ActionGroups.Add(new ActionGroup
                    {
                        EventId = eventId,
                        ActionDictionaryId = actionId
                    });
                }
                else
                {
                    //return RedirectToAction("Show", "Events", new { id = eventId });
                }
                var attendeeInEventList = db.EmployeeEventAssignments
                    .Where(x => x.EventId == 1)
                    .GroupBy(x => x.EmployeeId)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                foreach (var item2 in attendeeInEventList)
                {
                    db.EmployeeEventAssignments.Add(new EmployeeEventAssignment
                    {
                        EventId = eventId,
                        EmployeeId = item2.EmployeeId,
                        ActionDictionaryId = actionId,
                        ActionValue = false
                    });
                    db.SaveChanges();
                }
                //return RedirectToAction("Show", "Events", new { id = eventId });
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
