using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StartGrow.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El Nombre no puede tener mas de 20 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Nombre
        {
            get;
            set;
        }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El primer Apellido no puede tener mas de 20 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Apellido1
        {
            get;
            set;
        }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El segundo Apellido no puede tener mas de 20 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Apellido2
        {
            get;
            set;
        }

        [Required]
        [StringLength(8, ErrorMessage = "El NIF no puede tener mas de 8 caracteres")]
        public virtual int NIF
        {
            get;
            set;
        }
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "La Nacionalidad no puede tener mas de 40 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Nacionalidad
        {
            get;
            set;
        }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "El Pais de Residencia no puede tener mas de 40 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string PaisDeResidencia
        {
            get;
            set;
        }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "La Provincia no puede tener mas de 40 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Provincia
        {
            get;
            set;
        }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "El Municipio no puede tener mas de 40 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Municipio
        {
            get;
            set;
        }

        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "El Domicilio no puede tener mas de 40 caracteres")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z'']*$")]
        public virtual string Domicilio
        {
            get;
            set;
        }

        [Required]
        [StringLength(5, ErrorMessage = "El Codigo Postal no puede tener mas de 5 caracteres")]
        public virtual int CodPost
        {
            get;
            set;
        }
    }
}
