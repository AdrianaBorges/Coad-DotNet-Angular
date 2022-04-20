using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class RetornoController : Controller
    {
        public ContaSRV _serviceConta { get; set; }
        public ParcelasSRV _serviceParcela { get; set; }
        public ParcelaLiquidacaoSRV _serviceLiquidacao { get; set; }
        public OcorrenciaRetornoSRV _serviceOcorrencia { get; set; }
        public ParcelasRemessaSRV _serviceParcelaRemessa { get; set; }
        public CnabArquivosSRV _serviceRetorno { get; set; }
        public CnabArquivosItemErroSRV _serviceErro { get; set; }
        public DocLiquidacaoSRV _serviceDocLiq { get; set; }

        [Autorizar(IsAjax = true)]
        public void CarregarListas()
        {
            ViewBag.empresa = new EmpresaSRV().FindAll().Select(c => new SelectListItem() { Text = c.EMP_ID.ToString() + " - " + c.EMP_NOME_FANTASIA, Value = c.EMP_ID.ToString() });
            ViewBag.banco = new BancosSRV().BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            ViewBag.bco = new BancosSRV().FindAll().Select(c => new SelectListItem() { Text = c.BAN_ID + " " + c.BAN_NOME, Value = c.BAN_ID });
            ViewBag.docLiq = _serviceDocLiq.FindAll().Select(c => new SelectListItem() { Text = c.DLI_SIGLA + " " + c.DLI_DESCRICAO, Value = c.DLI_SIGLA });
            ViewBag.remessa = _serviceParcelaRemessa.Listar().Select(c => new SelectListItem() { Text = c.REM_ID + " Banco - " + c.BAN_ID, Value = c.REM_ID.ToString() });
            ViewBag.retorno = _serviceRetorno.Listar().Select(c => new SelectListItem() { Text = c.CNQ_ID + " - " + c.CNQ_NOME, Value = c.CNQ_ID.ToString() });

        }
        public ActionResult Index()
        {
            this.CarregarListas();

            return View();
        }
        public ActionResult Auditoria()
        {
            this.CarregarListas();

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarOcorrenciaRetorno(string _ban_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                
                var _lstOcorrenciaRetorno = _serviceOcorrencia.LerOcorrenciaRetorno(_ban_id);


                if (_lstOcorrenciaRetorno.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.Add("lstOcorrenciaRetorno", _lstOcorrenciaRetorno);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }



        [Autorizar(IsAjax = true)]
        public ActionResult BuscarAuditoriaRetorno(int? _cnq_id
                                                 , int? _rem_id
                                                 , string _parNumParcela
                                                 , string _parNossoNumero
                                                 , string _ban_id
                                                 , string _oct_codigo
                                                 , int _ipe_id = 0
                                                 , int _ppi_id = 0
                                                 , string _cnqnome = null
                                                 , int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                if ((_cnq_id == null || _cnq_id == 0) &&
                    (_rem_id == null || _rem_id == 0) &&
                    (_ban_id == null || _ban_id == "") &&
                    (_ipe_id == 0) && (_ppi_id == 0) &&
                    (_oct_codigo == null || _oct_codigo == "")   &&
                    (String.IsNullOrWhiteSpace(_parNumParcela))  &&
                    (String.IsNullOrWhiteSpace(_parNossoNumero)) &&
                    (String.IsNullOrWhiteSpace(_cnqnome)))
                    throw new Exception("É necessario informar ao menos um paramentro.");


                if (_parNossoNumero == "")
                    _parNossoNumero = null;

                if (_parNumParcela == "")
                    _parNumParcela = null;
                
                var _ret = _serviceParcela.BuscarAuditoriaRetorno(_cnq_id, _rem_id, _parNumParcela, _parNossoNumero, _ban_id, _oct_codigo, _ipe_id, _ppi_id, _cnqnome, _pagina);
                var _lstretorno = _ret.PAGINA_CONCILIACAO;

                
                if (_lstretorno.lista.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.AddPage("lstretorno", _lstretorno);
                response.Add("totalarqu", _ret.PAR_VLR_TOTAL);
                response.Add("totalpago", _ret.PAR_VLR_PAGO);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarErroRetorno(int? _cnq_id, string _parNumParcela, int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var _lstItemErro = _serviceErro.Listar((int)_cnq_id, _pagina, 8);


                if (_lstItemErro.lista.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.AddPage("lstItemErro", _lstItemErro);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarParcelasRetorno(int? _cnq_id, string _parNumParcela, int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {
  
                var _lstItemRetorno = _serviceParcela.BuscarparcelasRetorno(_cnq_id, _parNumParcela, _pagina, 8);
                 

                if (_lstItemRetorno.lista.Count() == 0 )
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.AddPage("lstItemRetorno", _lstItemRetorno);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        
        [Autorizar(IsAjax = true)]
        public ActionResult ListarRetorno(DateTime? _dtini, DateTime? _dtfinal, string _ban_id, string _nome, int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (_dtini == null || _dtfinal == null)
                    throw new Exception("Data não informada !!");

                var _listaretorno = _serviceRetorno.ListarArquivos((DateTime)_dtini, (DateTime)_dtfinal, _ban_id, _nome, _pagina);

                if (_listaretorno.lista.Count() == 0)
                    throw new Exception("Nenhum registro encontrado para o período selecionado!!");

                response.success = true;
                response.AddPage("listaretorno", _listaretorno);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BuscarParcela(string _parNumParcela)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _parRetorno = _serviceParcela.FindById(_parNumParcela);

                response.success = true;
                response.Add("parRetorno", _parRetorno);
                response.message = Message.Info("Processada com sucesso !!");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ProcessarArquivoRetorno(HttpPostedFileBase upload)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (Request.Files.Count == 0 || Request.Files[0].FileName == "")
                    throw new Exception("Erro ao processar arquivo. Arquivo vazio, Verifique!!");

                StreamReader linhas = new StreamReader(Request.Files[0].InputStream);

                _serviceParcela.ProcessarArquivoRetornoNovo(Request.Files[0], Request.Files[0].FileName, linhas);

                response.success = true;
                response.message = Message.Info("Processada com sucesso !!");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ProcessarRetorno(int _cnq_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _serviceParcela.ProcessarRetorno(_cnq_id);
     
                response.success = true;
                response.message = Message.Info("Processada com sucesso !!");
         
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(SysException.Show(ex));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
