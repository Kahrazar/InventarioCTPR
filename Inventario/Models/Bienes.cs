using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventario.Models
{
    public class Bienes
    {
        //Atributos
        [Key] [Required(ErrorMessage = "Campo Obligatorio")]
        [RegularExpression(@"([A-Z]{3}\d{3})|(\d+)", ErrorMessage = "No puede contener espacios ni simbolos.")]
        public string numeroDePatrimonio { get; set; }//llave primaria


        [Required(ErrorMessage = "Campo Obligatorio")]
        [RegularExpression(@"\d{8,10}", ErrorMessage = "Formato Incorrecto")]
        public string codigoDeBarras { get; set; }

        [StringLength(30)]
        public string descripcion { get; set; }

        public string anadidoPor { get; set; }//Llave foranea

        [RegularExpression(@"\d+", ErrorMessage = "Solo puedes digitar numeros")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Campo Obligatorio")]
        [ForeignKey("Factura")]
        public string numeroDeFactura { get; set; }//llave foranea
        public virtual Factura Factura{get;set;}

        [RegularExpression(@"\d{4,6}",ErrorMessage ="Formato Invalido")]
        public string ley { get; set; }

        [RegularExpression(@"[A-Za-z0-9]+",ErrorMessage= "Formato Invalido ")]
        [StringLength(10)]
        public string marca { get; set; }

        [StringLength(20)]
        public string modelo { get; set; }

        [RegularExpression(@"[a-zA-z0-9]+", ErrorMessage = "No puede contener espacions ni simbolos")]
        [StringLength(20)]
        public string serie { get; set; }
     
        [ForeignKey("Especialidad")]
        public int IDEspecialidad { get; set; }
        public virtual Especialidad Especialidad { get; set; }

        [StringLength(3)]
        [RegularExpression(@"[A-Z]-\d", ErrorMessage ="Formato invalido, digita una letra seguido de - y numeros")]
        public string ubicacion { get; set; }

        [Required(ErrorMessage = "Seleccion Obligatoria")]
        public EstadosEnum estado { get;  set; }

        [Required(ErrorMessage = "Seleccion Obligatoria")]
        public CondicionesEnum condicion { get;  set; }

        //Metodos constructores
        public Bienes()
        {
            numeroDePatrimonio = "CIF000";
            codigoDeBarras = "11111111";
            descripcion = "No especificado";
            anadidoPor = "Desconocido";
            numeroDeFactura = "No especificado";
            ley = "0000";
            marca = "Desconocido";
            modelo = "Desconocido";
            serie = "0000";
            IDEspecialidad= 1;
            ubicacion = "C-0";
            estado = estado = EstadosEnum.Excelente;
            condicion = condicion = CondicionesEnum.Activo;
        }
    }
}
