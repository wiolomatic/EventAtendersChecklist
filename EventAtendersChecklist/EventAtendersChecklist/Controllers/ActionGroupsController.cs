using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventAtendersChecklist.DAL;
using EventAtendersChecklist.Models;

namespace EventAtendersChecklist.Controllers
{
    public class ActionGroupsController : Controller
    {
        private eacContext db = new eacContext();

        // GET: ActionGroups
        public ActionResult Index()
        {
            var actionGroups = db.ActionGroups.Include(a => a.ActionDictionary).Include(a => a.Event);
            return View(actionGroups.ToList());
        }

        // GET: ActionGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionGroup actionGroup = db.ActionGroups.Find(id);
            if (actionGroup == null)
            {
                return HttpNotFound();
            }
            return View(actionGroup);
        }

        // GET: ActionGroups/Create
        public ActionResult Create()
        {
            ViewBag.ActionDictionaryId = new SelectList(db.ActionDictionary, "Id", "Name");
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name");
            return View();
        }

        // POST: ActionGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ActionDictionaryId,EventId")] ActionGroup actionGroup)
        {
            if (ModelState.IsValid)
            {
                db.ActionGroups.Add(actionGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActionDictionaryId = new SelectList(db.ActionDictionary, "Id", "Name", actionGroup.ActionDictionaryId);
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", actionGroup.EventId);
            return View(actionGroup);
        }

        // GET: ActionGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionGroup actionGroup = db.ActionGroups.Find(id);
            if (actionGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActionDictionaryId = new SelectList(db.ActionDictionary, "Id", "Name", actionGroup.ActionDictionaryId);
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", actionGroup.EventId);
            return View(actionGroup);
        }

        // POST: ActionGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ActionDictionaryId,EventId")] ActionGroup actionGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActionDictionaryId = new SelectList(db.ActionDictionary, "Id", "Name", actionGroup.ActionDictionaryId);
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", actionGroup.EventId);
            return View(actionGroup);
        }

        // GET: ActionGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionGroup actionGroup = db.ActionGroups.Find(id);
            if (actionGroup == null)
            {
                return HttpNotFound();
            }
            return View(actionGroup);
        }

        // POST: ActionGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionGroup actionGroup = db.ActionGroups.Find(id);
            db.ActionGroups.Remove(actionGroup);
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
