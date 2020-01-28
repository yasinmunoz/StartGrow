using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace StartGrow.Models
{
    public class ProyectoAreas
    {

        [Key]
        public virtual int ProyectoAreasId
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
        [ForeignKey("AreasId")]
        public virtual Areas Areas
        {
            get;
            set;
        }
        public virtual int AreasId
        {
            get;
            set;
        }
    }
}