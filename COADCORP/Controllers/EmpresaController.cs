using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.FiltersInfo;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using GenericCrud.Controllers;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class EmpresaController : GenericController<EMPRESA, EmpresaModel, int>    
    {
        public EmpresaController() : base()
        {

        }
        public EmpresaController(EmpresaSRV _service) : base(_service)
        {
            this._service = _service;
        }

        private EmpresaSRV _service { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        private void CarregarListas()
        {

            var lstempresa = new EmpresaSRV().FindAll();
            var lstbanco = new BancosSRV().FindAll();

            ViewBag.empresa = new SelectList(lstempresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            ViewBag.banco = new SelectList(lstbanco, "BAN_ID", "BAN_NOME");

        }
        public ActionResult Editar(int? EMP_ID)
        {

            ViewBag.ctaId = EMP_ID;

            this.CarregarListas();

            return View();


            //JSONResponse response = new JSONResponse();

            //try
            //{
            //    var _empresa = _service.FindById(EMP_ID);
            //    response.Add("empresa", _empresa);

            //}
            //catch (Exception e)
            //{
            //    response.success = false;
            //    response.message = Message.Fail(e);
            //}
            //return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(PorMenu = false)]
        public ActionResult SalvarConta(EmpresaModel empresa)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                _service.SaveOrUpdate(empresa);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult BuscarEmpresa(EmpresaFiltrosDTO filtros)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _lstempresa = _service.BuscarEmpresas(filtros);
                response.AddPage("lstempresa", _lstempresa);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarEmpresas()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstEmpresas = ServiceFactory.RetornarServico<EmpresaSRV>().FindAll();
                response.Add("lstEmpresas", lstEmpresas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}
