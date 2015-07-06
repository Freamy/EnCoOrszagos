using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EnCoOrszag.Models;
using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.ViewModell
{
    public class CountryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Gold { get; set; }
        public int Potato { get; set; }
        
        public int Score { get; set; }
    }
}