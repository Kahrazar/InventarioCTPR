using Inventario.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Services
{
    public class BienesRepository
    {


        public Boolean facturaExistente(string factura)
        {
            Boolean existe = true;

            using (ApplicationDbContext db = new ApplicationDbContext())
                try
                {
                    Factura facturaExistente = db.Facturas.Find(factura);

                    if (facturaExistente == null)
                    {
                        existe = false;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            return existe;
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

       public  List<Bienes> obtenerTodosLosBienes()
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    return db.Bienes.Include(e => e.Especialidad).ToList();
                }
                catch (Exception)
                {

                    throw;
                }
        }

        public List<Bienes> obtenerBienesDeBaja()
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var bienes = db.Bienes
                    .Include(b => b.Especialidad)
                    .Where(b => b.condicion == CondicionesEnum.DeBaja)
                    .ToList();

                    return bienes;
                }
                catch (Exception)
                {

                    throw;
                }
        }

        public List<Bienes> obtenerBienesActivos()//Este metodo extrae solamente los bienes que tienen la condicion de activo
        {
            using (var db = new ApplicationDbContext())
            try
            {
                    db.Configuration.LazyLoadingEnabled = false;
                    var bienes = db.Bienes
                    .Include(b => b.Especialidad)
                    .Where(b => b.condicion == 0)
                    .ToList();
                    
                    return bienes;
                }
            catch (Exception)
            {

                throw;
            }
        }


        public Bienes buscarBien(string id)//Este metodo busca El bien mediante el id que recibe en el parametro
        {
            using(var db = new ApplicationDbContext())
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                Bienes bien = db.Bienes
                .Include(a => a.Especialidad)
                .Where(a => a.numeroDePatrimonio == id)
                .SingleOrDefault();
                return bien;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public void darDeBaja(Bienes bienesM)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                 
                        bienesM.condicion = CondicionesEnum.DeBaja;
                        db.Entry(bienesM).State = EntityState.Modified;
                        db.SaveChanges();
                    
                }
                catch (Exception)
                {

                    throw;
                }
        }


        public void actualizarBien(Bienes bien)//Este metodo actualiza los datos
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    db.Entry(bien).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
             
        }

        public void anadirBien(Bienes bienes)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    db.Bienes.Add(bienes);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
        }
    }
}