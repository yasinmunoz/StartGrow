using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class Proyecto
    {
        [Key]
        public virtual int ProyectoId
        {
            get;
            set;
        }

        public IList<Inversion> Inversiones
        {
            get;
            set;
        }

        public IList<ProyectoAreas> ProyectoAreas
        {
            get;
            set;
        }

        public IList<ProyectoTiposInversiones> ProyectoTiposInversiones
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("RatingId")]
        public virtual Rating Rating
        {
            get;
            set;
        }
        public int RatingId
        {
            get;
            set;
        }

        [Required]
        public virtual String Nombre
        {
            get;
            set;
        }

        [Required]
        public virtual float MinInversion
        {
            get;
            set;
        }

        [Required]
        public virtual int Plazo
        {
            get;
            set;
        }

        [Required]
        public virtual float Interes
        {
            get;
            set;
        }

        [Required]
        public virtual float Importe
        {
            get;
            set;
        }

        [Required]
        public virtual int Progreso
        {
            get;
            set;
        }

        [Required]
        public virtual int NumInversores
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha expiración")]
        public virtual DateTime FechaExpiracion
        {
            get;
            set;
        }
    }
}
