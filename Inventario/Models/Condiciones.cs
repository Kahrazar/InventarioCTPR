using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventario.Models
{
    //Enum con las condiciones que puede tener un objeto 
    //Usado en los @Html.DropDownList de las vistas: Anadir, Actualizar Bienes

    public enum CondicionesEnum
    {
        Activo,
        DeBaja
    }
}