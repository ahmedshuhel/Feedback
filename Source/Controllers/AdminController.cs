using System.Web.Mvc;
using ComplaintBox.Web.Models;
using System.Linq;

namespace ComplaintBox.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {

            string name = User.Identity.Name;

            Organization org;
            int numberOfPendingTopics;
            int numberOfResolvedFeedbacks;
            using (var db = new CboxContext())
            {
                org = db.Organization.First(o => o.UserName == name);            
                numberOfPendingTopics = db.Complaints.Count(c => c.Status == "NEW" && c.OrganizationId == org.Id);
                numberOfResolvedFeedbacks = db.Complaints.Count(c => c.Status == "RESOLVED" && c.OrganizationId == org.Id);
            }

            var model = new AdminViewModel()
                {
                    UserName = string.IsNullOrEmpty(org.FullName) ? name : org.FullName,
                    NumberOfPendingTopics = numberOfPendingTopics,
                    NumbeOfResolvedFeedbacksToday = numberOfResolvedFeedbacks
                };

            return View(model);
        }
    }
}