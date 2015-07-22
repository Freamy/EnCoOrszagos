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
            bool logedin = Manager.IsLogedIn();

            if (logedin) {
                if(TempData["Response"] != null)
                    ViewBag.Message = TempData["Response"].ToString();
                List<BuildingViewModel> vmBuild = Manager.MakeBuildingViewModel();
                return View("Build", vmBuild);
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult BuildSomething(BuildingViewModel bvm)
        {
            Manager manager = new Manager();

            bool started = false;
            started = Manager.StartConstruction(bvm.Id);
            if (started)
            {
                TempData["Response"] = "Your construction is started.";
                ViewBag.Message = "Your construction is started.";
            }
            else
            {
                TempData["Response"] = "You can't make more than " + Manager.MAX_PARALLEL_CONSTRUCTIONS + " buildings.";
            }
            return RedirectToAction("Building");
        }
	}
}