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
        [ForeignKey("InversorId")]
        public virtual Inversor Inversor
        {
            get;
            set;
        }
        public virtual string InversorId
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
        public virtual string EstadosInversiones { get; set; }

        [Required]
        public virtual float Total
        {
            get;
            set;
        }

        public override bool Equals(Object obj)
        {

            Inversion inversion = obj as Inversion;
            //if ((this.Cuota == inversion.Cuota) && (this.Intereses == inversion.Intereses)
            //    && (this.InversionId == inversion.InversionId) && (this.ProyectoId == inversion.ProyectoId) && (this.TipoInversionesId == inversion.TipoInversionesId)
            //    && (this.Total == inversion.Total))

            if ((this.Cuota == inversion.Cuota) && (this.EstadosInversiones == inversion.EstadosInversiones)
               && (this.InversionId == inversion.InversionId) && (this.Total == inversion.Total))

                    return true;
            return false;
        }
    }
}