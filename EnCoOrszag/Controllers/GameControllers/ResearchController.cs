using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.ViewModell;
using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
     [Authorize]
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

        public ActionResult ResearchSomething(ResearchViewModel rvm)
        {

           TempData["Response"] = Manager.StartResearch(rvm.Id);

            return RedirectToAction("Research");
        }

	}
}