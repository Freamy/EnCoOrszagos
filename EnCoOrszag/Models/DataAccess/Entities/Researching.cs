﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Researching
    {
        public int Id { get; set; }
        public virtual Country Country { get; set; }
        public virtual Technology Technology { get; set; }
        public int FinishTurn { get; set; }
    }
}