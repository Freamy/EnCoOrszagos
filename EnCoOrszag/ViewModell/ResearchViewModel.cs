using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.ViewModell
{
    public class ResearchViewModel
    {
        public ICollection<TechnologyViewModel> Technologies { get; set; }
    }
}