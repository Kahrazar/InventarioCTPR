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
        private FacturasRepository factuRepositorio = new FacturasRepository();



        //******************************************************//
        //*************ACTIONS DE SUPERADMIN VIEWS**************//

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



        //******************************************************//
        //****************ACTIONS DE USUARIO VIEWS**************//

        //Acciones para la vista ver bienes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioFiltrado(CondicionesEnum condicion, VerBienesViewModel vm)
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
                bienesFiltrados = filtro.filtrarEspecialidad(bienesFiltrados, vm.IDEspecialidad);
            }

            if ((int)vm.estado == 0)
            {

            }
            else
            {
                listaPrueba = bienesFiltrados;
                listaPrueba = filtro.filtrarEstado(listaPrueba, (int)vm.estado);
                bienesFiltrados = listaPrueba;
            }
            modelo.bienes = bienesFiltrados;
            modelo.especialidades = repositorio.obtenerEspecialidades();
            return View("UsuarioVerBienes", modelo);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UsuarioBuscarVerBienes([Bind(Include = "numeroDePatrimonio")]VerBienesViewModel vm)
        {

            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.especialidades = repositorio.obtenerEspecialidades();
            List<Bienes> lista = new List<Bienes>();

            if (vm.numeroDePatrimonio == null)
            {
                ModelState.AddModelError("", "Debes llenar este campo");
                lista = repositorio.obtenerBienesActivos();
                modelo.bienes = lista;
                return View("UsuarioVerBienes", modelo);
            }
            else
            {
                Bienes bien = new Bienes();
                bien = repositorio.buscarBien(vm.numeroDePatrimonio);
                if (bien == null)
                {
                    lista = repositorio.obtenerBienesActivos();
                    modelo.bienes = lista;
                    ModelState.AddModelError("", "Bien no encontrado");
                    return View("UsuarioVerBienes", modelo);
                }
                else
                {
                    lista.Add(bien);
                    modelo.bienes = lista;
                    return View("UsuarioVerBienes", modelo);
                }
            }
        }

        public ActionResult UsuarioRestablecerLista()
        {
            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.bienes = repositorio.obtenerBienesActivos();
            modelo.especialidades = repositorio.obtenerEspecialidades();
            return View("UsuarioVerBienes", modelo);
        }

        public ActionResult UsuarioVerBienes()
        {
            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.bienes = repositorio.obtenerBienesActivos();
            modelo.especialidades = repositorio.obtenerEspecialidades();//Esta linea obtienes las especialidades de la base de datos, es importante para poder llenar los dropdownlist de filtrado
            return View(modelo);
        }


        //Acciones para la vista anadir Bienes
        public ActionResult UsuarioAnadirBienes()
        {
            //Este viewbag contiene los datos necesarios para que funcione  adecuadamente el DropDownList
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioAnadirBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,IDEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {

                repositorio.anadirBien(bienes);
                return RedirectToAction("UsuarioVerBienes");
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
        public ActionResult UsuarioBuscarBien(string id)
        {
            if (id == "")
            {
                ModelState.AddModelError("", "No puede dejar el espacio Buscar Vacio");
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                return View("UsuarioActualizarBienes");
            }
            else
            {
                Bienes bien = repositorio.buscarBien(id);
                if (bien == null)
                {
                    ModelState.AddModelError("", "Bien no encontrado");
                    ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                    return View("UsuarioActualizarBienes", bien);
                }
                else
                {
                    ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bien.IDEspecialidad);
                    return View("UsuarioActualizarBienes", bien);
                }
            }
        }

        //Accion GetRequest
        [HttpGet]
        public ActionResult UsuarioActualizarBienes()
        {
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
            return View();
        }

        //Accion Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioActualizarBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,idEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {
                repositorio.actualizarBien(bienes);
                return RedirectToAction("UsuarioVerBienes");
            }
            else
            {
                ModelState.AddModelError("", "Datos Invalidos");
            }
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bienes.IDEspecialidad);
            return View(bienes);
        }

        //Acciones para las vistas darbaja
        public ActionResult UsuarioRestablecerListaDarBaja()
        {
            DarBajaViewModel bienEncontrado = new DarBajaViewModel();
            bienEncontrado.especialidad = repositorio.obtenerEspecialidades();
            bienEncontrado.bienes = repositorio.obtenerBienesActivos();
            return View("UsuarioDarBaja", bienEncontrado);
        }


        public ActionResult UsuarioBuscarDarbaja(DarBajaViewModel vm)
        {
            DarBajaViewModel bienEncontrado = new DarBajaViewModel();
            bienEncontrado.especialidad = repositorio.obtenerEspecialidades();

            if (vm.numeroDePatrimonio == null)
            {
                ModelState.AddModelError("", "Debes Llenar este campo");
                bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                return View("UsuarioDarBaja", bienEncontrado);
            }
            else
            {
                Bienes bien = repositorio.buscarBien(vm.numeroDePatrimonio);
                if (bien == null)
                {

                    bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                    ModelState.AddModelError("", "Bien no encontrado");
                    return View("UsuarioDarBaja", bienEncontrado);
                }
                else
                {
                    if ((int)bien.condicion == 1)
                    {
                        bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                        ModelState.AddModelError("", "Ese bien ya esta de baja");
                        return View("UsuarioDarBaja", bienEncontrado);
                    }
                    else
                    {
                        List<Bienes> bienesl = new List<Bienes>();
                        bienesl.Add(bien);

                        bienEncontrado.bienes = bienesl;
                        return View("UsuarioDarBaja", bienEncontrado);
                    }
                }
            }
        }

        public ActionResult UsuarioFiltradoBajas(DarBajaViewModel vm, EstadosVMEnum estado)
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
            return View("UsuarioDarBaja", dbvm);
        }


        [HttpGet]
        public ActionResult UsuarioDarBaja()
        {

            DarBajaViewModel darbajavm = new DarBajaViewModel();
            darbajavm.bienes = repositorio.obtenerBienesActivos();
            darbajavm.especialidad = repositorio.obtenerEspecialidades();
            return View("UsuarioDarBaja", darbajavm);
        }


        public ActionResult UsuarioConfirmarBaja(string id)
        {
            Bienes bien = repositorio.buscarBien(id);

            return View(bien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioCambiarCondicion(Bienes bien)
        {
            bien = repositorio.buscarBien(bien.numeroDePatrimonio);
            repositorio.darDeBaja(bien);

            return RedirectToAction("UsuarioDarBaja");
        }


        // GET: Bienes/Delete/5
        public ActionResult UsuarioDelete(string id)
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
        [HttpPost, ActionName("UsuarioDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UsuarioDeleteConfirmed(string id)
        {
            Bienes bienes = db.Bienes.Find(id);
            db.Bienes.Remove(bienes);
            db.SaveChanges();
            return RedirectToAction("UsuarioIndex");
        }



        //******************************************************//
        //****************ACTIONS DE ADMIN VIEWS****************//

        //Acciones para la vista ver bienes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminFiltrado(CondicionesEnum condicion, VerBienesViewModel vm)
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
                bienesFiltrados = filtro.filtrarEspecialidad(bienesFiltrados, vm.IDEspecialidad);
            }

            if ((int)vm.estado == 0)
            {

            }
            else
            {
                listaPrueba = bienesFiltrados;
                listaPrueba = filtro.filtrarEstado(listaPrueba, (int)vm.estado);
                bienesFiltrados = listaPrueba;
            }
            modelo.bienes = bienesFiltrados;
            modelo.especialidades = repositorio.obtenerEspecialidades();
            return View("AdminVerBienes", modelo);
        }

        [ValidateAntiForgeryToken]
        public ActionResult AdminBuscarVerBienes([Bind(Include = "numeroDePatrimonio")]VerBienesViewModel vm)
        {

            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.especialidades = repositorio.obtenerEspecialidades();
            List<Bienes> lista = new List<Bienes>();

            if (vm.numeroDePatrimonio == null)
            {
                ModelState.AddModelError("", "Debes llenar este campo");
                lista = repositorio.obtenerBienesActivos();
                modelo.bienes = lista;
                return View("AdminVerBienes", modelo);
            }
            else
            {
                Bienes bien = new Bienes();
                bien = repositorio.buscarBien(vm.numeroDePatrimonio);
                if (bien == null)
                {
                    lista = repositorio.obtenerBienesActivos();
                    modelo.bienes = lista;
                    ModelState.AddModelError("", "Bien no encontrado");
                    return View("AdminVerBienes", modelo);
                }
                else
                {
                    lista.Add(bien);
                    modelo.bienes = lista;
                    return View("AdminVerBienes", modelo);
                }
            }
        }

        public ActionResult AdminRestablecerLista()
        {
            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.bienes = repositorio.obtenerBienesActivos();
            modelo.especialidades = repositorio.obtenerEspecialidades();
            return View("AdminVerBienes", modelo);
        }

        public ActionResult AdminVerBienes()
        {
            VerBienesViewModel modelo = new VerBienesViewModel();
            modelo.bienes = repositorio.obtenerBienesActivos();
            modelo.especialidades = repositorio.obtenerEspecialidades();//Esta linea obtienes las especialidades de la base de datos, es importante para poder llenar los dropdownlist de filtrado
            return View(modelo);
        }


        //Acciones para la vista anadir Bienes
        public ActionResult AdminAnadirBienes()
        {
            //Este viewbag contiene los datos necesarios para que funcione  adecuadamente el DropDownList
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminAnadirBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,IDEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {

                repositorio.anadirBien(bienes);
                return RedirectToAction("AdminVerBienes");
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
        public ActionResult AdminBuscarBien(string id)
        {
            if (id == "")
            {
                ModelState.AddModelError("", "No puede dejar el espacio Buscar Vacio");
                ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                return View("AdminActualizarBienes");
            }
            else
            {
                Bienes bien = repositorio.buscarBien(id);
                if (bien == null)
                {
                    ModelState.AddModelError("", "Bien no encontrado");
                    ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
                    return View("AdminActualizarBienes", bien);
                }
                else
                {
                    ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bien.IDEspecialidad);
                    return View("AdminActualizarBienes", bien);
                }
            }
        }

        //Accion GetRequest
        [HttpGet]
        public ActionResult AdminActualizarBienes()
        {
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad");
            return View();
        }

        //Accion Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminActualizarBienes([Bind(Include = "numeroDePatrimonio,codigoDeBarras,descripcion,anadidoPor,numeroDeFactura,ley,marca,modelo,serie,idEspecialidad,ubicacion,estado,condicion")] Bienes bienes)
        {
            if (ModelState.IsValid)
            {
                repositorio.actualizarBien(bienes);
                return RedirectToAction("AdminVerBienes");
            }
            else
            {
                ModelState.AddModelError("", "Datos Invalidos");
            }
            ViewBag.IDEspecialidad = new SelectList(db.Especialidad, "ID", "nombreEspecialidad", bienes.IDEspecialidad);
            return View(bienes);
        }

        //Acciones para las vistas darbaja
        public ActionResult AdminRestablecerListaDarBaja()
        {
            DarBajaViewModel bienEncontrado = new DarBajaViewModel();
            bienEncontrado.especialidad = repositorio.obtenerEspecialidades();
            bienEncontrado.bienes = repositorio.obtenerBienesActivos();
            return View("AdminDarBaja", bienEncontrado);
        }


        public ActionResult AdminBuscarDarbaja(DarBajaViewModel vm)
        {
            DarBajaViewModel bienEncontrado = new DarBajaViewModel();
            bienEncontrado.especialidad = repositorio.obtenerEspecialidades();

            if (vm.numeroDePatrimonio == null)
            {
                ModelState.AddModelError("", "Debes Llenar este campo");
                bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                return View("AdminDarBaja", bienEncontrado);
            }
            else
            {
                Bienes bien = repositorio.buscarBien(vm.numeroDePatrimonio);
                if (bien == null)
                {

                    bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                    ModelState.AddModelError("", "Bien no encontrado");
                    return View("AdminDarBaja", bienEncontrado);
                }
                else
                {
                    if ((int)bien.condicion == 1)
                    {
                        bienEncontrado.bienes = repositorio.obtenerBienesActivos();
                        ModelState.AddModelError("", "Ese bien ya esta de baja");
                        return View("AdminDarBaja", bienEncontrado);
                    }
                    else
                    {
                        List<Bienes> bienesl = new List<Bienes>();
                        bienesl.Add(bien);

                        bienEncontrado.bienes = bienesl;
                        return View("AdminDarBaja", bienEncontrado);
                    }
                }
            }
        }

        public ActionResult AdminFiltradoBajas(DarBajaViewModel vm, EstadosVMEnum estado)
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
            return View("AdminDarBaja", dbvm);
        }


        [HttpGet]
        public ActionResult AdminDarBaja()
        {

            DarBajaViewModel darbajavm = new DarBajaViewModel();
            darbajavm.bienes = repositorio.obtenerBienesActivos();
            darbajavm.especialidad = repositorio.obtenerEspecialidades();
            return View("AdminDarBaja", darbajavm);
        }


        public ActionResult AdminConfirmarBaja(string id)
        {
            Bienes bien = repositorio.buscarBien(id);

            return View(bien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCambiarCondicion(Bienes bien)
        {
            bien = repositorio.buscarBien(bien.numeroDePatrimonio);
            repositorio.darDeBaja(bien);

            return RedirectToAction("AdminDarBaja");
        }


        // GET: Bienes/Delete/5
        public ActionResult AdminDelete(string id)
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
        [HttpPost, ActionName("AdminDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult AdminDeleteConfirmed(string id)
        {
            Bienes bienes = db.Bienes.Find(id);
            db.Bienes.Remove(bienes);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }



        //*****************************************************************//
        //DISPOSE DATABASE
        

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