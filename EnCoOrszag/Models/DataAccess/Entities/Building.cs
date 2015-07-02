using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public virtual Country Country { get; set; }
        public virtual Blueprints Blueprint { get; set; }
        public int NumberOfBuildings { get; set; }
    }
}