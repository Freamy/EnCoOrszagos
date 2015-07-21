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
            bool logedin = Manager.IsLogedIn();

            if (logedin)
            {

                if (TempData["Response"] != null)
                    ViewBag.Message = TempData["Response"].ToString();

                List<ResearchViewModel> vmResearch = Manager.MakeResearchViewModel();
                return View(vmResearch);
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult ResearchSomething(string submit)
        {
            Manager manager = new Manager();

            TempData["Response"] = Manager.StartResearch(submit);

            return RedirectToAction("Research");
        }

	}
}