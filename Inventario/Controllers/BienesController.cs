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

        [HttpPost]
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

        [ValidateAntiForgeryToken]
        public ActionResult BuscarVerBienes([Bind(Include = "numeroDePatrimonio")]VerBienesViewModel vm)
        {
     
            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.especialidades = repositorio.obtenerEspecialidades();
            List<Bienes> lista = new List<Bienes>();

            if (vm.numeroDePatrimonio == null)
            {
                ModelState.AddModelError("", "Debes llenar este campo");
                lista = repositorio.obtenerBienesActivos();
                modelo.bienes = lista;
                return View("VerBienes",modelo);
            }
            else
            {
                Bienes bien = new Bienes();
               bien = repositorio.buscarBien(vm.numeroDePatrimonio);
                if (bien ==null)
                {
                    lista = repositorio.obtenerBienesActivos();
                    modelo.bienes = lista;
                    ModelState.AddModelError("", "Bien no encontrado");
                    return View("VerBienes", modelo);
                }
                else
                {
                    lista.Add(bien);
                    modelo.bienes = lista;
                    return View("VerBienes", modelo);
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
           modelo.especialidades = repositorio.obtenerEspecialidades();//Esta linea obtienes las especialidades de la base de datos, es importante para poder llenar los dropdownlist de filtrado
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
        public ActionResult AnadirBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,IDEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                repositorio.anadirBien(bienes);
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Llena correctamente todos los campos");
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bienes.IDEspecialidad);
                return View();
            }
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
        [ValidateAntiForgeryToken]
        public ActionResult DarBaja(List<Bienes> bienes)
        {
            List<Bienes> lista =  tempRepositorio.extraerData();
            repositorio.darDeBaja(lista);
            tempRepositorio.limpiar();
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