using ComplaintBox.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintBox.Web.Controllers
{
    public class OrganizationController : Controller
    {


        [HttpGet]
        public ActionResult RegisterOrganization()
        {
            //string title;
            //using (var db = new CboxContext())
            //{
            //    var settings = db.Settings.FirstOrDefault();
            //    title = settings.SubjectTitle;
            //}

            //ViewBag.SubjectTitle = title;

            return View(new Organization());
        }

        [HttpPost]
        public ActionResult RegisterOrganization(Organization organization)
        {

            if (ModelState.IsValid)
            {
                using (var db = new CboxContext())
                {
                    db.Organization.Add(organization);
                    db.SaveChanges();
                }

                return RedirectToAction("OrganizationList");
            }

            return View(organization);
        }

        public ActionResult OrganizationList()
        {
            List<Organization> orgs;
            using (var db = new CboxContext())
            {
                orgs = db.Organization.ToList();                
            }
            return View(orgs);
        }



//         public ActionResult ShowOrglists(int id)
//        {
//            
//            var viewModel = new ShowOrglists();
//            
//            using (var db = new CboxContext())
//            {
//
//               var org = db.Organization.Find(id);
//               viewModel.Organization = org.FullName;
//               viewModel.Orglists = db.Organization
//                .Where(c=> c.Id == id)
//                 .select(c => new Orglists()
//                   {
//                       Orgname = c.FullName,
//                       phoneNumber = c.PhoneNumber,
//                       
//                   }).ToList();
//                  
//                  
//            }
//
//            return View(viewModel);
//        }

    }




    
}
