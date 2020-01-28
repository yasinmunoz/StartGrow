using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.SolicitudViewModels
{
    public class SelectProyectosForSolicitudViewModel
    {
        public IEnumerable<Proyecto> proyectos { get; set; }

        //Para filtrar por tipos de inversion
        [Display(Name = "Tipo de Inversion")]
        public string[] tipoSeleccionado { get; set; }
        public IEnumerable<TiposInversiones> Tipos { get; set; }
        //Para filtrar por nombre del proyecto

        [Display (Name = "Nombre del proyecto")]
        public string nombreProyecto { get; set; }

        //Para filtrar por las Areas temáticas
        [Display(Name = "Areas Tematicas")]
        public string [] areasSeleccionada { get; set; }
        public IEnumerable<Areas> areas { get; set; }

        //Para filtrar por la fecha de expiración

        [Display(Name = "Fecha de Expiracion")]
        public string fecha { get; set; }

        //Para filtrar por el capital

        [Display(Name = "Capital")]
        public int capital { get; set; }
    }
}
