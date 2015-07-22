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
    public class ResearchingController : Controller
    {
        //
        // GET: /Researching/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Researching()
        {
            return View();
        }

        public ActionResult CurrentResearching()
        {
                if (TempData["Cancel"] != null)
                    ViewBag.Message = TempData["Cancel"].ToString();
                return View("Researching", Manager.MakeResearchingViewModel());
        }

        public ActionResult CancelResearch(ResearchingViewModel vmR)
        {
            Manager manager = new Manager();
            Manager.CancelResearch(vmR.Id);
            TempData["Cancel"] = "Research cancelled.";
            ModelState.Clear();
            return RedirectToAction("CurrentResearching");
        }
	}
}