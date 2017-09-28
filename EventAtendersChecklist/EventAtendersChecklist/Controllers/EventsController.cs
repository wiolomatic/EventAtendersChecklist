namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.DAL;
    using EventAtendersChecklist.Models;
    using EventAtendersChecklist.ModelsView;
    using OfficeOpenXml;
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Configuration;
    using System.Data.SqlClient;
    using EventAtendersChecklist.SignalR;

    /// <summary>
    /// Defines the <see cref="EventsController" />
    /// </summary>
    public class EventsController : Controller
    {
        /// <summary>
        /// Defines the db
        /// </summary>
        private eacContext db = new eacContext();

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

        [HttpGet]
        public ActionResult GetEvents()
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
                    dependancy.OnChange += dependancy_OnChange;
                    var reader = sqlcom.ExecuteReader();

                    var events = reader.Cast<IDataRecord>()
                       .Select(e => new Event()
                       {
                           Id = e.GetInt32(0),
                           Name = e.GetString(1),
                           StartDate = e.GetDateTime(2),
                           EndDate = e.GetDateTime(3)
                       }).ToList();
                    return PartialView("_EventsList", events.OrderBy(x=>x.StartDate));
                }
            }
        }

        void dependancy_OnChange(object sender, SqlNotificationEventArgs e)
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

        [HttpGet]
        public ActionResult GetEventGrid(int? id)
        {
            string query = "SELECT " +
                "EmployeeEventAssignments.EventId, " +
                "EmployeeEventAssignments.EmployeeId, " +
                "Employees.FirstName, " +
                "Employees.LastName, " +
                "Employees.Email, " +
                "ActionDictionaries.Name, " +
                "EmployeeEventAssignments.ActionValue, " +
                "EmployeeEventAssignments.ActionDictionaryId " +
                "FROM [EmployeeEventAssignments] " +
                "JOIN [Events] " +
                "ON Events.Id = EmployeeEventAssignments.EventId " +
                "JOIN [Employees] " +
                "ON EmployeeEventAssignments.EmployeeId = Employees.Id " +
                "JOIN [ActionDictionaries] ON EmployeeEventAssignments.ActionDictionaryId = ActionDictionaries.Id " +
                "WHERE Events.Id = @ID";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlcom = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    sqlcom.CommandType = CommandType.Text;
                    sqlcom.Parameters.AddWithValue("@ID", id);
                    sqlcom.Notification = null;
                    SqlDependency dependancy = new SqlDependency(sqlcom);
                    dependancy.OnChange += dependancy_OnChange;
                    var reader = sqlcom.ExecuteReader();
                    if(reader.HasRows)
                    {
                        var eventsSql = reader.Cast<IDataRecord>().
                            Select(eventAttender => new EmployeeInEventView()
                            {
                                EventID = eventAttender.GetInt32(0),
                                EmployeeId = eventAttender.GetInt32(1),
                                FirtName = eventAttender.GetString(2),
                                LastName = eventAttender.GetString(3),
                                Email = eventAttender.GetString(4),
                                ActionDictionaryId = eventAttender.GetInt32(7),
                                ActionName = eventAttender.GetString(5),
                                ActionValue = eventAttender.GetBoolean(6)
                            }).ToList();

                        var actions = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                        .Where(x => x.EventId == id).ToList();

                        var events = new ListOfAttendeesWithActions()
                        {
                            ActionDictionaryList = eventsSql.Select(x => new ActionDictionary()
                            {
                                Id = x.ActionDictionaryId,
                                Name = x.ActionName
                            }).GroupBy(x => x.Id).Select(x => x.First()).ToList(),
                            EventId = eventsSql.First().EventID,
                            EventAttenderList = eventsSql.Select(x => new EventAttender()
                            {
                                AttenderId = x.EmployeeId,
                                FirstName = x.FirtName,
                                LastName = x.LastName,
                                Email = x.Email,
                                Actions = actions.Where(z => z.EmployeeId == x.EmployeeId)
                                    .Select( y => new ActionValue()
                                    {
                                        ActionId = y.ActionDictionaryId,
                                        ActionName = y.ActionDictionary.Name,
                                        Value = y.ActionValue
                                    }).GroupBy(y => y.ActionId).Select(y => y.First()).ToList()
                            }).GroupBy(x => x.AttenderId).Select(x => x.First()).ToList()                         
                        };
                        return PartialView("_EventsGrid", events);
                    }
                    return PartialView("_EventsGrid");
                }
            }
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
        /// The DeleteEmployee
        /// </summary>
        /// <param name="EmployeeId">The <see cref="int"/></param>
        /// <param name="EventId">The <see cref="int"/></param>
        /// <returns>The <see cref="JsonResult"/></returns>
        public JsonResult ChangeCheckBoxValue(int EventId, int EmployeeId, int ActionID, bool value = true)
        {
            var query = "UPDATE [EmployeeEventAssignments] " +
                "SET ActionValue = @Value " +
                "WHERE EmployeeId = @EmployeeId AND " +
                "ActionDictionaryId = @ActionDicationaryID AND " +
                "EventId = @EventID";
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var result = false;
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlcom = new SqlCommand(query, sqlcon))
                {
                    sqlcon.Open();
                    sqlcom.CommandType = CommandType.Text;
                    sqlcom.Parameters.AddWithValue("@Value", value);
                    sqlcom.Parameters.AddWithValue("@EmployeeId", EmployeeId);
                    sqlcom.Parameters.AddWithValue("@ActionDicationaryID", ActionID);
                    sqlcom.Parameters.AddWithValue("@EventID", EventId);
                    sqlcom.Notification = null;
                    SqlDependency dependancy = new SqlDependency(sqlcom);
                    dependancy.OnChange += dependancy_OnChange;
                    sqlcom.ExecuteReader();
                    result = true;
                }
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
        public ActionResult ReadExcel(HttpPostedFileBase upload)
        {
            if (upload == null)
            {
                ViewBag.Message = "File could not be empty.";
                return View(string.Format("../Events/ImportExcelFile/{0}", EventId));
            }
            else if (Path.GetExtension(upload.FileName) == ".xlsx" || Path.GetExtension(upload.FileName) == ".xls")
            {
                ExcelPackage package = new ExcelPackage(upload.InputStream);
                var lol = EmployeeToDatabase.ToDataBase(package);
                AddToDatabase(lol);
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
        /// <param name="loafe">The <see cref="ListOfAttendeesFromExcel"/></param>
        /// <param name="id">The <see cref="int?"/></param>
        private ActionResult AddToDatabase(ListOfAttendeesFromExcel loafe)
        {
            int eventId = EventId;

            //Add attendees to database
            foreach (var item in loafe.ListOfEmployee)
            {
                if (db.Employees.Where(x => x.Email == item.Email).Count() == 0)
                {
                    db.Employees.Add(new Employee
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email
                    });
                    db.SaveChanges();
                }
                var idEmployee = db.Employees.Where(x => x.Email == item.Email)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                if (db.EmployeeEventAssignments.Where(x => x.EmployeeId == idEmployee).Count() == 0)
                {
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

                var attendeeInEventList = db.EmployeeEventAssignments
                    .Where(x => x.EventId == eventId)
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
            }
            return RedirectToAction("Show", "Events", new { id = eventId });
        }

        public void ExportToExcel(int? id)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
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

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "Report1";

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
                        if(actionForAttendee.Value == true)
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
                Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
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
