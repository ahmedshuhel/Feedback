using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ComplaintBox.Web.Models;
using MvcPaging;

namespace ComplaintBox.Web.Controllers
{
    public class OrganizationController : Controller
    {
        [HttpGet]
        public ActionResult SearchOrganizations()
        {
            return View();
        }

        
        public ActionResult OrganizationSearch(string searchTerm, int? page)
        {
            IList<OrganizationViewModel> orgs;
            int currentPageIndex = page.HasValue ? page.Value : 1;

            using (var db = new CboxContext())
            {
                orgs = db.Organization
                         .Where(o => o.FullName.Contains(searchTerm))
                         .Where(o => o.Status == OrganizationStatus.Published)
                         .OrderBy(o => o.FullName)
                         .Select(o => new OrganizationViewModel
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

            ViewBag.searchTerm = searchTerm;

            return View("OrganizationSearchResult", orgs.ToPagedList(currentPageIndex, 10));
        }



        public ActionResult OrganizationList(int? page)
        {

            int currentPageIndex = page.HasValue ? page.Value : 1;

            IList<OrganizationViewModel> orgs;

            using (var db = new CboxContext())
            {
                orgs = db.Organization
                         .Where(o => o.Status == OrganizationStatus.Published)
                         .OrderBy(o => o.FullName)
                         .Take(100).Select(
                             o => new OrganizationViewModel
                                 {
                                     OrganizationId = o.Id,
                                     Name = o.FullName,
                                     PhoneNumber = o.PhoneNumber,
                                     Address = o.Address,
                                 }).ToList();
            }

            

            return View(orgs.ToPagedList(currentPageIndex, 10));
        }
    }
}