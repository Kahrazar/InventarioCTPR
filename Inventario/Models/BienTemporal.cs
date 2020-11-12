using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventario.Models
{
    public class BienTemporal
    {
        [Key]
        public string nPatrimonioTemp { get; set; }
    }
}