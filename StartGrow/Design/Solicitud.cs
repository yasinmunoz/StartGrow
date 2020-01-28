using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartGrow.Models
{
    public class Solicitud
    {
        [Key]
        public virtual int SolicitudId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("TrabajadorId")]
        public virtual Trabajador Trabajador
        {
            get;
            set;
        }
        public virtual string TrabajadorId
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
        public virtual int Estado
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [Required]
        public virtual DateTime FechaSolicitud
        {
            get;
            set;
        }
    }

}

