using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class BuildingViewModel
    {
        public ICollection<BlueprintsViewModel> Blueprints { get; set; }
        public ICollection<int> Buildings { get; set; }

    }
}