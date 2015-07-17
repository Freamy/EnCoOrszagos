using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;

using System.Web.Script.Serialization;

using EnCoOrszag.Models.DataAccess;
using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class AssaultController : Controller
    {
        // GET: Assault
        public ActionResult Index()
        {
            return RedirectToAction("Assault");
        }

        public ActionResult Assault()
        {
            Manager manager = new Manager();
            if (manager.IsLogedIn()) {
                AssaultViewModel vmA = manager.MakeAssaultViewModel();
                return View(vmA);
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult SendAssault()
        {
            
            
            AssaultData data = new JavaScriptSerializer().Deserialize<AssaultData>(Request.Form.Get(0));
            

            
            Manager manager = new Manager();
            string name = data.Name;
            int[] warriors = { int.Parse(data.Archers), int.Parse(data.Knights), int.Parse(data.Elites) };
            manager.BuildAssault(name, warriors);
            return RedirectToAction("Assault");
           // return new HttpStatusCodeResult(HttpStatusCode.OK, "Attack Sent");
        }

        public class AssaultData
        {
            public string Name {get; set;}
            public string Archers { get; set; }
            public string Knights { get; set; }
            public string Elites { get; set; }
        }
    }
}