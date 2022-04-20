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
    public class TransportadorController : Controller
    {

        public List<SelectListItem> ListaTipoPessoa = new List<SelectListItem>();
        private TransportadorSRV _service = new TransportadorSRV();

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
           this.Carregarlistas();

           return View();
        }

        [Autorizar(Acesso = "Editar")]
        public ActionResult Novo()
        {
            ViewBag.Title = " Transportador (Novo) ";

            this.Carregarlistas();

            return View("Editar");
        }

        [Autorizar(Acesso = "Editar")]
        [HttpPost]
        public ActionResult Editar(int? _tra_id)
        {
            if (_tra_id != null)
               ViewBag.Title = " Transportador (Editar) ";

            ViewBag.tra_id = _tra_id;

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

            ViewBag.ListaPais = new SelectList(ListaPais, "PAI_ID", "PAI_NOME");
            ViewBag.ListaTipoPessoa = new SelectList(ListaTipoPessoa, "Value", "Text");
            ViewBag.ListaTipoFornecedor = new SelectList(ListaTipoFornecedor, "TIPO_FOR_ID", "TIPO_FOR_DESCRICAO");

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
        public ActionResult Pesquisar(string razaosocial, int pagina = 1, int itensPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();
            if (razaosocial != null)
            {
                Pagina<TransportadorDTO> page = _service.BuscarPorRazaoSocial(razaosocial, pagina, itensPorPagina);
                response.AddPage("listaTransportador", page);
                response.success = true;
            }
            else
            {
                response.success = false;
                response.message = Message.Fail("Informe a razao social ou parte dela.");
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarTela(int? _tra_id)
        {
            JsonResponse resultado = new JsonResponse();

            try
            {
              
                if (_tra_id == null)
                    throw new Exception("Erro ao carregar o transportador  " + _tra_id.ToString());

                TransportadorDTO a = _service.FindByIDFull((int)_tra_id);

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
        public ActionResult Incluir(TransportadorDTO _transportador)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
             

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
           
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(TransportadorDTO _transportador)
        {
            JSONResponse resultado = new JSONResponse();
            try
            {
                if (_transportador.TRA_ID > 0)
                {
                   _service.Merge(_transportador, "TRA_ID");
                }
                else
                {
                    _service.Save(_transportador);
                }

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

        [Autorizar(IsAjax = true)]
        public ActionResult Excluir(TransportadorDTO _transportador)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                _service.Delete(_transportador, "TRA_ID");

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
