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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var usr = db.Usuarios.Single(u => u.Cedula == user.Cedula && u.Contraseña == user.Contraseña);
                if (usr != null)
                {
                    Session["idCedula"] = usr.Cedula.ToString();
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Numero de cedula y/o contraseña incorrectas");
                }
            }
            return View();
        }

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
        }





    }
}