﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataModels.PviProyectoFinalDBStoredProcedures;
using G03_Sistema_Condominios.Models;
using DataModels;

namespace G03_Sistema_Condominios.Controllers
{
    public class CobroController : Controller
    {
        // GET: Cobro
        [HttpGet]
        public ActionResult Index()
        {
            var list = new List<SpConsultarCobrosResult>();
            try
            {
                using(var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarCobros().ToList();
                    
                }
            }
            catch{ }

            return View(list);
        }

        public ActionResult CrearModificarCobro(int? idCobro) 
        {
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

                    }
                    else
                    {
                        db.SpRestaurarDetalleCobroPorIdCobro(cobro.IdCobro);

                        foreach (var id in servicios)
                        {
                            idServicio = int.Parse(id);
                            db.SpAgregarServiciosCobro(idServicio, cobro.IdCobro);
                        }

                        db.SpActualizarMontoCobro(cobro.IdCasa, cobro.IdCobro);
                    }
                    
                }
            }
            catch { }

            return Json(cobroView);
        }


        public JsonResult Clientes()
        {
            var list = new List<Dropdown>();
            try
            {
                using (var db = new PviProyectoFinalDB("MyDatabase"))
                {
                    list = db.SpConsultarClientesActivos().Select(_ => new Dropdown 
                    {
                        Id = _.Id_persona,
                        Nombre = _.Cliente
                    }).ToList();
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