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

            ResearchViewModel vmResearch = manager.makeResearchViewModel();
            return View(vmResearch);
        }

        public ActionResult ResearchSomething(string submit)
        {
            Manager manager = new Manager();


            bool started = false;

            started = manager.startResearch(submit);

            if (started)
            {
                ViewBag.Message = "Your research has started.";
            }
            else
            {
                ViewBag.Message = "You can't make more then one research";
            }

            return View("Response");
        }

        public ActionResult CheatResearch()
        {
            Manager manager = new Manager();
            manager.finishResearch();
            return RedirectToAction("Research");

        }
	}
}