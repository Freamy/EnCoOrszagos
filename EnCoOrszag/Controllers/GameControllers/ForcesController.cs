using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class ForcesController : Controller
    {
        //
        // GET: /Forces/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Forces()
        {
            ViewBag.Message = "This is your standing forces in the city.";

            return View();
        }
	}
}