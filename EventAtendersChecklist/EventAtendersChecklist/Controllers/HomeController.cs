using EventAtendersChecklist.Data;
using EventAtendersChecklist.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventAtendersChecklist.Controllers
{
    public class HomeController : Controller
    {
        MyEventAttendersEntities db = new MyEventAttendersEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Export()
        {
            ViewBag.Message = "Your application export attenders.";
            List<AttendersViewModel> attenders = db.Attenders.Select(x => new AttendersViewModel
            {
                FristName = x.FristName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList();

            return View(attenders);
        }

        public void ExportToExcel()
        {
            List<AttendersViewModel> attenders = db.Attenders.Select(x => new AttendersViewModel
            {
                FristName = x.FristName,
                LastName = x.LastName,
                Email = x.Email
            }).ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "Communication";
            ws.Cells["B1"].Value = "Com1";

            ws.Cells["A2"].Value = "Report";
            ws.Cells["B2"].Value = "Report1";

            ws.Cells["A3"].Value = "Data";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Attender ID";
            ws.Cells["B6"].Value = "First Name";
            ws.Cells["C6"].Value = "Last Name";
            ws.Cells["E6"].Value = "Email";

            if(attenders != null)
            {
                int rowStart = 7;
                foreach (var item in attenders)
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("#fadce1")));
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.Id;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.FristName;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.LastName ;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.Email;
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
    }
}