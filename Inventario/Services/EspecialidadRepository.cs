using Inventario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventario.Services
{
    public class EspecialidadRepository
    {
        public int generarID()
        {
            int id;
            List<Especialidad> lista = obtenerEspecialidades();

            id = lista.Count;
            id = id + 2;
            return id;
        }

        public void anadirEspecialidad(Especialidad especialidad)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    db.Especialidad.Add(especialidad);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
        }
        public List<Especialidad> obtenerEspecialidades()//Este metodo extrae las especialiadades de la base de datos
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    return db.Especialidad.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
        }
    }
}