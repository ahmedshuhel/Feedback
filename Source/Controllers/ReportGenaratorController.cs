using ComplaintBox.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ComplaintBox.Web.Controllers
{
    public class ReportGenaratorController : Controller
    {

        [HttpGet]
        public ActionResult DateWiseComplaints(int id)
        {
            Settings setting;
            IList<SelectListItem> subjects;
            using (var db = new CboxContext())
            {
                subjects = db.Subjects
                    .Where(s => s.OrganizationId == id)
                    .OrderBy(s => s.Title)
                    .AsEnumerable()
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Title,
                        Value = s.Id.ToString()
                    }).ToList();

                subjects.Add(new SelectListItem()
                {
                    Text = "All",
                    Value = "-1"
                });
                setting = db.Settings.FirstOrDefault(s => s.OrganizationId == id);
            }
            ViewBag.Subjects = subjects;
            ViewBag.SubjectTitle = setting.SubjectTitle;
            ViewBag.id = id;
            return View();
        }


        [HttpPost]
        public ActionResult DateWiseComplaints(DateTime fromDate, DateTime toDate, string state,int id,int subjectId)
        {
            Settings setting;
            IList<SelectListItem> subjects;
            List<Complaint> complaints;

            DateTime endDate = toDate.AddDays(1);

            using (var db = new CboxContext())
            {
                setting = db.Settings.FirstOrDefault(s => s.OrganizationId == id);

                subjects = db.Subjects
                    .Where(s => s.OrganizationId == id)
                    .OrderBy(s => s.Title)
                    .AsEnumerable()                    
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Title,
                        Value = s.Id.ToString()
                    }).ToList();

                subjects.Add(new SelectListItem() { Text = "All", Value = "-1" });


                // state ["All", --]
                // subject [-1 , ---]
                
                // date
                // organiztion id,
              
                  var query =  db.Complaints
                        .Where(c => c.ComplainDate >= fromDate && c.ComplainDate < endDate)
                        .Where(c => c.OrganizationId == id)                        
                        .Include(c => c.Subject);

                  if (state != "All")
                  {
                      query = query.Where(c => c.Status == state);
                  }

                  if (subjectId != -1)
                  {
                      query = query.Where(c => c.SubjectId == subjectId);
                  }

                  complaints = query.ToList();
                
            }

            ViewBag.Subjects = subjects;
            ViewBag.SubjectTitle = setting.SubjectTitle;

            return View("ShowDateWiseComplaint", complaints);
        }

        public ActionResult ComplaintList(int id)
        {
            Settings setting;
            List<Complaint> complaints;

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);


            using (var db = new CboxContext())
            {
                complaints = db.Complaints
                                .Where(c => c.ComplainDate >= today && c.ComplainDate < tomorrow)
                                .Where(s => s.OrganizationId == id)
                                .Include(c => c.Subject)
                                 .ToList();

                setting = db.Settings.FirstOrDefault(s => s.OrganizationId == id);

                db.SaveChanges();

              }

            ViewBag.SubjectTitle = setting.SubjectTitle;

            return View("ComplaintList", complaints);
        }


        [HttpGet]
        public ActionResult ShowDetail(int id)
        {
            List<Complaint> complaints;
            using (var db = new CboxContext())
            {
                complaints = db.Complaints.Where(c => c.Id == id).ToList();
                db.SaveChanges();
            }

            return View("ShowDetail", complaints);

        }

        [HttpGet]
        public ActionResult MonthWiseComplaints(int id)
        {
            ViewBag.id = id;
            return View();
        
        }

        [HttpPost]
        public ActionResult MonthWiseComplaints(int month, int year, string state,int id)

        {
          
            var totalDays = DateTime.DaysInMonth(year, month);

            Dictionary<int, int> monthlyComplain = new Dictionary<int, int>();

            using (var db = new CboxContext())
            {

                for (int i = 1; i <= totalDays; i++)
                {
                    DateTime fromDate = new DateTime(year, month, i);

                    DateTime toDate = fromDate.AddDays(1);

                    if (state == "All")
                    {

                        int complainTotal = db.Complaints.Count(c => c.ComplainDate >= fromDate && c.ComplainDate < toDate && c.OrganizationId==id);

                        monthlyComplain.Add(i, complainTotal);
                    }
                    else
                    {
                        int complainTotal = db.Complaints.Count(c => c.ComplainDate >= fromDate && c.ComplainDate < toDate && c.Status == state && c.OrganizationId == id);

                        monthlyComplain.Add(i, complainTotal);
                    }
                }                
            }


            return View("ShowMonthWiseComplaints", monthlyComplain);
        
        }

    }
}
    

