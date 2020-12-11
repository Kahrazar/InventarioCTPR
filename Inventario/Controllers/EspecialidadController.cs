using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.Services;
using Inventario.Models;
using System.Net;

namespace Inventario.Controllers
{
    public class EspecialidadController : Controller
    {
        //Objetos de uso
        EspecialidadRepository repositorio = new EspecialidadRepository();


        //******************************************************//
        //*************ACTIONS DE SUPERADMIN VIEWS**************//

        // GET: Especialidad
        public ActionResult VerEspecialidades()
        {
            return View(repositorio.obtenerEspecialidades());
        }

        // GET: Especialidad/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Especialidad/Create
        public ActionResult AgregarEspecialidad()
        {
            return View();
        }

        // POST: Especialidad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarEspecialidad(Especialidad collection)
        {
           collection.ID = repositorio.generarID();
            if (ModelState.IsValid)
            {
                repositorio.anadirEspecialidad(collection);
                return RedirectToAction("VerEspecialidades");
            }
            return View();
        }

        // GET: Especialidad/Edit/5
        public ActionResult EditarEspecialidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = repositorio.buscarEspecialidad(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
            
           
        }

        // POST: Especialidad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEspecialidad(Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                repositorio.editarEspecialidad(especialidad);
                return RedirectToAction("VerEspecialidades");
            }
            else
            {
                return View("EditarEspecialidad",especialidad);
            }
        }

        // GET: Especialidad/Delete/5
        public ActionResult EliminarEspecialidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = repositorio.buscarEspecialidad(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidad/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEspecialidad(int id)
        {
            repositorio.reasignarEspecialidad(id);
            repositorio.eliminarEspecialidad(id);
         
                return RedirectToAction("VerEspecialidades");

        }



        //******************************************************//
        //****************ACTIONS DE ADMIN VIEWS****************//

        // GET: Especialidad
        public ActionResult AdminVerEspecialidades()
        {
            return View(repositorio.obtenerEspecialidades());
        }

        // GET: Especialidad/Details/5
        public ActionResult AdminDetails(int id)
        {
            return View();
        }

        // GET: Especialidad/Create
        public ActionResult AdminAgregarEspecialidad()
        {
            return View();
        }

        // POST: Especialidad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminAgregarEspecialidad(Especialidad collection)
        {
            collection.ID = repositorio.generarID();
            if (ModelState.IsValid)
            {
                repositorio.anadirEspecialidad(collection);
                return RedirectToAction("AdminVerEspecialidades");
            }
            return View();
        }

        // GET: Especialidad/Edit/5
        public ActionResult AdminEditarEspecialidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = repositorio.buscarEspecialidad(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);


        }

        // POST: Especialidad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEditarEspecialidad(Especialidad especialidad)
        {
            if (ModelState.IsValid)
            {
                repositorio.editarEspecialidad(especialidad);
                return RedirectToAction("AdminVerEspecialidades");
            }
            else
            {
                return View("AdminEditarEspecialidad", especialidad);
            }
        }

        // GET: Especialidad/Delete/5
        public ActionResult AdminEliminarEspecialidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = repositorio.buscarEspecialidad(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidad/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEliminarEspecialidad(int id)
        {
            repositorio.reasignarEspecialidad(id);
            repositorio.eliminarEspecialidad(id);

            return RedirectToAction("AdminVerEspecialidades");

        }












    }

}




