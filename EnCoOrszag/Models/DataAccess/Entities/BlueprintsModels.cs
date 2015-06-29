using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Blueprints
    {
        public int Id { get; set; }
        public bool Repeatable { get; set; }
        public int BuildTime { get; set; }
        public int Cost { get; set; }
        public int Score { get; set; }
    }
}