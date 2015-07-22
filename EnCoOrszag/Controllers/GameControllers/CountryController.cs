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
     [Authorize]
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
            return View(Manager.MakeCountryViewModel());
        }

        public ActionResult EndTurn()
        {
            Manager.EndTurn();
            return RedirectToAction("Country");
        }

	}
}