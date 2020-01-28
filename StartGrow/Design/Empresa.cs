using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartGrow.Models
{
    public class Empresa : Inversor
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "La Denominacion Social no puede tener mas de 50 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string DenominacionSocial
        {
            get;
            set;
        }

        [Required]
        [Display(Name = "Fecha de constitución")]
        [DataType(DataType.Date)]
        public virtual DateTime FechaDeConstitucion
        {
            get;
            set;
        }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La Actividad no puede tener mas de 100 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Actividad
        {
            get;
            set;
        }

        [Required]
        [StringLength(9, ErrorMessage = "El CIF no puede tener mas de 9 caracteres")]
        public virtual int CIF
        {
            get;
            set;
        }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "El Pais del Domicilio Social no puede tener mas de 25 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string PaisDelDomicilioSocial
        {
            get;
            set;
        }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "La Provincia del Domicilio Social no puede tener mas de 25 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string ProvinciaDelDomicilioSocial
        {
            get;
            set;
        }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "El Municipio del Domicilio Social no puede tener mas de 25 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string MunucipioDelDomicilioSocial
        {
            get;
            set;
        }

        [Required]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "El Domicilio Social no puede tener mas de 25 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string DomicilioSocial
        {
            get;
            set;
        }
    }
}