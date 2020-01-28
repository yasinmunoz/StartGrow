using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.PreferenciasViewModel
{
    public class SelectPreferenciasForInversorViewModel
    {

        public IEnumerable<TiposInversiones> TiposInversiones { get; set; }

        public IEnumerable<Areas> Areas { get; set; }

        public IEnumerable<Rating> Rating { get; set; }
    }
}
