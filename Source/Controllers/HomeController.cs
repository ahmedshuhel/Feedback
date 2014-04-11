using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ComplaintBox.Web.Models;

namespace ComplaintBox.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult SearchOrganizations()
        {
            return View();
        }

        public ActionResult OrganizationList()
        {

            IList<OrganizationViewModel> orgs;

            using (var db = new CboxContext())
            {
                orgs = db.Organization
                    .OrderBy(o => o.FullName)
                    .Take(10).Select(
                    o => new OrganizationViewModel()
                    {
                        Name = o.FullName, PhoneNumber = o.PhoneNumber
                    }).ToList();
            }


            return View(orgs);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            ViewBag.CustomerName = "Nazrana Mahin";
            ViewBag.Email = "najrana@ymail.com";
            return View();
        }
    }
}
