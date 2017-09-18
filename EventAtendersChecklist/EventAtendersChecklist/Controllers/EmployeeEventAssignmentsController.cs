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
    public class EmployeeEventAssignmentsController : Controller
    {
        private eacContext db = new eacContext();

        // GET: EmployeeEventAssignments
        public ActionResult Index()
        {
            var employeeEventAssignments = db.EmployeeEventAssignments.Include(e => e.Event);
            return View(employeeEventAssignments.ToList());
        }

        // GET: EmployeeEventAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeEventAssignment employeeEventAssignment = db.EmployeeEventAssignments.Find(id);
            if (employeeEventAssignment == null)
            {
                return HttpNotFound();
            }
            return View(employeeEventAssignment);
        }

        // GET: EmployeeEventAssignments/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name");
            return View();
        }

        // POST: EmployeeEventAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmpoyeeId,EventId")] EmployeeEventAssignment employeeEventAssignment)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeEventAssignments.Add(employeeEventAssignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", employeeEventAssignment.EventId);
            return View(employeeEventAssignment);
        }

        // GET: EmployeeEventAssignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeEventAssignment employeeEventAssignment = db.EmployeeEventAssignments.Find(id);
            if (employeeEventAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", employeeEventAssignment.EventId);
            return View(employeeEventAssignment);
        }

        // POST: EmployeeEventAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmpoyeeId,EventId")] EmployeeEventAssignment employeeEventAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeEventAssignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", employeeEventAssignment.EventId);
            return View(employeeEventAssignment);
        }

        // GET: EmployeeEventAssignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeEventAssignment employeeEventAssignment = db.EmployeeEventAssignments.Find(id);
            if (employeeEventAssignment == null)
            {
                return HttpNotFound();
            }
            return View(employeeEventAssignment);
        }

        // POST: EmployeeEventAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeEventAssignment employeeEventAssignment = db.EmployeeEventAssignments.Find(id);
            db.EmployeeEventAssignments.Remove(employeeEventAssignment);
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
