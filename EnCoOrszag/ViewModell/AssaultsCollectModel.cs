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
        //komment: itt most kiadunk egy listát a db-entitásokról, úgyhogy egy panda valahol meghalt
        public ICollection<Force> Forces { get; set; }
        
    }
}