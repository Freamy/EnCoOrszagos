using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Force
    {
        public int Id { get; set; }
        public int Size { get; set; }

        public virtual ICollection<UnitType> Type { get; set; }
    }
}