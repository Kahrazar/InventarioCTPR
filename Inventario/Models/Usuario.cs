using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;

namespace Inventario.Models
{
    public class Usuario
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Nombre completo es requerido.")]
        public int Cedula { get; set; }
        
        [Required(ErrorMessage ="Nombre completo es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida.")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Rol de usuario es requerido.")]
        public string NivelDePrivilegio { get; set; }

    }
}