using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class UnitTypes
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Cost { get; set; }
        public int Upkeep { get; set; }
        public int Score { get; set; }
    }

    public class Group
    {
        public UnitTypes UnitType { get; set;}
        public int Size { get; set; }
    }

    public class Army
    {
        public Country Country { get; set; }
        public Country TargetCountry { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}