using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventario.Models;

namespace Inventario.Services
{
    //Esta clase se encarga de filtrar la lista que provienen de la vista "VerBienes"
    public class Filtrador
    {
        public List<Bienes> filtrarCondicion(int condicion){//El termino condicion hace referencia a si el objeto esta Activo o DeBaja en el inventario
            List<Bienes> bienesFiltrados = new List<Bienes>();
            BienesRepository repositorio = new BienesRepository();
            if (condicion == 0)//0 Equivale a "Activo"
            {
                bienesFiltrados = repositorio.obtenerBienesActivos();
            }
            else
            {
                bienesFiltrados = repositorio.obtenerBienesDeBaja();
            }
            return bienesFiltrados;
        }

        public List<Bienes> filtrarEstado(List<Bienes> bienesAfiltrar,int estadoAFiltrar)
        {
            List<Bienes> bienesEstado = new List<Bienes>();

            foreach (Bienes item in bienesAfiltrar)
            {
                if (estadoAFiltrar-1 == (int)item.estado)
                {
                    bienesEstado.Add(item);
                }
            }
            return bienesEstado;
        }


        public List<Bienes> filtrarEspecialidad(List<Bienes> bienesAfiltrar, int especialidad)
        {
            List<Bienes> bienesFiltrados = new List<Bienes>();
            foreach (Bienes item in bienesAfiltrar)
            {
                if (item.IDEspecialidad == especialidad)
                {
                    bienesFiltrados.Add(item);
                }
            }
            return bienesFiltrados;     
        }

    }
}