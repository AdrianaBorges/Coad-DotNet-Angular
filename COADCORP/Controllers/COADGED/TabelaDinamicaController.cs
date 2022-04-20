using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.SEGURANCA.Service;
using GenericCrud.Excel;
using System.IO;
using System.Reflection;
using COAD.CORPORATIVO.SessionUtils;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Threading;
using System.Web.Security;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using COAD.COADGED.Model.DTO.Custons;
using COAD.SEGURANCA.Model;

namespace  COADCORP.Controllers.Controllers
{
    public class TabelaDinamicaController : Controller
    {
        public class campos
        {
            public string Text {get; set;}
            public int Value {get; set;}
        }
        public class CamposFormula
        {
            public string Campo { get; set; }
            public string Formula { get; set; }
        }
        private TabDinamicaSRV _tabDinSRV = new TabDinamicaSRV();
        private TabDinamicaItemSRV _tabDinItemSRV = new TabDinamicaItemSRV();
        private TabDinamicaConfigSRV _tabDinConfigSRV = new TabDinamicaConfigSRV();
        private TabDinamicaConfigItemSRV _tabDinConfigItemSRV = new TabDinamicaConfigItemSRV();
        private TabDinamicaPublicacaoSRV _tabDinPublicacaoSRV = new TabDinamicaPublicacaoSRV();

        public string _tdc_id = null; 
        public string _nometabela = null;
        public HttpPostedFileBase uploadFile = null;

        public List<string> _utilizado = new List<string>();

        public List<campos> CarregarCampoString()
        {            //---------------
            List<campos> _campostring = new List<campos>();

            _campostring.Add(new campos { Text = "TAB_STRING01", Value = 1 });
            _campostring.Add(new campos { Text = "TAB_STRING02", Value = 2 });
            _campostring.Add(new campos { Text = "TAB_STRING03", Value = 3 });
            _campostring.Add(new campos { Text = "TAB_STRING04", Value = 4 });
            _campostring.Add(new campos { Text = "TAB_STRING05", Value = 5 });
            _campostring.Add(new campos { Text = "TAB_STRING06", Value = 6 });
            _campostring.Add(new campos { Text = "TAB_STRING07", Value = 7 });
            _campostring.Add(new campos { Text = "TAB_STRING08", Value = 8 });
            _campostring.Add(new campos { Text = "TAB_STRING09", Value = 9 });
            _campostring.Add(new campos { Text = "TAB_STRING10", Value = 10 });
            _campostring.Add(new campos { Text = "TAB_STRING11", Value = 11 });
            _campostring.Add(new campos { Text = "TAB_STRING12", Value = 12 });
            _campostring.Add(new campos { Text = "TAB_STRING13", Value = 13 });
            _campostring.Add(new campos { Text = "TAB_STRING14", Value = 14 });
            _campostring.Add(new campos { Text = "TAB_STRING15", Value = 15 });
            _campostring.Add(new campos { Text = "TAB_STRING16", Value = 16 });
            _campostring.Add(new campos { Text = "TAB_STRING17", Value = 17 });
            _campostring.Add(new campos { Text = "TAB_STRING18", Value = 18 });
            _campostring.Add(new campos { Text = "TAB_STRING19", Value = 19 });
            _campostring.Add(new campos { Text = "TAB_STRING20", Value = 20 });
            _campostring.Add(new campos { Text = "TAB_STRING21", Value = 21 });
            _campostring.Add(new campos { Text = "TAB_STRING22", Value = 22 });
            _campostring.Add(new campos { Text = "TAB_STRING23", Value = 23 });
            _campostring.Add(new campos { Text = "TAB_STRING24", Value = 24 });
            _campostring.Add(new campos { Text = "TAB_STRING25", Value = 25 });
            _campostring.Add(new campos { Text = "TAB_STRING26", Value = 26 });
            _campostring.Add(new campos { Text = "TAB_STRING27", Value = 27 });
            _campostring.Add(new campos { Text = "TAB_STRING28", Value = 28 });
            _campostring.Add(new campos { Text = "TAB_STRING29", Value = 29 });
            _campostring.Add(new campos { Text = "TAB_STRING30", Value = 30 });
            _campostring.Add(new campos { Text = "TAB_STRING31", Value = 31 });
            _campostring.Add(new campos { Text = "TAB_STRING32", Value = 32 });
            _campostring.Add(new campos { Text = "TAB_STRING33", Value = 33 });
            _campostring.Add(new campos { Text = "TAB_STRING34", Value = 34 });
            _campostring.Add(new campos { Text = "TAB_STRING35", Value = 35 });
            _campostring.Add(new campos { Text = "TAB_STRING36", Value = 36 });
            _campostring.Add(new campos { Text = "TAB_STRING37", Value = 37 });
            _campostring.Add(new campos { Text = "TAB_STRING38", Value = 38 });
            _campostring.Add(new campos { Text = "TAB_STRING39", Value = 39 });
            _campostring.Add(new campos { Text = "TAB_STRING40", Value = 40 });
            _campostring.Add(new campos { Text = "TAB_STRING41", Value = 41 });
            _campostring.Add(new campos { Text = "TAB_STRING42", Value = 42 });
            _campostring.Add(new campos { Text = "TAB_STRING43", Value = 43 });
            _campostring.Add(new campos { Text = "TAB_STRING44", Value = 44 });
            _campostring.Add(new campos { Text = "TAB_STRING45", Value = 45 });
            _campostring.Add(new campos { Text = "TAB_STRING46", Value = 46 });
            _campostring.Add(new campos { Text = "TAB_STRING47", Value = 47 });
            _campostring.Add(new campos { Text = "TAB_STRING48", Value = 48 });
            _campostring.Add(new campos { Text = "TAB_STRING49", Value = 49 });
            _campostring.Add(new campos { Text = "TAB_STRING50", Value = 50 });
            _campostring.Add(new campos { Text = "TAB_STRING51", Value = 51 });
            _campostring.Add(new campos { Text = "TAB_STRING52", Value = 52 });
            _campostring.Add(new campos { Text = "TAB_STRING53", Value = 53 });
            _campostring.Add(new campos { Text = "TAB_STRING54", Value = 54 });
            _campostring.Add(new campos { Text = "TAB_STRING55", Value = 55 });
            _campostring.Add(new campos { Text = "TAB_STRING56", Value = 56 });
            _campostring.Add(new campos { Text = "TAB_STRING57", Value = 57 });
            _campostring.Add(new campos { Text = "TAB_STRING58", Value = 58 });
            _campostring.Add(new campos { Text = "TAB_STRING59", Value = 59 });
            _campostring.Add(new campos { Text = "TAB_STRING60", Value = 60 });
             
            _campostring.Add(new campos { Text = "RET_FERIADOS", Value = 31 });


            //---------------

            return _campostring;

        }
        public List<campos> CarregarCampoInt()
        {
            List<campos> _campoint = new List<campos>();

            _campoint.Add(new campos { Text = "TAB_INT01", Value = 1 });
            _campoint.Add(new campos { Text = "TAB_INT02", Value = 2 });
            _campoint.Add(new campos { Text = "TAB_INT03", Value = 3 });
            _campoint.Add(new campos { Text = "TAB_INT04", Value = 4 });
            _campoint.Add(new campos { Text = "TAB_INT05", Value = 5 });
            _campoint.Add(new campos { Text = "TAB_INT06", Value = 6 });
            _campoint.Add(new campos { Text = "TAB_INT07", Value = 7 });
            _campoint.Add(new campos { Text = "TAB_INT08", Value = 8 });
            _campoint.Add(new campos { Text = "TAB_INT09", Value = 9 });
            _campoint.Add(new campos { Text = "TAB_INT10", Value = 10 });
            //---------------

            return _campoint;

        }
        public List<campos> CarregarCampoData()
        {
            List<campos> _campodata = new List<campos>();

            _campodata.Add(new campos { Text = "TAB_DATA01", Value = 1 });
            _campodata.Add(new campos { Text = "TAB_DATA02", Value = 2 });
            _campodata.Add(new campos { Text = "TAB_DATA03", Value = 3 });
            _campodata.Add(new campos { Text = "TAB_DATA04", Value = 4 });
            _campodata.Add(new campos { Text = "TAB_DATA05", Value = 5 });
            _campodata.Add(new campos { Text = "TAB_DATA06", Value = 6 });
            _campodata.Add(new campos { Text = "TAB_DATA07", Value = 7 });
            _campodata.Add(new campos { Text = "TAB_DATA08", Value = 8 });
            _campodata.Add(new campos { Text = "TAB_DATA09", Value = 9 });
            _campodata.Add(new campos { Text = "TAB_DATA10", Value = 10 });

            return _campodata;

        }
        public void Carregarlistas()
        {

            List<LinhaProdutoDTO> _listaLinhaProduto = new LinhaProdutoSRV().FindAll().ToList();
            IList<TitulacaoDTO> _listaTitulacao = new TitulacaoSRV().ListarGrandeGrupo();
            IList<TabDinamicaGrupoDTO> ListaTabDimGrugo = new TabDinamicaGrupoSRV().FindAll();
            
            List<SelectListItem> Lista = new List<SelectListItem>();
            IList<TabDinamicaGrupoDTO> _listagrupo = new TabDinamicaGrupoSRV().FindAll();
            IList<UsuarioModel> _listausuario = new UsuarioSRV().FindAll();

            Lista.Add(new SelectListItem { Text = "Tabela Dinâmica", Value = "1" });
            Lista.Add(new SelectListItem { Text = "Simulador", Value = "2" });
            Lista.Add(new SelectListItem { Text = "Tabela/Simulador", Value = "3" });
            Lista.Add(new SelectListItem { Text = "Tabela/Simulador Personalizado", Value = "4" });

            ViewBag.ListaUsuario = new SelectList(_listausuario, "USU_LOGIN", "USU_NOME");
            ViewBag.ListaTipo = new SelectList(Lista, "Value", "Text");
            ViewBag.Listagrupo = new SelectList(_listagrupo, "TGR_ID", "TGR_DESCRICAO");
            ViewBag.ListaTitulacao = new SelectList(_listaTitulacao, "TIT_ID", "TIT_DESCRICAO");
            ViewBag.ListaLinhaProduto = new SelectList(_listaLinhaProduto, "LIN_PRO_ID", "LIN_PRO_DESCRICAO");

        }

