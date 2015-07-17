using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.Models.DataAccess;
using EnCoOrszag.Models.DataAccess.Entities;
using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class BuildingController : Controller
    {
        //
        // GET: /Building/
        public ActionResult Index()
        {
            return View("Build");
        }

        public ActionResult Building()
        {
            Manager manager = new Manager();

            bool logedin = manager.IsLogedIn();

            if (logedin) {
                if(TempData["Response"] != null)
                    ViewBag.Message = TempData["Response"].ToString();
                List<BuildingViewModel> vmBuild = manager.MakeBuildingViewModel();
                return View("Build", vmBuild);
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }


        public ActionResult BuildSomething(string submit)
        {
            Manager manager = new Manager();

            bool started = false;

            started = manager.StartConstruction(submit);
            if (started)
            {
                TempData["Response"] = "Your construction is started.";
                ViewBag.Message = "Your construction is started.";
            }
            else
            {
                TempData["Response"] = "You can't make more than " + manager.MAX_PARALLEL_CONSTRUCTIONS + " buildings.";
            }
            return RedirectToAction("Building");
        }

        public ActionResult CheatBuilding()
        {
            Manager manager = new Manager();
            return RedirectToAction("Building");
        }
	}
}