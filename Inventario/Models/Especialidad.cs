using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Models
{
    public class Especialidad
    {
        //Este Modelo representado la especialidad a la que esta asignado un Bien
        //Atributos
        [Key]
        public int ID { get; set; }//Llave Primaria
        [StringLength(20)]
        public string nombreEspecialidad { get; set; }
        public ICollection<Bienes> Bienes { get; set; }


        //Metodo Constructor
        public Especialidad()
        {
            ID = 00;
            nombreEspecialidad = "Ninguna";
        }
    }
}