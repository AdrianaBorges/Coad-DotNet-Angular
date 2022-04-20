using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class ProdutoComposicaoController : Controller
    {
        public ProdutoComposicaoSRV _service { get; set; }
        public TipoPeriodoSRV _tipoPeriodoSRV { get; set; }
        public ProdutoComposicaoTipoPeriodoSRV _produtoComposicaoTipoPeriodo { get; set; }
        public TipoVendaSRV _tipoVendaSRV { get; set; }
        public ProdutosSRV _produtoSRV { get; set; }
        public TipoProdutoComposicaoSRV _tipoProdutoComposicaoSRV { get; set; }
        public TipoEnvioSRV _tipoEnvio { get; set; }
        public TipoPeriodoSRV _tipoPeriodo { get; set; }
        public UnidadeNegocioSRV _unidadeNegocio { get; set; }
        public AreasSRV _areasSRV { get; set; }
        public GrupoSRV _grupoSRV { get; set; }
        public TipoProdutoSRV _tipoProduto { get; set; }

        private void _PreencherViewBagCombos()
        {
            IList<ProdutosDTO> produtos = _produtoSRV.FindAllValid(true);
            IList<TipoProdutoComposicaoDTO> tiposProduto = _tipoProdutoComposicaoSRV.FindAll();
            IList<TipoEnvioDTO> tipoEnvio = _tipoEnvio.FindAll();
            IList<TipoPeriodoDTO> tipoPeriodo = _tipoPeriodo.FindAll();
            IList<UnidadeNegocioDTO> unidadeNegocio = _unidadeNegocio.FindAll();

            ViewBag.produtos = produtos;
            ViewBag.tiposProduto = tiposProduto;
            ViewBag.tipoEnvio = tipoEnvio;
            ViewBag.tipoPeriodo = tipoPeriodo;
            ViewBag.unidadeNegocio = unidadeNegocio;
        }

        [Autorizar]
        public ActionResult Index()
        {
            IList<AreasCorpDTO> areas = _areasSRV.FindAll();
            IList<GrupoDTO> grupos = _grupoSRV.FindAll();
            IList<TipoProdutoDTO> tipoProd = _tipoProduto.FindAll();

            ViewBag.areas = areas;
            ViewBag.grupos = grupos;
            ViewBag.tipoProd = tipoProd;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ProdutosComposicao(string nome = null,
            string nomeEstrangeiro = null,
            int pagina = 1,
            int itensPorPaginas = 7,
            bool? produtoInteresse = null,
            int? proId = null,
            int? empId = null)
        {
            var page = _service.ListarProdutosExcetoCursos(nome, produtoInteresse, pagina, itensPorPaginas, empId);
            JSONResponse response = new JSONResponse();
            response.AddPage("produtosComposicao", page);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(TipoAcesso = NivelAcesso.Editar)]
        public ActionResult Novo()
        {
            _PreencherViewBagCombos();
            return View("Edit");
        }

        [Autorizar(TipoAcesso = NivelAcesso.Editar)]
        [HttpPost]
        public ActionResult Editar(int composicaoId)
        {
            _PreencherViewBagCombos();
            ViewBag.composicaoId = composicaoId;
            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(ProdutoComposicaoDTO produtoComposicao)
        {
            JSONResponse result = new JSONResponse();

            JSONResponse response = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var produtoCompostoRetorno = _service.SalvarProdutoComposicao(produtoComposicao);
                    result.Add("produtoComposto", produtoCompostoRetorno);
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                result.success = false;
                result.message = Message.Fail(e);
            }
            return Json(result);
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadComposicao(int composicaoId)
        {
            var produtoComposicao = _service.FindByIdFullLoad(composicaoId, false, true);
            JSONResponse response = new JSONResponse();
            response.Add("produtocomposicao", produtoComposicao);

            if (composicaoId > 0)
            {
                bool ehCurso = _service.ChecaProdutoComposicaoEhCurso(composicaoId);
                response.Add("ehCurso", ehCurso);
            }
            return Json(response);
        }


        [Autorizar(TipoAcesso = NivelAcesso.Excluir)]
        [HttpPost]
        public ActionResult Remover(int composicaoId)
        {
            try
            {
                _service.DeletarProdutoComposicao(composicaoId);
                TempData["message"] = Message.Success("Composição Deletada com sucesso!");
            }
            catch (Exception e)
            {
                TempData["message"] = Message.Fail(e.Message);
            }
            return RedirectToAction("Index");

        }

        //[Autorizar(TipoAcesso = NivelAcesso.Acesso)]
        //[HttpPost]
        //public ActionResult Detalhes(int composicaoId)
        //{
        //    _PreencherViewBagCombos();
        //    ViewBag.composicaoId = composicaoId;
        //    return View("Detalhes");
        //}

        [Autorizar(IsAjax = true)]
        public ActionResult ListarProdutosDeInteresse(string nome = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var produtosComposicao = _service.ProdutosDeInteresse(nome);
                response.Add("produtosComposicao", produtosComposicao);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarProdutosPorUen(int? uenId, string nome = null,
            int pagina = 1,
            int itensPorPaginas = 7,
            bool? produtoInteresse = null,
            int? empId = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var produtosComposicao = _service.ListarProdutosPorUen(uenId, nome, produtoInteresse, pagina, itensPorPaginas, empId);
                response.AddPage("produtosComposicao", produtosComposicao);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarTipoPeriodo()
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoPeriodo = _tipoPeriodoSRV.FindAll();
                response.Add("lstTipoPeriodo", lstTipoPeriodo);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult listarTipoPeriodoDoProdutoComposto(int? CMP_ID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoPeriodo = _tipoPeriodoSRV.ListarTipoPeriodoDoProduto(CMP_ID);
                response.Add("lstTipoPeriodoDoProduto", lstTipoPeriodo);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarTipoVenda()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstTipoVenda = _tipoVendaSRV.FindAll();
                response.Add("lstTipoVenda", lstTipoVenda);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult DeletarProdutoComposicao(int? cmpId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                int? repId = SessionContext.GetIdRepresentante();
                string usuario = SessionContext.login;

                _service.DeletarProdutoComposicao(cmpId);
                SysException.RegistrarLog($"Produto composto {cmpId} marcado como deletado com sucesso!!", "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Proposta marcada manualmente como paga!!");

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
    }
}
