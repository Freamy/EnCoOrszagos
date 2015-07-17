using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;

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
            Manager manager = new Manager();
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Attack Sent");
        }
    }
}