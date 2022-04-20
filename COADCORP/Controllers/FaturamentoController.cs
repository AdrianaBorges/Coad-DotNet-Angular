using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Controllers;
using GenericCrud.Models.Filtros;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class FaturamentoController : GenericController<PEDIDO_CRM, PedidoCRMDTO, int>
    {
        //
        // GET: /Faturamento/
        public PropostaSRV _propostaSRV { get; set; }
        public PropostaItemSRV _propostaItemSRV { get; set; }
        public NotaFiscalLoteItemSRV _notaFiscalLoteItem { get; set; }
        public NotaFiscalLoteSRV _notaFiscalLote { get; set; }
        private PedidoCRMSRV _pedidoSRV { get; set; }
        public NotaFiscalSRV _notaFiscal { get; set; }


        public FaturamentoController(PedidoCRMSRV pedidoCRMSRV) : base(pedidoCRMSRV)
        {
            this._pedidoSRV = pedidoCRMSRV;
            
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Painel()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult PesquisarPropostaPendConf(RequisicaoPaginacao requisicao, DateTime? dataInicial = null, DateTime? dataFinal = null, string query = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var propostPend = _propostaSRV.PesquisarPropostaPendConf(query, dataInicial, dataFinal, requisicao);
                response.AddPage("propostPend", propostPend);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult MarcarManualmenteVariasPropostasComoPaga(List<int> lstPrtId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                var batchResp = _propostaItemSRV.MarcarManualmenteVariasPropostasComoPaga(lstPrtId, repId, usuario);
                result.Add("batchResp", batchResp);

                SysException.RegistrarLog("Propostas marcadas manualmente como paga!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Propostas marcadas manualmente como paga!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarNfeLoteItmComErroOuPendente(RequisicaoPaginacao requisicao)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (requisicao != null)
                    requisicao.registrosPorPagina = 5;

                var lotePend = _notaFiscalLoteItem.ListarNfeLoteItmComErroOuPendente(requisicao);
                response.AddPage("lotePend", lotePend);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult CancelarProcessamentoLote(ICollection<int> lstNflId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _notaFiscalLote.CancelarProcessamentoLote(lstNflId);
                
                SysException.RegistrarLog("Processamento do lote cancelado com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Processamento do lote cancelado com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult CancelarProcessamentoItem(ICollection<int> lstNliId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _notaFiscalLoteItem.CancelarProcessamentoItem(lstNliId);

                SysException.RegistrarLog("Processamento do lote item cancelado com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Processamento do lote item cancelado com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [Autorizar(IsAjax = true)]
        public ActionResult AdicionarVariasNotasAoLoteNFe(ICollection<int> ListCodLoteItm)
        {
            JSONResponse result = new JSONResponse();
            try
            {

                var path = Server.MapPath("/");
                var batchContext = new BatchContext();
                var lotes = _notaFiscal.AdicionarVariasNotasAoLote(ListCodLoteItm, path, batchContext);

                result.Add("batchResp", batchContext);

                if (lotes != null)
                {
                    result.Add("lotes", lotes);
                    result.Add("codLotes", lotes.Select(x => x.LoteID));
                }

                SysException.RegistrarLog("Envio Agendado!", "", SessionContext.autenticado);
                result.success = true;

            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
