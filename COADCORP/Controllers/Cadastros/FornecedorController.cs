using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COADCORP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class FornecedorController : Controller
    {
        public List<SelectListItem> ListaTipoPessoa = new List<SelectListItem>();
        private FornecedorSRV _service = new FornecedorSRV();
        //
        // GET: /Fornecedor/
        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            ViewBag.Title = "Fornecedor (Pesquisar) ";

            this.Carregarlistas();

            return View();
        }

        [Autorizar(Acesso = "Editar")]
        public ActionResult Novo()
        {
        
            ViewBag.Title = "Fornecedor (Novo) ";
            
            this.Carregarlistas();

            return View("Editar");
        }

        [Autorizar(Acesso = "Editar")]
        [HttpPost]
        public ActionResult Editar(int? _for_id)
        {
            if (_for_id != null)
                ViewBag.Title = " Fornecedor (Editar) ";
                       
            ViewBag.for_id = _for_id;

            this.Carregarlistas();

            return View();
        }
   
        public void Carregarlistas()
        {

            //----- Buscar apenas fornecedores que são operadoras de cartão de crédito
            List<TipoFornecedorDTO> ListaTipoFornecedor = new TipoFornecedorSRV().FindAll().ToList();
            List<PaisDTO> ListaPais = new PaisSRV().FindAll().ToList();
            //-----

            ListaTipoPessoa.AddRange(new[]{
                            new SelectListItem() { Text = "Física", Value = "F" },
                            new SelectListItem() { Text = "Jurídica", Value = "J" }
            });

            //-----

            ViewBag.ListaTipoPessoa = new SelectList(ListaTipoPessoa, "Value", "Text");
            ViewBag.ListaTipoFornecedor = new SelectList(ListaTipoFornecedor, "TIPO_FOR_ID", "TIPO_FOR_DESCRICAO");
            ViewBag.ListaPais = new SelectList(ListaPais, "PAI_ID", "PAI_NOME");

        }

        #region JSON
        /// <summary>
        /// Metodos acionados via controller Javascript via JSON
        /// </summary>
        /// <param name="TotVendas"></param>
        /// <returns></returns>
        /// 
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Pesquisar(string razaosocial, int pagina=1, int itensPorPagina=10)
        {
            JSONResponse response = new JSONResponse();
            if (razaosocial != null)
            {
                Pagina<FornecedorDTO> page = _service.BuscarPorRazaoSocial(razaosocial, pagina, itensPorPagina);
                response.AddPage("listaFornecedor", page);
            }
            else
            {
                response.success = false;
                response.message = Message.Fail("Informe a razao social ou parte dela.");
            }
            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregaTela(int? _for_id)
        {
            JsonResponse resultado = new JsonResponse();

            try
            {
             
                if (_for_id == null)
                    throw new Exception("Erro ao carregar o fornecedor  " + _for_id.ToString());

                FornecedorDTO a = new FornecedorSRV().FindByIDFull((int)_for_id);

                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.Message = "Erro ao carregar os dados ( " + SysException.Show(ex) + " )";

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(FornecedorDTO _fornecedor)
        {
            JSONResponse resultado = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    if (_fornecedor.FOR_ID > 0)
                        _service.Merge(_fornecedor, "FOR_ID");
                    else
                        _service.Save(_fornecedor);
                    
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
            catch(Exception ex)
            {
                resultado.success = false;
                resultado.message = Message.Fail(ex);

                SysException.RegistrarLog(resultado.message.message, "", SessionContext.autenticado);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult Excluir(FornecedorDTO _fornecedor)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                _service.Delete(_fornecedor, "FOR_ID");

                resultado.success = true;
                resultado.message = Message.Info("Operação realizada com sucesso !!");

                SysException.RegistrarLog(resultado.message.message, "", SessionContext.autenticado);

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                resultado.success = false;
                resultado.message = Message.Fail(ex);
                
                SysException.RegistrarLog(resultado.message.message, "", SessionContext.autenticado);
                 
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
    }
}
