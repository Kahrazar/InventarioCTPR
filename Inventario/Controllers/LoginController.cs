using Inventario.Models;
using Inventario.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class LoginController : Controller
    {

        // private UsuariosRepo UR = new UsuariosRepo();
        //pr ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AccountManage()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.Usuarios.ToList());
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(Usuario cuenta)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Usuarios.Add(cuenta);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = "Usuario: " + cuenta.Nombre + " registrado correctamente.";
            }
            return View();
        }


        //Metodo que muestra la vista "Login"
        public ActionResult Login()
        {
            return View();
        }

        //Metodo que comprueba los datos para iniciar sesion en el inventario
        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    //Comprueba que la cedula y contraseña ingresados coincidan con los de la Base de Datos
                    var usr = db.Usuarios.FirstOrDefault(u => u.Cedula == user.Cedula && u.Contraseña == user.Contraseña);
                    if (usr != null)//Si la cedula y contraseña coiciden, entra a la pagina de inicio
                    {
                        Session["idCedula"] = usr.Cedula.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                    else//De lo contrario indica que son incorrectos
                    {
                        ModelState.AddModelError("", "Numero de cedula y/o contraseña incorrectas");
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
            return View();
        }

        /*Metodo para comprobar si se inicio sesion
        public ActionResult LoggedIn()
        {
            if (Session["idCedula"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }*/
       
        //Metodos para Editar Usuarios
        [HttpGet]
        public ActionResult EditarUsuario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Usuario user = db.Usuarios.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(Usuario user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                    try
                    {
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("AccountManage");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
            }else
            {
                ModelState.AddModelError("","Datos Invalidos");
            }
            return View(user);
        }

        //Metodos para Eliminar Usuarios
        [HttpGet]
        public ActionResult EliminarUsuario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Usuario user = db.Usuarios.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(user);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int id )
        {
            using (var db = new ApplicationDbContext())
                try
                {
                    Usuario user = db.Usuarios.Find(id);
                    db.Usuarios.Remove(user);
                    db.SaveChanges();
                    return RedirectToAction("AccountManage");
                }
                catch (Exception)
                {

                    throw;
                }
        }
    }
}