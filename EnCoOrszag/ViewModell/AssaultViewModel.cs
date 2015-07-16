using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class AssaultViewModel
    {
        public virtual ICollection<Country> Countries { get; set; }
        public virtual ICollection<Army> Armies { get; set; }

        public virtual ICollection<AssaultsCollectModel> Assaults { get; set; }
    }
}