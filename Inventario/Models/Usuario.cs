using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario.Models
{ 
   
        public class Usuario
        {

            [Key]
            public int UserID { get; set; }

            [Display(Name = "Numero de cedula")]
            [Required(ErrorMessage = "Numero de cedula es requerido.")]
            [MaxLength(9)]
            [RegularExpression(@"\d{9}", ErrorMessage = "Numero de cedula debe tener 9 digitos")]
            public string Cedula { get; set; }
            
            [MaxLength(40)]
            [Display(Name = "Nombre Completo")]
            [Required(ErrorMessage = "Nombre completo es requerido.")]
            [RegularExpression(@"[A-Za-zñ _]+[A-Za-zñ _]+[A-Za-zñ _]", ErrorMessage = "Nombre debe contener solo letras")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Contraseña es requerida.")]
            [DataType(DataType.Password)]
            public string Contraseña { get; set; }

            [Display(Name = "Nivel de privilegio")]
            [Required(ErrorMessage = "Rol de usuario es requerido.")]
            public PrivilegiosEnum privilegio { get; set; }

            [Display(Name = "Confirmar contraseña")]
            [Required(ErrorMessage = "Contraseña es requerida.")]
            [DataType(DataType.Password)]
            [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmarContrasenia { get; set; }

        }

   
}