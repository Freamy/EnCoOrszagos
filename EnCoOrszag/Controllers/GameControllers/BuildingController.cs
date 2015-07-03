﻿using System;
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

            bool logedin = manager.isLogedIn();

            if (logedin) { 
                BuildingViewModel vmBuild = manager.makeBuildingViewModel();
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

            started = manager.startConstruction(submit);
            if (started)
            {
                ViewBag.Message = "Your construction is started.";
            }
            else
            {
                ViewBag.Message = "You can't make more then one building at the same time.";
            }
            
            return View("Response");
        }

        public ActionResult CheatBuilding()
        {
            Manager manager = new Manager();
            manager.finishConstruction();
            return RedirectToAction("Building");
        }
	}
}