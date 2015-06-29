using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Forces
    {
        public int Id { get; set; }
        public virtual Country Country { get; set; }
        public virtual Group Group { get; set; }
    }
}