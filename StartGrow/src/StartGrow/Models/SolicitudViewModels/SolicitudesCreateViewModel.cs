 using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models.SolicitudViewModels
{
    public class SolicitudesCreateViewModel
    {

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

        public virtual IList<SolicitudCreateViewModel> Solicitudes
        {
            get;
            set;
        }


        public SolicitudesCreateViewModel()
        {
            this.Solicitudes = new List<SolicitudCreateViewModel>();
        }

    }
    public class SolicitudCreateViewModel
    {

        

        public String rating
        {
            get;
            set;
        }
        public double? interes
        {
            get;
            set;
        }
        public int? plazo
        {
            get;
            set;
        }

        public Estados estados
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        public DateTime FechaSolicitud
        {
            get;
            set;
        }

        public virtual Solicitud solicitud
        {
            get;
            set;
        }


        public SolicitudCreateViewModel()
        {
            this.solicitud = new Solicitud();
        }

    }
}