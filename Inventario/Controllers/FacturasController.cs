using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inventario.Models;
using Inventario.Services;

namespace Inventario.Controllers
{

    public class FacturasController : Controller
    {
        //Objetos en uso
        private ApplicationDbContext db = new ApplicationDbContext();
        private FacturasRepository FacturaRepositorio = new FacturasRepository();


        //******************************************************//
        //*************ACTIONS DE SUPERADMIN VIEWS**************//

        public ActionResult VerFacturas()
        {
            return View(db.Facturas.ToList());
        }

        // GET: Facturas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = FacturaRepositorio.buscarfactura(id);
            if (factura == null)
            {
                return HttpNotFound();
            }

            return File(factura.urlDeFactura, "application/pdf");
        }

        // GET: Facturas/Create
        public ActionResult AgregarFactura()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarFactura(Factura factura, HttpPostedFileBase archivo)
        {

            if (ModelState.IsValid)
            {

                if (FacturaRepositorio.facturaExistente(factura))
                {
                    ModelState.AddModelError("", "Factura Existente");
                    return View(factura);
                }

                if (archivo != null && archivo.ContentLength > 0)
                {
                    string nombreArchivo = Path.GetFileName(archivo.FileName);
                    string url = Path.Combine(Server.MapPath("~/App_Data/uploads"), nombreArchivo);
                    string url2 = Path.Combine(Server.MapPath("~/App_Data/uploads"), factura.numeroDeFactura + ".pdf");
                    archivo.SaveAs(url);
                    FacturaRepositorio.renombrar(url, url2);
                    ViewBag.mensaje = "Factura Subida";
                    factura.urlDeFactura = url2;
                    FacturaRepositorio.agregarFactura(factura);
                }
                else
                {
                    ModelState.AddModelError("", "Debes subir la factura");
                }
            }

            return View(factura);
        }

        // GET: Facturas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "numeroDeFactura,urlDeFactura")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Factura factura = db.Facturas.Find(id);
            db.Facturas.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        


        //******************************************************//
        //*************CONTROLADORES DE ADMIN VIEWS*************//

        public ActionResult AdminVerFacturas()
        {
            return View(db.Facturas.ToList());
        }

        // GET: Facturas/Details/5
        public ActionResult AdminDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = FacturaRepositorio.buscarfactura(id);
            if (factura == null)
            {
                return HttpNotFound();
            }

            return File(factura.urlDeFactura, "application/pdf");
        }


        // GET: Facturas/Create
        public ActionResult AdminAgregarFactura()
        {
            return View();
        }

        // POST: Facturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminAgregarFactura(Factura factura, HttpPostedFileBase archivo)
        {

            if (ModelState.IsValid)
            {

                if (FacturaRepositorio.facturaExistente(factura))
                {
                    ModelState.AddModelError("", "Factura Existente");
                    return View(factura);
                }

                if (archivo != null && archivo.ContentLength > 0)
                {
                    string nombreArchivo = Path.GetFileName(archivo.FileName);
                    string url = Path.Combine(Server.MapPath("~/App_Data/uploads"), nombreArchivo);
                    string url2 = Path.Combine(Server.MapPath("~/App_Data/uploads"), factura.numeroDeFactura + ".pdf");
                    archivo.SaveAs(url);
                    FacturaRepositorio.renombrar(url, url2);
                    ViewBag.mensaje = "Factura Subida";
                    factura.urlDeFactura = url2;
                    FacturaRepositorio.agregarFactura(factura);
                }
                else
                {
                    ModelState.AddModelError("", "Debes subir la factura");
                }
            }

            return View(factura);
        }


        // GET: Facturas/Edit/5
        public ActionResult AdminEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit([Bind(Include = "numeroDeFactura,urlDeFactura")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public ActionResult AdminDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult AdminDeleteConfirmed(string id)
        {
            Factura factura = db.Facturas.Find(id);
            db.Facturas.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //**********************************************************************//
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
