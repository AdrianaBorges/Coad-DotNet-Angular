using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Service;

namespace COADCORP.Controllers.Consultoria
{
    public class URAController : Controller
    {
        //
        // GET: /URA/

        private ClienteSRV _serviceCliente = new ClienteSRV();
        private AssinaturaSRV _srvAssinatura = new AssinaturaSRV();
        private UraLogSRV _srvUraLog = new UraLogSRV();
        private UraCoadSRV _service = new UraCoadSRV();
        private URASRV _srvURA = new URASRV();
        private UraProdutoSRV _srvUraProd = new UraProdutoSRV();
        private UraConfigSRV _srvUraConfig = new UraConfigSRV();
        private UraProdutoAreaSRV _srvUraProdArea = new UraProdutoAreaSRV();
        private HistAtendUraSRV _atendimentoUra = new HistAtendUraSRV();
        private HistAtendEmailSRV _atendimentoEmail = new HistAtendEmailSRV();

        [Autorizar(PorMenu = true)]
        public ActionResult ConsultarURA()
        {
            ViewBag.Title = " Clientes atualizados na URA (Situação Atual) ";

            this.Carregar();

            return View();
        }

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            ViewBag.Title = " URA (Lista Configuração) ";

            this.Carregar();

            return View();
        }
        
        [Autorizar(Acesso = "Editar")]
        public ActionResult Configurar(string _ura_id, int? _pro_id)
        {
            ViewBag.Title = " URA (Configurar) ";

            this.Carregar();

            ViewBag.uraid = _ura_id;
            ViewBag.proid = _pro_id;

            return View();
        }

        [Autorizar(PorMenu = true)]
        public ActionResult ListarConsultas()
        {
            return View();
        }

        [Autorizar(PorMenu = true)]
        public ActionResult PesquisarClientes()
        {
            ViewBag.Title = " Clientes com acesso a URA (COADCORP)";

            this.Carregar();

            return View();
        }
        //[Autorizar(PorMenu = true)]
        public ActionResult AtulizarAssinatura(string assinatura)
        {
            ViewBag.Assinatura = assinatura;
            ViewBag.Title = " Atualizar Assinatura";

            return View();
        }

