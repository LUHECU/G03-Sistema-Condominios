﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;
using G03_Sistema_Condominios.Models;
using DataModels;
using System.Drawing;

namespace G03_Sistema_Condominios.Controllers
{
    public class CobroController : Controller
    {
        // GET: Cobro
        [HttpGet]
        public ActionResult Index(string propietario, string estado, int? mes, int? anno)
        {
            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //Variables de modelos
            LoginController login = new LoginController();
            ModelCobroView cobroView = new ModelCobroView();
            var listCobros = new List<SpConsultarCobrosResult>();
            var listCobrosCliente = new List<SpConsultarCobroPorClienteResult>();
            var clientes = new List<Dropdown>();

            //Variables de sesión
            var idUsuario = (Session["UserId"] != null)? (int)Session["UserId"] : 0;
            var tipoUsuario = (Session["UserTipo"] != null)? Session["UserTipo"].ToString() : "";
            var nombreUsuario = (Session["UserName"] != null)? Session["UserName"].ToString() : "";

            //Se cargan las listas de meses y años
            cobroView.annos = new List<int>() { 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034 };
            cobroView.meses = new List<string>() { "Enero", "Febrero", "Marzo", "Mayo", "Abril", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };




            try
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    //Verifi si el usuario es empleado
                    if (tipoUsuario.Equals("Empleado"))
                    {
                        //Carga la vista del empleado

                        listCobros = db.SpConsultarCobros().ToList();
                        clientes = db.SpConsultarClientesActivos().Select(_ => new Dropdown { Id = _.Id_persona, Nombre = _.Cliente}).ToList();
                        cobroView.CobrosList = listCobros;
                        ViewBag.Clientes = clientes;

                        // Aplicar filtros
                        if (!string.IsNullOrEmpty(propietario))
                        {
                            listCobros = listCobros.Where(c => c.Persona.Contains(propietario)).ToList();
                        }
                        if (!string.IsNullOrEmpty(estado))
                        {
                            listCobros = listCobros.Where(c => c.Estado == estado).ToList();
                        }
                        if (mes.HasValue)
                        {
                            listCobros = listCobros.Where(c => c.Mes == mes.Value).ToList();
                        }
                        if (anno.HasValue)
                        {
                            listCobros = listCobros.Where(c => c.Anno == anno.Value).ToList();
                        }
                    }
                    else 
                    {
                        //Carga la vista del cliente

                        listCobrosCliente = db.SpConsultarCobroPorCliente(idUsuario).ToList();
                        cobroView.CobrosClienteList = listCobrosCliente;
                    }
                    
                }

            }
            catch{}

