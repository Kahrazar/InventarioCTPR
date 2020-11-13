using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventario.Models;
using Inventario.Services;
using Inventario.ViewModels;

namespace Inventario.Controllers
{
    public class BienesController : Controller
    {
        //Objetos de uso
        private Filtrador filtro = new Filtrador();
        private ApplicationDbContext db = new ApplicationDbContext();
        private BienesRepository repositorio = new BienesRepository();//Objeto que proporcionara los datos provenientes de la BD
        private TempRepository tempRepositorio = new TempRepository();

        //Acciones para la vista ver bienes

        [ValidateAntiForgeryToken]
        public ActionResult Filtrado(CondicionesEnum condicion, VerBienesViewModel vm)
        {
            VerBienesViewModel modelo = new VerBienesViewModel();

            List<Bienes> bienesFiltrados = new List<Bienes>();
            List<Bienes> listaPrueba = new List<Bienes>();

            bienesFiltrados = filtro.filtrarCondicion((int)condicion);

            if (vm.IDEspecialidad == 0)
            {

            }
            else
            {
                bienesFiltrados = filtro.filtrarEspecialidad(bienesFiltrados,vm.IDEspecialidad);
            }
                  

            if ((int)vm.estado == 0)
            {

            }
            else
            {
                listaPrueba = bienesFiltrados;
                listaPrueba = filtro.filtrarEstado(listaPrueba,(int)vm.estado);

                bienesFiltrados = listaPrueba;
            }

            modelo.bienes = bienesFiltrados;
            modelo.especialidades = repositorio.obtenerEspecialidades();
            return View("VerBienes", modelo);
        }

        public ActionResult BuscarVerBienes(string id)
        {
            List<Bienes> lista = new List<Bienes>();
            if (id == "")
            {
                lista = repositorio.obtenerBienesActivos();
               
                return View("VerBienes",lista);
            }
            else
            {
                Bienes bien = new Bienes();
                bien = repositorio.buscarBien(id);
                if (bien == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    lista.Add(bien);
                    return View("VerBienes", lista);
                }
            }
        }

    public ActionResult RestablecerLista()
        {
            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.bienes = repositorio.obtenerBienesActivos();
            modelo.especialidades = repositorio.obtenerEspecialidades();
            return View("VerBienes",modelo);
        }

    public ActionResult VerBienes()
        { 
            VerBienesViewModel modelo = new VerBienesViewModel();

           modelo.bienes = repositorio.obtenerBienesActivos();
           modelo.especialidades = repositorio.obtenerEspecialidades();
           return View(modelo);
        }

       
        //Acciones para la vista anadir Bienes
        public ActionResult AnadirBienes()
        {
            //Este viewbag contiene los datos necesarios para que funcione  adecuadamente el DropDownList
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnadirBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,idEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {
                repositorio.anadirBien(bienes);
            }
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bienes.IDEspecialidad);
            return View();
        }

        //Acciones Para Actualizar
        [ValidateAntiForgeryToken]
        public ActionResult BuscarBien(string id)
        {
            if (id == "")
            {
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                return RedirectToAction("ActualizarBienes");
            } else
            {
                Bienes bien = repositorio.buscarBien(id);
                if (bien == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bien.IDEspecialidad);
              
                return View("ActualizarBienes", bien);
            }
        }
        //Accion GetRequest
        [HttpGet]
        public ActionResult ActualizarBienes()
        {
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
            return View();
        }
        //Accion Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,idEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {
                repositorio.actualizarBien(bienes);
                return RedirectToAction("VerBienes");
            }
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bienes.IDEspecialidad);
            return View(bienes);
        }

        //Acciones para Dar de baja
        [ValidateAntiForgeryToken]
        public ActionResult BuscarDarBaja(string id)
        {  
            if (id == "")
            {
                return RedirectToAction("DarBaja");
            }
            else
            {
                Bienes bien = repositorio.buscarBien(id);
                if (bien == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    tempRepositorio.anadirBienTemp(bien);
                    List<Bienes>bienesDarBaja= tempRepositorio.extraerData();
                    return View("DarBaja",bienesDarBaja);
                }
            }
        }

        // List<Bienes> bienesDarBaja = new List<Bienes>();
        [HttpGet]
        public ActionResult DarBaja()
        {

            tempRepositorio.limpiar();


            return View("DarBaja");
        }
 
        [HttpPost]
        public ActionResult DarBaja(List<Bienes> bienes)
        {

          List<Bienes> lista =  tempRepositorio.extraerData();
     
            repositorio.darDeBaja(lista);
            
            //var model = bienesDarBaja;   bienesDarBaja
            return View();
        }

        // GET: Bienes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bienes bienes = db.Bienes.Find(id);
            if (bienes == null)
            {
                return HttpNotFound();
            }
            return View(bienes);
        }

        // POST: Bienes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Bienes bienes = db.Bienes.Find(id);
            db.Bienes.Remove(bienes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}