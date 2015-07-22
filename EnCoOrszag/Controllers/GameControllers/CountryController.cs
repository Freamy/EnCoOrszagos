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
            bool logedin = Manager.IsLogedIn();

            if (logedin) {
                return View(Manager.MakeCountryViewModel());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult EndTurn()
        {
            Manager.EndTurn();
            RedirectToAction("country"); //komment: ?:))
            return RedirectToAction("Country");
        }

	}
}