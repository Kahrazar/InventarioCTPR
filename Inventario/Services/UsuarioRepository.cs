using Inventario.Models;
using Inventario.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Inventario.Services
{
    public class UsuarioRepository
    {
        

        public Usuario AutenticarUsuario(String ced, String con)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
              
            {
                try//Comprueba que la cedula y contraseña ingresados coincidan con los de la Base de Datos
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Usuario usr = db.Usuarios
                    .Where(a => a.Cedula == ced && a.Contraseña == con)
                    .FirstOrDefault();
                    return usr;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Usuarios.ToList();
            }

        }

        public void RegistrarUsuario(Usuario cuenta)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Usuarios.Add(cuenta);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public Usuario BuscarUsuario(int? id)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Usuario user = db.Usuarios.Find(id);
                    return user;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditarUsuario(Usuario user)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
        }

        public void EliminarUsuario(int id)
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    Usuario user = db.Usuarios.Find(id);
                    db.Usuarios.Remove(user);
                    db.SaveChanges();                 
                }
                catch (Exception)
                {
                    throw;
                }
        }
    }


}