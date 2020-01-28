using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class InversionRecuperada
    {
        [Key]
        public virtual int InversionRecuperadaId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("MonederoId")]
        public virtual Monedero Monedero
        {
            get;
            set;
        }
        public virtual int MonederoId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("InversionId")]
        public virtual Inversion Inversion
        {
            get;
            set;
        }
        public virtual int InversionId
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Date)]
        public virtual DateTime FechaRecuperacion
        {
            get;
            set;
        }
        [Required]
        public virtual string Comentario
        {
            get;
            set;
        }
        [Required]
        public virtual float CantidadRecuperada
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            InversionRecuperada recuperada = obj as InversionRecuperada;
       

            if ((this.CantidadRecuperada == recuperada.CantidadRecuperada) && (this.Comentario == recuperada.Comentario)
               && (this.InversionId == recuperada.InversionId) && (this.InversionRecuperadaId == recuperada.InversionRecuperadaId))

                return true;
            return false;
        }


    }
}

