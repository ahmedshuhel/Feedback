using System.Collections.Generic;
using System.Linq;
using ComplaintBox.Web.Models;
using System.Web.Mvc;

namespace ComplaintBox.Web.Controllers
{
    public class OrganizationController : Controller
    {



        [HttpGet]
        public ActionResult SearchOrganizations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchOrganizations(string searchTerm)
        {

            IList<OrganizationViewModel> orgs;

            using (var db = new CboxContext())
            {
                orgs = db.Organization
                    .Where(o=> o.FullName.Contains(searchTerm))
                    .OrderBy(o => o.FullName)
                    .Take(10).Select(
                    o => new OrganizationViewModel
                    {
                        OrganizationId = o.Id,
                        Name = o.FullName,
                        PhoneNumber = o.PhoneNumber,
                        Address = o.Address,
                    }).ToList();
            }

            if (orgs.Count == 1)
            {
                var org = orgs.First();
                return RedirectToAction("Feedback", "Feedback", new {id = org.OrganizationId});
            }


            return View("OrganizationList", orgs);
        }

        public ActionResult OrganizationList()
        {

            IList<OrganizationViewModel> orgs;

            using (var db = new CboxContext())
            {
                orgs = db.Organization
                    .OrderBy(o => o.FullName)
                    .Take(10).Select(
                    o => new OrganizationViewModel
                    {
                        OrganizationId = o.Id,
                        Name = o.FullName,
                        PhoneNumber = o.PhoneNumber,
                        Address = o.Address,
                    }).ToList();
            }


            return View(orgs);
        }

    }
}
