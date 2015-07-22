using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.ViewModell
{
    public class ResearchViewModel
    {
        //public ICollection<TechnologyViewModel> Technologies { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Researched { get; set; }
    }
}