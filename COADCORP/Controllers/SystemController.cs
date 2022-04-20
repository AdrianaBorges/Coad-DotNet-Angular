using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Filters;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.FISCAL.Service.Integracoes;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    [AutorizarCustom("AdmSys", PorMenu = false)]
    public class SystemController : Controller
    {
        //
        // GET: /System/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SetMinResources()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                SysUtils.useMinResouces = !SysUtils.useMinResouces;

                SysPropertiesDTO properties = new SysPropertiesDTO()
                {
                    Homologacao = SysUtils.InHomologation(),
                    CodAdquirente = SysUtils.BuscarCodAdiquirente(),
                    EmailTeste = SysUtils.RetornaEmailDeTeste(),
                    EmailTesteAtivo = SysUtils.EmailTesteAtivo(),
                    HostName = SysUtils.RetornarHostName(),
                    PathXML = SysUtils.RetornarPathNFeXML(),
                    UseMinResource = SysUtils.useMinResouces
                };

                response.Add("properties", properties);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarPropriedades()
        {
            JSONResponse response = new JSONResponse();
            try
            {

                SysPropertiesDTO properties = new SysPropertiesDTO()
                {
                    Homologacao = SysUtils.InHomologation(),
                    CodAdquirente = SysUtils.BuscarCodAdiquirente(),
                    EmailTeste = SysUtils.RetornaEmailDeTeste(),
                    EmailTesteAtivo = SysUtils.EmailTesteAtivo(),
                    HostName = SysUtils.RetornarHostName(),
                    PathXML = SysUtils.RetornarPathNFeXML(),
                    UseMinResource = SysUtils.useMinResouces
                };

                response.Add("properties", properties);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcessarBaixaPropostaPedido(bool teste = false)
        {
            JSONResponse result = new JSONResponse();
            try
            {

                ServiceFactory.RetornarServico<ParcelasSRV>().ProcessarBaixaPropostaPedido(false);
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

        [Autorizar(IsAjax = true)]
        public ActionResult RegerarVariasNotasFiscais(string data)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var path = HttpContext.Server.MapPath("~/");
                ServiceFactory
                    .RetornarServico<ItemPedidoSRV>()
                    .RegerarTodasAsNotasFiscais(path);

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

        public ActionResult ProcessarJobPropostasPagAtrasados()
        {
            JSONResponse result = new JSONResponse();
            try
            {

                ServiceFactory.RetornarServico<PropostaItemSRV>().ProcessarJobPropostasPagAtrasados(null);
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


        public ActionResult ProcessarJobNotificacaoPagAtrasados()
        {
            JSONResponse result = new JSONResponse();
            try
            {

                ServiceFactory.RetornarServico<PropostaItemSRV>().ProcessarJobNotiReprePropostasPagAtrasados(null);
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
        public ActionResult ProcessarImportacao()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<ImportacaoSRV>().ProcessarImportacao();
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


        public ActionResult TestarAdicionarPedidoNota(int? codPedido)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var path = Server.MapPath("/");
                var resp = ServiceFactory.RetornarServico<NotaFiscalSRV>().AdicionarPedidoLoteVigente(codPedido, 2);
                result.Add("retornoLote", resp);
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

        public ActionResult EnviarLoteVigente()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().ExecutarTarefaEnviarLoteVigente();
                //result.Add("retornoLote", resp);
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

        public ActionResult ProcessarRetornoLoteEnviado()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().ExecutarTarefaProcessarRetornoLote();
                //result.Add("retornoLote", resp);
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

        public ActionResult TestarCriacaoEmail()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<FilaEmailSRV>().TestarCriacaoDeEmail();
                //result.Add("retornoLote", resp);
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

        public ActionResult TestarNotaServico(int? codItemPedido)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var retorno = ServiceFactory.RetornarServico<NotaFiscalSRV>().TestarNotaServico(codItemPedido);
                result.Add("retorno", retorno);
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


        public ActionResult EnviarLoteVigenteNfse()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().ExecutarTarefaEnviarLoteVigenteNfSe();
                //result.Add("retornoLote", resp);
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

        public ActionResult ProcessarRetornoLoteEnviadoNfse()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().ExecutarTarefaProcessarRetornoLoteNfse();
                //result.Add("retornoLote", resp);
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


        public ActionResult EnviarBoletosEmAberto()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<ParcelasSRV>().EnviarBoletosEmAberto();
                //result.Add("retornoLote", resp);
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
        
        public ActionResult GerarBoleto(string codParcela)
        {
            try
            {
                var bytes = ServiceFactory.RetornarServico<PropostaItemSRV>().RetornarBytesDoBoleto(codParcela);
                return File(bytes, "application/pdf", "Boleto.pdf");
                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                throw new Exception("Não é possível gerar o boleto", ex);
                
            }


        }

        public ActionResult ReexecutarEnvioNfe()
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<NotaFiscalSRV>().ReexecutarCallbackNfeAutorizada();
                //result.Add("retornoLote", resp);
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
