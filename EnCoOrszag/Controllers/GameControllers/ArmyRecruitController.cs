using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.Models.DataAccess;

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
            return View(new Manager().makeArmyRecruitViewModel());
        }
    }
}