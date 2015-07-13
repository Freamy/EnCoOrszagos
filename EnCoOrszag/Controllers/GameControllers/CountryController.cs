using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;

using EnCoOrszag.Models;
using EnCoOrszag.Models.DataAccess.Entities;

using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.Entities
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/
        public ActionResult Index()
        {
            return View("Country");
        }

        //Main country screen
        public ActionResult Country()
        {
            Manager manager = new Manager();
            bool logedin = manager.isLogedIn();

            if (logedin) {
                if (TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
                return View(manager.makeCountryViewModel());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult EndTurn()
        {
            Manager manager = new Manager();
            TempData["Message"] = manager.endTurn();
            RedirectToAction("country");
            return RedirectToAction("Country");
        }

	}
}