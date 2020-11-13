using Inventario.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void reasignarEspecialidad(int id)
        {
        
            using (var db = new ApplicationDbContext())
                try
                {
                    List<Bienes> bienes = db.Bienes.Where(b => b.IDEspecialidad == id).ToList();
                    if (bienes == null)
                    {

                    }
                    else
                    {
                        foreach (var item in bienes)
                        {
                            item.IDEspecialidad = 1;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                      
                       
                    }
                
                }
                catch (Exception)
                {

                    throw;
                }
        }

        public Especialidad buscarEspecialidad(int? id)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                   Especialidad especialidad = db.Especialidad.Find(id);
                    return especialidad;
                }
                catch (Exception)
                {

                    throw;
                }
                        
        }

        public void editarEspecialidad(Especialidad especialidad)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    db.Entry(especialidad).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
        }

        public void eliminarEspecialidad(int id)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    Especialidad espe = db.Especialidad.Find(id);
                    db.Especialidad.Remove(espe);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
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