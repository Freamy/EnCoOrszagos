using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;

using EnCoOrszag.Models;

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

        public ActionResult Test(CountryViewModel mvC, String btnText)
        {
            //Csak uzenet nem irja be az adatbazisba.
            ViewBag.Message = "You recived " + mvC.Gold + " gold.";

            return View("Country");
        }
	}
}