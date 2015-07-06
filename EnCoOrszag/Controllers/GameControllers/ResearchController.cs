using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class ResearchController : Controller
    {
        //
        // GET: /Research/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Research()
        {
            Manager manager = new Manager();

            if (TempData["Response"] != null)
                ViewBag.Message = TempData["Response"].ToString();

            List<ResearchViewModel> vmResearch = manager.makeResearchViewModel();
            return View(vmResearch);
        }

        public ActionResult ResearchSomething(string submit)
        {
            Manager manager = new Manager();


            bool started = false;

            started = manager.startResearch(submit);

            if (started)
            {
                TempData["Response"] = "Your research has started.";
            }
            else
            {
                TempData["Response"] = "You can't make more then " + manager.MAX_PARALLEL_RESEARCHES + " researches at the same time.";
            }

            return RedirectToAction("Research");
        }

        public ActionResult CheatResearch()
        {
            Manager manager = new Manager();
            //manager.finishResearch();
            return RedirectToAction("Research");

        }

	}
}