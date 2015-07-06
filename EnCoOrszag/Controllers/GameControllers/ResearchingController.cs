using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
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
            Manager manager = new Manager();
            return View("Researching", manager.makeResearchingViewModel());
        }

        public ActionResult CancelResearch(ResearchingViewModel vmR)
        {
            Manager manager = new Manager();
            manager.cancelResearch(vmR.Id);
            ViewBag.Message = vmR.Id;
            ModelState.Clear();
            return View("Researching", manager.makeResearchingViewModel());
            //return RedirectToAction("Researching");
        }
	}
}