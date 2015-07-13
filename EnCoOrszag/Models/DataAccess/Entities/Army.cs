using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Army
    {
        public int Id { get; set; }

        public virtual UnitType Type { get; set; }
        public int Size { get; set; }
    }
}