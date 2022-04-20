using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Fiscal
{
    public class VendasCartaoController : Controller
    {

        //
        // GET: /VendasCartao/
        public TotalVendasCartaoSRV _service = new TotalVendasCartaoSRV();
        public List<SelectListItem> ListaMeses = new List<SelectListItem>();

        [Autorizar]
        public ActionResult Index()
        {   
            ViewBag.Title = "Movimentação Financeira (Pesquisar)";

            this.Carregarlistas();

            return View();
        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Novo()
        {
            ViewBag.Title = "Movimentação Financeira (Novo)";

            this.Carregarlistas();

            return View();
        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int _emp_id, int _for_id, int _mes, int _ano)
        {
            ViewBag.Title = "Movimentação Financeira (Editar)";

            this.Carregarlistas();

            ViewBag.EMP_ID = _emp_id;
            ViewBag.FOR_ID = _for_id;
            ViewBag.TVC_MES = _mes;
            ViewBag.TVC_ANO = _ano;

            return View();
        }

        [Autorizar]
        public void Carregarlistas()
        {

            //-----

            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
          
            //----- Buscar apenas fornecedores que são operadoras de cartão de crédito
            List<FornecedorDTO> ListaFornecedor = new FornecedorSRV().BuscarPorTipo(1).ToList();
            //-----
            
            ListaMeses.AddRange(new[]{
                            new SelectListItem() { Text = "Janeiro", Value = "1" },
                            new SelectListItem() { Text = "Fevereiro", Value = "2" },
                            new SelectListItem() { Text = "Março", Value = "3" },
                            new SelectListItem() { Text = "Abril", Value = "4" },
                            new SelectListItem() { Text = "Maio", Value = "5" },
                            new SelectListItem() { Text = "Junho", Value = "6" },
                            new SelectListItem() { Text = "Julho", Value = "7" },
                            new SelectListItem() { Text = "Agosto", Value = "8" },
                            new SelectListItem() { Text = "Setembro", Value = "9" },
                            new SelectListItem() { Text = "Outubro", Value = "10" },
                            new SelectListItem() { Text = "Novembro", Value = "11" },
                            new SelectListItem() { Text = "Dezembro", Value = "12" }
            });

            //-----

            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaFornecedor = new SelectList(ListaFornecedor, "FOR_ID", "FOR_RAZAO_SOCIAL");
         
        }

        #region JSON
        /// <summary>
        /// Metodos acionados via controller Javascript via JSON
        /// </summary>
        /// <param name="TotVendas"></param>
        /// <returns></returns>
        /// 
        [Autorizar(IsAjax = true)]
        public ActionResult Pesquisar(int? _mesatual, int? _anoatual, int? _emp_id, int numpagina=1, int linhas=10)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                Pagina<TotalVendasCartaoDTO> a = null;

                if (_emp_id != null )
                    a = _service.BuscarPorEmpresa((int)_emp_id, _mesatual, _anoatual, numpagina, linhas);
                else
                {
                    resultado.success = false;
                    resultado.message = Message.Fail("Informe a empresa para realizar a consulta!! ");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                if (a.lista != null && a.lista.Count() > 0)
                {
                    resultado.AddPage("ListaVendas", a);
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    resultado.success = false;
                    resultado.message = Message.Fail("Nenhum registro encontrado!! ");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public ActionResult CarregaTela(int? _emp_id, int? _for_id, int? _mesatual, int? _anoatual)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                TotalVendasCartaoDTO a = null;

                if (_emp_id != null)
                    a = _service.BuscarPorEmpresa((int)_emp_id, (int)_for_id, (int)_mesatual, (int)_anoatual);
                else
                {
                    resultado.success = false;
                    resultado.message = Message.Fail("Informe a empresa para realizar a consulta!! ");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }

                if (a != null )
                {
                    resultado.success = true;
                    resultado.Add("VendasCartao", a);
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    resultado.success = false;
                    resultado.message = Message.Fail("Nenhum registro encontrado!! ");
                    return Json(resultado, JsonRequestBehavior.AllowGet);
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
        public ActionResult Incluir(TotalVendasCartaoDTO venda)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                _service.Save(venda);

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
        public ActionResult Salvar(TotalVendasCartaoDTO venda)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                _service.Merge(venda , "EMP_ID", "FOR_ID", "TVC_MES", "TVC_ANO");

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
        public ActionResult Excluir(TotalVendasCartaoDTO venda)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                _service.Delete(venda , "EMP_ID", "FOR_ID", "TVC_MES", "TVC_ANO");

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
