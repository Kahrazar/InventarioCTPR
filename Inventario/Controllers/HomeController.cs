﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario.Controllers
{
    public class HomeController : Controller
    {
        //GET: IndexSuperadmin
        public ActionResult IndexSuperadmin()
        {
            return View();
        }
        //GET: IndexAdmin
        public ActionResult IndexAdmin()
        {
            return View();
        }
        //GET: IndexUsuario
        public ActionResult IndexUsuario()
        {
            return View();
        }
    }
}