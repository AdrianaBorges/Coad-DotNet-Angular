using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADAGENDA.Controllers
{
    public class ProdutoController : Controller
    {
        private ProdutosSRV _service = new ProdutosSRV();
        private LinhaProdutoSRV _serviceLin = new LinhaProdutoSRV();
        private ProdutoFamiliaSRV _serviceFam = new ProdutoFamiliaSRV(); 
        private void _PreencherViewBagCombos()
        {
            IList<AreasCorpDTO> areas = new AreasSRV().FindAll();
            IList<GrupoDTO> grupos = new GrupoSRV().FindAll();
            IList<UnidadeMedidaDTO> unidades = new UnidadeMedidaSRV().FindAll();
            IList<TipoProdComportamentoDTO> tipoComportamento = new TipoProdComportamentoSRV().FindAll();
            IList<TipoProdutoDTO> tipoProd = new TipoProdutoSRV().FindAll();
            IList<UnidadeMedidaDTO> unidadeMedida = new UnidadeMedidaSRV().FindAll();
            IList<LinhaProdutoDTO> _LinhaProduto = _serviceLin.FindAll();
            IList<ProdutoFamiliaDTO> _FamiliaProduto = _serviceFam.FindAll(); 

            ViewBag.areas = areas;
            ViewBag.grupos = grupos;
            ViewBag.unidades = unidades;
            ViewBag.tipoComportamento = tipoComportamento;
            ViewBag.tipoProd = tipoProd;
            ViewBag.LinhaProduto = _LinhaProduto;
            ViewBag.FamiliaProduto = _FamiliaProduto; 
            ViewBag.unidadeMedida = unidadeMedida;
        }

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            IList<AreasCorpDTO> areas = new AreasSRV().FindAll();
            IList<GrupoDTO> grupos = new GrupoSRV().FindAll();           
            IList<TipoProdutoDTO> tipoProd = new TipoProdutoSRV().FindAll();

            ViewBag.areas = areas;
            ViewBag.grupos = grupos;
            ViewBag.tipoProd = tipoProd;
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Produtos(Boolean? ativo, int? grupoId, int? tipoProdutoId, int? areaId, string sigla, string nome, int pagina = 0)
        {
            Pagina<ProdutosDTO> page = _service.Produtos(ativo, grupoId, tipoProdutoId, areaId, sigla: sigla, nome: nome, pagina: pagina, itensPorPagina: 12);

            JSONResponse response = new JSONResponse();
            response.AddPage("produtos", page);

            return Json(response);
        }

        [Autorizar(Acesso = "Editar")]
        public ActionResult Novo()
        {

            ViewBag.Title = " Produto (Novo) ";

            this._PreencherViewBagCombos();

            return View("Editar");
        }

        [Autorizar(Acesso = "Editar")]
        [HttpPost]
        public ActionResult Editar(int? produtoId)
        {

            if (produtoId != null)
                ViewBag.Title = " Produto (Editar)";

            this._PreencherViewBagCombos();

            ViewBag.produtoId = produtoId;

            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(ProdutosDTO produto)
        {
            JSONResponse resultado = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarProduto(produto);

                    resultado.success = true;
                    resultado.message = Message.Info("Operação realizada com sucesso !!");

                    SysException.RegistrarLog(resultado.message.message, "", SessionContext.autenticado);

                    return Json(resultado);
                }
                else
                {
                    resultado.success = false;
                    resultado.SetMessageFromModelState(ModelState);
                    return Json(resultado);
                }
            }
            catch (Exception ex)
            {
                resultado.success = false;
                resultado.message = Message.Fail(ex);

                SysException.RegistrarLog(resultado.message.message, "", SessionContext.autenticado);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadProduto(int produtoId)
        {
            var produto = _service.FindById(produtoId);
            JSONResponse response = new JSONResponse();
            response.Add("produto", produto);

            return Json(response);
        }

        [Autorizar]
        [HttpPost]
        public ActionResult Remover(int produtoId)
        {

            try
            {
                _service.DeletarProduto(produtoId);

                TempData["message"] = Message.Success("Operadora Deletada com sucesso!");
            }
            catch (Exception e)
            {
                TempData["message"] = Message.Fail(e.Message);
            }
            return RedirectToAction("Index");
            
        }

        [Autorizar]
        [HttpPost]
        public ActionResult Detalhes(int produtoId)
        {
            _PreencherViewBagCombos();
            ViewBag.produtoId = produtoId;
            return View("Detalhes");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult LstProdutos()
        {
            IList<ProdutosDTO> produtos = _service.FindAllValid();
            JSONResponse response = new JSONResponse();
            response.Add("produtos", produtos);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult LstProdutosVenda()
        {
            IList<ProdutosDTO> produtos = _service.FindAllValid(true);
            JSONResponse response = new JSONResponse();
            response.Add("produtos", produtos);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarPorNome(string nome)
        {
            IList<ProdutosDTO> lstProdutos = _service.ListarPorNome(nome);
            JSONResponse response = new JSONResponse();
            response.Add("lstProdutos", lstProdutos);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarProdutoAnexoPorNome(string nome)
        {
            IList<ProdutosDTO> lstProdutosAnexos = _service.ListarProdutosAnexosPorNome(nome);
            JSONResponse response = new JSONResponse();
            response.Add("lstProdutosAnexos", lstProdutosAnexos);

            return Json(response);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult ListarPorNomeAutocomplete(string nome)
        {
            IList<AutoCompleteDTO<int>> lstProdutos = _service.ListarPorNomeAutocomplete(nome);
            JSONResponse response = new JSONResponse();
            response.Add("lstProdutos", lstProdutos);

            return Json(response);
        }
    }
}
