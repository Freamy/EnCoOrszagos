﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class ResearchingController : Controller
    {
        //
        // GET: /Researching/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Researching()
        {
            return View();
        }

        public ActionResult CurrentResearching()
        {
            Manager manager = new Manager();
            bool logedin = manager.isLogedIn();
            if (logedin)
            {
                if (TempData["Cancel"] != null)
                    ViewBag.Message = TempData["Cancel"].ToString();
                return View("Researching", manager.makeResearchingViewModel());
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult CancelResearch(ResearchingViewModel vmR)
        {
            Manager manager = new Manager();
            manager.cancelResearch(vmR.Id);
            TempData["Cancel"] = "Research cancelled.";
            ModelState.Clear();
            return RedirectToAction("CurrentResearching");
        }
	}
}