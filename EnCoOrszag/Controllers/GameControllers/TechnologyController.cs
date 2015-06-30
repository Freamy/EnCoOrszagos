using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class TechnologyController : Controller
    {
        //
        // GET: /Technology/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Technology()
        {

            ViewBag.Message("Admin area for adding new technologies to the database.");
            return View();
        }
	}
}