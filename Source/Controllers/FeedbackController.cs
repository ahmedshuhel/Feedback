using System;
using System.Web.Mvc;
using ComplaintBox.Web.Models;

namespace ComplaintBox.Web.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public ActionResult Feedback(int id)
        {
            return View(new FeedbackViewModel()
                {
                    OrganizationId = id
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
                        Status = "NEW"
                    };

                db.Complaints.Add(complaint);
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