using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplaintBox.Web.Models;

namespace ComplaintBox.Web.Controllers
{
    public class ComplainController : Controller
    {
        [HttpGet]
        public ActionResult NewComplaints(int id)
        {   Settings setting;
            List<Complaint> complaints;
           
         
            using (var db = new CboxContext())
            {
                complaints = db.Complaints
                                .Where(c => c.Status == "NEW")
                                .Where(s => s.OrganizationId == id)
                                .Include(c => c.Subject)
                                .ToList();
                setting = db.Settings.FirstOrDefault(s => s.OrganizationId == id);
              
            }
            ViewBag.SubjectTitle = setting.SubjectTitle;
            return View(complaints);
        }


        [HttpGet]
        public ActionResult ResolvedComplaints()
        {

            List<Complaint> complaints;
            using (var db = new CboxContext())
            {
                complaints = db.Complaints.Where(c => c.Status == "Resolved").ToList();
            }
            return View(complaints);
        }


        [HttpGet]
        public ActionResult ResolveComplain(int id)
        {


            using (var db = new CboxContext())
            {
                var c = db.Complaints.Find(id);
                c.Status = "Resolved";

                db.SaveChanges();

            }
            ViewBag.id = id;
            return View("ResolveComplain");
        }

        public ActionResult SaveComment(int id,string Comment)
        {
           

            using (var db = new CboxContext())
            {
                var c = db.Complaints.Find(id);
                c.Comment = Comment;

                db.SaveChanges();
            }
            ViewBag.id = id;
            return RedirectToAction("CommenttList");
        }

        public ActionResult CommenttList()
        {
            List<Complaint> complaints;
            using (var db = new CboxContext())
            {
                complaints = db.Complaints.Where(c => c.Status == "Resolved").ToList();
            }
            return View(complaints);
        
        }

        [HttpGet]
        public ActionResult WriteComplain(int id)
        {
            IList<SelectListItem> subjects;

            Settings setting;

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


                setting = db.Settings.FirstOrDefault(s=> s.OrganizationId == id);
            }


            ViewBag.Subjects = subjects;
            ViewBag.SubjectTitle = setting.SubjectTitle;

            return View(new Complaint()
            {
                OrganizationId = id
            });
        }


        [HttpPost]
        public ActionResult WriteComplain(Complaint complaint)
        {
            complaint.ComplainDate = DateTime.Today;
            complaint.Status = "NEW";


            if (ModelState.IsValid)
            {
                using (var db = new CboxContext())
                {
                    db.Complaints.Add(complaint);
                    db.SaveChanges();
                }

                return RedirectToAction("UserView");
            }



            IList<SelectListItem> subjects;
          
            List<Settings> setting;

            using (var db = new CboxContext())
            {
                subjects = db.Subjects
                    .OrderBy(s => s.Title)
                    .AsEnumerable()
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Title,
                        Value = s.Id.ToString()
                    }).ToList();


                setting = db.Settings
                    .OrderBy(s=>s.SubjectTitle)
                    .ToList();

            }


            ViewBag.Subjects = subjects;
            ViewBag.Settings = setting;

            return View(complaint);
        }


        public ActionResult Feedback()
        {
            return View();
        }

        public ActionResult UserView()
        {
            return View();
        
        }
    }
}