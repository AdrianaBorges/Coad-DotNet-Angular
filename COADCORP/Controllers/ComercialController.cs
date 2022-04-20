using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Excel;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers
{
    public class ComercialController : Controller
    {
        [Autorizar(IsAjax = true)]
        public ActionResult SemanaFaturamento()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ApuracaoVendas()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult MetaRepresentante()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Encarteiramento()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Oferta()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public void Carregarlistas()
        {
            ViewBag.grupos = new GrupoSRV().FindAll();

            ViewBag.ListaBancos = new BancosSRV().BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });


        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(  int? _grupo_id
                                        , int _vigencia = 0
                                        , int _atraso = 0
                                        , bool _quitado = false
                                        , int? _qtdecontratos = null
                                        , string _anocoad = null
                                        , string _uf = null)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                string _nomearquivo = null;

                var _listaClientesAtivos = ServiceFactory.RetornarServico<ClienteSRV>().ClientesAtivosLista(_grupo_id
                                                                                                          , _vigencia
                                                                                                          , _atraso
                                                                                                          , _quitado
                                                                                                          , _qtdecontratos
                                                                                                          , _anocoad
                                                                                                          , _uf);

                if (_nomearquivo == null)
                    _nomearquivo = DateTime.Now.Day.ToString() +
                                   DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString() +
                                   DateTime.Now.Millisecond.ToString() +
                                   DateTime.Now.Minute.ToString();

                var _retorno = new ExcelLoad().Export(_listaClientesAtivos, _nomearquivo, System.Web.HttpContext.Current);

                SysException.RegistrarLog("Arquivo gerado com sucesso (" + _retorno + ")", "", SessionContext.autenticado);

                response.Add("retorno", _retorno);
                response.success = true;
                response.message = Message.Info("Arquivo gerado com sucesso" + _retorno);

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
        public ActionResult BuscarClientesAtivos(int? _grupo_id
                                                ,int _vigencia = 0
                                                ,int _atraso = 0
                                                ,bool _quitado = false
                                                ,int? _qtdecontratos =null
                                                ,string _anocoad = null
                                                ,string _uf = null
                                                ,int _pagina = 1)
        {
         


            JSONResponse response = new JSONResponse();
            try
            {
                var _listaClientesAtivos = ServiceFactory.RetornarServico<ClienteSRV>().ClientesAtivos(_grupo_id 
                                                                                                       ,_vigencia
                                                                                                       ,_atraso 
                                                                                                       ,_quitado
                                                                                                       , _qtdecontratos
                                                                                                       , _anocoad
                                                                                                       , _uf
                                                                                                       , _pagina);
                response.AddPage("listaClientesAtivos", _listaClientesAtivos);
                response.success = true;

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
        public ActionResult ListarApuracaoVendas(int _mes, int _ano, int _repid, bool _semana)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (_semana)
                {
                    var _lstApuracaoVendas = ServiceFactory.RetornarServico<ApuracaoPremiacaoSemanaSRV>().ListarApuracaoVendas(_mes, _ano, _repid);
                    response.Add("lstApuracaoVendas", _lstApuracaoVendas);
                }
                else {
                    var _lstApuracaoVendas = ServiceFactory.RetornarServico<ApuracaoPremiacaoMensalSRV>().ListarApuracaoVendas(_mes, _ano, _repid);
                    response.Add("lstApuracaoVendas", _lstApuracaoVendas);
                }
                response.success = true;

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
        public ActionResult SalvarMetaRepresentante(SemanaPremiacaoReprDTO _meta)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<SemanaPremiacaoReprSRV>().SaveOrUpdateNonIdentityKeyEntity(_meta);
                response.success = true;

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
        public ActionResult ListarSemanaRep(int _semana, DateTime _dtini, DateTime _dtfim, int _repid)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lstMetaRep = ServiceFactory.RetornarServico<SemanaPremiacaoReprSRV>().ListarMetaSemanaRep(_semana, _dtini, _dtfim, _repid);
                response.Add("lstMetaRep", _lstMetaRep);
                response.success = true;

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
        public ActionResult TransfCarteira(string _car_id, CarteiraAssinaturaDTO _cartAssin)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<CarteiraAssinaturaSRV>().TransfAssinatura(_car_id, _cartAssin);
                response.success = true;

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
        public ActionResult ListarClientesCarteira(string _car_id, string _asn_num_assinatura)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lstClientesCarteira = ServiceFactory.RetornarServico<CarteiraAssinaturaSRV>().BuscarClientes(_car_id, _asn_num_assinatura);
                response.Add("lstClientesCarteira", _lstClientesCarteira);
                response.success = true;

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
        public ActionResult ListarSemanas(int _PEF_MES, int _PEF_ANO)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                var _lstSemanas = ServiceFactory.RetornarServico<PeriodoFaturamentoSRV>().ListarSemanas(_PEF_MES, _PEF_ANO);
                response.Add("lstSemanas", _lstSemanas);
                response.success = true;

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
        public ActionResult SalvarSemanas(List<PeriodoFaturamentoDTO> _semanas)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lstSemanas = ServiceFactory.RetornarServico<PeriodoFaturamentoSRV>().SaveOrUpdateAll(_semanas);
                response.success = true;

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



    }
}
