using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class ArmyViewModel
    {
        public virtual Country Origin { get; set; }
        public virtual Country TargetCountry { get; set; }
    }
}