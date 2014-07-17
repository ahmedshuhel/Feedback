using ComplaintBox.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ComplaintBox.Web.Controllers
{
    public class SettingController : Controller
    {
        [HttpGet]
        public ActionResult Setting(int id)
        {
           
            return View(new Settings() { OrganizationId = id });        
        }

        [HttpPost]
        public ActionResult Setting(Settings setting)
        {
            
            using (var db = new CboxContext())
            {
                var set = db.Settings.FirstOrDefault(s=>s.OrganizationId==setting.OrganizationId);

                if (set == null)
                {
                    db.Settings.Add(setting);
                }
                else 
                {
                    set.SubjectTitle = setting.SubjectTitle;
                }

                db.SaveChanges();
            }
         

            return RedirectToAction("ShowSubject");

        }
        public ActionResult ShowSubject()
        {
            List<Settings> sub;

            using (var db = new CboxContext())
            {
                sub = db.Settings.ToList();
                db.SaveChanges();
            }

            return View(sub);
        }

    }
}
