using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using GenericCrud.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class TabelaPrecoController : Controller
    {
        public TabelaPrecoSRV _service { get; set; }
        public RegiaoTabelaPrecoSRV _regiaoTabPreSRV { get; set; }
        public ProdutoComposicaoTipoPeriodoSRV _produtoComposicaoTipoPeriodo { get; set; }

        private void _PreencherViewBagCombos()
        {
            IList<ProdutosDTO> produtos = ServiceFactory.RetornarServico<ProdutosSRV>().FindAllValid(true);
            IList<TipoProdutoComposicaoDTO> tiposProduto = ServiceFactory.RetornarServico<TipoProdutoComposicaoSRV>().FindAll();
            IList<TipoEnvioDTO> tipoEnvio = ServiceFactory.RetornarServico<TipoEnvioSRV>().FindAll();
            IList<TipoPeriodoDTO> tipoPeriodo = ServiceFactory.RetornarServico<TipoPeriodoSRV>().FindAll();
            IList<UnidadeNegocioDTO> unidadeNegocio = ServiceFactory.RetornarServico<UnidadeNegocioSRV>().FindAll();

            ViewBag.produtos = produtos;
            ViewBag.tiposProduto = tiposProduto;
            ViewBag.tipoEnvio = tipoEnvio;
            ViewBag.tipoPeriodo = tipoPeriodo;
            ViewBag.unidadeNegocio = unidadeNegocio;
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(TabelaPrecoSaveRequestDTO tabelaPrecoSaveRequest)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                _service.SalvarTabelaPreco(tabelaPrecoSaveRequest);
                result.message = Message.Success("Dados salvos com sucesso.");
            }
            catch(Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            
            return Json(result);

        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ValidarTabelaPreco(TabelaPrecoDTO tabelaPreco)
        {
            JSONResponse result = new JSONResponse();
            if (ModelState.IsValid)
            {
                return Json(result);

            }
            else
            {
                result.success = false;
                result.SetMessageFromModelState(ModelState);
                return Json(result);
            }
        }

        //[Autorizar]
        //[HttpPost]
        //public ActionResult ReadComposicao(int composicaoId)
        //{
        //    var produtoComposicao = _service.FindById(composicaoId);
        //    JSONResponse response = new JSONResponse();
        //    response.Add("produtocomposicao", produtoComposicao);

        //    return Json(response);
        //}


        //[Autorizar]
        //[HttpPost]
        //public ActionResult Remover(int composicaoId)
        //{
        //    try
        //    {
                
        //        TempData["message"] = Message.Success("Composição Deletada com sucesso!");
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["message"] = Message.Fail(e.Message);
        //    }
        //    return RedirectToAction("Index");

        //}

        //[Autorizar]
        //[HttpPost]
        //public ActionResult Detalhes(int composicaoId)
        //{
        //    _PreencherViewBagCombos();
        //    ViewBag.composicaoId = composicaoId;
        //    return View("Detalhes");
        //}

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadTabelaPreco(int? CMP_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var tabelaPreco = _service.GetTabelaPrecoByComposicao((int) CMP_ID);
                response.Add("tabelaPreco", tabelaPreco);

                if (CMP_ID != null)
                {
                    bool ehCurso = new ProdutoComposicaoSRV().ChecaProdutoComposicaoEhCurso((int)CMP_ID);
                    response.Add("ehCurso", ehCurso);
                }
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
        public ActionResult ListarRegiaoTabelaPreco(int? CMP_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var RG_ID = SessionUtil.GetRegiao();
                var regiaoTabelaPreco = _regiaoTabPreSRV.ListarRegiaoTabelaPrecoPorRegiaoEProdutoComposicao(RG_ID, CMP_ID);
                response.Add("regiaoTabelaPreco", regiaoTabelaPreco);
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
        public ActionResult ListarResumoDeParcelamento(int? CMP_ID, int? TPG_ID, int? QTD, int? TTP_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var RG_ID = SessionUtil.GetRegiao();
                var regiaorResumoParcelas = _regiaoTabPreSRV.ListarResumoDeParcelamento(RG_ID, CMP_ID, TPG_ID, QTD, TTP_ID);
                response.Add("regiaoResumoParcelas", regiaorResumoParcelas);
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
        public ActionResult GetCombosTabelaPreco()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var condicaoPagamento = new CondicaoPagamentoSRV().FindAll();
                var tipoPagamento = new TipoPagamentoSRV().FindAll();
                var tipoPagamentoSimples = new TipoPagamentoSRV().ListarTipoPagamentoSimples();
                var lstRegioes = new RegiaoSRV().FindAll();

                response.Add("lstCondicaoPagamento", condicaoPagamento);
                response.Add("lstTipoPagamento", tipoPagamento);
                response.Add("lstTipoPagamentoSimples", tipoPagamentoSimples);
                response.Add("lstRegioes", lstRegioes);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

    }
}
