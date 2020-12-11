using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventario.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inventario.ViewModels
{
    public class VerBienesViewModel
    {
       [RegularExpression(@"([A-Z]{3}\d{3})|(\d+)",ErrorMessage = "Formato Invalido")]
        public string numeroDePatrimonio { get; set; }

        public IEnumerable<Bienes> bienes { get; set; }
        public IEnumerable<Especialidad> especialidades { get; set; }
        public int IDEspecialidad { get; set; }
        public EstadosVMEnum estado { get; set; }

    }
}