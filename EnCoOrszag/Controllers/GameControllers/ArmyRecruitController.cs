﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.Models.DataAccess;

using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class ArmyRecruitController : Controller
    {
        // GET: ArmyRecruit
        public ActionResult Index()
        {
            return View(new Manager().MakeArmyRecruitViewModel());
        }

        public ActionResult ArmyRecruit()
        {
            Manager manager = new Manager();
            bool logedin = manager.IsLogedIn();
            if (logedin)
            {
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                }
                return View(manager.MakeArmyRecruitViewModel());
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Recruit(ArmyRecruitViewModel vmAR)
        {
            ModelState.Clear();
            string message =  new Manager().RecruitTroops(vmAR.Id, vmAR.hAmount);
            TempData["Message"] = message;
            return RedirectToAction("ArmyRecruit");
        }
    }
}