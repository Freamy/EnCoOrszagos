using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class BattleHistory
    {
        public int Id { get; set; }
        public String Attacker { get; set; }
        public String Defender { get; set; }
        public int WonPotato { get; set; }
        public int WonGold { get; set; }
        public String Result { get; set; }
        public String Losses { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Turn { get; set; }
    }
}