using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnCoOrszag.ViewModell
{
    public class ArmyRecruitViewModel
    {
        //komment: kommentek. nem csak itt, mindenhol, pl(vagy valami hasonló, nemtom pontosan ez mire jó):

        /// <summary>
        /// Ennyi krumpli a fenntartása az adott egységnek
        /// </summary>
        public int Potato { get; set; }
        public int Gold { get; set; }
        public List<UnitTypeViewModel> Types { get; set; }

        public int AllSpace { get; set; }
        public int OccupiedSpace { get; set; }

        //komment: a "h" nem illik az elejére :\ death for camelCase
        public int hAmount { get; set; }

        public int Id { get; set; }
    }
}