using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

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
            Manager manager = new Manager();
            if (TempData["Cancelled"] != null)
                ViewBag.Message = TempData["Cancelled"].ToString();
            return View("CurrentConstruction", manager.makeConstructionViewModel());
        }

        [HttpPost]
        public ActionResult CancelConstruction(string Cancel, ConstructionViewModel vmC)
        {
            Manager manager = new Manager();
            manager.cancelConstruction(vmC.Id);
            ModelState.Clear();
            TempData["Cancelled"] = "Construction cancelled.";
            return RedirectToAction("CurrentConstructions", "Construction");
        }
	}
}