using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Assault
    {
        public int Id { get; set; }

        public virtual ICollection<Force> Forces { get; set; }
        public virtual ICollection<Country> Origin { get; set; }
        public virtual Country Target { get; set; }
    }
}