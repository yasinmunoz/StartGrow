using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class TiposInversiones
    {
        [Key]
        public virtual int TiposInversionesId
        {
            get;
            set;
        }

        public virtual IList<Preferencias> Preferencias
        {
            get;
            set;
        }

        public IList<ProyectoTiposInversiones> ProyectoTiposInversiones
        {
            get;
            set;
        }

        public IList<Inversion> Inversiones
        {
            get;
            set;
        }

        [Required]
        public virtual string Nombre
        {
            get;
            set;
        }
    }
}
