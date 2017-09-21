using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EventAtendersChecklist.DAL;
using EventAtendersChecklist.Models;
using EventAtendersChecklist.ModelsView;

namespace EventAtendersChecklist.Controllers
{
    public class EventsController : Controller
    {
        private eacContext db = new eacContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList().OrderBy(x => x.StartDate));
        }

        // GET: Events/Details/5
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
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employ = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == id & x.ActionDictionaryId == 1).ToList();

            var test = db.EmployeeEventAssignments.Include(x => x.Event).Include(x => x.Employee)
                .Where(x => x.EventId == id).ToList();

            var listOfActions = db.ActionGroups.Include(x => x.ActionDictionary).Include(x => x.Event)
               .Where(x => x.EventId == id)
               .Select(x => x.ActionDictionary).ToList();

            var list = new TestView()
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
                                        Actions = from ea in test.Where(x => x.EmployeeId == e.EmployeeId)
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
