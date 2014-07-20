using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using ComplaintBox.Web.Models;
using System.Linq;
using System.Data.Entity;

namespace ComplaintBox.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            string name = User.Identity.Name;
            var model = GetAdminViewModel(name);
            return View(model);
        }


        private static AdminViewModel GetAdminViewModel(string userName)
        {
            Organization org;
            Settings settings;

            int numberOfPendingTopics;
            int numberOfResolvedFeedbacks;
            using (var db = new CboxContext())
            {
                org = db.Organization.First(o => o.UserName == userName);
                numberOfPendingTopics = db.Complaints.Count(c => c.Status == "NEW" && c.OrganizationId == org.Id);
                numberOfResolvedFeedbacks = db.Complaints.Count(c => c.Status == "RESOLVED" && c.OrganizationId == org.Id);
                settings = db.Settings.FirstOrDefault(s => s.OrganizationId == org.Id);
            }

            var model = new AdminViewModel()
                {
                    UserName = string.IsNullOrEmpty(org.FullName) ? userName : org.FullName,
                    NumberOfPendingTopics = numberOfPendingTopics,
                    NumbeOfResolvedFeedbacksToday = numberOfResolvedFeedbacks,
                    OrganizationId = org.Id,
                    OrganizationStatus = org.Status

                };

            if (settings != null)
            {
                if (!string.IsNullOrEmpty(settings.SubjectTitle))
                {
                    model.SubjectTitle = settings.SubjectTitle;
                }
            }

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
                subjectTitle.NewSubjectTitle = settings.SubjectTitle;
            }

            return View(subjectTitle);
        }

        [HttpPost]
        public ActionResult SetSubjectTitle(SetSubjectTitle model)
        {

            string name = User.Identity.Name;
            var adminViewModel = GetAdminViewModel(name);
            var subjectTitle = Mapper.DynamicMap<AdminViewModel, SetSubjectTitle>(adminViewModel);
            subjectTitle.NewSubjectTitle = model.NewSubjectTitle;


            using (var db = new CboxContext())
            {
                var settings = db.Settings.FirstOrDefault(s => s.OrganizationId == adminViewModel.OrganizationId);

                if (settings == null)
                {
                    settings = new Settings
                        {
                            OrganizationId = adminViewModel.OrganizationId,
                            SubjectTitle = model.NewSubjectTitle
                        };

                    db.Settings.Add(settings);
                }
                else
                {
                    settings.SubjectTitle = model.NewSubjectTitle;
                }


                db.SaveChanges();
            }

            return View(subjectTitle);
        }


        public ActionResult ListTopic()
        {
            var adminViewModel = GetAdminViewModel(User.Identity.Name);

            ListTopicViewModel model = Mapper.DynamicMap<AdminViewModel, ListTopicViewModel>(adminViewModel);


            List<Subject> subjects;

            using (var db = new CboxContext())
            {
                subjects = db.Subjects.Where(s => s.OrganizationId == adminViewModel.OrganizationId).ToList();
            }

            model.Topics = subjects.Select(s => new Topic()
                {
                    TopicId = s.Id,
                    TopicTitle = s.Title,
                    OrgId = s.OrganizationId
                }).ToList();


            return View(model);
        }


        public ActionResult AddNewTopic()
        {

            string name = User.Identity.Name;
            var adminVm = GetAdminViewModel(name);
            var newTopic = Mapper.DynamicMap<AdminViewModel, NewTopicViewModel>(adminVm);

            newTopic.TopicTitle = "";
            newTopic.OrgId = adminVm.OrganizationId;

            return View(newTopic);
        }


        public ActionResult EditTopic(int id)
        {

            string name = User.Identity.Name;
            var adminVm = GetAdminViewModel(name);
            var topic = Mapper.DynamicMap<AdminViewModel, EditTopicViewModel>(adminVm);

            Subject subject;
            using (var db = new CboxContext())
            {
                subject = db.Subjects.Find(id);
            }


            topic.TopicTitle = subject.Title;
            topic.OrgId = subject.OrganizationId;
            topic.TopicId = subject.Id;

            return View(topic);
        }

        [HttpPost]
        public ActionResult EditTopic(EditTopicViewModel model)
        {

            using (var db = new CboxContext())
            {
                Subject subject = db.Subjects.Find(model.TopicId);
                subject.Title = model.TopicTitle;
                db.SaveChanges();
            }

            return RedirectToAction("ListTopic");
        }

        public ActionResult DeleteTopic(int id)
        {

            using (var db = new CboxContext())
            {
                Subject subject = db.Subjects.Find(id);
                db.Subjects.Remove(subject);
                db.SaveChanges();
            }

            return RedirectToAction("ListTopic");
        }

        [HttpPost]
        public ActionResult AddNewTopic(NewTopicViewModel model)
        {

            using (var db = new CboxContext())
            {
                db.Subjects.Add(new Subject()
                    {
                        Title = model.TopicTitle,
                        OrganizationId = model.OrgId
                    });

                db.SaveChanges();
            }

            return RedirectToAction("ListTopic");
        }


        [HttpPost]
        public ActionResult OrganizationDetails(OrganizationDetailViewModel vm)
        {
            using (var db = new CboxContext())
            {
                var org = db.Organization.Find(vm.Id);
                org.FullName = vm.OrganizationName;
                org.PhoneNumber = vm.PhoneNumber;
                org.Address = vm.Address;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult OrganizationDetails()
        {
            var adminVm = GetAdminViewModel(User.Identity.Name);
            var model = Mapper.DynamicMap<AdminViewModel, OrganizationDetailViewModel>(adminVm);

            var db = new CboxContext();

            var organization = db.Organization.Find(adminVm.OrganizationId);


            model.Id = model.OrganizationId;
            model.OrganizationName = organization.FullName;
            model.PhoneNumber = organization.PhoneNumber;
            model.Address = organization.Address;



            return View(model);
        }


        public ActionResult RecentFeedbackList()
        {
            var model = GetFeedbackList("ALL");
            return View(model);
        }

        private FeedbackListViewModel GetFeedbackList(string status)
        {
            var viewModel = GetAdminViewModel(User.Identity.Name);
            var model = Mapper.DynamicMap<AdminViewModel, FeedbackListViewModel>(viewModel);

            List<FeedBackViewModel> feedbacks;

            using (var db = new CboxContext())
            {

                var query = db.Complaints.Where(c => c.OrganizationId == viewModel.OrganizationId)
                              .Include(c => c.Subject);

                if (status == "NEW")
                {
                    query = query.Where(c => c.Status == "NEW");
                }
                if (status == "RESOLVED")
                {
                    query = query.Where(c => c.Status == "RESOLVED");
                }

                feedbacks =   query.AsEnumerable()
                              .Select(c => new FeedBackViewModel()
                                  {
                                      Email = c.EmailAddress,
                                      FeedbackUser = c.Complainer,
                                      FeedbackDate = c.ComplainDate,
                                      Feedback = c.Description,
                                      Topic = c.Subject == null ? "" : c.Subject.Title,
                                      FeedbackId = c.Id,
                                      Status = c.Status
                                  }).ToList();
            }


            model.FeedBackViewModels = feedbacks;
            return model;
        }


        public ActionResult ResolvedFeedbackList()
        {
            var model = GetFeedbackList("RESOLVED");
            return View(model);
        }

        public ActionResult NewFeedbackList()
        {
            var model = GetFeedbackList("NEW");
            return View(model);
        }

        public ActionResult FeedbackDetails(int id)
        {
            var admivVm = GetAdminViewModel(User.Identity.Name);
            
            
            var model = Mapper.DynamicMap<AdminViewModel, FeedbackDetailsViewModel>(admivVm);

            Complaint complaint;
            using (var db = new CboxContext())
            {
                complaint = db.Complaints.Where(c => c.Id == id).Include(c => c.Subject).First();
            }

            model.FeedbackUser = complaint.Complainer;
            model.FeedbackDate = complaint.ComplainDate;
            model.Email = complaint.EmailAddress;
            model.Phone = complaint.PhoneNumber;
            model.Comment = complaint.Comment;
            model.Feedback = complaint.Description;
            model.Status = complaint.Status;
            model.Topic = complaint.Subject.Title;
            model.FeedbackId = complaint.Id;

            return View(model);
        }

        public ActionResult ResolveFeedback(FeedbackDetailsViewModel vm)
        {

            using (var db = new CboxContext())
            {
               var  complaint = db.Complaints.Find(vm.FeedbackId);
                complaint.Comment = vm.Comment;
                complaint.Status = "RESOLVED";
                db.SaveChanges();
            }


            return RedirectToAction("ResolvedFeedbackList");
        }


        public ActionResult DeActivateOrganization(int id)
        {
            using (var db = new CboxContext())
            {
                var organization = db.Organization.Find(id);
                organization.Status = OrganizationStatus.New;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult ActivateOrganization(int id)
        {
            using (var db = new CboxContext())
            {
                Organization organization = db.Organization.Find(id);
                organization.Status = OrganizationStatus.Published;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}