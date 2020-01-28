using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartGrow.Models
{
    public class Trabajador : ApplicationUser
    {
        [Required]
        public virtual string PuestoDeTrabajo
        {
            get;
            set;
        }

        public virtual IList<Solicitud> SolicitudesTratadas
        {
            get;
            set;
        }
    }
}
