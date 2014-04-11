using System.Collections.Generic;
using System.Linq;
using ComplaintBox.Web.Models;
using System.Web.Mvc;

namespace ComplaintBox.Web.Controllers
{
    public class OrganizationController : Controller
    {

        [HttpGet]
        public ActionResult OrganizationDetails(int id)
        {
            return View(new OrganizationDetailViewModel()
                {
                    Id = id
                });
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
                    o => new OrganizationViewModel
                    {
                        Name = o.FullName,
                        PhoneNumber = o.PhoneNumber,
                        Address = o.Address,
                    }).ToList();
            }


            return View(orgs);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel vm)
        {
            var organization = new Organization()
            {
                UserName = vm.UserName,
                Password = vm.Password,
                EmailAddress = vm.Email,
            };

            using (var db = new CboxContext())
            {
                db.Organization.Add(organization);
                db.SaveChanges();
            }
            return RedirectToAction("OrganizationDetails", new {id = organization.Id});
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

            return RedirectToAction("OrganizationList");
        }
    }
}
