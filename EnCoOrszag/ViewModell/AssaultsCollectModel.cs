using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class AssaultsCollectModel
    {
        public Country Country { get; set; }
        public ICollection<Force> Forces { get; set; }
        
    }
}