            return View(cobroView);
        }

        // Acción para filtrar cobros de manera asíncrona
        [HttpPost]
        public ActionResult FiltrarCobros(int? propietario, string estado, int? mes, int? anno)
        {
            //Variable para almacenar listas de cobros
            List<SpConsultarCobrosResult> list = new List<SpConsultarCobrosResult>();
            //Variable para almacenar modelos
            ModelCobroView cobroView = new ModelCobroView();

            //Se cargan las listas de meses y años
            cobroView.annos = new List<int>() { 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034 };
            cobroView.meses = new List<string>() { "Enero", "Febrero", "Marzo", "Mayo", "Abril", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };


            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCobros().ToList();

                    // Aplicar filtros
                    if (propietario.HasValue)
                    {
                        list = list.Where(c => c.Id_persona == propietario).ToList();
                    }
                    if (!string.IsNullOrEmpty(estado))
                    {
                        list = list.Where(c => c.Estado == estado).ToList();
                    }
                    if (mes.HasValue)
                    {
                        list = list.Where(c => c.Mes == mes.Value).ToList();
                    }
                    if (anno.HasValue)
                    {
                        list = list.Where(c => c.Anno == anno.Value).ToList();
                    }

                    cobroView.CobrosList = list;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return Json(new { success = false, message = ex.Message });
            }

            return PartialView("CobrosTable", cobroView); ;
        }

        public ActionResult DetalleCobro(int? idCobro) 
        {

            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //Variables de sesión de usuario
            var usuarioNombre = Session["UserName"].ToString();
            var usuarioTipo = Session["UserTipo"].ToString();
            var usuarioId = (int)Session["UserId"];

            //Variables del modelos
            var cobro = new ModelCobro();
            var servicios = new List<SpConsultarServiciosResult>();
            var detalleCobro = new List<SpConsultarDetallePorIdCobroResult>();
            var cobroView = new ModelCobroView();
            var bitacora = new List<SpConsultarBitacoraResult>();

            using (var db = new PviProyectoFinalDB("MyDatabase"))
            {

                cobro = db.SpConsultarCobroPorId(idCobro).Select(_ => new ModelCobro
                {
                    IdCobro = _.Id_cobro,
                    Persona = _.Persona,
                    IdPersona = _.Id_persona,
                    NumCasa = _.Nombre_casa,
                    PrecioCasa = _.PrecioCasa,
                    monto = _.Monto,
                    mes = _.Mes,
                    anno = _.Anno,
                    estado = _.Estado
                }).FirstOrDefault();

                //Verifica si el cobro pertenece al usuario cliente
                if (cobro != null && usuarioTipo.Equals("Cliente") && usuarioId != cobro.IdPersona)
                {
                    TempData["Resultado"] = "No puede modificar un cobro que no este a su nombre. Solicite a un empleado realizar esta acción.";
                    return RedirectToAction("Index", "Cobro");
                }

                //Verifica si el usuario es empleado
                if (usuarioTipo.Equals("Empleado"))
                {
                    bitacora = db.SpConsultarBitacora(idCobro).ToList();
                }
                else 
                {
                    bitacora = db.SpConsultarBitacora(idCobro).Where(_ => _.Id_persona == usuarioId).ToList();
                }

                detalleCobro = db.SpConsultarDetallePorIdCobro(idCobro).ToList();
                servicios = db.SpConsultarServicios().ToList();
                
                //Se almacenan los modelos en un único modelo
                cobroView.DetalleCobro = detalleCobro;
                cobroView.Servicios = servicios;
                cobroView.Cobro = cobro;
                cobroView.Bitacora = bitacora;

            }
            return View(cobroView);
        }

        public ActionResult CrearModificarCobro(int? idCobro) 
        {
            //Verificador de inicio de sesión
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //Usuario
            var usuario = new ModelUsuario();
            usuario.Nombre = Session["UserName"].ToString();
            usuario.Tipo = Session["UserTipo"].ToString();
            usuario.Id = (int) Session["UserId"];

            //Variables de modelos
            var cobro = new ModelCobro();
            var servicios = new List<SpConsultarServiciosResult>();
            var detalleCobro = new List<SpConsultarDetallePorIdCobroResult>();
            var cobroView = new ModelCobroView();

            //Se cargan las listas de meses y años
            cobroView.annos = new List<int>() {2024,2025,2026,2027,2028,2029,2030,2031,2032,2033,2034};
            cobroView.meses  = new List<string>() {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};



            try 
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    cobro = db.SpConsultarCobroPorId(idCobro).Select(_ => new ModelCobro{
                        IdCobro = _.Id_cobro,
                        IdCasa = _.Id_casa,
                        IdPersona = _.Id_persona,
                        mes = _.Mes,
                        anno = _.Anno,
                        estado = _.Estado
                    }).FirstOrDefault();

                    //Verifica si un empleado trata de modificar un cobro a su nombre
                    if (cobro != null && usuario.Tipo.Equals("Empleado") && usuario.Id == cobro.IdPersona)
                    {
                        TempData["Resultado"] = "No puede modificar un cobro que este a su nombre. Solicite ayuda de otro empleado.";
                        return RedirectToAction("Index", "Cobro");
                    }

                    //Verifica cliente trata de modificar un cobro
                    if (cobro != null && usuario.Tipo.Equals("Cliente"))
                    {
                        TempData["Resultado"] = "Permisos insuficientes, no puede modificar cobros. Solicite ayuda a un empleado para realizar esta acción.";
                        return RedirectToAction("Index", "Cobro");
                    }

                    //Verifica si se intenta modificar un cobro eliminado
                    if (cobro != null && cobro.estado.Equals("Eliminado"))
                    {
                        TempData["Resultado"] = "El cobro no puede ser modificado, debido a que ha sido eliminado.";
                        return RedirectToAction("Index", "Cobro");
                    }

                    //Verifica si se intenta modificar un cobro pagado
                    if (cobro != null && cobro.estado.Equals("Pagado"))
                    {
                        TempData["Resultado"] = "El cobro no puede ser modificado, debido a que ha sido pagado.";
                        return RedirectToAction("Index", "Cobro");
                    }

                    detalleCobro = db.SpConsultarDetallePorIdCobro(idCobro).ToList();
                    servicios = db.SpConsultarServicios().ToList();

                    //Se almacenan los modelos en un único modelo
                    cobroView.DetalleCobro = detalleCobro;
                    cobroView.Servicios = servicios;
                    cobroView.Cobro = cobro;

                }
            }
            catch { }

            cobroView.Usuario = usuario;

            return View(cobroView);
        }

        [HttpPost]
        public JsonResult CrearModificarCobro(ModelCobro cobro, List<string> servicios)
        {

            //Se instacia el cobroView nuevamente
            var cobroView = new ModelCobroView();

            //Se cargan las listas de meses y años
            cobroView.annos = new List<int>() { 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034 };
            cobroView.meses = new List<string>() { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            //Se declaran variables para la inserción de servicios
            var idServicio = 0;

            //Variables de sesión de usuario
            var usuarioNombre = Session["UserName"].ToString();
            var usuarioTipo = Session["UserTipo"].ToString();
            var usuarioId = (int)Session["UserId"];

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    if (cobro.IdCobro == 0)
                    {

                        var cobrosPendientes = db.SpVerificarCobrosPendientePorPeriodo(cobro.IdCasa, cobro.mes, cobro.anno).FirstOrDefault();

                        //Verifica si un usuario trata de crear un cobro con una casa que tiene un periodo igual y estado pendiente
                        if (cobrosPendientes != null && cobrosPendientes.Column1 == 1)
                        {
                            TempData["Resultado"] = "No se puede crear un nuevo cobro con una casa con un cobro pendiente en el mismo periodo.";
                            return Json(1);
                        }

                        //Se crea un cobro sencillo
                        db.SpCrearCobro(cobro.IdCasa, cobro.mes, cobro.anno);

                        //Se asignan los servicios al cobro recien creado
                        foreach (var id in servicios)
                        {
                            idServicio = int.Parse(id);
                            db.SpAgregarServiciosCobro(idServicio, 0);
                        }

                        //Actualiza el monto del cobro
                        db.SpActualizarMontoCobro(cobro.IdCasa, 0);

                        //Se registra en bitacora la acción realizada
                        db.SpAgregarBitacora(0, usuarioId, "CREACION");
                    }
                    else
                    {
                        //Se verifica si el usuario es empleado o si le pertenece el cobro
                        if(cobro.IdPersona == usuarioId || usuarioTipo.Equals("Empleado"))
                        {
                            //Restaura los servicios asociados al cobro en nulo
                            db.SpRestaurarDetalleCobroPorIdCobro(cobro.IdCobro);

                            //Se asignan los servicios al cobro recien creado
                            foreach (var id in servicios)
                            {
                                idServicio = int.Parse(id);
                                db.SpAgregarServiciosCobro(idServicio, cobro.IdCobro);
                            }

                            //Actualiza el monto del cobro
                            db.SpActualizarMontoCobro(cobro.IdCasa, cobro.IdCobro);

                            //Se registra en bitacora la acción realizada
                            db.SpAgregarBitacora(cobro.IdCobro, usuarioId, "MODIFICACION");
                        }

                    }
                    
                }
                
            }
            catch {}

            return Json(cobroView);
        }

        public JsonResult Clientes()
        {
            //Variables de usuario
            var usuarioNombre = Session["UserName"].ToString();
            var usuarioTipo = Session["UserTipo"].ToString();
            var usuarioId = (int)Session["UserId"];

            //Variable para el Dropdown
            var list = new List<Dropdown>();

            try
            {
                //Verifica si el usuario es empleado para mostrar todos las personas activas
                if (usuarioTipo.Equals("Empleado")) 
                {
                    using (var db = new PviProyectoFinalDB("MyDatabase"))
                    {
                        list = db.SpConsultarClientesActivos().Select(_ => new Dropdown
                        {
                            Id = _.Id_persona,
                            Nombre = _.Cliente
                        }). Where(_ => _.Id != usuarioId).ToList();
                    }
                }
                else 
                {
                    list.Add(new Dropdown { Id = usuarioId, Nombre = usuarioNombre});
                }
                
            }
            catch{ }

            return Json(list);
        }

        public JsonResult Casas(int? idCliente)
        {
            //Variable para el Dropdown
            var list = new List<Dropdown>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCasasActivasPorCliente(idCliente).Select(_ => new Dropdown
                    {
                        Id = _.Id_casa,
                        Nombre = _.Nombre_casa
                    }).ToList();
                }
            }
            catch { }

            return Json(list);
        }

        public JsonResult EliminarCobro(int idCobro) 
        {
            var resultado = string.Empty;

            //Variable para almacenar el usuario que realizo la acción
            var usuarioId = (int)Session["UserId"];

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    //Se realiza la inactivación del cobro
                    db.SpEliminarCobro(idCobro);
                    //Se registra en bitacora la acción realizada
                    db.SpAgregarBitacora(idCobro, usuarioId, "ELIMINACION");
                }
            }
            catch { }
            return Json(resultado);
        }

        public JsonResult PagarCobro(int idCobro)
        {
            var resultado = string.Empty;

            //Variable para almacenar el usuario que realizo la acción
            var usuarioId = (int)Session["UserId"];

            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    //Se realiza el pago del cobro
                    db.SpPagarCobro(idCobro);
                    //Se registra en bitacora la acción realizada
                    db.SpAgregarBitacora(idCobro, usuarioId, "MODIFICACION");
                }
            }
            catch { }
            return Json(resultado);
        }


    }
}