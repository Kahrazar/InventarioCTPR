using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventario.Services;
using Inventario.Models;

namespace Inventario.Controllers
{
    public class EspecialidadController : Controller
    {
        EspecialidadRepository repositorio = new EspecialidadRepository();   
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
            repositorio.anadirEspecialidad(collection);
            if (ModelState.IsValid)
            {
                try
                {
                    

                    return RedirectToAction("VerEspecialidades");
                }
                catch
                {
                    return View();
                }
            }

            return View();

        }

        // GET: Especialidad/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Especialidad/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Especialidad/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Especialidad/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
