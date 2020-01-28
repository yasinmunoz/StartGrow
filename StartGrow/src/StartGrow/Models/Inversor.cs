using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class Inversor : ApplicationUser
    {
        public IList<Inversion> Inversiones
        {
            get;
            set;
        }

        public virtual IList<Preferencias> Preferencias
        {
            get;
            set;
        }

        public Monedero Monedero
        {
            get;
            set;
        }
    }
}