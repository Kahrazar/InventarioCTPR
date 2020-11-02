using Inventario.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class BienesController : Controller
    {
        //Este objeto se encarga de proveer los datos de la Bd


      

        public BienesController()
        {
            repositorio = new BienesRepository();
        }
        

        // GET: Bienes
        public ActionResult VerBienes()
        {  
            
            private BienesRepository repositorio;
           var model = repositorio.obtenerTodosLosBienes();
            return View(model);
        }

        public ActionResult AnadirBienes()
        {
            return View();
        }

        public ActionResult ActualizarBienes()
        {
            return View();
        }

        public ActionResult DarBaja()
        {
            return View();
        }
    }
}