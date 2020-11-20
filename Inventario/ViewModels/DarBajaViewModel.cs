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
        public List<string> darBajas { get; set; }
    }
}