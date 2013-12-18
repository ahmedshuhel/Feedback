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
        public ActionResult RegisterOrganization(Organization Organization)
        {

            if (ModelState.IsValid)
            {
                using (var db = new CboxContext())
                {
                    db.Organization.Add(Organization);
                    db.SaveChanges();
                }

                return RedirectToAction("OrganizationList");
            }

            return View(Organization);
        }

        public ActionResult OrganizationList()
        {
            List<Organization> Organization;
            using (var db = new CboxContext())
            {
                Organization = db.Organization.ToList();
                db.SaveChanges();
            }
            return View(Organization);
        }





    }
}
