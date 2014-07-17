using System.Web.Mvc;
using AutoMapper;
using ComplaintBox.Web.Models;
using System.Linq;

namespace ComplaintBox.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {

            string name = User.Identity.Name;

            var model = GetAdminViewModel(name);
            return View(model);
        }


        private static AdminViewModel GetAdminViewModel(string userName)
        {
            Organization org;
            int numberOfPendingTopics;
            int numberOfResolvedFeedbacks;
            using (var db = new CboxContext())
            {
                org = db.Organization.First(o => o.UserName == userName);
                numberOfPendingTopics = db.Complaints.Count(c => c.Status == "NEW" && c.OrganizationId == org.Id);
                numberOfResolvedFeedbacks = db.Complaints.Count(c => c.Status == "RESOLVED" && c.OrganizationId == org.Id);
            }

            var model = new AdminViewModel()
                {
                    UserName = string.IsNullOrEmpty(org.FullName) ? userName : org.FullName,
                    NumberOfPendingTopics = numberOfPendingTopics,
                    NumbeOfResolvedFeedbacksToday = numberOfResolvedFeedbacks,
                    OrganizationId = org.Id
                };
            return model;
        }


        public ActionResult SetSubjectTitle()
        {

            string name = User.Identity.Name;

            var adminViewModel = GetAdminViewModel(name);

            Settings settings;
            using (var db = new CboxContext())
            {
                settings = db.Settings.FirstOrDefault(s => s.OrganizationId == adminViewModel.OrganizationId);
            }
            
            var subjectTitle = Mapper.DynamicMap<AdminViewModel, SetSubjectTitle>(adminViewModel);

            if (settings != null)
            {
                subjectTitle.SubjectTitle = settings.SubjectTitle;
            }

            return View(subjectTitle);
        }

        [HttpPost]
        public ActionResult SetSubjectTitle(SetSubjectTitle model)
        {

            string name = User.Identity.Name;
            var adminViewModel = GetAdminViewModel(name);
            var subjectTitle = Mapper.DynamicMap<AdminViewModel, SetSubjectTitle>(adminViewModel);
            subjectTitle.SubjectTitle = model.SubjectTitle;


            using (var db = new CboxContext())
            {
                var settings = db.Settings.FirstOrDefault(s => s.OrganizationId == adminViewModel.OrganizationId);

                if (settings == null)
                {
                    settings = new Settings
                        {
                            OrganizationId = adminViewModel.OrganizationId,
                            SubjectTitle = model.SubjectTitle
                        };

                    db.Settings.Add(settings);
                }
                else
                {
                    settings.SubjectTitle = model.SubjectTitle;
                }


                db.SaveChanges();
            }

            return View(subjectTitle);
        }

    }
}