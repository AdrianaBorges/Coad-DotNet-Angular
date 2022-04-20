using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.FiltersInfo;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using GenericCrud.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class ContaController :  GenericController<CONTA, ContaDTO, int>
    {

        public ContaController() : base()
        {

        }
        public ContaController(ContaSRV _service) : base(_service)
        {
            this._service = _service;
        }
        private ContaSRV _service { get; set; }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Editar(int? CTA_ID)
        {
            ViewBag.ctaId = CTA_ID;

            this.CarregarListas();

            return View();
        }
        [Autorizar(PorMenu = false)]
        public ActionResult BuscarConta(ContaFiltrosDTO filtros)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _lstconta = _service.BuscarContas(filtros);
                response.AddPage("lstconta", _lstconta);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(PorMenu = false)]
        public ActionResult CarregarConta(int? CTA_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _conta = _service.FindById(CTA_ID);
                response.Add("conta", _conta);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult SalvarConta(ContaDTO conta)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                _service.SalvarConta(conta);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        private void CarregarListas()
        {

            var lstempresa = new EmpresaSRV().FindAll();
            var lstbanco = new BancosSRV().FindAll();

            ViewBag.empresa = new SelectList(lstempresa, "EMP_ID", "EMP_RAZAO_SOCIAL");
            ViewBag.banco = new SelectList(lstbanco, "BAN_ID", "BAN_NOME");

        }

    }
}
