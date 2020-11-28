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
        private UsuarioRepository userRepo= new UsuarioRepository();

        public ActionResult AccountManage()
        {
            return View(userRepo.ObtenerUsuarios());
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
                userRepo.RegistrarUsuario(cuenta);
                ModelState.Clear();
                ViewBag.Message = "Usuario: " + cuenta.Nombre + " registrado correctamente.";
            }
            else
            {
                ModelState.AddModelError("","Datos incorrectos");
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
        public ActionResult Login(Usuario usuarioTempo)
        {
            Usuario usr = new Usuario();
            usr = userRepo.AutenticarUsuario(usuarioTempo.Cedula,usuarioTempo.Contraseña);
            if (usr != null)
            {
                return RedirectToAction("Index", "Home", usr);
            }
            ModelState.AddModelError("", "Usuario incorrecto");
            return View("Login");         
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
                Usuario user = userRepo.BuscarUsuario(id);
                if (user == null)
                {
                    return HttpNotFound();
                }                   
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(Usuario user)
        {
            if (ModelState.IsValid)
            {
                userRepo.EditarUsuario(user);
                return RedirectToAction("AccountManage");
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
                Usuario user = userRepo.BuscarUsuario(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
              
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int id )
        {
            userRepo.EliminarUsuario(id);
            return RedirectToAction("AccountManage");
        }
    }
}