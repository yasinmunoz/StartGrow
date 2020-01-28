using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StartGrow.Models
{
    public class ProyectoTiposInversiones
    {
        [Key]
        public virtual int ProyectoTiposInversionesId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("ProyectoId")]
        public virtual Proyecto Proyecto
        {
            get;
            set;
        }
        public virtual int ProyectoId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("TiposInversionesId")]
        public virtual TiposInversiones TiposInversiones
        {
            get;
            set;
        }
        public virtual int TiposInversionesId
        {
            get;
            set;
        }
    }
}