using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class Preferencias
    {
        [Key]
        public virtual int PreferenciasId
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
        public int TiposInversionesId
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
        [ForeignKey("AreasId")]
        public virtual Areas Areas
        {
            get;
            set;
        }
        public int AreasId
        {
            get;
            set;
        }

        [Required]
        [ForeignKey("InversorId")]
        public virtual ApplicationUser Inversor
        {
            get;
            set;
        }
        public virtual string InversorId
        {
            get;
            set;
        }

    }
}