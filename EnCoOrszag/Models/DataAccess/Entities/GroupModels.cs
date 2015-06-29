using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public virtual UnitTypes UnitType { get; set; }
        public int Size { get; set; }
    }
}