using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.ViewModell
{
    public class ArmyRecruitViewModel
    {
        public int Potato { get; set; }
        public int Gold { get; set; }
        public List<UnitTypeViewModel> Types { get; set; }

        public int hAmount { get; set; }
    }
}