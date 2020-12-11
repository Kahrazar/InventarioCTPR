using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventario.Models;
using System.IO;
namespace Inventario.Services
{
    public class FacturasRepository
    {
        public Boolean facturaExistente(Factura factura)
        {
            Boolean existe = true;
      
            using (ApplicationDbContext db = new ApplicationDbContext())
                try
                {   
                    Factura facturaExistente = db.Facturas.Find(factura.numeroDeFactura);

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

        public Factura buscarfactura(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
                try
                {
                   Factura facturaEncontrada =  db.Facturas.Find(id);
                    return facturaEncontrada;
                }
                catch (Exception)
                {

                    throw;
                }
        }

        public void agregarFactura(Factura Factura)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
                try
                {
                    db.Facturas.Add(Factura);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
        }


        public void renombrar(string url,string url2)
        {
            File.Move(url,url2 );
        }
    }
}