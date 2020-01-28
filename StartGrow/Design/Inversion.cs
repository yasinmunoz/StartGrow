using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class Inversion
    {
        [Key]
        public virtual int InversionId
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
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser
        {
            get;
            set;
        }
        public virtual string ApplicationUserId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("TipoInversionesId")]
        public virtual TiposInversiones TipoInversiones
        {
            get;
            set;
        }
        public virtual int TipoInversionesId
        {
            get;
            set;
        }

        [Required]
        public virtual float Cuota
        {
            get;
            set;
        }
        [Required]
        public virtual float Intereses
        {
            get;
            set;
        }
        [Required]
        public virtual float Total
        {
            get;
            set;
        }
    }
}