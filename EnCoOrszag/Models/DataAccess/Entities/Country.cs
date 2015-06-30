using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }


        public virtual ICollection<Group> StandingForces { get; set; }
        public virtual ICollection<Army> Armies { get; set; }

        public virtual ICollection<Construction> Construction { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Researching> Researching { get; set; }
        public virtual ICollection<Research> Researches { get; set; }

        public int Gold { get; set; }
        public int Potato { get; set; }

        public int Score { get; set; }
    }
}