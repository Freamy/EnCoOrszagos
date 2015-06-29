using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{

    public class Global
    {
        public int RoundNumber { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
    }

    public class Country
    {
        public virtual ICollection<Army> Armies { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Research> Researches { get; set; }
        public int Score { get; set; }
    }
}