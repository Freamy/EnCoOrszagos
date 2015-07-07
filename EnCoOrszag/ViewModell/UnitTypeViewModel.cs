﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.ViewModell
{
    public class UnitTypeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Cost { get; set; }
        public int Upkeep { get; set; }
        public int Payment { get; set; }
    }
}