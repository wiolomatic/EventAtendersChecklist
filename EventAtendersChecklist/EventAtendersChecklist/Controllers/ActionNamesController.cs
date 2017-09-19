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
    public class ActionNamesController : Controller
    {
        private eacContext db = new eacContext();

        // GET: ActionNames
        public ActionResult Index()
        {
            return View(db.ActionNames.ToList());
        }

        // GET: ActionNames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionName actionName = db.ActionNames.Find(id);
            if (actionName == null)
            {
                return HttpNotFound();
            }
            return View(actionName);
        }

        // GET: ActionNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ActionName actionName)
        {
            if (ModelState.IsValid)
            {
                db.ActionNames.Add(actionName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionName);
        }

        // GET: ActionNames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionName actionName = db.ActionNames.Find(id);
            if (actionName == null)
            {
                return HttpNotFound();
            }
            return View(actionName);
        }

        // POST: ActionNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ActionName actionName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionName).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionName);
        }

        // GET: ActionNames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionName actionName = db.ActionNames.Find(id);
            if (actionName == null)
            {
                return HttpNotFound();
            }
            return View(actionName);
        }

        // POST: ActionNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionName actionName = db.ActionNames.Find(id);
            db.ActionNames.Remove(actionName);
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
