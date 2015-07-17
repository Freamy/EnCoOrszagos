using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class ConstructionController : Controller
    {
        //
        // GET: /Construction/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CurrentConstructions()
        {
            Manager manager = new Manager();
            bool logedin = manager.IsLogedIn();
            if (logedin) {
                if (TempData["Cancelled"] != null) 
                    ViewBag.Message = TempData["Cancelled"].ToString();
                return View("CurrentConstruction", manager.MakeConstructionViewModel());
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult CancelConstruction(string Cancel, ConstructionViewModel vmC)
        {
            Manager manager = new Manager();
            
            manager.CancelConstruction(vmC.Id);
            ModelState.Clear();
            TempData["Cancelled"] = "Construction cancelled.";
            return RedirectToAction("CurrentConstructions", "Construction");
        }
	}
}