        [Autorizar(PorMenu=true)]
        public ActionResult Index()
        {
           
            SessionContext.PutInSession<string>("LOGIN_PORTAL", SessionContext.autenticado.USU_LOGIN);

            this.Carregarlistas();

            return View();
        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Importar()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Exportar()
        {
            this.Carregarlistas();

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CarregaTelaImp()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var ListaTabRef = _tabDinConfigSRV.ListarTabDinamica(null, null, 1);

                response.Add("listatabelas", ListaTabRef);
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
        public ActionResult buscarDescricaoMenu(string _id)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var tabconfig = new TabDinamicaConfigDTO();
                var tabela = new TabDinamicaDTO();
                var tbitem = new Pagina<TabDinamicaItemDTO>();

                if (_id != null && _id != "")
                {
                    tabela = _tabDinSRV.FindById(_id);
                }

                response.Add("tabela", tabela);
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
        public ActionResult CarregarTela(string _id, Boolean _ChecaPublicado = false, int? _tgr_tipo = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var tabconfig = new TabDinamicaConfigDTO();
                var tabela = new TabDinamicaDTO();

                if (_id != null && _id!="")
                {
                    tabconfig = _tabDinConfigSRV.FindById(_id);
                    tabela = _tabDinSRV.FindById(_id);

                    if (_ChecaPublicado && tabconfig.TDC_DATA_PUBLICACAO == null)
                        throw new Exception("Este simulador não está mais disponível. Em breve novidades. Aguarde!!");

                }

                if (tabconfig.TAB_DINAMICA_CONFIG_ITEM != null)
                    tabconfig.TAB_DINAMICA_CONFIG_ITEM = tabconfig.TAB_DINAMICA_CONFIG_ITEM.OrderBy(x => x.TCI_ORDEM_APRESENTACAO).ToList();

                if (tabconfig.TDC_TIPO == 1 || tabconfig.TDC_TIPO == 3)
                    tabela.TAB_DINAMICA_ITEM.Clear();

                response.Add("campostring", this.CarregarCampoString());
                response.Add("campoint", this.CarregarCampoInt());
                response.Add("campodata", this.CarregarCampoData());
                response.Add("tabconfig", tabconfig);
                response.Add("tabela", tabela);
                
                response.Add("lstTabelas", _tabDinConfigSRV.ListarTabDinamica(null, null, 1,false));
                response.Add("lstSimuladores", _tabDinConfigSRV.ListarTabDinamica(null, null, 2, false, _tgr_tipo));
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
        public ActionResult BuscarItemTela(string _id, int _pagina = 1)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var tbitem = new Pagina<TabDinamicaItemDTO>();

                if (_id != null && _id != "")
                    tbitem = _tabDinItemSRV.ListarTabDinamicaPag(_id, _pagina);

                response.AddPage("tbitem", tbitem);
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
        public ActionResult MostrarInfAdicionais(string _id, string _uf, int pagina = 1)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var _listparam = new List<ParamConsultaDTO>();
                var _param = new ParamConsultaDTO();
                _param.campo = "TAB_STRING01";
                _param.valor = _uf;
                _listparam.Add(_param);


                _uf = _uf.Trim();

                var tabref = _tabDinSRV.FindById(_id);
                

                var tabrefitem = _tabDinItemSRV.ListarTabDinamicaItemUF(_id, _uf, pagina, 10); 

                tabref.TAB_DINAMICA_ITEM = tabrefitem.lista.ToList();

                var tabrefcfg = _tabDinConfigSRV.FindById(_id);
                
                response.Add("tabref", tabref);
                response.AddPage("tabrefitem", tabrefitem);
                response.Add("tabrefcfg", tabrefcfg);
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
        public ActionResult Editar(string _tipo, string _id)
        {
            this.Carregarlistas();

            ViewBag.tipo = _tipo;
            ViewBag.id = _id;

            return View();
        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult EditarTabela(string _tipo, string _id)
        {
            this.Carregarlistas();

            ViewBag.tipo = _tipo;
            ViewBag.id = _id;

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Salvar(string _id, TabDinamicaConfigDTO _config, TabDinamicaDTO _tabela)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    if (_config.TDC_TIPO == 2 && _tabela == null)
                    {
                        _tabela = new TabDinamicaDTO();
                        _tabela.TAB_DINAMICA_ITEM = new List<TabDinamicaItemDTO>();
                    }

                    _tabDinConfigSRV.SalvarTabelaeConfig(_id, _config, _tabela);

                    SysException.RegistrarLog("Dados atualizados com sucesso!!", "", SessionContext.autenticado);

                    response.success = true;
                    response.message = Message.Info("Dados atualizados com sucesso!!");

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string erro = "";
                    response.success = false;
                    response.SetMessageFromModelState(ModelState);
                    foreach(var _item  in response.validationMessage)
                    {
                        for (var ind = 0; ind <= _item.Value.Count-1 ; ind++ )
                        {
                            erro += " --- "+_item.Value[ind]+" \n ";
                        }
                    }

                    response.message = Message.Fail(erro);

                    return Json(response, JsonRequestBehavior.AllowGet);
                }

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
        public ActionResult SalvarItemTabela(string _id, TabDinamicaItemDTO _item)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                _item.TDC_ID = _id;
                _item.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;

                var _retorno = new TabDinamicaItemDTO();

                if (_item.TAB_ID == 0)
                {
                    _item.TAB_DATA_INCLUSAO = DateTime.Now;
                    _retorno = _tabDinItemSRV.Save(_item);
                }
                else
                {
                    _item.TAB_DATA_ALTERA = DateTime.Now;
                    _retorno = _tabDinItemSRV.Merge(_item, "TDC_ID", "TAB_ID");
                }
                
                
                SysException.RegistrarLog("Item da tabela dinâmica atualizado com sucesso!! id => "+_id , "", SessionContext.autenticado);

                result.success = true;
                result.Add("retorno", _retorno);
                result.message = Message.Info("Dados atualizados com sucesso!!");

                return Json(result, JsonRequestBehavior.AllowGet);
     
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

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExcluirTabela(string _id)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                if (_id == null && _id == "")
                    throw new Exception("ID da tabela não informado!");

                _tabDinConfigSRV.ExcluirTabelaeConfig(_id);

                SysException.RegistrarLog("Tabela dinâmica excluída com sucesso!! id => " + _id, "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Dados atualizados com sucesso!!");
                return Json(result, JsonRequestBehavior.AllowGet);

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

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExcluirItemTabela(TabDinamicaItemDTO _item)
        {
            JSONResponse result = new JSONResponse();

            try
            {

                _tabDinItemSRV.Delete(_item, "TDC_ID", "TAB_ID");

                SysException.RegistrarLog("Item da tabela excluído com sucesso!! id => " + _item.TDC_ID, "", SessionContext.autenticado);

                result.success = true;
                result.message = Message.Info("Dados atualizados com sucesso!!");
                return Json(result, JsonRequestBehavior.AllowGet);

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

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTabelaDinamica(string[] _tdc_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                foreach (var _id in _tdc_id)
                {
                    var _listatabela = _tabDinItemSRV.ListarTabDinamica(_id);
                    var _retorno = "B"+_id.Replace("-", "");
                    response.Add(_retorno, _listatabela);
                }

                //if (listatabela == null)
                //    throw new Exception("Nenhum registro encontrado para a consulta");

                
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
        public ActionResult Pesquisar(string _tab_descricao = null, int _tdc_tipo = 0, bool _publicados = false, int _tgr_id = 0, int _tit_id = 0, int pagina = 1, int registroPorPagina = 20)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listatabela = _tabDinConfigSRV.ListarTabDinamicaPag(_tab_descricao, _tdc_tipo, _publicados, _tgr_id, _tit_id, pagina, registroPorPagina);

                if (listatabela == null)
                    throw new Exception("Nenhum registro encontrado para a consulta");

                response.AddPage("listatabela", listatabela);
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
        public ActionResult PesquisarTabDinamica(string _id, int _pagina = 1, int _registroPorPagina = 20, List<ParamConsultaDTO> _p = null, string _palavrachave = null )
        {
            JSONResponse response = new JSONResponse();

            var _msgPersonalizada = _tabDinSRV.FindById(_id).TAB_ERRO_MSG;

            try
            {
                this.CarregarCampoString();

                List<ParamConsultaDTO> _lista = new List<ParamConsultaDTO>();

                if (_palavrachave != null)
                {
                    var listacamposstring = this.CarregarCampoString();

                    foreach (var _l in listacamposstring)
                    {
                        ParamConsultaDTO _param = new ParamConsultaDTO();
                        _param.campo = _l.Text;
                        _param.valor = _palavrachave.Trim();
                        _lista.Add(_param);
                    }
                }
                else
                {
                    if (_p != null && _p.Count > 0)
                    {
                        foreach (var _l in _p)
                        {
                            if (_l.valor != null)
                            {
                                _l.valor = _l.valor.Trim();
                                _lista.Add(_l);
                            }
                        }
                    }
                }

                var tbitem = _tabDinItemSRV.ListarTabDinamicaPag(_id, _pagina, _registroPorPagina, _lista, (_palavrachave != null));

                if (tbitem.lista.Count() == 0)
                    if (_msgPersonalizada != null)
                       throw new Exception(_msgPersonalizada);
                    else
                       throw new Exception("Nenhum item foi encontrado para a pesquisa realizada. Altere os parametros e realize a pesquisa novamente!");

                response.AddPage("tbitem", tbitem);
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
        [Autorizar(PorMenu = false)]
        public ActionResult TabelaTipiCest(Boolean interno = false)
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                   return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", "TabelaTipiCest", code);

                    if (erro != null)
                        throw new Exception(erro);
                }

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

                return View();

            }

        }
        public ActionResult PesquisarTabelaTipi(string _ncm, string _cest, int _tipo)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                List<ParamConsultaDTO> _lista = new List<ParamConsultaDTO>();

                IList<TabelaArvoreDTO> tbtipi = new List<TabelaArvoreDTO>();



                if (_tipo > 0)
                {
                    if ((_ncm != null && _ncm != "") || _cest != null)
                        tbtipi = _tabDinSRV.ListarTabelaTipi(_ncm, _cest);
                    else
                        throw new Exception("Informe algum parametro para realizar a consulta.");
                }
                else
                    tbtipi = _tabDinSRV.ListarTabelaTipi();


                response.Add("tbtipi", tbtipi);
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
        public ActionResult PesquisarTabelaCest(string _ncm, string _cest, int _tipo)
        {
            JSONResponse response = new JSONResponse();

            try
            {

                List<ParamConsultaDTO> _lista = new List<ParamConsultaDTO>();

                IList<TabelaArvoreDTO> tbtcest = new List<TabelaArvoreDTO>();
                
                if (_tipo > 0)
                {
                    if ((_ncm != null && _ncm != "") || _cest != null)
                        tbtcest = _tabDinSRV.ListarTabelaCest(_ncm, _cest);
                    else
                        throw new Exception("Informe algum parametro para realizar a consulta.");
                }
                else
                    tbtcest = _tabDinSRV.ListarTabelaCest();


                response.Add("tbtcest", tbtcest);
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
        public ActionResult BuscarConfigTabItem(string _tdc_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _lstTabelasCfgItem = new TabDinamicaConfigItemSRV().ListarTabDinamica(_tdc_id);

                response.Add("lstTabelasCfgItem", _lstTabelasCfgItem);
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
        public ActionResult BuscarBanner(string _tdc_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<FuncionalidadeDTO> _funcionalidade = new FuncionalidadeSRV().ListarPorReferencia(_tdc_id);

                response.Add("funcionalidade", _funcionalidade);
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
        public ActionResult BuscarMenuTabelas(int _tipo, int? _grupo, int? _tgr_tipo)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _listaTabDimGrugo = new TabDinamicaGrupoSRV().BuscarGrupoTabela(_tipo, _grupo, _tgr_tipo);
                //var _listaMaisAcessadas = new LogSimuladorSRV().BuscarTabelasMaisAcessadas(10, _tipo, _tgr_tipo);
                //var _listaAcessadas = new LogSimuladorSRV().BuscarTabelasAcessadas(10, _tipo, _tgr_tipo);

                //response.Add("listaMaisAcessadas", _listaMaisAcessadas); 
                //response.Add("listaAcessadas", _listaAcessadas);
                response.Add("ListaTabDimGrugo", _listaTabDimGrugo);
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
        /// <summary>
        /// Este metodo é utilizado no portal e apresenta a tabela dinâmica dentro do modulo de simuladores.
        /// </summary>
        /// <param name="id">ID da Tabela</param>
        /// <param name="interno">Indica se a chamada é interna ou externa</param>
        /// <returns></returns>
        [Autorizar(PorMenu = false)]
        public ActionResult Tabela(string id, Boolean interno = false)
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", id, code);

                    if (erro != null)
                        throw new Exception(erro);
                }

                ViewBag.id = id; 
                ViewBag.tipo = 1;
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

                return View();

            }

        }
        /// <summary>
        /// Este metodo é utilizado no portal e apresenta o simulador dinâmico dentro do modulo de simuladores.
        /// </summary>
        /// <param name="id">ID do simulador</param>
        /// <param name="interno">Indica se a chamada é interna ou externa</param>
        /// <returns></returns>
        [Autorizar(PorMenu = false)]
        public ActionResult Simulador(string id, Boolean interno = false) 
        {
            try
            {

                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", id, code);

                    if (erro != null)
                        throw new Exception(erro);
                }

                ViewBag.id = id; 
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

                return View();
            }

        }
        /// <summary>
        /// Metodo utilizado no portal para executar o simulador ICMS/EC87. 
        /// Este metodo pode ser utilizado no portal e a view possui uma formatação padrão  (Não é dinâmico)
        /// </summary>
        /// <param name="id">ID do simulador</param>
        /// <param name="interno">Indica se a chamada é interna ou externa</param>
        /// <returns></returns>
        [Autorizar(PorMenu = false)]
        public ActionResult SimuladorICMS(string id, Boolean interno = false)
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", id, code);

                    if (erro != null)
                        throw new Exception(erro);
                }

                ViewBag.id = id;
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

                return View();
            }

        }
        /// <summary>
        /// Metodo utilizado no portal - Faz a chamada para a tela de simuladores(Dinâmicos)
        /// </summary>
        /// <returns></returns>
        public ActionResult Simuladores(int? tipo)
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

          
                ViewBag.id = "";
                ViewBag.tipoGrupo = tipo;

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

                return RedirectToAction("Info","Home");

            }

        }

        /// <summary>
        /// Metodo utilizado no portal - Faz a chamada para a tela de Tabelas (Dinâmicos)
        /// </summary>
        /// <returns></returns>
        public ActionResult Tabelas (int? _grupo)
        {
            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                ViewBag.id = "";

                if (_grupo == null)
                    _grupo = 0;

                ViewBag.grupo = _grupo;

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

        public ActionResult TabelaSimuladorPersonalizado(string id, Boolean interno = false)
        {

            try
            {
                if (SessionContext.GetInSession<string>("LOGIN_PORTAL") == null)
                    return Content("<script>parent.window.open('http://www.coad.com.br/cadastre-se/');</script>");

                var code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", id, code);

                    if (erro != null)
                        throw new Exception(erro);
                }

                //             ViewBag.nomeUnico = id;
                //              ViewBag.tipo = 2;
                ViewBag.checa = (interno == false);

                if (id.Equals("LUCRO_PRESUMIDO"))
                {
                    return View("~/Views/TabelaDinamica/Personalizados/LucroPresumido.cshtml");
                }
                return View("");

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

                return View();
            }

        }

        /// <summary>
        /// Metodo utilizado no portal - Faz o logout por meio do portal
        /// </summary>
        public ActionResult LogOutPortal()
        {
            if (SessionContext.autenticado != null)
            {
                SysException.RegistrarLog("LogOff Usuário (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);
            }

            SessionContext.RemoveSession(System.Web.HttpContext.Current);

            return View();
        }

        /// <summary>
        /// Metodo utilizado no portal - Faz o login por meio do portal
        /// </summary>
        /// <param name="code">ID do cliente</param>
        /// <param name="encodecod">Senha do cliente</param>
        /// <returns></returns>
        public ActionResult LoginPortal(string code, string encodecod)
        {
            try
            {
                AssinaturaSenhaSRV _assSRV = new AssinaturaSenhaSRV();
                UsuarioSRV _usuario = new UsuarioSRV();

                string url =  System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

              //  string _retorno = "";

                if (code == null || encodecod == null)
                    throw new Exception("Usuário não autorizado ou não logado no sistema. Verifique seu login e senha.");
  
                if (code.IndexOf(Convert.ToChar("@")) > 0)
                    new CadastroGratuitoSRV().RealizarLogin(code, encodecod, "COADCORP", System.Web.HttpContext.Current);
                else if (_assSRV.BuscarPorId(code) == null)
                    new CadastroGratuitoSRV().RealizarLogin(code, encodecod, "COADCORP", System.Web.HttpContext.Current);
                else
                {
                    AssinaturaSenhaDTO _senha = _assSRV.BuscarSenhaAtiva(code);

                    if (_senha == null)
                        throw new Exception("Usuário não autorizado ou não logado no sistema. Verifique seu login e senha.");

                    string _senhaHash = SessionContext.HashMD5(_senha.ASN_SENHA);

                    if (_senha.ASN_NUM_ASSINATURA.ToUpper() != code.ToUpper() || _senhaHash != encodecod)
                        throw new Exception("Usuário não autorizado ou não logado no sistema. Atualize a tela para continuar acessando esta funcionalidade.");

                }

                //-----------------

                Autenticado _autenticado = new Autenticado();
                System.Web.HttpContext.Current.Session.Timeout = 240;

                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.PATH = url;
                _autenticado.SESSION_ID = System.Web.HttpContext.Current.Session.SessionID;
                _autenticado.SESSION_TIMEOUT = System.Web.HttpContext.Current.Session.Timeout;
                _autenticado.SESSION_TIMEOUT_RESTANTE = System.Web.HttpContext.Current.Session.Timeout;
                _autenticado.SIS_ID = "COADCORP";
                _autenticado.EMP_ID = 1;
                _autenticado.ADMIN = false;
                _autenticado.EMAIL = code;
                _autenticado.USU_LOGIN = "MOBILEUSER";
                _autenticado.DATA_LOGIN = DateTime.Now;
                _autenticado.MEIO_ACESSO = "PORTAL/APP";

                SessionContext.autenticado = _autenticado;
                SessionContext.perfis_usuario = _usuario.ListarPerfil(_autenticado.EMP_ID, _autenticado.USU_LOGIN, _autenticado.SIS_ID);
                SessionContext.menu_usuario = _usuario.MontaMenu(_autenticado.PER_ID, _autenticado.EMP_ID, _autenticado.SIS_ID, _autenticado.ADMIN);
                SessionContext.sistemas_coad = new SistemaSRV().Listar();

                SessionContext.AddSessionGlobal(System.Web.HttpContext.Current);

                FormsAuthentication.SetAuthCookie(_autenticado.USU_LOGIN, false);

                SysException.RegistrarLog("LogIn Usuário (" + _autenticado.USU_LOGIN + ")", "", _autenticado);
                
                //-----------------


                ViewBag.code = code;
                ViewBag.encodecod = encodecod;

                SessionContext.PutInSession<string>("LOGIN_PORTAL", code);

                ViewBag.id = "";

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
        /// <summary>
        /// Metodo utilizado no portal para executar o simulador ICMS/EC87. 
        /// Este metodo pode ser utilizado no portal e no aplicativo (webview) pois a view não possui formatação.
        /// </summary>
        /// <param name="code">ID do cliente</param>
        /// <param name="encodecod">Senha do cliente</param>
        /// <param name="interno">Indica se a chamada é interna ou externa</param>
        /// <returns></returns>
        public ActionResult SimuladoreMobile(string code, string encodecod, Boolean interno = false)
        {
            try
            {
                AssinaturaSenhaSRV _assSRV = new AssinaturaSenhaSRV();
                UsuarioSRV _usuario = new UsuarioSRV();

                if (code == null || encodecod == null)
                    throw new Exception("Usuário não autorizado ou não logado no sistema. Verifique seu login e senha.");

                if (code.IndexOf(Convert.ToChar("@")) > 0)
                    new CadastroGratuitoSRV().RealizarLogin(code, encodecod, "COADCORP", System.Web.HttpContext.Current);
                else if (_assSRV.BuscarPorId(code) == null)
                    new CadastroGratuitoSRV().RealizarLogin(code, encodecod, "COADCORP", System.Web.HttpContext.Current);
                else
                {
                    AssinaturaSenhaDTO _senha = _assSRV.BuscarSenhaAtiva(code);

                    if (_senha == null)
                        throw new Exception("Usuário não autorizado ou não logado no sistema. Verifique seu login e senha.");

                    string _senhaHash = SessionContext.HashMD5(_senha.ASN_SENHA);

                    if (_senha.ASN_NUM_ASSINATURA != code || _senhaHash != encodecod)
                        throw new Exception("Usuário não autorizado ou não logado no sistema. Atualize a tela para continuar acessando esta funcionalidade.");

                }

                ViewBag.id = "1228f7e8-b4a0-45e3-bf67-099c7fda840b";
                ViewBag.tipo = 2;
                ViewBag.checa = (interno == false);

                //-----------------

                Autenticado _autenticado = new Autenticado();
                System.Web.HttpContext.Current.Session.Timeout = 240;
                string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                
                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.PATH = url;
                _autenticado.SESSION_ID = System.Web.HttpContext.Current.Session.SessionID;
                _autenticado.SESSION_TIMEOUT = System.Web.HttpContext.Current.Session.Timeout;
                _autenticado.SESSION_TIMEOUT_RESTANTE = System.Web.HttpContext.Current.Session.Timeout;
                _autenticado.SIS_ID = "COADCORP";
                _autenticado.EMP_ID = 1;
                _autenticado.ADMIN = false;
                _autenticado.EMAIL = code;
                _autenticado.USU_LOGIN = "MOBILEUSER";
                _autenticado.DATA_LOGIN = DateTime.Now;
                _autenticado.MEIO_ACESSO = "PORTAL/APP";

                SessionContext.autenticado = _autenticado;
                SessionContext.perfis_usuario = _usuario.ListarPerfil(_autenticado.EMP_ID, _autenticado.USU_LOGIN, _autenticado.SIS_ID);
                SessionContext.menu_usuario = _usuario.MontaMenu(_autenticado.PER_ID, _autenticado.EMP_ID, _autenticado.SIS_ID, _autenticado.ADMIN);
                SessionContext.sistemas_coad = new SistemaSRV().Listar();

                SessionContext.AddSessionGlobal(System.Web.HttpContext.Current);

                FormsAuthentication.SetAuthCookie(_autenticado.USU_LOGIN, false);

                SysException.RegistrarLog("LogIn Usuário (" + _autenticado.USU_LOGIN + ")", "", _autenticado);

                //-----------------

                SessionContext.PutInSession<string>("LOGIN_PORTAL", code);

                if (!interno)
                {
                    string erro = this.RegistrarLogSimulador("L", "1228f7e8-b4a0-45e3-bf67-099c7fda840b", code);

                    if (erro != null)
                        throw new Exception(erro);
                }
                
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
        public ActionResult Menu(string _id)
        {
            try
            {
                string _code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (_code == null)
                    throw new Exception("Usuário não autorizado ou não logado no sistema. Verifique seu login e senha.");

                ViewBag.id = _id;

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

                return View();

            }

        }
        public string RegistrarLogSimulador(string _tipoacesso, string _tdc_id, string _code)
        {
            try
            {

                LogSimuladorDTO log = new LogSimuladorDTO();

                if (_code.IndexOf(Convert.ToChar("@")) > 0)
                {
                    log.ASN_NUM_ASSINATURA = "GRATUITO";
                    log.LSI_EMAIL_ACESSO = _code;
                }
                else
                    log.ASN_NUM_ASSINATURA = _code;

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
        [Autorizar(IsAjax = true)]
        public ActionResult RegistrarLogCalculo(string _tipoacesso, string _tdc_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _code = SessionContext.GetInSession<string>("LOGIN_PORTAL");

                if (_code == null)
                    _code = "99A00001";

                this.RegistrarLogSimulador(_tipoacesso, _tdc_id, _code);

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

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarValor(string _tdc_id, string[] _param)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _lista = new List<ParamConsultaDTO>();

                for(var i =0 ;i<=_param.Count();i++)
                {
                    var paramConsulta = new ParamConsultaDTO();
                    paramConsulta.campo = "";
                    paramConsulta.valor = _param[0];
                    paramConsulta.esperado = "";

                    _lista.Add(paramConsulta);
                }

                var _valorRetorno = _tabDinItemSRV.BuscarItem(_tdc_id, _lista);

                response.Add("valorRetorno", _valorRetorno);
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

        [Autorizar(Acesso = "Editar")]
        [HttpPost]
        public ActionResult ImportarXLS(string _tdc_id = null, string _nometabela = null, HttpPostedFileBase uploadFile = null)
        {
            try
            {
                if (_nometabela.Trim() == null)
                    throw new Exception("Nome da tabela não informada!!");
             
                this._tdc_id = _tdc_id;
                this._nometabela = _nometabela;
                this.uploadFile = uploadFile;

                //--------------
                SessionContext.usu_login_desktop = SessionContext.autenticado.USU_LOGIN;
                AutenticadoThread.TOTAL_LINHAS = 1; 
                AutenticadoThread.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                AutenticadoThread.USU_SENHA = SessionContext.autenticado.USU_SENHA;
                AutenticadoThread.USU_NOME = SessionContext.autenticado.USU_NOME;
                AutenticadoThread.EMP_ID = SessionContext.autenticado.EMP_ID;
                AutenticadoThread.PATH = SessionContext.autenticado.PATH;
                AutenticadoThread.PER_ID = SessionContext.autenticado.PER_ID;
                AutenticadoThread.ADMIN = SessionContext.autenticado.ADMIN;
                AutenticadoThread.SIS_ID = SessionContext.autenticado.SIS_ID;
                AutenticadoThread.EMAIL = SessionContext.autenticado.EMAIL;
                AutenticadoThread.ERRO_PROCESSO = "";

                //---------------
                    
                //Thread ImportarThread = new Thread(ImportarExcel);
                
                //ImportarThread.Start();

                _tdc_id = this.ImportarExcelProc();

                //--------------

                SysException.RegistrarLog("Importação de Tabela Realizada com sucesso !!", "", SessionContext.autenticado);

                this.Carregarlistas();

                return RedirectToAction("Editar", new {_tipo = 1 ,  _id = _tdc_id });

            //    return View("Importar");

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

                ViewBag.message = SessionUtil.RecursiveShowExceptionsMessage(ex);

                this.Carregarlistas();

                return View("Importar");
            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(string _tdc_id = null, string _nomearquivo = null)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                if (_nomearquivo.Trim() == null)
                    throw new Exception("Nome do arquivo não informado!!");

                string[,] _planilha = null;

                IList<TabDinamicaConfigItemDTO> _tbcfgitem = _tabDinConfigSRV.FindById(_tdc_id).TAB_DINAMICA_CONFIG_ITEM.ToList();
                IList<TabDinamicaItemDTO> _tbitem = _tabDinItemSRV.ListarTabDinamica(_tdc_id);
                
                if (_tbitem.Count>0)
                {
                    _planilha = new string[(_tbitem.Count+1), _tbcfgitem.Count];
                    
                    for (int ind = 0; ind <= _tbcfgitem.Count - 1; ind++)
                    {
                        var _campodb = _tbcfgitem[ind].TCI_NOME_CAMPODB;

                        if (_campodb == null || _campodb == "")
                            _campodb = "INFORME_NOME";

                        _planilha[0, ind] = _tbcfgitem[ind].TCI_NOME_CAMPO;
                    }

                    for (int lin = 0; lin <= _tbitem.Count - 1; lin++)
                    {

                        for (int ind = 0; ind <= _tbcfgitem.Count - 1; ind++)
                        {
                            var _campodb = _tbcfgitem[ind].TCI_NOME_CAMPODB;

                            //----------
                            PropertyInfo propertySet = _tbitem[lin].GetType().GetProperty(_campodb.Trim());
                            if (propertySet.GetValue(_tbitem[lin]) != null)
                                _planilha[(lin + 1), ind] = propertySet.GetValue(_tbitem[lin]).ToString();

                        }

                    }

                }

                var _retorno = new ExcelLoad().Export(_nomearquivo, _planilha, System.Web.HttpContext.Current);

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
                response.message = Message.Fail(SessionUtil.RecursiveShowExceptionsMessage(ex));
                return Json(response, JsonRequestBehavior.AllowGet);

            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult CalularSimulador(string _campoformula, string _formula, TabDinamicaConfigDTO _tabconfig, TabDinamicaItemDTO _tabItem)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                CamposFormula campocalc = new CamposFormula();

                if (_formula != null)
                {
               
                    foreach (var _item in _tabconfig.TAB_DINAMICA_CONFIG_ITEM)
                    {
                        PropertyInfo campoformula = _tabItem.GetType().GetProperty(_item.TCI_NOME_CAMPODB.Trim());

                        string _campo = _item.TCI_NOME_CAMPODB;
                        string _valor = campoformula.GetValue(_tabItem).ToString();

                        _formula = _formula.Replace(_campo, _valor);

                    }

                    campocalc.Campo = _campoformula;
                    campocalc.Formula = _formula;

                }


                response.Add("campocalc", campocalc);
                response.success = true;
                response.message = Message.Info("Formula Gerada " + _formula);

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
        public ActionResult Publicar(TabDinamicaConfigDTO _config)
        {
             JSONResponse response = new JSONResponse();

            try
            {
                _tabDinConfigSRV.Publicar(_config);

                SysException.RegistrarLog("Tabela =>" + _config.TDC_ID + ", publicada com sucesso!", "", SessionContext.autenticado);

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
        public ActionResult RemoverPublicacao(TabDinamicaConfigDTO _config)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                _tabDinConfigSRV.RemoverPublicacao(_config);

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
        public void ImportarExcel()
        {
            TabDinamicaDTO _tab = new TabDinamicaDTO();
            TabDinamicaConfigDTO _tabcfg = new TabDinamicaConfigDTO();


            if (_tdc_id != null && _tdc_id != "")
            {
                _tab = new TabDinamicaSRV().FindById(_tdc_id);
                _tabcfg = new TabDinamicaConfigSRV().FindById(_tdc_id);
            }

            _tab.TAB_DESCRICAO = _nometabela;
            _tab.TAB_DINAMICA_ITEM = new List<TabDinamicaItemDTO>();

            _tabcfg.TDC_NOME_TABELA = _nometabela;
            _tabcfg.TDC_TIPO = 1;
            _tabcfg.TAB_DINAMICA_CONFIG_ITEM = new List<TabDinamicaConfigItemDTO>();


            if (uploadFile.ContentLength > 0)
            {
                string _nomearquivo = Path.GetFileName(uploadFile.FileName);
                string _arquivo = HttpContext.Server.MapPath("~/temp/") + Path.GetFileName(uploadFile.FileName);
                string _filePath = Path.Combine(HttpContext.Server.MapPath("~/temp/"), Path.GetFileName(uploadFile.FileName));
                uploadFile.SaveAs(_filePath);


                string[,] _conteudo = new ExcelLoad().Load(_filePath);

                int _lin = _conteudo.GetLength(0) - 1;
                int _col = _conteudo.GetLength(1) - 1;

                AutenticadoThread.TOTAL_LINHAS = _lin; 

                List<campos> _campostring = this.CarregarCampoString();

                for (int _ind = 0; _ind <= _lin; _ind++)
                {
                    //----------------  
                    TabDinamicaItemDTO _tabItem = new TabDinamicaItemDTO();
                    //----------------

                    for (int _ind2 = 0; _ind2 <= _col; _ind2++)
                    {
                        string _campodb = _campostring.Where(x => x.Value == (_ind2 + 1)).FirstOrDefault().Text;

                        if (_ind == 0)
                        {
                            //----------
                            TabDinamicaConfigItemDTO _tabcfgItem = new TabDinamicaConfigItemDTO();

                            _tabcfgItem.TCI_NOME_CAMPO = "Vazio";
                            _tabcfgItem.TCI_TAMANHO_CAMPO = 10;

                            if (_conteudo[_ind, _ind2] != null)
                            {
                                _tabcfgItem.TCI_NOME_CAMPO = _conteudo[_ind, _ind2].ToString().Trim();
                                _tabcfgItem.TCI_TAMANHO_CAMPO = _conteudo[_ind, _ind2].ToString().Trim().Length;
                            }

                            _tabcfgItem.TCI_NOME_CAMPODB = _campodb;
                            _tabcfgItem.TCI_ALINHAMENTO_CAMPO = "E";
                            _tabcfgItem.TCI_ORDEM_APRESENTACAO = _ind2;
                            _tabcfgItem.TCI_TIPO_CAMPO = "S";
                            _tabcfg.TAB_DINAMICA_CONFIG_ITEM.Add(_tabcfgItem);

                            //----------
                        }
                        else
                        {
                            //----------
                            PropertyInfo propertySet = _tabItem.GetType().GetProperty(_campodb.Trim());

                            if (_conteudo[_ind, _ind2] != null)
                                propertySet.SetValue(_tabItem, _conteudo[_ind, _ind2].ToString().Trim(), null);
                        }
                    }

                    if (_ind > 0)
                        _tab.TAB_DINAMICA_ITEM.Add(_tabItem);

                    AutenticadoThread.TOTAL_LINHAS -= 1;

                }

            }

            _tdc_id = _tabDinConfigSRV.SalvarTabelaeConfig(_tdc_id, _tabcfg, _tab, true);

        }
        public string ImportarExcelProc()
        {
            TabDinamicaDTO _tab = new TabDinamicaDTO();
            TabDinamicaConfigDTO _tabcfg = new TabDinamicaConfigDTO();


            if (_tdc_id != null && _tdc_id != "")
            {
                _tab = new TabDinamicaSRV().FindById(_tdc_id);
                _tabcfg = new TabDinamicaConfigSRV().FindById(_tdc_id);
            }

            _tab.TAB_DESCRICAO = _nometabela;
            _tab.TAB_DINAMICA_ITEM = new List<TabDinamicaItemDTO>();

            _tabcfg.TDC_NOME_TABELA = _nometabela;
            _tabcfg.TDC_TIPO = 1;
            _tabcfg.TAB_DINAMICA_CONFIG_ITEM = new List<TabDinamicaConfigItemDTO>();
            
            try
            {
                string _campossql = "";
                string _valorsql = "";
                string _stringvalorsql = "";
                string _stringsql = "";
                string _usu_login = "";

                _usu_login = SessionContext.autenticado.USU_LOGIN;

                // --------

                if (uploadFile.ContentLength > 0)
                {
                    string _nomearquivo = Path.GetFileName(uploadFile.FileName);
                    string _arquivo = HttpContext.Server.MapPath("~/temp/") + Path.GetFileName(uploadFile.FileName);
                    string _filePath = Path.Combine(HttpContext.Server.MapPath("~/temp/"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(_filePath);

                    string[,] _conteudo = new ExcelLoad().Load(_filePath);

                    int _lin = _conteudo.GetLength(0) - 1;
                    int _col = _conteudo.GetLength(1) - 1;

                    var _tipocampo = new string[_conteudo.GetLength(1)];

                    AutenticadoThread.TOTAL_LINHAS = 1;

                    List<campos> _campostring = this.CarregarCampoString();


                    for (int _ind = 0; _ind <= _lin; _ind++)
                    {
                        AutenticadoThread.TOTAL_LINHAS = _ind;
                        //----------------  
                        TabDinamicaItemDTO _tabItem = new TabDinamicaItemDTO();
                        //----------------

                        for (int _ind2 = 0; _ind2 <= _col; _ind2++)
                        {
                            string _campodb = _campostring.Where(x => x.Value == (_ind2 + 1)).FirstOrDefault().Text;

                            if (_ind == 0)
                            {
                                //----------

                                TabDinamicaConfigItemDTO _tabcfgItem = new TabDinamicaConfigItemDTO();

                                var _campo = "";

                                if (_conteudo[_ind, _ind2] != null)
                                    _campo = _conteudo[_ind, _ind2].ToString();

                                _tabcfgItem.TCI_TIPO_CAMPO = "S";

                                var _ehdata = _campo.IndexOf("[D]");
                                var _ehfloat = _campo.IndexOf("[F]");

                                if (_ehdata > -1)
                                {
                                    _campo = _conteudo[_ind, _ind2].ToString().Replace("[D]", "");
                                    _tabcfgItem.TCI_TIPO_CAMPO = "D";
                                }

                                if (_ehfloat > -1)
                                {
                                    _campo = _conteudo[_ind, _ind2].ToString().Replace("[F]", "");
                                    _tabcfgItem.TCI_TIPO_CAMPO = "F";
                                }
                                
                                //----------

                                _tabcfgItem.TCI_NOME_CAMPO = "Vazio";
                                _tabcfgItem.TCI_TAMANHO_CAMPO = 10;

                                if (_conteudo[_ind, _ind2] != null)
                                {
                                    _tabcfgItem.TCI_NOME_CAMPO = _campo.ToString().Trim();
                                    _tabcfgItem.TCI_TAMANHO_CAMPO = _conteudo[_ind, _ind2].ToString().Trim().Length;
                                }

                                _tabcfgItem.TCI_NOME_CAMPODB = _campodb;
                                _tabcfgItem.TCI_ALINHAMENTO_CAMPO = "E";
                                _tabcfgItem.TCI_ORDEM_APRESENTACAO = _ind2;
                              
                                _tabcfg.TAB_DINAMICA_CONFIG_ITEM.Add(_tabcfgItem);

                                _tipocampo[_ind2] = _tabcfgItem.TCI_TIPO_CAMPO;

                                //----------
                                if (_campossql == "")
                                    _campossql = _campodb;
                                else
                                    _campossql += "," + _campodb;
                                //----------
                            }
                            else
                            {
                                //----------

                                if (_conteudo[_ind, _ind2] != null)
                                {

                                    string _conteudoCampo = _conteudo[_ind, _ind2].ToString().Replace("'", "");


                                    if (_tipocampo[_ind2] == "D")
                                    {
                                        DateTime _data00 = new DateTime();
                                        DateTime _data01 = new DateTime(0001, 01, 01);

                                        string[] separador = { "/", " " };
                                        string[] data = _conteudoCampo.Split(separador, StringSplitOptions.RemoveEmptyEntries);


                                        DateTime.TryParse((data[2] + "/" + data[1] + "/" + data[0]), out _data00);

                                        if (_data00 != _data01)
                                            _conteudoCampo = data[0] + "/" + data[1] + "/" + data[2]; 
                                    }

                                    if (_valorsql == "")
                                        _valorsql = "'" + _conteudoCampo + "'";
                                    else
                                        _valorsql += ",'" + _conteudoCampo + "'";
                                }
                                else
                                {
                                    if (_valorsql == "")
                                        _valorsql = "''";
                                    else
                                        _valorsql += ",''";
                                }

                            }
                        }
                        

                        if (_ind > 0)
                        {
                            if (_stringvalorsql == "")
                                _stringvalorsql = "('" + _tdc_id + "'," + _valorsql + ",'" + _usu_login + "')";
                            
                            _stringsql += "INSERT INTO TAB_DINAMICA_ITEM " + _campossql + " values " + _stringvalorsql+";";

                            _stringvalorsql = "";

                            _valorsql = "";
                        }
                        else
                        {
                            _tdc_id = _tabDinConfigSRV.SalvarTabelaeConfig(_tdc_id, _tabcfg, _tab, true);

                            _campossql = "(TDC_ID," + _campossql + ", USU_LOGIN)";
                        }

                    }

                }

                _tabDinConfigSRV.ImportarTabelaDinamica(_stringsql);
                                
                AutenticadoThread.TOTAL_LINHAS = 0;

                return _tdc_id;

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

                
                AutenticadoThread.TOTAL_LINHAS = 0;

                throw new Exception(ex.Message);

            }

        }
        public ActionResult ContadorProcessamento()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                response.Add("ERRO_PROCESSO", AutenticadoThread.ERRO_PROCESSO); 
                response.Add("TOTAL_LINHAS", AutenticadoThread.TOTAL_LINHAS); 
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
        
    }
}
