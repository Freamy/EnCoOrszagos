using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class Technologies
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public int Cost { get; set; }
        public int ResearchTime { get; set; }
        public bool Repeatable { get; set; }
        public int Cost { get; set; }
        public int Score { get; set; }
    }

    public class Research
    {
        public Country Country { get; set; }
        public Technologies Technology { get; set; }
        public int Finished { get; set; } // 0: Not done, n: How many times its repeated.
        public int FinishRound { get; set; }
    }
}