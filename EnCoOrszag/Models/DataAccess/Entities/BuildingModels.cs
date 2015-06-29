using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{

    public class Blueprints
    {
        public bool Repeatable { get; set; }
        public int BuildTime { get; set; }
        public int Cost { get; set; }
        public int Score { get; set; }
    }

    public class Building
    {
        public Country Country { get; set; }
        public Blueprints Blueprint { get; set; }
        public int FinishRound { get; set; }
        public int NumberOfBuildings { get; set; }
    }
}