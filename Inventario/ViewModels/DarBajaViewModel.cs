using Inventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventario.ViewModels
{
    public class DarBajaViewModel
    {
        public List<Bienes> bienes { get; set; } 
        public List<Especialidad> especialidad { get; set; }

        public int IDEspecialidad { get; set; }
        public EstadosVMEnum estado { get; set; }
    }
}