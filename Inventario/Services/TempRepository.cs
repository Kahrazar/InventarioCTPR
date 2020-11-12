using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventario.Models;
namespace Inventario.Services
{
    public class TempRepository
    {
        public void limpiar()
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    List<BienTemporal> lista = db.BienTemporal.ToList();

                    foreach (var item in lista)
                    {
                        db.BienTemporal.Remove(item);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
        }

        public List<Bienes> extraerData()
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    List<BienTemporal> listabt = new List<BienTemporal>();
                    List<Bienes> listab = new List<Bienes>();
                    listabt = db.BienTemporal.ToList();
                    string p;
                    Bienes bien = new Bienes();

                    foreach (var item in listabt)
                    {
                        p = item.nPatrimonioTemp;
                        bien = db.Bienes.Find(p);

                        listab.Add(bien);
                    }
                    return listab;
                }
                catch (Exception)
                {

                    throw;
                } 
        }



        public void anadirBienTemp(Bienes bienT)
        {
            BienTemporal BT = new BienTemporal();
            string p = bienT.numeroDePatrimonio;
            BT.nPatrimonioTemp = p; 
            using (var db = new ApplicationDbContext())
                try
                {
                    db.BienTemporal.Add(BT);
                    db.SaveChanges();
                }
               catch (Exception)
                {

                   throw;
               }
        }

    }
}