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
        [Key]
        public string numeroDePatrimonio { get; set; }//llave primaria

        [Required]
        public string codigoDeBarras { get;  set; }

         [StringLength(30)]
        public string descripcion { get; set; }
       
        public string anadidoPor { get; set; }//Llave foranea

        [Required]
        public string numeroDeFactura { get;  set; }//llave foranea

        public string ley { get; set; }

        [StringLength(20)]
        public string marca { get; set; }

        [StringLength(20)]
        public string modelo { get; set; }

        [StringLength(20)]
        public string serie { get; set; }

    
        public int IDEspecialidad { get; set; }

        //LlaveForanea
        [ForeignKey("IDEspecialidad")]
        public virtual Especialidad Especialidad { get; set; }


        [StringLength(3)]
        public string ubicacion { get; set; }

        [Required]
        public EstadosEnum estado { get;  set; }

        [Required]
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
