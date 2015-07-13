using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            Manager manager = new Manager();
            AssaultViewModel vmA = manager.makeAssaultViewModel();
            return View(vmA);
        }
    }
}