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
    public class ActionDictionaryController : Controller
    {
        private eacContext db = new eacContext();

        // GET: ActionDictionary
        public ActionResult Index()
        {
            return View(db.ActionDictionary.ToList());
        }

        // GET: ActionDictionary/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionDictionary actionName = db.ActionDictionary.Find(id);
            if (actionName == null)
            {
                return HttpNotFound();
            }
            return View(actionName);
        }

        // GET: ActionDictionary/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionDictionary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ActionDictionary actionName)
        {
            if (ModelState.IsValid)
            {
                db.ActionDictionary.Add(actionName);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionName);
        }

        // GET: ActionDictionary/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionDictionary actionName = db.ActionDictionary.Find(id);
            if (actionName == null)
            {
                return HttpNotFound();
            }
            return View(actionName);
        }

        // POST: ActionDictionary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ActionDictionary actionName)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionName).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionName);
        }

        // GET: ActionDictionary/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionDictionary actionName = db.ActionDictionary.Find(id);
            if (actionName == null)
            {
                return HttpNotFound();
            }
            return View(actionName);
        }

        // POST: ActionDictionary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionDictionary actionName = db.ActionDictionary.Find(id);
            db.ActionDictionary.Remove(actionName);
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
