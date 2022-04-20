using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class FeriadoController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.ano = DateTime.Now.Year;
 
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDiasNaoUteis(DateTime _dataini, DateTime _datafim)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                FeriadoSRV _feriado = new FeriadoSRV();

                var qtdeferiado = _feriado.BuscarDiasNaoUteis(_dataini, _datafim);

                response.Add("qtdeferiado", qtdeferiado);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDiasUteisFeriado(DateTime _dataini, DateTime _datafim)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                FeriadoSRV _feriado = new FeriadoSRV();

                var qtdeferiado = _feriado.BuscarDiasUteisFeriado(_dataini, _datafim);

                response.Add("qtdeferiado", qtdeferiado);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarFeriado(int? _ano)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                FeriadoSRV _feriado = new FeriadoSRV();

                var listaferiados = _feriado.ListarAgrupado((int)_ano);

                response.Add("listaferiados", listaferiados);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(FeriadoDTO _item)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                FeriadoSRV _feriado = new FeriadoSRV();

                _feriado.SaveOrUpdate(_item);

                response.success = true;
                response.message = Message.Info("Incluído com sucesso !!");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Excluir(FeriadoDTO _item)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                FeriadoSRV _feriado = new FeriadoSRV();

                _feriado.Delete(_item);

                response.success = true;
                response.message = Message.Info("Excluído com sucesso!!");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
