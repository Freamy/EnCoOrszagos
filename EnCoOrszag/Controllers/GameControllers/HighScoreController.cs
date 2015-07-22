using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EnCoOrszag.Models.DataAccess;

namespace EnCoOrszag.Controllers.GameControllers
{
     [Authorize]
    public class HighScoreController : Controller
    {
        // GET: HighScore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Highscore()
        {
            return View("Highscore", Manager.MakeHighScoreViewModel());
        }
    }
}