using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace COADCORP.Controllers.COADGED
{
    public class ManualdpController : Controller
    {
        private TipoAtoSRV _serviceTpAto = new TipoAtoSRV();
        private OrgaoSRV _serviceOrgao = new OrgaoSRV();
        public void CarregaDados()
        {

            var tpAto = _serviceTpAto.Listar(1);
            var orgao = _serviceOrgao.Listar(1);
          
            ViewBag.tpAto = tpAto.Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });
            ViewBag.orgao = orgao.Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() });

        }
        public ActionResult AbrirDocDocumento(int? param01 = null, int? param02 = null)
        {
            try
            {
                string q = null;

                if (param01!=null&& param02 != null)
                    return RedirectToAction("Sumario", "Manualdp", new {interno = false, mai_id = q , man_id = param02,  mod_id = param01 });
                else
                    return RedirectToAction("Sumario", "Manualdp", new {interno = false, mai_id = param01, man_id = q, mod_id = q });

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

                TempData["message"] = ex.Message;

                return RedirectToAction("Info", "Home");

            }

        }

        /// <summary>
        /// Este metodo é utilizado no portal e apresenta o simulador dinâmico dentro do modulo de simuladores.
        /// </summary>
        /// <param name="id">ID do simulador</param>
        /// <param name="interno">Indica se a chamada é interna ou externa</param>
        /// <returns></returns>
        [Autorizar(PorMenu = false)]
        public ActionResult Sumario(Boolean interno = false, int? mai_id = null, int? man_id = null, int? mod_id = null) 
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return new RedirectResult("/Home/Info?type=Retorne ao portal COAD e acesse novamente o simulador.");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", "aed12df9-79df-4a55-99d3-921f407997b8", code);

                    if (erro != null)
                        throw new Exception(erro);
                }

                ViewBag.mai_id = (mai_id==null)?0:mai_id;
                ViewBag.man_id = (man_id==null)?0:man_id;
                ViewBag.mod_id = (mod_id==null)?0:mod_id;
                ViewBag.tipo = 2;
                ViewBag.checa = (interno == false);

                return View();

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

                TempData["message"] = ex.Message;

                return RedirectToAction("Info", "Home");

            }
          
        }
        public string RegistrarLogSimulador(string _tipoacesso, string _tdc_id, string _code)
        {
            try
            {

                LogSimuladorDTO log = new LogSimuladorDTO();

                //if (_code.IndexOf(Convert.ToChar("@")) > 0)
                //{
                    log.ASN_NUM_ASSINATURA = "GRATUITO";
                    log.LSI_EMAIL_ACESSO = _code;
                //}
                //else
                //    log.ASN_NUM_ASSINATURA = _code;

                log.LSI_DATA_ACESSO = DateTime.Now;
                log.LSI_URL_ACESSO = SessionContext.autenticado.PATH;
                log.LSI_IP_ACESSO = SessionContext.autenticado.IP_ACESSO;
                log.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                log.LSI_TIPO_ACESSO = _tipoacesso;
                log.TDC_ID = _tdc_id;

                log = new LogSimuladorSRV().Save(log);

                return null;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public ActionResult Index()
        {
            SessionContext.PutInSession<string>("LOGIN_PORTAL", SessionContext.autenticado.USU_LOGIN);

            this.CarregaDados();

            return View();
        }
        public ActionResult Configurar()
        {
            return View();
        }
        public ActionResult ListarItensAlterados(int _pagina = 1)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _mdpitem = new ManualDPItemSRV();

                var _ItensAlterados = _mdpitem.BuscarItensAlterados(90,5);

                response.Add("ItensAlterados", _ItensAlterados);
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
        public ActionResult IdentarItem(ManualDPItemDTO _item)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _mdpitem = new ManualDPItemSRV();


                var _qtdeItens = 0;
                var _itemprincipal = _mdpitem.BuscarItemPrincipal(_item);

                if (_itemprincipal != null)
                    _qtdeItens =_mdpitem.BuscarQtdeItens(_itemprincipal.MAI_ID);

                if (_itemprincipal != null)
                {
                    _item.MAI_ID_PAI = _itemprincipal.MAI_ID;
                    _item.MAI_INDEX = _qtdeItens; 
                    _item.MAI_NIVEL += 1;

                    var _itemretorno = new ManualDPItemSRV().Merge(_item);
                }

                var _listaitens = new ManualDPItemSRV().MontarIndiceAssunto(_item.MAN_ID);

                response.Add("listaporassunto", _listaitens);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult RecuarItem(ManualDPItemDTO _item)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _mdpitem = new ManualDPItemSRV();

                var _itemprincipal = _mdpitem.FindById((int)_item.MAI_ID_PAI);


                var _qtdeItens = 0;
                var _idpaidopai = _itemprincipal.MAI_ID_PAI;

                if (_idpaidopai > 0)
                    _qtdeItens = _mdpitem.BuscarQtdeItens((int)_itemprincipal.MAI_ID_PAI);
                else
                    _qtdeItens = _mdpitem.BuscarQtdeItensAssunto((int)_itemprincipal.MAN_ID);

                //if (_qtdeItens < 1)
                //    _qtdeItens = 1;

                if (_itemprincipal != null)
                {
                    _item.MAI_ID_PAI = _idpaidopai;
                    _item.MAI_INDEX = _qtdeItens;
                    _item.MAI_NIVEL = _itemprincipal.MAI_NIVEL;
                    var _itemretorno = new ManualDPItemSRV().Merge(_item);

                }

                var _listaitens = new ManualDPItemSRV().MontarIndiceAssunto(_item.MAN_ID);

                response.Add("listaporassunto", _listaitens);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult Menu()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ListarItensRelacionados(string _titulo)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaitensrel = new ManualDPItemSRV().Listar(_titulo);
                response.success = true;
                response.Add("listaitensrel", _listaitensrel);

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
        public ActionResult Referencia(int id)
        {
            return RedirectToAction("Sumario", new { interno = false, mai_id = id });
        }
        [Autorizar(IsAjax = true)]
        public ActionResult PesquisarAssuntoPorModulo(string _assunto, int _mod_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaassunto = new ManualDPSRV().Listar(_assunto, _mod_id);
                response.success = true;
                response.Add("listaassunto", _listaassunto);

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
        public ActionResult PesquisarAssunto(string _assunto)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaassunto = new ManualDPSRV().Listar(_assunto);
                response.success = true;
                response.Add("listaassunto", _listaassunto);

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
        public ActionResult AbrirDoc(int _mai_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _item = new ManualDPItemSRV().FindById(_mai_id);
                var _links = new ManualDPLinkSRV().Listar(_mai_id);

                foreach (var _lnk in _links)
                {
                    _item.MAI_DESCRICAO = _item.MAI_DESCRICAO.Replace(_lnk.LNK_TAG, _lnk.LNK_LINK);
                }

                response.Add("item", _item);

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
        public ActionResult CarregarTela(int _mai_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _item = new ManualDPItemSRV().FindById(_mai_id);

                response.Add("item", _item);
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
        public ActionResult Pesquisar(ParamConsultaManualDTO _param, int _pagina = 1)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaitens = new ManualDPItemSRV().Pesquisar(_param,_pagina);

                response.AddPage("listaitens", _listaitens);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult PesquisarReferencia(string _mai_titulo)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaitens = new ManualDPItemSRV().Pesquisar(_mai_titulo);

                response.Add("listaitens", _listaitens);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult ListarItensPorAssunto(int _man_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaitens = new ManualDPItemSRV().MontarIndiceAssunto(_man_id);
                
                var _links = new ManualDPLinkSRV().ListarPorModulo(_man_id);
                
                foreach (var _item in _listaitens)
                {
                    foreach (var _lnk in _links)
                    {
                        _item.MAI_DESCRICAO = _item.MAI_DESCRICAO.Replace(_lnk.LNK_TAG, _lnk.LNK_LINK);
                    }
                }
                

                response.Add("listaporassunto", _listaitens);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult ListarModulos()
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listamodulos = new ManualDPModuloSRV().BuscarModulos();

                response.Add("listamodulo", _listamodulos);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult BuscarModuloAssunto(int _mod_id, int _man_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _modulo = new ManualDPModuloSRV().FindById(_mod_id);
                var _assunto = new ManualDPSRV().FindById(_man_id);

                response.Add("modulo", _modulo);
                response.Add("assunto", _assunto);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult BuscarPalavraChave(string _mai_descricao, int _pagina = 1)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listaitens = new ManualDPItemSRV().ListarPorPagina(_mai_descricao, _pagina);
                
          

                response.AddPage("listaitens", _listaitens);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult BuscarModulo(int _mod_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _modulo = new ManualDPModuloSRV().FindById(_mod_id);

                response.Add("modulo", _modulo);
                response.success = true;
                response.message = Message.Info("Ok");

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
        public ActionResult BuscarModuloSelect(int _mai_id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _modulo = new ManualDPItemSRV().BuscarModuloSelect(_mai_id);

                response.Add("modulo", _modulo);
                response.success = true;
                response.message = Message.Info("Ok");

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
        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int? _mai_id)
        {
            this.CarregaDados();

            ViewBag.mai_id = _mai_id;

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(ManualDPItemDTO _manual)
        {

            JSONResponse response = new JSONResponse();
            
            var itematualizado = _manual.MAN_ID.ToString() + " - " + _manual.MAN_ASSUNTO;

            try
            {
                 new ManualDPItemSRV().Salvar(_manual);

                 SysException.RegistrarLog("Dados atualizados com sucesso!! (" + itematualizado + ")", "", SessionContext.autenticado);

                 response.success = true;
                 response.message = Message.Info("Dados atualizados com sucesso!!");
                 return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex)+" Erro (" + itematualizado + ")" , SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex) + " Erro (" + itematualizado + ")", SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult SalvarOrdenado(ManualDPItemDTO _item, ManualDPItemDTO _atual)
        {

            JSONResponse response = new JSONResponse();

            var _servico = new ManualDPItemSRV();

            try
            {
                _servico.SaveOrUpdate(_item);
                _servico.SaveOrUpdate(_atual);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult SalvarAssuntoOrdenado(ManualDPDTO _item, ManualDPDTO _atual)
        {

            JSONResponse response = new JSONResponse();

            var _servico = new ManualDPSRV();

            try
            {
                _servico.SaveOrUpdate(_item);
                _servico.SaveOrUpdate(_atual);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult SalvarAssunto(ManualDPDTO _manual)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _srv =  new ManualDPSRV();

                if (_manual.MAN_INDEX == null)
                    _manual.MAN_INDEX = 1;

                new ManualDPSRV().SaveOrUpdate(_manual);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult ExcluirAssunto(ManualDPDTO _manual)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                new ManualDPSRV().Delete(_manual);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult Publicar(int _mai_id, int _tipo)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _manual = new ManualDPItemSRV().FindById(_mai_id);

                _manual.DATA_ALTERA = DateTime.Now;
                _manual.USU_LOGIN_ALT = SessionContext.autenticado.USU_LOGIN;

                if (_tipo == 0)
                    _manual.MAI_DATA_PUBLICACAO = null;
                else
                    _manual.MAI_DATA_PUBLICACAO = DateTime.Now;
        
                new ManualDPItemSRV().Salvar(_manual); 

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult PublicarAssunto(ManualDPDTO _manual, int _tipo)
        {

            JSONResponse response = new JSONResponse();

            try
            {
        
                if (_tipo == 0)
                    _manual.MAN_DATA_PUBLICACAO = null;
                else
                    _manual.MAN_DATA_PUBLICACAO = DateTime.Now;

                new ManualDPSRV().Merge(_manual);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                var _listaassunto = new ManualDPSRV().Listar();

                response.success = true;
                response.Add("listaassunto", _listaassunto);
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult PublicarAssuntoGeral(ManualDPDTO _manual, int _tipo)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                new ManualDPSRV().PublicarAssuntoGeral(_manual, _tipo);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                var _listaassunto = new ManualDPSRV().Listar();

                response.success = true;
                response.Add("listaassunto", _listaassunto);
                response.message = Message.Info("Dados atualizados com sucesso!!");
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
        public ActionResult RestaurarIndice(ManualDPDTO _manual)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                new ManualDPSRV().RestaurarIndice(_manual);

                SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                response.success = true;
                response.message = Message.Info("Dados atualizados com sucesso!!");
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

