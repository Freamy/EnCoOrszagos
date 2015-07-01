using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;

using EnCoOrszag.Models;

using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.Entities
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Country()
        {
            ViewBag.Message = "Your country page. You can view your country's information and reach the Build and Recruit options from here.";

            return View();
        }

        public ActionResult Test(CountryViewModel mvC)
        {
            using (var db = new ApplicationDbContext())
            {
                // Ezt kikell szervezni a data accesben egy manager osztalyba
               // db.Country.Add(new EnCoOrszag.Models.DataAccess.Entities.Country());
            }
            //Csak uzenet nem irja be az adatbazisba.
            ViewBag.Message = "You recived " + mvC.Gold + " gold.";
            return View("Country");
        }
	}
}