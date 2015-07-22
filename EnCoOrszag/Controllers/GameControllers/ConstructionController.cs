using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
    [Authorize]
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

                if (TempData["Cancelled"] != null) 
                    ViewBag.Message = TempData["Cancelled"].ToString();
                return View("CurrentConstruction", Manager.MakeConstructionViewModel());
 
        }

        [HttpPost]
        public ActionResult CancelConstruction(string Cancel, ConstructionViewModel vmC)
        {           
            Manager.CancelConstruction(vmC.Id);
            TempData["Cancelled"] = "Construction cancelled.";
            return RedirectToAction("CurrentConstructions", "Construction");
        }
	}
}