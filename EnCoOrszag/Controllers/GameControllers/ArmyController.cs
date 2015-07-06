using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class ArmyController : Controller
    {
        //
        // GET: /Army/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Army()
        {

            return View();
        }

        public ActionResult Recruit()
        {
            return View("Recruit");
        }
	}
}