        public ActionResult PesquisarAssinatura(string _assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _ass = new AssinaturaSRV().FindByIdFullLoaded(_assinatura);

                if (_ass != null)
                    _ass.ASN_ATIVA = new ClienteSRV().VerificarAssinaturaAtiva(_assinatura);

                response.success = true;
                response.Add("assinatura", _ass);
                response.message = Message.Info("Ok");
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        

        [Autorizar(IsAjax = true)]
        public ActionResult ListarConfigURA(string _ura_id, int pagina = 1, int itensPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (_ura_id != null)
                {
                    Pagina<CONFIG_URA_vw> page = _srvURA.ListarConfigUra(_ura_id, pagina, itensPorPagina);
                    response.AddPage("ListaConfig", page);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail("Informe a Ura!!");
                }

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                response.success = false;
                response.message = Message.Fail("Erro ao carregar os dados ( " + SysException.Show(ex) + " )");

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BuscarProdAreas(string _ura_id, int _pro_id, string _uf_sigla )
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _listaProdAreas = _srvUraProdArea.BuscarAreas(_ura_id, _pro_id, _uf_sigla);

                if (_listaProdAreas == null)
                    throw new Exception(@"Nenhum resultado encontrado para a pesquisa.");

                response.success = true;
                response.Add("listaProdAreas", _listaProdAreas);
             
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarClientes(string _cli_nome, int? _asn_id, int pagina = 1, int itensPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();

            if (_cli_nome.Length >= 3)
            {
                Pagina<AssinaturaDTO> page = new AssinaturaSRV().BuscarClientesPorAssinatura("", _cli_nome, pagina, itensPorPagina);

                response.AddPage("listaClientes", page);
            }
            else
            {
                response.success = false;
                response.message = Message.Fail("Informe a no minimo 3 caracteres para realizar a consulta!!");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [Autorizar(IsAjax = true)]
        public ActionResult ListarClientesURA(string _ura_id, string _asn_id, string _telefone, int pagina = 1, int itensPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (_ura_id != null)
                {
                    Pagina<UraCoadDTO> page = _service.BuscarClientes(_ura_id, _asn_id, _telefone, pagina, itensPorPagina);
                    response.AddPage("listaClientes", page);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail("Informe a Ura!!");
                }

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                response.success = false;
                response.message = Message.Fail("Erro ao carregar os dados ( " + SysException.Show(ex) + " )");

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        public void Carregar()
        {

            List<URA> ListaUras = _srvURA.BuscarTodos().ToList();
            List<UraProdutoDTO> ListaProdutos = _srvUraProd.BuscarProdutos("").ToList();

            ViewBag.ListaUras = new SelectList(ListaUras, "URA_ID ", "URA_ID ");
            ViewBag.ListaProdutos = new SelectList(ListaProdutos, "PRO_ID", "PRODUTO.PRO_NOME" );

        }

        #region JSON
        /// <summary>
        /// Metodos acionados via controller Javascript via JSON
        /// </summary>
        /// <returns></returns>
        /// 
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ListarAssinaturaConsultas(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int pagina = 1, int itensPorPagina = 15)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listaQtdeConsultas = _srvAssinatura.ConsultasPorPeriodoPag(_asn_id, _dtini, _dtfim, pagina, itensPorPagina);

                if (listaQtdeConsultas != null)
                {

                    response.success = true;
                    response.AddPage("listaQtdeConsultas", listaQtdeConsultas);
                    response.message = Message.Info("Ok");
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"Nenhum resultado encontrado para a pesquisa.");
                }
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
        public ActionResult Pesquisar(string _ura_id, int? _pro_id, int pagina = 1, int itensPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();
            if (_pro_id != null)
            {

                UraProdutoDTO UraProduto = _srvUraProd.FindById(_ura_id, (int)_pro_id);
                response.Add("UraProduto", UraProduto);

                Pagina<UraConfigDTO> page = _srvUraConfig.BuscarConfiguracaoPaginas(_ura_id, (int)_pro_id, pagina, itensPorPagina);
                response.AddPage("listaUfAcesso", page);

                response.success = true;
            }
            else
            {
                response.success = false;
                response.message = Message.Fail("Informe o produto.");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarUras()
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                List<URA> a = _srvURA.BuscarTodos().ToList();
                resultado.Add("listaUras", a);
                resultado.success = true;

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.success = false;
                resultado.message = Message.Fail("Erro ao carregar os dados ( " + SysException.Show(ex) + " )");

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarProdutos(string _ura_id)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                List<UraProdutoDTO> a = _srvUraProd.BuscarProdutos(_ura_id).ToList();
                resultado.Add("listaprodutos", a);
                resultado.success = true;

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.success = false;
                resultado.message = Message.Fail("Erro ao carregar os dados ( " + SysException.Show(ex) + " )");

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarAreas(int _prod_id, string _ura_id, string _uf_sigla)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                List<AreaConsultoriaDTO> a = new AreaConsultoriaSRV().BuscarNaoCadastrada(_prod_id, _ura_id, _uf_sigla).ToList();
                resultado.Add("listaarea", a);
                resultado.success = true;

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.success = false;
                resultado.message = Message.Fail("Erro ao carregar os dados ( " + SysException.Show(ex) + " )");

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregarUF(int _prod_id, string _ura_id)
        {
            JSONResponse resultado = new JSONResponse();

            try
            {
                List<UFDTO> a = new UFSRV().BuscarNaoCadastrada(_prod_id, _ura_id).ToList();
                resultado.Add("listauf", a);
                resultado.success = true;

                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.success = false;
                resultado.message = Message.Fail("Erro ao carregar os dados ( " + SysException.Show(ex) + " )");

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }
        //---------------
        [Autorizar(IsAjax = true)]
        public ActionResult SalvarProdArea(UraProdutoAreaDTO _uraProdutoArea)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                _srvUraProdArea.Save(_uraProdutoArea);

                resultado.success = true;
     
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
        public ActionResult ExcluirProdArea(UraProdutoAreaDTO _uraProdutoArea)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                _srvUraProdArea.Delete(_uraProdutoArea);

                resultado.success = true;

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
        public ActionResult EditarUraConfig(UraConfigDTO _uraconfig, string _tipo)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                if (_tipo == "I")
                   _srvUraConfig.Save(_uraconfig);
                else
                    _srvUraConfig.Delete(_uraconfig);

                resultado.success = true;
                
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
        public ActionResult EditarUraProduto(UraProdutoDTO _uraproduto, string _tipo)
        {

            JSONResponse resultado = new JSONResponse();

            try
            {
                if (_tipo == "I")
                    _srvUraProd.Merge(_uraproduto);
                else
                    _srvUraProd.Delete(_uraproduto);

                resultado.success = true;
                
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



        [Autorizar(IsAjax = true, GerenteDepartamento = "SAC,TI")]
        public ActionResult AtualizarUra(string _assinatura, int? _qtde, Boolean? _ativo)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                _serviceCliente.AtualizarUra(_assinatura, _qtde, _ativo);

                response.success = true;
                response.message = Message.Info("Atualização das URAs realizada com sucesso!");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDetConsultaUra(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listaQtdeConsEmail = _srvAssinatura.ConsultasPorPeriodo(_asn_id);

                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("listaQtdeConsEmail", listaQtdeConsEmail);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ListaConsultasUra(string _asn_id, string _ura_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var detconsultas = _atendimentoUra.BuscarPorPeriodo(_asn_id, _ura_id);
                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("detconsultas", detconsultas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDetConsultaEmail(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var detconsultas = _atendimentoEmail.BuscarPorPeriodo(_asn_id);

                response.success = true;
                response.message = Message.Info("Ok");
                response.Add("detconsultas", detconsultas);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
