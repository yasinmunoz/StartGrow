using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.PreferenciasViewModel
{
    public class SelectedPreferenciasForInversor
    {
        public string[] IdsToAddAreas { get; set; }
        public string[] IdsToAddTiposInversion { get; set; }
        public string[] IdsToAddRating { get; set; }
    }
}