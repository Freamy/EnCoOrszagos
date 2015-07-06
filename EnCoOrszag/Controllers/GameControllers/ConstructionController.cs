using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View("Construction");
        }

        public ActionResult CancelConstruction()
        {
            return RedirectToAction("Building", "Building");
        }
	}
}