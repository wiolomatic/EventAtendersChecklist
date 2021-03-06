﻿namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.DAL;
    using EventAtendersChecklist.Models;
    using EventAtendersChecklist.ModelsView;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="ActionDictionariesController" />
    /// </summary>
    [Authorize]
    [RoleAuthorize(Roles = "HR")]
    public class ActionDictionariesController : Controller
    {
        /// <summary>
        /// Defines the db
        /// </summary>
        private EacContext db = new EacContext();

        // GET: ActionDictionaries
        /// <summary>
        /// The Index
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult Index()
        {
            return View(db.ActionDictionary.ToList());
        }

        // GET: ActionDictionaries/Details/5
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
            ActionDictionary actionDictionary = db.ActionDictionary.Find(id);
            if (actionDictionary == null)
            {
                return HttpNotFound();
            }
            return View(actionDictionary);
        }

        // GET: ActionDictionaries/Create
        /// <summary>
        /// The Create
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionDictionaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Create
        /// </summary>
        /// <param name="actionDictionary">The <see cref="ActionDictionary"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ActionDictionary actionDictionary)
        {
            if (ModelState.IsValid)
            {
                db.ActionDictionary.Add(actionDictionary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionDictionary);
        }

        /// <summary>
        /// The AddActionToEvent
        /// </summary>
        /// <param name="id">The <see cref="int"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult AddActionToEvent(int id)
        {
            AddActionToEventViewModel actionModel = new AddActionToEventViewModel()
            {
                EventID = id,
                ActionDictionaryProp = new ActionDictionary()
            };

            if (actionModel == null)
            {
                return HttpNotFound();
            }
            return View(actionModel);
        }

        /// <summary>
        /// The AddActionToEvent
        /// </summary>
        /// <param name="actionModel">The <see cref="AddActionToEventViewModel"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActionToEvent(AddActionToEventViewModel actionModel)
        {
            if (ModelState.IsValid)
            {
                var eventId = actionModel.EventID;
                var name = actionModel.ActionDictionaryProp.Name;

                //Add Action to Database if doesnt exist
                var actionExist = db.ActionDictionary.Where(x => x.Name == name).Count();
                if (actionExist == 0)
                {
                    db.ActionDictionary.Add(new ActionDictionary
                    {
                        Name = actionModel.ActionDictionaryProp.Name
                    });
                    db.SaveChanges();
                }

                //Get Action Id from Database
                var actionId = db.ActionDictionary.Where(x => x.Name == actionModel.ActionDictionaryProp.Name)
                    .Select(x => x.Id)
                    .ToList()
                    .First();

                // Add correct actionGroup if doesnt exist
                var actionGroupExist = db.ActionGroups.Where(x => x.EventId == eventId & x.ActionDictionaryId == actionId).Count();
                if (actionGroupExist == 0)
                {
                    db.ActionGroups.Add(new ActionGroup
                    {
                        EventId = eventId,
                        ActionDictionaryId = actionId
                    });
                    db.SaveChanges();
                }

                //Take all attendee in event and add to them new action
                var attendeeInEventList = db.EmployeeEventAssignments
                    .Where(x => x.EventId == eventId)
                    .GroupBy(x => x.EmployeeId)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                foreach (var item in attendeeInEventList)
                {
                    if (db.EmployeeEventAssignments
                        .Where(x => x.EmployeeId == item.EmployeeId &
                        x.EventId == item.EventId &
                        x.ActionDictionaryId == actionId)
                        .Count() == 0)
                    {
                        db.EmployeeEventAssignments.Add(new EmployeeEventAssignment
                        {
                            EventId = eventId,
                            EmployeeId = item.EmployeeId,
                            ActionDictionaryId = actionId,
                            ActionValue = false
                        });
                        //db.SaveChanges();
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Show", "Events", new { id = eventId });
            }
            return View(actionModel);
        }

        // GET: ActionDictionaries/Edit/5
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
            ActionDictionary actionDictionary = db.ActionDictionary.Find(id);
            if (actionDictionary == null)
            {
                return HttpNotFound();
            }
            return View(actionDictionary);
        }

        // POST: ActionDictionaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The Edit
        /// </summary>
        /// <param name="actionDictionary">The <see cref="ActionDictionary"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ActionDictionary actionDictionary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionDictionary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionDictionary);
        }

        // GET: ActionDictionaries/Delete/5
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
            ActionDictionary actionDictionary = db.ActionDictionary.Find(id);
            if (actionDictionary == null)
            {
                return HttpNotFound();
            }
            return View(actionDictionary);
        }

        // POST: ActionDictionaries/Delete/5
        /// <summary>
        /// The DeleteConfirmed
        /// </summary>
        /// <param name="id">The <see cref="int"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionDictionary actionDictionary = db.ActionDictionary.Find(id);
            db.ActionDictionary.Remove(actionDictionary);
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
