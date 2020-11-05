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
        [Required]
        [StringLength(10)]
        public string IdCedula { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public string Nombre { get; set; }
    }
}