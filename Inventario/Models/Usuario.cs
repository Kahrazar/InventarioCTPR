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

            [Required(ErrorMessage = "Cedula completa es requerido.")]
            [StringLength(10)]
            public string Cedula { get; set; }

            [Required(ErrorMessage = "Nombre completo es requerido.")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Contraseña es requerida.")]
            [DataType(DataType.Password)]
            public string Contraseña { get; set; }

            [Display(Name = "Nivel de privilegio")]
            [Required(ErrorMessage = "Rol de usuario es requerido.")]
            public PrivilegiosEnum privilegio { get; set; }

        }

   
}