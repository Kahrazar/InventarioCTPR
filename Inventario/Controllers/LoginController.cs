using Inventario.Models;
using Inventario.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class LoginController : Controller
    {

        // private UsuariosRepo UR = new UsuariosRepo();
        //private ApplicationDbContext db = new ApplicationDbContext();

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
                        return RedirectToAction("Index","Home");
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

        public ActionResult EditarUsuario(Usuario user)
        {
            return View(user);
        }

        public ActionResult MostrarUsuario(Usuario user)
        {
            return View(user);
        }

        public ActionResult EliminarUsuario(Usuario user)
        {
            return View(user);
        }
       
    }
}