using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.ViewModell
{
    public class BlueprintsViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Repeatable { get; set; }
        public int BuildTime { get; set; }
        public int Cost { get; set; }
        public int Score { get; set; }

        public int NoOfFinishedBlueprints { get; set; }
        public int NoOfTurnsLeft { get; set; }
    }
}