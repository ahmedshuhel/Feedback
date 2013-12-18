using ComplaintBox.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComplaintBox.Web.Controllers
{
    public class SubjectController : Controller
    {

        [HttpGet]
        public ActionResult AddSubject(int id)
        {
            return View(new Subject() { OrganizationId = id});
          
        }
               
       

        [HttpPost]
        public ActionResult AddSubject(Subject subject)
        {
           
            using (var db = new CboxContext())
            {
                     
                db.Subjects.Add(subject);
               
                db.SaveChanges();
            }
            return RedirectToAction("ShowTitle");
           
            
        }

        public ActionResult ShowTitle()
        {
            List<Subject> Subject;
            using (var db = new CboxContext())
            {
                Subject = db.Subjects.ToList();
                db.SaveChanges();
            }


            return View(Subject);
        }

        public ActionResult EditTitle(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult EditNewTitle(int id,string newtitle)
        {
          
            using (var db = new CboxContext())
            {
                var sub = db.Subjects.Find(id);
                sub.Title = newtitle;
                db.SaveChanges();

            }

            return RedirectToAction("ShowTitle");
        }

        public ActionResult DeleteTitle(int id)
        {

            using (var db = new CboxContext())
            {
                var sub = db.Subjects.Find(id);
                db.Subjects.Remove(sub);
                db.SaveChanges();

            }
            return RedirectToAction("ShowTitle");
        }
        
    }
}
