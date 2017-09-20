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
    public class ActionDictionariesController : Controller
    {
        private eacContext db = new eacContext();

        // GET: ActionDictionaries
        public ActionResult Index()
        {
            return View(db.ActionDictionary.ToList());
        }

        // GET: ActionDictionaries/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionDictionaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ActionDictionaries/Edit/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionDictionary actionDictionary = db.ActionDictionary.Find(id);
            db.ActionDictionary.Remove(actionDictionary);
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
