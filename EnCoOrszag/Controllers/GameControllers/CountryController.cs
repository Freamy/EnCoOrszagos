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
            return View();
        }

        public ActionResult Test(CountryViewModel mvC)
        {

            int o = 0;
            // kikell szervezni egy manager osztalyba
            /*
            using (var db = new ApplicationDbContext())
            {
                //db...
            }
            */
            //Csak uzenet nem irja be az adatbazisba.
            o = new Manager().getGoldAmount(mvC); // <- A manager fuggveny mar beirja az adatbazisba a gold valtozast.
            ViewBag.Message = "You recived " + mvC.Gold + " gold."+" "+o;
            return View("Country");
        }
	}
}