using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ComplaintBox.Web.Models;
using System.Linq;

namespace ComplaintBox.Web.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public ActionResult Feedback(int id)
        {
            Organization org;
            List<Subject> subjects;
            Settings settings;
            using (var db = new CboxContext())
            {
                org = db.Organization.Find(id);
                subjects = db.Subjects.Where(s=> s.OrganizationId == id).ToList();
                settings = db.Settings.FirstOrDefault(s => s.OrganizationId == id);
            }

            ViewBag.OrganizationName = org.FullName;

            IEnumerable<SelectListItem> subjectList = subjects.Select(s => new SelectListItem()
                {
                    Text = s.Title, Value = s.Id.ToString()
                });


            return View(new FeedbackViewModel()
                {
                    OrganizationId = id,
                    Subjects = subjectList,
                    TopicTitle = settings != null && !string.IsNullOrEmpty(settings.SubjectTitle) ? settings.SubjectTitle : "Topic"
                });
        }


        [HttpPost]
        public ActionResult Feedback(FeedbackViewModel vm)
        {
            using (var db = new CboxContext())
            {
                var complaint = new Complaint()
                    {
                        OrganizationId = vm.OrganizationId,
                        Address = vm.Address,
                        PhoneNumber = vm.Phone,
                        EmailAddress = vm.Email,
                        Complainer = vm.Name,
                        Description = vm.Feedback,
                        ComplainDate = DateTime.Today,
                        SubjectId = vm.TopicId,
                        Status = "NEW"
                    };

                db.Complaints.Add(complaint);
                db.SaveChanges();
            }

            return RedirectToAction("ThankYou", new {name = vm.Name});
        }



        public ActionResult ThankYou(string name)
        {
            ViewBag.CustomerName = name;            
            return View();
        }
    }
}