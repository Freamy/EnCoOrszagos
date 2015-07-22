using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;

using System.Web.Script.Serialization;

using EnCoOrszag.Models.DataAccess;
using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Controllers.GameControllers
{
    public class AssaultController : Controller
    {
        // GET: Assault
        public ActionResult Index()
        {
            return RedirectToAction("Assault");
        }

        public ActionResult Assault()
        {
            if (Manager.IsLogedIn()) {
                AssaultViewModel vmA = Manager.MakeAssaultViewModel();
                return View(vmA);
            }
            else
            {
                ViewBag.Message = "Please register a new user.";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult SendAssault()
        {

            //komment: model bindert használjunk, sose a request.form-ból!!
            AssaultData data = new JavaScriptSerializer().Deserialize<AssaultData>(Request.Form.Get(0));
            
            string name = data.Name;
            //komment: jujj, miért parsolgatunk? miért nem alapból int-ek a viewmodelben?
            // másik: az üzleti logika honnan fogja tudni, hogy milyen sorrendben érkeznek a darabszámok az egységekből?:)
            // mi van, ha lesz egy új egység? és ha valami miatt nem ilyen sorrendben kerülnek a tömbbe? semmi hibát nem kapok
            // a manager-ben, csak nem fog jól működni
            int[] warriors = { int.Parse(data.Archers), int.Parse(data.Knights), int.Parse(data.Elites) };
            Manager.BuildAssault(name, warriors);
            //komment: ez asszinkron módon van meghívva, vagyis a redirectnek nem lesz hatása.
            // ehelyett válaszolni kéne valamit json-ben a javascriptnek, hogy tudja, sikeres volt-e a kérés,
            // vagy valami hiba történt, és ha igen, akkor mi volt az
            return RedirectToAction("Assault");
           // return new HttpStatusCodeResult(HttpStatusCode.OK, "Attack Sent");
        }

        public class AssaultData
        {
            public string Name {get; set;}
            public string Archers { get; set; }
            public string Knights { get; set; }
            public string Elites { get; set; }
        }
    }
}