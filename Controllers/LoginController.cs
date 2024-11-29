using G03_Sistema_Condominios.Models;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;


namespace G03_Sistema_Condominios.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
  
        public ActionResult Login(Login model)
        {
           
                try
                {
                    using (var db = new PviProyectoFinalDB("MyDatabase"))
                    {
                        var user = db.SpLogin(model.Email, model.Password).FirstOrDefault();

                        if (user != null)
                        {
                            // Guardar la información del usuario en la sesión
                            Session["UserName"] = user.Nombre + " " + user.Apellido;
                            Session["UserId"] = user.IdPersona;
                            Session["UserTipo"] = user.TipoPersona;

                            // Redireccionar a la página principal
                            return RedirectToAction("Index", "Cobro");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Correo electrónico o contraseña incorrectos.");
                        }
                    }
                }
                catch (Exception ex)
                {
                 
                }
            

           
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}