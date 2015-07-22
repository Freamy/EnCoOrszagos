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
                //komment: ha tempdatát szeretnénk használni üzenetküldésre, készítsünk hozzá wrapper osztályt, ami elfedi magát a tempdata
                // használatát, mivel ez egy string-es dictionary gyakorlatilag, nagyon hamar elburjánzhat, hogy milyen key-el mi van, 
                // mit hol kell visszaolvasni, stb. stb.
                // másik: ezt nagyon feleslegesnek érzem. ha már mindenképp tempdatát bányászunk, akkor miért tesszük át külön a viewbag-be?
                // a Tempdata ugyanúgy elérhető a cshtml-ekben is.
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
            Manager manager = new Manager();//komment: ez sem kell ugye már a staticok miatt

            TempData["Response"] = Manager.StartResearch(submit);

            return RedirectToAction("Research");
        }

	}
}