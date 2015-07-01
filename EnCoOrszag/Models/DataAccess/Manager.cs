using EnCoOrszag.Models.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Models.DataAccess
{
    public class Manager
    {
        public int getGoldAmount(CountryViewModel mvC)
        {
            int g = 0;
            using (var db = new ApplicationDbContext())
            {
                Country c = db.Countries.Find(1);
                c.Gold += mvC.Gold;
                db.SaveChanges();
            }
            return g;
        }
    }
}