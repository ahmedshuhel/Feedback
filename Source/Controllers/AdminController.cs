using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ComplaintBox.Web.Models;
using MvcPaging;

namespace ComplaintBox.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private const int PageSize = 10;

        private readonly string[] _months = new[]
            {
                "January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
                "November", "December"
            };

        public ActionResult Index()
        {
            string name = User.Identity.Name;
            AdminViewModel model = GetAdminViewModel(name);
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
                numberOfResolvedFeedbacks =
                    db.Complaints.Count(c => c.Status == "RESOLVED" && c.OrganizationId == org.Id);
                settings = db.Settings.FirstOrDefault(s => s.OrganizationId == org.Id);
            }

            var model = new AdminViewModel
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

            AdminViewModel adminViewModel = GetAdminViewModel(name);

            Settings settings;
            using (var db = new CboxContext())
            {
                settings = db.Settings.FirstOrDefault(s => s.OrganizationId == adminViewModel.OrganizationId);
            }

            SetSubjectTitle subjectTitle = Mapper.DynamicMap<AdminViewModel, SetSubjectTitle>(adminViewModel);

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
            AdminViewModel adminViewModel = GetAdminViewModel(name);
            SetSubjectTitle subjectTitle = Mapper.DynamicMap<AdminViewModel, SetSubjectTitle>(adminViewModel);
            subjectTitle.NewSubjectTitle = model.NewSubjectTitle;


            using (var db = new CboxContext())
            {
                Settings settings = db.Settings.FirstOrDefault(s => s.OrganizationId == adminViewModel.OrganizationId);

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
            AdminViewModel adminViewModel = GetAdminViewModel(User.Identity.Name);

            ListTopicViewModel model = Mapper.DynamicMap<AdminViewModel, ListTopicViewModel>(adminViewModel);


            List<Subject> subjects;

            using (var db = new CboxContext())
            {
                subjects = db.Subjects.Where(s => s.OrganizationId == adminViewModel.OrganizationId).ToList();
            }

            model.Topics = subjects.Select(s => new Topic
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
            AdminViewModel adminVm = GetAdminViewModel(name);
            NewTopicViewModel newTopic = Mapper.DynamicMap<AdminViewModel, NewTopicViewModel>(adminVm);

            newTopic.TopicTitle = "";
            newTopic.OrgId = adminVm.OrganizationId;

            return View(newTopic);
        }


        public ActionResult EditTopic(int id)
        {
            string name = User.Identity.Name;
            AdminViewModel adminVm = GetAdminViewModel(name);
            EditTopicViewModel topic = Mapper.DynamicMap<AdminViewModel, EditTopicViewModel>(adminVm);

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
                db.Subjects.Add(new Subject
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
                Organization org = db.Organization.Find(vm.Id);
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
            AdminViewModel adminVm = GetAdminViewModel(User.Identity.Name);
            OrganizationDetailViewModel model = Mapper.DynamicMap<AdminViewModel, OrganizationDetailViewModel>(adminVm);

            var db = new CboxContext();

            Organization organization = db.Organization.Find(adminVm.OrganizationId);


            model.Id = model.OrganizationId;
            model.OrganizationName = organization.FullName;
            model.PhoneNumber = organization.PhoneNumber;
            model.Address = organization.Address;


            return View(model);
        }


        private FeedbackListViewModel GetFeedbackList(string status, int? page, int? topicId = null,
                                                      DateTime? fromDate = null, DateTime? toDate = null)
        {
            int currentPageIndex = page.HasValue ? page.Value : 1;

            AdminViewModel viewModel = GetAdminViewModel(User.Identity.Name);
            FeedbackListViewModel model = Mapper.DynamicMap<AdminViewModel, FeedbackListViewModel>(viewModel);

            List<FeedBackViewModel> feedbacks;

            using (var db = new CboxContext())
            {
                IQueryable<Complaint> query = db.Complaints.Where(c => c.OrganizationId == viewModel.OrganizationId)
                                                .Include(c => c.Subject);

                if (status == "NEW")
                {
                    query = query.Where(c => c.Status == "NEW");
                }
                if (status == "RESOLVED")
                {
                    query = query.Where(c => c.Status == "RESOLVED");
                }

                if (fromDate != null && toDate != null)
                {
                    query = query.Where(c => c.ComplainDate >= fromDate && c.ComplainDate < toDate);
                }


                if (topicId != null && topicId > 0)
                {
                    query = query.Where(c => c.SubjectId == topicId);
                }


                feedbacks = query.Where(c => c.OrganizationId == viewModel.OrganizationId)
                                 .OrderBy(c => c.ComplainDate)
                                 .AsEnumerable()
                                 .Select(c => new FeedBackViewModel
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


            model.FeedBackViewModels = feedbacks.ToPagedList(currentPageIndex, PageSize);

            return model;
        }

        public ActionResult FeedbackList(int? page)
        {
            ViewBag.FeedbackListTitle = "Recent Feedback List";

            FeedbackListViewModel model = GetFeedbackList("ALL", page);
            return View(model);
        }


        public ActionResult TodaysFeedback(int? page)
        {
            ViewBag.FeedbackListTitle = "Today's Feedback List";

            DateTime? fromDate = DateTime.Today;
            DateTime? toDate = DateTime.Today.AddDays(1);

            FeedbackListViewModel model = GetFeedbackList("ALL", page, null, fromDate, toDate);
            return View("FeedbackList", model);
        }

        public ActionResult WeeksFeedback(int? page)
        {
            DateTime? fromDate = DateTime.Today.AddDays(-7);
            DateTime? toDate = DateTime.Today.AddDays(1);


            ViewBag.FeedbackListTitle = "Feedbaks of Last Seven Days";

            FeedbackListViewModel model = GetFeedbackList("ALL", page, null, fromDate, toDate);
            return View("FeedbackList", model);
        }


        public ActionResult MonthsFeedback(int? page)
        {
            int month = DateTime.Today.Month;
            int daysInMonth = DateTime.DaysInMonth(DateTime.Today.Year, month);

            DateTime? fromDate = new DateTime(DateTime.Today.Year, month, 1);
            DateTime? toDate = new DateTime(DateTime.Today.Year, month, daysInMonth);

            ViewBag.FeedbackListTitle = "Feedback List of " + _months[month - 1] + ", " + DateTime.Today.Year.ToString();
            FeedbackListViewModel model = GetFeedbackList("ALL", page, null, fromDate, toDate);
            return View("FeedbackList", model);
        }



         public ActionResult AdvancedFeedbackFilter()
         {
             AdminViewModel adminViewModel = GetAdminViewModel(User.Identity.Name);
             List<Subject> subjects;
             using (var db = new CboxContext())
             {
                 subjects = db.Subjects.Where(s => s.OrganizationId == adminViewModel.OrganizationId).ToList();
             }

             IEnumerable<SelectListItem> single = new[]
                {
                    new SelectListItem()
                        {
                            Value = "-1",
                            Text = "ALL"
                        }, 
                };

             IEnumerable<SelectListItem> subjectList = subjects.Select(
                 s => new SelectListItem
                 {
                     Text = s.Title,
                     Value = s.Id.ToString()
                 });



             ViewBag.Subjects = single.Union(subjectList);

             return View(adminViewModel);
         }

        
        public ActionResult AdvancedFeedbackList(AdvancedFilter filter, int? page)
        {

            ViewBag.TopicId = filter.TopicId;
            ViewBag.FromDate = filter.FromDate;
            ViewBag.ToDate = filter.ToDate;
            ViewBag.Status = filter.Status;


            if (filter.TopicId > 0)
            {
                Subject subject;
                using (var db = new CboxContext())
                {
                    subject = db.Subjects.Find(filter.TopicId);
                }
                ViewBag.Topic = subject.Title;
    
            }
            else
            {
                ViewBag.Topic = "ALL";
            }
            
            var model = GetFeedbackList(filter.Status, page, filter.TopicId, filter.FromDate, filter.ToDate);
            return View(model);
        }


        public ActionResult ResolvedFeedbackList(int? page)
        {
            FeedbackListViewModel model = GetFeedbackList("RESOLVED", page);
            return View(model);
        }

        public ActionResult NewFeedbackList(int? page)
        {
            FeedbackListViewModel model = GetFeedbackList("NEW", page);
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
                Complaint complaint = db.Complaints.Find(vm.FeedbackId);
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
                Organization organization = db.Organization.Find(id);
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