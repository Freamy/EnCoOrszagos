using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class BuildingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Repeatable { get; set; }
        public int BuildTime { get; set; }
        public int Cost { get; set; }
        public int Score { get; set; }

        public int NoOfFinishedBlueprints { get; set; }
    }
}