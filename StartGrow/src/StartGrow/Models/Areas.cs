using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class Areas
    {
        [Key]
        public virtual int AreasId
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

        public IList<ProyectoAreas> ProyectoAreas
        {
            get;
            set;
        }

        public virtual IList<Preferencias> Preferencias
        {
            get;
            set;
        }
    }
}