using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventario.Models;
using System.Web.Mvc;

namespace Inventario.ViewModels
{
    public class VerBienesViewModel
    {
        public IEnumerable<Bienes> bienes { get; set; }

        public IEnumerable<Especialidad> especialidades { get; set; }
        public int IDEspecialidad { get; set; }
    }
}