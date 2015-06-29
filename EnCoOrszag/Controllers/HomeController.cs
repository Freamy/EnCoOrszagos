using EnCoOrszag.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace EnCoOrszag.Controllers
{
    public class HomeController : Controller
    {
        private readonly Orszag Orszag;

        public HomeController(Orszag orszag)
        {
            Orszag = orszag;
        }

        public async Task<ActionResult> Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var asd = db.Orszagok.Where(o => o.Name.Contains("a"));
                asd = asd.Where(o => o.Name.StartsWith("e"));

                var result = asd.ToList();

                foreach (var item in asd)
                {

                }
                foreach (var item in result)
                {

                }

                var orszag = await db.Orszagok.Include(o => o.Seregek).SingleOrDefaultAsync(o => o.Id == 3);


                var list = new List<SelectListItem>();
                foreach (var item in orszag.Seregek)
                {
                    list.Add(new SelectListItem
                        {
                            Value = item.Orszag.ToString()
                        });
                }

                list = orszag.Seregek.Select(o => new SelectListItem
                {
                    Value = o.Orszag.ToString()
                }).ToList();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact(int erer)
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}