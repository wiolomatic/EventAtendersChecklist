using EventAtendersChecklist.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventAtendersChecklist.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveData(List<Attenders> employees)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MyEventAttendersEntities dc = new MyEventAttendersEntities())
                {
                    foreach (var i in employees)
                    {
                        dc.Attenders.Add(i);
                    }
                    try
                    {
                        dc.SaveChanges();
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}