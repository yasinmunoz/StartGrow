using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.InversionRecuperadaViewModels
{
    public class SelectInversionForRecuperarInversionViewModel
    {
        //Lista de Inversiones
        public IEnumerable<Inversion> Inversiones { get; set; }
       



        //Utilizado para filtrar por AREA
        public SelectList Areas;
        [Display(Name = "Areas")]
        public string inversionAreaSeleccionada { get; set; }
         
        //Utilizado para filtrar por ESTADO
        public SelectList Estados;
        [Display(Name = "Estados")]
        public string inversionEstadoSeleccionado { get; set; }


        //Utilizado para filtrar por TIPO
        public SelectList Tipos;
        [Display(Name = "Tipo")]
        public string inversionTipoSeleccionado { get; set; }


        //Utilizado para filtrar por RATING
        public SelectList Ratings;
        [Display(Name = "Rating")]
        public string inversionRatingSeleccionado { get; set; }
       
        
        
        //Utilizado para filtrar por el ID de la inversión
        [Display(Name = "ID")]
        public int idInv{ get; set; }
        
    }
}
