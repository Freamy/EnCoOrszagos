using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Research
    {
        public int Id { get; set; }
        public virtual Country Country { get; set; }
        public virtual Technology Technology { get; set; }
        public bool Finished { get; set; } // 0: Not done, n: How many times its repeated.
    }
}