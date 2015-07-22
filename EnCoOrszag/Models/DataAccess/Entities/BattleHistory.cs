using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.Models.DataAccess.Entities
{
    public class BattleHistory
    {
        public int Id { get; set; }
        //komment: ne használjuk a nagybetűs stringet, ő egy osztályba csomagolt kisbetűs string, sokkal nagyobb footprinttel.
        // ez csak arra való, ha kollekcióban object ős helyén akarunk stringet tárolni, vagy más okból referenciatípusra van szükségünk 
        public string Attacker { get; set; }
        public string Defender { get; set; }
        public int WonPotato { get; set; }
        public int WonGold { get; set; }
        public string Result { get; set; }
        public string Losses { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Turn { get; set; }
    }
}