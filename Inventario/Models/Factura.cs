using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario.Models
{
    public class Factura
    {
     [Key][Required][RegularExpression(@"\d+", ErrorMessage = "Solo puede contener numeros")][MaxLength(20)]
     public string numeroDeFactura { get; set; }

     public string urlDeFactura { get; set; }

    }
}