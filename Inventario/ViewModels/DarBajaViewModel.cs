using Inventario.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario.ViewModels
{
    public class DarBajaViewModel
    {
        [Required(ErrorMessage ="Debes llenar este campo")]
        [RegularExpression(@"([A-Z]{3}\d{3})|(\d+)", ErrorMessage = "No puede contener espacios ni simbolos.")]
        public string numeroDePatrimonio { get; set; }

        public List<Bienes> bienes { get; set; } 
        public List<Especialidad> especialidad { get; set; }

        public int IDEspecialidad { get; set; }
        public EstadosVMEnum estado { get; set; }
    }
}