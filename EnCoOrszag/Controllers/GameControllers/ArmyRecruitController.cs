using System;
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
            return View(new Manager().makeArmyRecruitViewModel());
        }

        public ActionResult ArmyRecruit()
        {
            if (TempData["ID"] != null)
            {
                ViewBag.Message = TempData["ID"].ToString() +" "+ TempData["Amount"].ToString();
            }
            return View(new Manager().makeArmyRecruitViewModel());
        }

        public ActionResult Recruit(ArmyRecruitViewModel vmAR)
        {
            TempData["ID"] = vmAR.Id;
            TempData["Amount"] = vmAR.hAmount;
            ModelState.Clear();
            new Manager().recruitTroops(vmAR.Id, vmAR.hAmount);
          //  return View("ArmyRecruit", new Manager().makeArmyRecruitViewModel());
            return RedirectToAction("ArmyRecruit");
        }
    }
}