using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.Models.DataAccess;

using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Controllers.GameControllers
{
     [Authorize]
    public class ArmyRecruitController : Controller
    {
        // GET: ArmyRecruit
        public ActionResult Index()
        {
            return View(Manager.MakeArmyRecruitViewModel());
        }

        public ActionResult ArmyRecruit()
        {
           if (TempData["Message"] != null)
           {
               ViewBag.Message = TempData["Message"].ToString();
           }
           return View(Manager.MakeArmyRecruitViewModel());
        }

        public ActionResult Recruit(ArmyRecruitViewModel vmAR)
        {
            string message =  Manager.RecruitTroops(vmAR.Id, vmAR.HAmount);
            TempData["Message"] = message;
            return RedirectToAction("ArmyRecruit");
        }
    }
}