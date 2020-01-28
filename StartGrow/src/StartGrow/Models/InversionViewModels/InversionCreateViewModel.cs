using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.InversionViewModels
{
    public class InversionesCreateViewModel
    {
        public InversionesCreateViewModel()
        {
            this.inversiones = new List<InversionCreateViewModel>();            
        }
        [Display(Name = "Nombre")]
        public virtual string Name
        {
            get;
            set;
        }
        [Display(Name = "Primer Apellido")]
        public virtual string FirstSurname
        {
            get;
            set;
        }
        [Display(Name = "Segundo Apellido")]
        public virtual string SecondSurname
        {
            get;
            set;
        }
        [Display(Name = "Cantidad en Monedero")]
        public virtual decimal Cantidad
        {
            get;
            set;
        }
        public virtual IList<InversionCreateViewModel> inversiones
        {
            get;
            set;
        }        
    }

    public class InversionCreateViewModel
    {
        public InversionCreateViewModel()
        {
            this.inversion = new Inversion();
        }

        public virtual string NombreProyecto
        {
            get;
            set;
        }

        public virtual float MinInver
        {
            get;
            set;
        }

        public virtual float Cuota
        {
            get;
            set;
        }
        public virtual int Plazo
        {
            get;
            set;
        }
        public virtual float Interes
        {
            get;
            set;
        }        

        public virtual String Rating
        {
            get;
            set;
        }

        public virtual Inversion inversion
        {
            get;
            set;
        }

        public virtual Proyecto Proyecto
        {
            get;
            set;
        }

        public virtual decimal Cantidad
        {
            get;
            set;
        }

        public virtual int ProyectoId
        {
            get;
            set;
        }

        
        public SelectList TiposInversion;
        //needed to store the genre selected by the user
        
        public string TiposInversionSelected { get; set; }


    }


}
