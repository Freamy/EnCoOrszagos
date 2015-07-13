using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class AssaultsCollectModel
    {
        public virtual ICollection<Assault> Assault { get; set; }
        public virtual ICollection<Force> Forces { get; set; }
    }
}