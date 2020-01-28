using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.InversionRecuperadaViewModels
{
    public class InversionRecuperadaCreateViewModel
    {
        //Lista de Inversiones
        public virtual IList<Inversion> Inversiones
        {
            get;
            set;
        }

        public virtual string Name { get; set; }

        [Display(Name = "Primer Apellido")]
        public virtual string FirstSurname { get; set; }

        [Display(Name = "Segundo Apellido")]
        public virtual string SecondSurname { get; set; }

        //It will be necessary whenever we need a relationship with ApplicationUser or any child class
        public string InversorId { get; set; }

        public InversionRecuperadaCreateViewModel()
        {
            this.Inversiones = new List<Inversion>();
        }

        public virtual IList<InversionRecuperada> InversionesRecuperadas
        {
            get;
            set;
        }





    }
}
