using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Army
    {
        public int Id { get; set; }
        public virtual Country Country { get; set; }
        public virtual Country TargetCountry { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}