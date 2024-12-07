using System;
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
        public ActionResult Index()
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

            //Variables de sesión
            var idUsuario = (Session["UserId"] != null)? (int)Session["UserId"] : 0;
            var tipoUsuario = (Session["UserTipo"] != null)? Session["UserTipo"].ToString() : "";
            var nombreUsuario = (Session["UserName"] != null)? Session["UserName"].ToString() : "";

            try
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    if (tipoUsuario.Equals("Empleado"))
                    {
                        listCobros = db.SpConsultarCobros().ToList();
                        cobroView.CobrosList = listCobros;
                    }
                    else 
                    {
                        listCobrosCliente = db.SpConsultarCobroPorCliente(idUsuario).ToList();
                        cobroView.CobrosClienteList = listCobrosCliente;
                    }
                    
                }

            }
            catch{}

            return View(cobroView);
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
                    NumCasa = _.Nombre_casa,
                    PrecioCasa = _.PrecioCasa,
                    monto = _.Monto,
                    mes = _.Mes,
                    anno = _.Anno,
                    estado = _.Estado
                }).FirstOrDefault();

                if(usuarioTipo.Equals("Empleado"))
                {
                    bitacora = db.SpConsultarBitacora(idCobro).ToList();
                }
                else 
                {
                    bitacora = db.SpConsultarBitacora(idCobro).Where(_ => _.Id_persona == usuarioId).ToList();
                }

                detalleCobro = db.SpConsultarDetallePorIdCobro(idCobro).ToList();
                servicios = db.SpConsultarServicios().ToList();

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

            var cobro = new ModelCobro();
            var servicios = new List<SpConsultarServiciosResult>();
            var detalleCobro = new List<SpConsultarDetallePorIdCobroResult>();
            var cobroView = new ModelCobroView();

            //Se cargan las listas de meses y años
            cobroView.annos = new List<int>() {2024,2025,2026,2027,2028,2029,2030,2031,2032,2033,2034};
            cobroView.meses  = new List<string>() {"Enero", "Febrero", "Marzo", "Mayo", "Abril", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};



            try 
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {

                    cobro = db.SpConsultarCobroPorId(idCobro).Select(_ => new ModelCobro{
                        IdCobro = _.Id_cobro,
                        IdCasa = _.Id_casa,
                        IdPersona = _.Id_persona,
                        mes = _.Mes,
                        anno = _.Anno
                    }).FirstOrDefault();

                    detalleCobro = db.SpConsultarDetallePorIdCobro(idCobro).ToList();
                    servicios = db.SpConsultarServicios().ToList();

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
            cobroView.meses = new List<string>() { "Enero", "Febrero", "Marzo", "Mayo", "Abril", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

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
                        db.SpCrearCobro(cobro.IdCasa, cobro.mes, cobro.anno);

                        foreach (var id in servicios)
                        {
                            idServicio = int.Parse(id);
                            db.SpAgregarServiciosCobro(idServicio, 0);
                        }

                        db.SpActualizarMontoCobro(cobro.IdCasa, 0);
                        db.SpAgregarBitacora(0, usuarioId, "CREACION");
                    }
                    else
                    {
                        if(cobro.IdPersona == usuarioId || usuarioTipo.Equals("Empleado"))
                        {
                            db.SpRestaurarDetalleCobroPorIdCobro(cobro.IdCobro);

                            foreach (var id in servicios)
                            {
                                idServicio = int.Parse(id);
                                db.SpAgregarServiciosCobro(idServicio, cobro.IdCobro);
                            }

                            db.SpActualizarMontoCobro(cobro.IdCasa, cobro.IdCobro);
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

            var usuarioNombre = Session["UserName"].ToString();
            var usuarioTipo = Session["UserTipo"].ToString();
            var usuarioId = (int)Session["UserId"];
            var list = new List<Dropdown>();

            try
            {
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


    }
}