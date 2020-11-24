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
        private BienesRepository repositorio = new BienesRepository();//Objeto que proporcionara los datos provenientes de la Bd
        private ViewModelMaper maper = new ViewModelMaper();
        private EspecialidadRepository repoEspecialidad = new EspecialidadRepository();
       
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
             
                repositorio.anadirBien(bienes);
                return RedirectToAction("VerBienes");
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
                ModelState.AddModelError("","No puede dejar el espacio Buscar Vacio");
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                return View("ActualizarBienes");
            } else
            {
                Bienes bien = repositorio.buscarBien(id);
                if (bien == null)
                {
                    ModelState.AddModelError("", "Bien no encontrado");
                    ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                    return View("ActualizarBienes", bien);
                }
                else
                {
                    ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad",bien.IDEspecialidad);
                    return View("ActualizarBienes", bien);
                }
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
            else
            {
                ModelState.AddModelError("", "Datos Invalidos");
            }
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bienes.IDEspecialidad);
            return View(bienes);
        }

        //Acciones para las vistas darbaja
        public ActionResult RestablecerListaDarBaja()
        {
            DarBajaViewModel bienEncontrado = new DarBajaViewModel();
            bienEncontrado.especialidad = repositorio.obtenerEspecialidades();
            bienEncontrado.bienes = repositorio.obtenerBienesActivos();
            return View("DarBaja",bienEncontrado);
        }


        public ActionResult BuscarDarbaja(DarBajaViewModel vm)
        {
            DarBajaViewModel bienEncontrado = new DarBajaViewModel();
            bienEncontrado.especialidad = repositorio.obtenerEspecialidades();

            if (vm.numeroDePatrimonio == null)
            {
                ModelState.AddModelError("", "Debes Llenar este campo");
                bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                return View("DarBaja", bienEncontrado);
            }
            else
            {
                Bienes bien = repositorio.buscarBien(vm.numeroDePatrimonio);
                if (bien == null)
                {
               
                    bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                    ModelState.AddModelError("", "Bien no encontrado");
                    return View("DarBaja", bienEncontrado);
                }
                else
                {
                    if ((int)bien.condicion == 1 )
                    {
                        bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                        ModelState.AddModelError("", "Ese bien ya esta de baja");
                        return View("DarBaja", bienEncontrado);
                    }
                    else
                    {
                        List<Bienes> bienesl = new List<Bienes>();
                        bienesl.Add(bien);

                        bienEncontrado.bienes = bienesl;
                        return View("DarBaja", bienEncontrado);
                    }
                }
            }
        }

       public ActionResult FiltradoBajas(DarBajaViewModel vm, EstadosVMEnum estado)
        {
            List<Bienes> bienesFiltrados = repositorio.obtenerBienesActivos();
            DarBajaViewModel dbvm = new DarBajaViewModel();
            dbvm.especialidad = repositorio.obtenerEspecialidades();
            int especialidadaFiltrar = vm.IDEspecialidad;
        
            if (vm.IDEspecialidad == 0)
            {
               
            
            }
            else
            {
                bienesFiltrados = filtro.filtrarEspecialidad(bienesFiltrados, especialidadaFiltrar);
            }
    

            if ((int)estado == 0)
            {

            }
            else
            {
                bienesFiltrados = filtro.filtrarEstado(bienesFiltrados, (int)estado);
            }
            dbvm.bienes = bienesFiltrados;
            return View("DarBaja", dbvm);
        }


        [HttpGet]
        public ActionResult DarBaja()
        {

            DarBajaViewModel darbajavm = new DarBajaViewModel();
            darbajavm.bienes = repositorio.obtenerBienesActivos();
            darbajavm.especialidad = repositorio.obtenerEspecialidades();
            return View("DarBaja",darbajavm);
        }
 

        public ActionResult ConfirmarBaja(string id)
        {
            Bienes bien = repositorio.buscarBien(id);

            return View(bien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarCondicion(Bienes bien)
        {
            bien = repositorio.buscarBien(bien.numeroDePatrimonio);
            repositorio.darDeBaja(bien);

            return RedirectToAction("DarBaja");
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