using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using COAD.COADGED.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Repositorios.Base;
using RTE;
using System.Text;
using COAD.COADGED.Repositorios.Contexto;
using System.IO;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Service.PortalCoad;
using COAD.CORPORATIVO.SessionUtils;
using GenericCrud.Exceptions.ErrorHandling;
using System.Web.Hosting;
using System.Data.OleDb;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Security.Permissions;
using GenericCrud.Service;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    [ValidateInput(false)]
    public class PublicacaoAreaConsultoriaController : Controller
    {
        // sigla do cargo...
        private string cargoSigla;

        // preparando objetos de serviços...
        public PublicacaoAreaConsultoriaSRV _service { get; set; }
        public PublicacaoEditadaSRV _servicePublicacaoEditada { get; set; }
        public PublicacaoAlteracaoRevogacaoSRV _servicePublicacaoAlteracaoRevogacao { get; set; }
        public PublicacaoPalavraChaveSRV _servicePublicacaoPalavraChave { get; set; }
        public PublicacaoTitulacaoSRV _servicePublicacaoTitulacao { get; set; }
        public PublicacaoRemissivoSRV _servicePublicacaoRemissivo { get; set; }
        public PublicacaoRemissaoSRV _servicePublicacaoRemissao { get; set; }
        public PublicacaoConfigSRV _servicePublicacaoConfig { get; set; }
        public PublicacaoUfSRV _servicePublicacaoUf { get; set; }
        public PublicacaoSRV _servicePublicacao { get; set; }

        // preparando objetos de revisão...
        public PublicacaoRevisaoSRV _servicePublicacaoRevisao { get; set; }
        public PublicacaoRevisaoColaboradorSRV _servicePublicacaoRevisaoColaborador { get; set; }

        // dados cadastrais...
        public InformativoSRV _serviceInformativo { get; set; }
        public UfSRV _serviceUf { get; set; }
        public ColaboradorSRV _serviceColaborador { get; set; }
        public TitulacaoSRV _serviceTitulacao { get; set; }
        public TipoMateriaSRV _serviceTpMateria { get; set; }
        public TipoAtoSRV _serviceTpAto { get; set; }
        public VeiculoSRV _serviceVeiculo { get; set; }
        public OrgaoSRV _serviceOrgao { get; set; }
        public SecoesSRV _serviceSecao { get; set; }
        public LabelsSRV _serviceLabel { get; set; }
        public AreasSRV _serviceAreas { get; set; }
        public CapitalSRV _serviceCapital { get; set; }

        // portal...
        public Tab_31SRV _serviceTab31 = new Tab_31SRV();
        public Tab_31_htmlSRV _serviceTab31html = new Tab_31_htmlSRV();
        public Tab_30SRV _serviceTab30 = new Tab_30SRV();
        public Tab_30_htmlSRV _serviceTab30html = new Tab_30_htmlSRV();

        /// <summary>
        /// ALT: 01/01/2016 - Prepara todas as instâncias do RTE Editor
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="oninit"></param>
        /// <returns></returns>
        protected Editor PrepararEditor(string obj, Action<Editor> oninit)
        {
            obj = String.IsNullOrWhiteSpace(obj) ? "Editor1" : obj;
            Editor editor = new Editor(System.Web.HttpContext.Current, obj);

            editor.ClientFolder = "/richtexteditor/";
            editor.ContentCss = "/richtexteditor/styles/richtexteditor.css";
            editor.AjaxPostbackUrl = Url.Action("EditorAjaxHandler");

            if (oninit != null)
                oninit(editor);

            bool isajax = editor.MvcInit();
            if (isajax)
                return editor;

            if (this.Request.HttpMethod == "POST")
            {
                string formdata = this.Request.Form["RTEAjaxInvoke_Control"];
                if (formdata != null)
                    editor.LoadFormData(formdata);
            }

            return editor;
        }

        /// <summary>
        /// Método interno de request do RTE Editor
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult EditorAjaxHandler()
        {
            string obj = this.Request.Form["RTEAjaxInvoke_Control"];
            PrepararEditor(obj, delegate (Editor editor) { });
            return new EmptyResult();
        }

        /// <summary>
        /// Retorna um array contendo [0]=Cargo e [1]=Sigla do cargo do usuário logado
        /// </summary>
        /// <returns></returns>
        public string[] _cargoSiglaCarregar()
        {
            string[] _retornar = new string[2];

            if (SessionContext.per_id == "REDACAO")
            {
                _retornar[0] = "Redator";
                _retornar[1] = "RDC";
            }
            else if (SessionContext.per_id == "REVISAO_TEC")
            {
                _retornar[0] = "Revisor Técnico";
                _retornar[1] = "RVT";
            }
            else if (SessionContext.per_id == "DIGITACAO")
            {
                _retornar[0] = "Digitador";
                _retornar[1] = "DGT";
            }
            else if (SessionContext.per_id == "REVISAO_ORT")
            {
                _retornar[0] = "Revisor Ortográfico";
                _retornar[1] = "RVO";
            }
            else if (SessionContext.per_id == "DIAGRAMACAO_IMP" || SessionContext.per_id == "DIAGRAMACAO_WEB")
            {
                _retornar[0] = "Diagramador";
                _retornar[1] = "DIA";
            }

            return _retornar;
        }

        /// <summary>
        /// Carrega as informações de login para a tela de publicação
        /// </summary>
        public ActionResult _dadosLoginCarregar()
        {
            JSONResponse response = new JSONResponse();

            if (SessionContext.autenticado != null)
            {
                var colaborador = _serviceColaborador.Colaboradores(null, SessionContext.autenticado.USU_LOGIN).lista.FirstOrDefault();
                var colecId = _serviceColaborador.BuscarColecionadorDoColaborador(SessionContext.autenticado.USU_LOGIN);
                var area = _serviceAreas.FindById(colecId);

                response.Add("_colecionadorId", colecId);
                response.Add("_colecionadorNome", area == null ? "" : area.ARE_CONS_DESCRICAO);
                response.Add("_colaborador", SessionContext.autenticado.USU_LOGIN);
                response.Add("_colaboradorNome", colaborador.COL_NOME);
                response.Add("_cargoEsigla", _cargoSiglaCarregar());
            }
            else
            {
                response.Add("_colecionadorId", "");
                response.Add("_colecionadorNome", "");
                response.Add("_colaborador", "");
                response.Add("_colaboradorNome", "");
                response.Add("_cargoEsigla", "");
            }

            return Json(response);
        }

        /// <summary>
        /// Retorna as informações da partial {_basico} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _basicoCarregar()
        {
            JSONResponse response = new JSONResponse();

            response.Add("_areas", _serviceAreas.FindAll().OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() }));
            response.Add("_uf", _serviceUf.FindAll().OrderBy(x => x.UF_ID).Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() }));
            response.Add("_tpMateria", _serviceTpMateria.FindAll().OrderBy(x => x.TIP_MAT_DESCRICAO).Where(x => x.TIP_MAT_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() }));

            Pagina<InformativoDTO> informativosEmProducao;
            try
            {
                informativosEmProducao = _serviceInformativo.Informativos();
                response.Add("_informativo", _serviceInformativo.FindAll().OrderByDescending(x => x.INF_ANO).ThenByDescending(x => x.INF_NUMERO).Where(x => x.INF_ATIVO == 1).Select(c => new SelectListItem() { Text = c.INF_ANO.ToString() + "/" + c.INF_NUMERO.ToString(), Value = c.INF_ANO.ToString() + "/" + c.INF_NUMERO.ToString() }));
            }
            catch
            {
                List<SelectListItem> informativo = new List<SelectListItem>();
                informativo.AddRange(new[] { new SelectListItem() { Text = "Nenhum", Value = "" } });
                response.Add("_informativo", new SelectList(informativo, "Value", "Text"));
            }

            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            response.Add("_ativo", new SelectList(ativo, "Value", "Text"));

            return Json(response);
        }

        /// <summary>
        /// Retorna as informações da partial {_origem} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _origemCarregar()
        {
            JSONResponse response = new JSONResponse();
            response.Add("_tpAto", _serviceTpAto.FindAll().OrderBy(x => x.TIP_ATO_DESCRICAO).Where(x => x.TIP_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() }));
            response.Add("_orgao", _serviceOrgao.FindAll().OrderBy(x => x.ORG_DESCRICAO).Where(x => x.ORG_ATIVO == 1).Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() }));
            return Json(response);
        }

        /// <summary>
        /// Retorna as informações da partial {_veiculacao} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _veiculacaoCarregar()
        {
            JSONResponse response = new JSONResponse();
            response.Add("_veiculo", _serviceVeiculo.FindAll().OrderBy(x => x.TVI_DESCRICAO).Where(x => x.TVI_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TVI_DESCRICAO, Value = c.TVI_ID.ToString() }));
            return Json(response);
        }

        /// <summary>
        /// Retorna as informações da partial {_revogacao} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _revogacaoCarregar()
        {
            return _origemCarregar();
        }

        /// <summary>
        /// Retorna as informações da partial {_revigoracao} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _revigoracaoCarregar()
        {
            return _origemCarregar();
        }

        /// <summary>
        /// Retorna as informações da partial {_alteracao} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _alteracaoCarregar()
        {
            return _origemCarregar();
        }

        /// <summary>
        /// Retorna as informações da partial {_titulacao} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _titulacaoCarregar()
        {
            JSONResponse response = new JSONResponse();
            response.Add("_capital", _serviceCapital.FindAll().OrderBy(x => x.CAP_NOME).Select(c => new SelectListItem() { Text = c.CAP_NOME, Value = c.CAP_ID.ToString() }));
            return Json(response);
        }

        /// <summary>
        /// Retorna as informações da partial {_localizacao} da tela de publicação
        /// </summary>
        /// <returns></returns>
        public ActionResult _localizacaoCarregar()
        {
            JSONResponse response = new JSONResponse();
            response.Add("_secao", _serviceSecao.FindAll().OrderBy(x => x.SEC_DESCRICAO).Where(x => x.SEC_ATIVO == 1).Select(c => new SelectListItem() { Text = c.SEC_DESCRICAO, Value = c.SEC_ID.ToString() }));
            response.Add("_label", _serviceLabel.FindAll().OrderBy(x => x.LBL_NOME).Where(x => x.LBL_ATIVO == 1).Select(c => new SelectListItem() { Text = c.LBL_NOME, Value = c.LBL_ID.ToString() }));
            return Json(response);
        }

        /// <summary>
        /// Cria os editores
        /// </summary>
        public void criarEditores(bool editar = true, string revisao = "0")
        {
            Editor obj = null;

            // revisão ortográfica tem um editor a mais
            if (revisao == "O")
            {
                obj =
                PrepararEditor("EditorRapido", delegate (Editor EditorRapido)
                {
                    EditorRapido.LoadFormData("");
                    EditorRapido.DisabledItems = "save";
                    EditorRapido.Width = Unit.Percentage(100);
                    EditorRapido.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                    EditorRapido.Font.Name = "Helvetica";
                    EditorRapido.SetConfig("readonly", false);
                    EditorRapido.AutoFocus = true;
                });
                ViewBag.PUB_CONTEUDO_RESENHA_RVO = obj.MvcGetString();
            }

            // ementa
            obj =
            PrepararEditor("Ementa", delegate (Editor Ementa)
            {
                Ementa.LoadFormData("");
                Ementa.DisabledItems = "save";
                Ementa.Width = Unit.Percentage(100);
                Ementa.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Ementa.Font.Name = "Helvetica";
                //Ementa.SetConfig("readonly", !editar);
                Ementa.AutoFocus = false;
            });
            ViewBag.PUB_EMENTA = obj.MvcGetString();

            // manchete
            obj =
            PrepararEditor("Manchete", delegate (Editor Manchete)
            {
                Manchete.LoadFormData("");
                Manchete.DisabledItems = "save";
                Manchete.Width = Unit.Percentage(100);
                Manchete.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Manchete.Font.Name = "Helvetica";
                //Manchete.SetConfig("readonly", !editar);
                Manchete.AutoFocus = false;
            });
            ViewBag.PUB_MANCHETE = obj.MvcGetString();

            // ementa do portal
            obj =
            PrepararEditor("EmentaPt", delegate (Editor EmentaPt)
            {
                EmentaPt.LoadFormData("");
                EmentaPt.DisabledItems = "save";
                EmentaPt.Width = Unit.Percentage(100);
                EmentaPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                EmentaPt.Font.Name = "Helvetica";
                //EmentaPt.SetConfig("readonly", !editar);
                EmentaPt.AutoFocus = false;
            });
            ViewBag.PUB_EMENTA_PORTAL = obj.MvcGetString();

            // manchete do portal
            obj =
            PrepararEditor("ManchetePt", delegate (Editor ManchetePt)
            {
                ManchetePt.LoadFormData("");
                ManchetePt.DisabledItems = "save";
                ManchetePt.Width = Unit.Percentage(100);
                ManchetePt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                ManchetePt.Font.Name = "Helvetica";
                //ManchetePt.SetConfig("readonly", !editar);
                ManchetePt.AutoFocus = false;
            });
            ViewBag.PUB_MANCHETE_PORTAL = obj.MvcGetString();

            // integra
            obj =
            PrepararEditor("Integra", delegate (Editor Integra)
            {
                Integra.LoadFormData("");
                Integra.DisabledItems = "save";
                Integra.Width = Unit.Percentage(100);
                Integra.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Integra.Font.Name = "Helvetica";
                //Integra.SetConfig("readonly", !editar);
                Integra.AutoFocus = false;
            });
            ViewBag.PUB_CONTEUDO = obj.MvcGetString();

            // resenha
            obj =
            PrepararEditor("Resenha", delegate (Editor Resenha)
            {
                Resenha.LoadFormData("");
                Resenha.DisabledItems = "save";
                Resenha.Width = Unit.Percentage(100);
                Resenha.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Resenha.Font.Name = "Helvetica";
                //Resenha.SetConfig("readonly", !editar);
                Resenha.AutoFocus = false;
            });
            ViewBag.PUB_CONTEUDO_RESENHA = obj.MvcGetString();

            // remissao
            obj =
            PrepararEditor("Remissao", delegate (Editor Remissao)
            {
                Remissao.LoadFormData("");
                Remissao.DisabledItems = "save";
                Remissao.Width = Unit.Percentage(100);
                Remissao.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Remissao.Font.Name = "Helvetica";
                Remissao.SetConfig("readonly", false);
                Remissao.AutoFocus = false;
            });
            ViewBag.PRE_REMISSAO = obj.MvcGetString();

            // publicar
            obj =
            PrepararEditor("Publicar", delegate (Editor Publicar)
            {
                Publicar.LoadFormData("");
                Publicar.DisabledItems = "save";
                Publicar.Width = Unit.Percentage(100);
                Publicar.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Publicar.Font.Name = "Helvetica";
                Publicar.SetConfig("readonly", false);
                Publicar.AutoFocus = false;
            });
            ViewBag.Publicar = obj.MvcGetString();
        }

        //

        /// <summary>
        /// Editor para tela rápida
        /// </summary>
        public void criarEditorRapido()
        {
            var obj =
            PrepararEditor("EditorRapido", delegate (Editor EditorRapido)
            {
                EditorRapido.LoadFormData("");
                EditorRapido.DisabledItems = "save";
                EditorRapido.Width = Unit.Percentage(100);
                EditorRapido.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                EditorRapido.Font.Name = "Helvetica";
                EditorRapido.SetConfig("readonly", false);
                EditorRapido.AutoFocus = true;
            });
            ViewBag.EditorRapido = obj.MvcGetString();
        }

        //

        [Autorizar(IsAjax = true)]
        public ActionResult Index2()
        {
            // carrega viewbag...
            this.InicializaViewBag();

            return View();
        }

        //

        [Autorizar(IsAjax = true)]
        public ActionResult Index()
        {
            List<SelectListItem> informativo = new List<SelectListItem>();
            informativo.AddRange(new[] { new SelectListItem() { Text = "Nenhum", Value = "" } });

            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.colaboradores = _serviceColaborador.FindAll().OrderBy(x => x.COL_NOME).Where(x => x.COL_ATIVO == 1).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });
            ViewBag.tpMateria = _serviceTpMateria.FindAll().OrderBy(x => x.TIP_MAT_DESCRICAO).Where(x => x.TIP_MAT_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });
            ViewBag.tpAto = _serviceTpAto.FindAll().OrderBy(x => x.TIP_ATO_DESCRICAO).Where(x => x.TIP_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });
            ViewBag.uf = _serviceUf.FindAll().OrderBy(x => x.UF_ID).Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");
            ViewBag.informativo = new SelectList(informativo, "Value", "Text");
            ViewBag.colecionadorId = _serviceColaborador.BuscarColecionadorDoColaborador(SessionContext.autenticado.USU_LOGIN);
            ViewBag.areas = _serviceAreas.FindAll().OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View();
        }

        //

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public void RegistrarLiberacaoMateria(int pub_id)
        {
            var publicacaoEditada = _servicePublicacaoEditada.BuscarPublicacaoSendoEditada(pub_id);
            foreach (var pe in publicacaoEditada)
            {
                if (pe != null)
                {
                    pe.EDT_LIBERADA = DateTime.Now;

                    _servicePublicacaoEditada.SalvarPublicacaoEditada(pe);
                }
            }
        }

        //

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult AcessadaPor(int publicacaoId)
        {
            JSONResponse response = new JSONResponse();
            response.Add("acessada", _servicePublicacaoEditada.Buscar(publicacaoId).lista);
            return Json(response);
        }

        //

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult QuemEstaEditando(int publicacaoId)
        {
            var quem = _servicePublicacaoEditada.BuscarQuemEstaEditando(publicacaoId);

            JSONResponse response = new JSONResponse();
            response.Add("quemEstaEditando", quem == null ? "Ninguém" : quem.USU_LOGIN);
            return Json(response);
        }

        //

        [ValidateInput(false)]
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Novo(bool editorRapido = false)
        {
            ViewBag.operacao = "I";
            ViewBag.revisao = false;
            ViewBag.colecionadorId = false;
            ViewBag.publicacaoId = false;
            ViewBag.cabecaMateria = false;

            if (editorRapido)
                this.criarEditorRapido();
            else
                this.criarEditores();

            return View("Editar");
        }

        //

        [ValidateInput(false)]
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Editar(int publicacaoId, int? colecionadorId, string revisao = "0", bool editorRapido = false)
        {
            bool lSomenteLeitura = _servicePublicacaoEditada.BuscarSomenteLeitura(publicacaoId);

            _servicePublicacaoEditada.RegistrarEdicaoMateria(publicacaoId, lSomenteLeitura);

            ViewBag.operacao = lSomenteLeitura ? "C" : "A";
            ViewBag.revisao = revisao;
            ViewBag.colecionadorId = colecionadorId;
            ViewBag.publicacaoId = publicacaoId;
            ViewBag.cabecaMateria = false;

            if (editorRapido)
                this.criarEditorRapido();
            else
                this.criarEditores(true, revisao);

            return View("Editar");
        }

        //

        [ValidateInput(false)]
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Detalhes(int publicacaoId, int? colecionadorId, string revisao = "0", bool editorRapido = false)
        {
            _servicePublicacaoEditada.RegistrarEdicaoMateria(publicacaoId, true);

            ViewBag.operacao = "C";
            ViewBag.revisao = revisao;
            ViewBag.colecionadorId = colecionadorId;
            ViewBag.publicacaoId = publicacaoId;
            ViewBag.cabecaMateria = false;

            if (editorRapido)
                this.criarEditorRapido();
            else
                this.criarEditores(false, revisao);

            return View("Editar");
        }

        // Carregando, inicializando viewbags...
        public void InicializaViewBag(Boolean? novo = null, int? publicacaoId = null, int? colecionadorId = null)
        {
            // informativos em produção
            Pagina<InformativoDTO> informativosEmProducao;
            try
            {
                informativosEmProducao = _serviceInformativo.Informativos();
                ViewBag.informativo = _serviceInformativo.FindAll().OrderByDescending(x => x.INF_ANO).ThenByDescending(x => x.INF_NUMERO).Where(x => x.INF_ATIVO == 1).Select(c => new SelectListItem() { Text = c.INF_ANO.ToString() + "/" + c.INF_NUMERO.ToString(), Value = c.INF_ANO.ToString() + "/" + c.INF_NUMERO.ToString() });
            }
            catch
            {
                List<SelectListItem> informativo = new List<SelectListItem>();
                informativo.AddRange(new[] { new SelectListItem() { Text = "Nenhum", Value = "" } });
                ViewBag.informativo = new SelectList(informativo, "Value", "Text");
            }

            // buscando a publicacaoAreaConsultoria...
            var publicacaoAreaConsultoria = (publicacaoId != null && colecionadorId != null) ? _service.FindById(publicacaoId, colecionadorId) : null;

            // colaborador - login
            ViewBag.colaborador = SessionContext.autenticado.USU_LOGIN;

            // colecionador do colaborador
            ViewBag.colecionadorId = _serviceColaborador.BuscarColecionadorDoColaborador(ViewBag.colaborador);

            // colecionador - nome
            var area = _serviceAreas.FindById(ViewBag.colecionadorId);
            ViewBag.colecionadorNome = area == null ? "" : area.ARE_CONS_DESCRICAO;
            ViewBag.cabecaMateria = (publicacaoAreaConsultoria == null || publicacaoAreaConsultoria.PUBLICACAO.TIP_MAT_ID == null) ? "" :
                                     _serviceTpMateria.FindById(publicacaoAreaConsultoria.PUBLICACAO.TIP_MAT_ID).ARE_CABECA_MATERIA.ToString();

            // cargo do colaborador
            Pagina<ColaboradorDTO> colaborador = _serviceColaborador.Colaboradores(null, ViewBag.colaborador);
            ViewBag.colaboradorNome = colaborador.lista.FirstOrDefault().COL_NOME;
            if (SessionContext.per_id == "REDACAO")
            {
                ViewBag.cargo = "Redator"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_DESCRICAO;
                ViewBag.cargoSigla = "RDC"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_SIGLA;
            }
            else if (SessionContext.per_id == "REVISAO_TEC")
            {
                ViewBag.cargo = "Revisor Técnico"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_DESCRICAO;
                ViewBag.cargoSigla = "RVT"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_SIGLA;
            }
            else if (SessionContext.per_id == "DIGITACAO")
            {
                ViewBag.cargo = "Digitador"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_DESCRICAO;
                ViewBag.cargoSigla = "DGT"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_SIGLA;
            }
            else if (SessionContext.per_id == "REVISAO_ORT")
            {
                ViewBag.cargo = "Revisor Ortográfico"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_DESCRICAO;
                ViewBag.cargoSigla = "RVO"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_SIGLA;
            }
            else if (SessionContext.per_id == "DIAGRAMACAO_IMP" || SessionContext.per_id == "DIAGRAMACAO_WEB")
            {
                ViewBag.cargo = "Diagramador"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_DESCRICAO;
                ViewBag.cargoSigla = "DIA"; //colaborador.lista.FirstOrDefault().CARGOS.CRG_SIGLA;
            }

            this.cargoSigla = ViewBag.cargoSigla;

            // ativo=1 ou inativo=0
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // UF
            var uf = _serviceUf.FindAll();
            ViewBag.uf = uf.OrderBy(x => x.UF_ID).Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });

            // gg
            var titulacao = _serviceTitulacao.FindAll();
            if (ViewBag.colecionadorId > 0)
            {
                int colID = (ViewBag.colecionadorId == 3) ? 4 : ViewBag.colecionadorId;
                ViewBag.gg = titulacao.OrderBy(x => x.TIT_DESCRICAO).Where(tit => tit.TIT_TIPO == "G" && tit.ARE_CONS_ID == colID).Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO + (ViewBag.colecionadorId == 2 && !String.IsNullOrWhiteSpace(c.UF_ID)) ? " - " + c.UF_ID : "", Value = c.TIT_ID.ToString() });
            }
            else
            {
                ViewBag.gg = titulacao.OrderBy(x => x.TIT_DESCRICAO).Where(tit => tit.TIT_TIPO == "G").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() });
            }

            // tipo de matéria
            var tpMateria = _serviceTpMateria.FindAll();
            ViewBag.tpMateria = tpMateria.OrderBy(x => x.TIP_MAT_DESCRICAO).Where(x => x.TIP_MAT_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });

            // tipo de ato
            var tpAto = _serviceTpAto.FindAll();
            ViewBag.tpAto = tpAto.OrderBy(x => x.TIP_ATO_DESCRICAO).Where(x => x.TIP_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });

            // veículo
            var veiculo = _serviceVeiculo.FindAll();
            ViewBag.veiculo = veiculo.OrderBy(x => x.TVI_DESCRICAO).Where(x => x.TVI_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TVI_DESCRICAO, Value = c.TVI_ID.ToString() });

            // órgão
            var orgao = _serviceOrgao.FindAll();
            ViewBag.orgao = orgao.OrderBy(x => x.ORG_DESCRICAO).Where(x => x.ORG_ATIVO == 1).Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() });

            // seção
            var secao = _serviceSecao.FindAll();
            ViewBag.secao = secao.OrderBy(x => x.SEC_DESCRICAO).Where(x => x.SEC_ATIVO == 1).Select(c => new SelectListItem() { Text = c.SEC_DESCRICAO, Value = c.SEC_ID.ToString() });

            // label
            var label = _serviceLabel.FindAll();
            ViewBag.label = label.OrderBy(x => x.LBL_NOME).Where(x => x.LBL_ATIVO == 1).Select(c => new SelectListItem() { Text = c.LBL_NOME, Value = c.LBL_ID.ToString() });

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // colaboradores
            var colaboradores = _serviceColaborador.FindAll();
            ViewBag.colaboradores = colaboradores.OrderBy(x => x.COL_NOME).Where(x => x.COL_ATIVO == 1).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });

            // capital
            var capital = _serviceCapital.FindAll();
            ViewBag.capital = capital.OrderBy(x => x.CAP_NOME).Select(c => new SelectListItem() { Text = c.CAP_NOME, Value = c.CAP_ID.ToString() });
        }

        // Gg
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Gg(int? colecionadorId, string[] ufs = null)
        {
            // grande grupo
            colecionadorId = (colecionadorId == 3) ? 4 : colecionadorId;
            var gg = _serviceTitulacao.Gg(colecionadorId, ufs).lista.Where(x => x.TIT_ATIVO == 1).OrderBy(x => x.TIT_DESCRICAO).Select(x => new { TIT_ID = x.TIT_ID, TIT_DESCRICAO = x.TIT_DESCRICAO });

            JSONResponse response = new JSONResponse();
            response.Add("gg", gg);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Verbetes(int ggId)
        {
            // verbetes
            var verbetes = _serviceTitulacao.Verbetes(ggId).lista.Where(x => x.TIT_ATIVO == 1).OrderBy(x => x.TIT_DESCRICAO).Select(x => new { TIT_ID = x.TIT_ID, TIT_DESCRICAO = x.TIT_DESCRICAO });

            JSONResponse response = new JSONResponse();
            response.Add("verbetes", verbetes);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Subverbetes(int vbId)
        {
            // subverbetes
            var subverbetes = _serviceTitulacao.Subverbetes(vbId).lista.Where(x => x.TIT_ATIVO == 1).OrderBy(x => x.TIT_DESCRICAO).Select(x => new { TIT_ID = x.TIT_ID, TIT_DESCRICAO = x.TIT_DESCRICAO });

            JSONResponse response = new JSONResponse();
            response.Add("subverbetes", subverbetes);

            return Json(response);
        }

        [ValidateInput(false)]
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Novo2()
        {
            // carrega viewbag...
            this.InicializaViewBag(true);

            // ementa
            var obj =
            PrepararEditor("Ementa", delegate (Editor Ementa)
            {
                Ementa.LoadFormData("");
                Ementa.DisabledItems = "save";
                Ementa.Width = Unit.Percentage(100);
                Ementa.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Ementa.Font.Name = "Helvetica";
            });
            ViewBag.PUB_EMENTA = obj.MvcGetString();

            // manchete
            obj =
            PrepararEditor("Manchete", delegate (Editor Manchete)
            {
                Manchete.LoadFormData("");
                Manchete.DisabledItems = "save";
                Manchete.Width = Unit.Percentage(100);
                Manchete.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Manchete.Font.Name = "Helvetica";
            });
            ViewBag.PUB_MANCHETE = obj.MvcGetString();

            // ementa do portal
            obj =
            PrepararEditor("EmentaPt", delegate (Editor EmentaPt)
            {
                EmentaPt.LoadFormData("");
                EmentaPt.DisabledItems = "save";
                EmentaPt.Width = Unit.Percentage(100);
                EmentaPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                EmentaPt.Font.Name = "Helvetica";
            });
            ViewBag.PUB_EMENTA_PORTAL = obj.MvcGetString();

            // manchete do portal
            obj =
            PrepararEditor("ManchetePt", delegate (Editor ManchetePt)
            {
                ManchetePt.LoadFormData("");
                ManchetePt.DisabledItems = "save";
                ManchetePt.Width = Unit.Percentage(100);
                ManchetePt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                ManchetePt.Font.Name = "Helvetica";
            });
            ViewBag.PUB_MANCHETE_PORTAL = obj.MvcGetString();

            // integra
            obj =
            PrepararEditor("Integra", delegate (Editor Integra)
            {
                Integra.LoadFormData("");
                Integra.DisabledItems = "save";
                Integra.Width = Unit.Percentage(100);
                Integra.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Integra.Font.Name = "Helvetica";
            });
            ViewBag.PUB_CONTEUDO = obj.MvcGetString();

            // resenha
            obj =
            PrepararEditor("Resenha", delegate (Editor Resenha)
            {
                Resenha.LoadFormData("");
                Resenha.DisabledItems = "save";
                Resenha.Width = Unit.Percentage(100);
                Resenha.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Resenha.Font.Name = "Helvetica";
            });
            ViewBag.PUB_CONTEUDO_RESENHA = obj.MvcGetString();

            // remissao
            obj =
            PrepararEditor("Remissao", delegate (Editor Remissao)
            {
                Remissao.LoadFormData("");
                Remissao.DisabledItems = "save";
                Remissao.Width = Unit.Percentage(100);
                Remissao.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Remissao.Font.Name = "Helvetica";
            });
            ViewBag.PRE_REMISSAO = obj.MvcGetString();

            // publicar
            obj =
            PrepararEditor("Publicar", delegate (Editor Publicar)
            {
                Publicar.LoadFormData("");
                Publicar.DisabledItems = "save";
                Publicar.Width = Unit.Percentage(100);
                Publicar.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                Publicar.Font.Name = "Helvetica";
            });
            ViewBag.Publicar = obj.MvcGetString();

            // define que os registros serão incluídos...
            ViewBag.lIncluir = "S";

            return View("Novo");
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        [HttpPost]
        public ActionResult Editar2(int publicacaoId, int? colecionadorId, string revisao = "0")
        {
            // carrega viewbag...
            this.InicializaViewBag();

            // veio de uma revisão?
            ViewBag.revisao = revisao;

            // identificando o colecionador
            ViewBag.colecionadorId = colecionadorId;

            // buscando a publicacaoAreaConsultoria...
            var publicacaoAreaConsultoria = _service.FindById(publicacaoId, colecionadorId);

            publicacaoAreaConsultoria.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO = (ICollection<PublicacaoAlteracaoRevogacaoDTO>)_servicePublicacaoAlteracaoRevogacao.PublicacaoAlteracaoRevogacao(null, publicacaoAreaConsultoria.PUB_ID).lista;

            if (publicacaoAreaConsultoria != null && publicacaoAreaConsultoria.ARE_CONS_ID != null && publicacaoAreaConsultoria.PUB_ID != null)
            {
                // resenha
                var obj =
                PrepararEditor("Resenha", delegate (Editor Resenha)
                {
                    Resenha.LoadFormData("");
                    Resenha.DisabledItems = "save";
                    Resenha.Width = Unit.Percentage(100);
                    Resenha.Text = publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA;
                    Resenha.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                    Resenha.Font.Name = "Helvetica";
                });
                ViewBag.PUB_CONTEUDO_RESENHA = obj.MvcGetString();

                // remissões...
                string txtCabecalho = "";
                string txtComRemissao = "";

                if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null)
                {
                    // substituindo as remissões...
                    txtComRemissao = _service.RemissaoMateriaImpressa(publicacaoAreaConsultoria).MATERIA_IMPRESSA.ToString();

                    // substituindo o cabeçalho da matéria...
                    txtCabecalho = _service.CabecaMateriaImpressa(publicacaoAreaConsultoria).ToString();
                }
                obj =
                PrepararEditor("Remissao", delegate (Editor Remissao)
                {
                    Remissao.LoadFormData("");
                    Remissao.DisabledItems = "save";
                    Remissao.Width = Unit.Percentage(100);
                    Remissao.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA == null) ? "" : "Matéria: " + publicacaoAreaConsultoria.PUB_ID.ToString() + "<br>" + txtCabecalho + txtComRemissao;
                    Remissao.SetConfig("readonly", (revisao != "0"));
                    Remissao.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                    Remissao.Font.Name = "Helvetica";
                });
                ViewBag.PRE_REMISSAO = obj.MvcGetString();

                // Digitadores não terão isso \\
                if (this.cargoSigla != "DGT")
                {
                    if (this.cargoSigla != "RVO")
                    {
                        // ementa
                        obj =
                        PrepararEditor("Ementa", delegate (Editor Ementa)
                        {
                            Ementa.LoadFormData("");
                            Ementa.DisabledItems = "save";
                            Ementa.Width = Unit.Percentage(100);
                            Ementa.Text = publicacaoAreaConsultoria.PUB_EMENTA;
                            Ementa.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            Ementa.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_EMENTA = obj.MvcGetString();

                        // manchete
                        obj =
                        PrepararEditor("Manchete", delegate (Editor Manchete)
                        {
                            Manchete.LoadFormData("");
                            Manchete.DisabledItems = "save";
                            Manchete.Width = Unit.Percentage(100);
                            Manchete.Text = publicacaoAreaConsultoria.PUB_MANCHETE;
                            Manchete.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            Manchete.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_MANCHETE = obj.MvcGetString();

                        // ementa portal
                        obj =
                        PrepararEditor("EmentaPt", delegate (Editor EmentaPt)
                        {
                            EmentaPt.LoadFormData("");
                            EmentaPt.DisabledItems = "save";
                            EmentaPt.Width = Unit.Percentage(100);
                            EmentaPt.Text = publicacaoAreaConsultoria.PUB_EMENTA_PORTAL;
                            EmentaPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            EmentaPt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_EMENTA_PORTAL = obj.MvcGetString();

                        // manchete portal
                        obj =
                        PrepararEditor("ManchetePt", delegate (Editor ManchetePt)
                        {
                            ManchetePt.LoadFormData("");
                            ManchetePt.DisabledItems = "save";
                            ManchetePt.Width = Unit.Percentage(100);
                            ManchetePt.Text = publicacaoAreaConsultoria.PUB_MANCHETE_PORTAL;
                            ManchetePt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            ManchetePt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_MANCHETE_PORTAL = obj.MvcGetString();
                    }

                    // integra
                    obj =
                    PrepararEditor("Integra", delegate (Editor Integra)
                    {
                        Integra.LoadFormData("");
                        Integra.DisabledItems = "save";
                        Integra.Width = Unit.Percentage(100);
                        Integra.Text = publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO;
                        Integra.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                        Integra.Font.Name = "Helvetica";
                    });
                    ViewBag.PUB_CONTEUDO = obj.MvcGetString();

                    // publicar...
                    if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO != null)
                    {
                        // substituindo o cabeçalho da matéria...
                        txtCabecalho = _service.CabecaMateriaImpressa(publicacaoAreaConsultoria, true).ToString();

                        // montando a matéria...
                        publicacaoAreaConsultoria.PUBLICAR_PORTAL = "Matéria: " + publicacaoAreaConsultoria.PUB_ID.ToString() + "<br>" + txtCabecalho + publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO;

                        // ortográfico
                        obj =
                        PrepararEditor("OrtograficoPt", delegate (Editor OrtograficoPt)
                        {
                            OrtograficoPt.LoadFormData("");
                            OrtograficoPt.DisabledItems = "save";
                            OrtograficoPt.Width = Unit.Percentage(100);
                            OrtograficoPt.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO == null) ?
                                                  publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT :
                                                  publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO.ToString();
                            if (ViewBag.cargoSigla != "RVO")
                                OrtograficoPt.SetConfig("readonly", (revisao != "0"));
                            OrtograficoPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            OrtograficoPt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_CONTEUDO_RVO = obj.MvcGetString();
                    }

                    obj =
                    PrepararEditor("Publicar", delegate (Editor Publicar)
                    {
                        Publicar.LoadFormData("");
                        Publicar.DisabledItems = "save";
                        Publicar.Width = Unit.Percentage(100);
                        Publicar.Text = (publicacaoAreaConsultoria.PUBLICAR_PORTAL == null) ? "" : publicacaoAreaConsultoria.PUBLICAR_PORTAL.ToString();
                        Publicar.SetConfig("readonly", (revisao != "0"));
                        Publicar.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                        Publicar.Font.Name = "Helvetica";
                    });
                    ViewBag.Publicar = obj.MvcGetString();
                }

                if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null)
                {
                    // digitador
                    obj =
                    PrepararEditor("Digitador", delegate (Editor Digitador)
                    {
                        Digitador.LoadFormData("");
                        Digitador.DisabledItems = "save";
                        Digitador.Width = Unit.Percentage(100);
                        Digitador.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT == null) ?
                                          txtCabecalho + txtComRemissao :
                                          publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT.ToString();
                        if (ViewBag.cargoSigla != "DGT")
                            Digitador.SetConfig("readonly", (revisao != "0"));
                        Digitador.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                        Digitador.Font.Name = "Helvetica";
                    });
                    ViewBag.PUB_CONTEUDO_RESENHA_DGT = obj.MvcGetString();

                    if (this.cargoSigla != "DGT")
                    {
                        // ortográfico Impresso
                        obj =
                        PrepararEditor("Ortografico", delegate (Editor Ortografico)
                        {
                            Ortografico.LoadFormData("");
                            Ortografico.DisabledItems = "save";
                            Ortografico.Width = Unit.Percentage(100);
                            Ortografico.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO == null) ?
                                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT :
                                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO.ToString();
                            if (ViewBag.cargoSigla != "RVO")
                                Ortografico.SetConfig("readonly", (revisao != "0"));
                            Ortografico.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            Ortografico.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_CONTEUDO_RESENHA_RVO = obj.MvcGetString();
                    }
                }

                if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO != null)
                {
                    if (this.cargoSigla == "RVO")
                    {
                        // ortográfico Portal
                        obj =
                        PrepararEditor("OrtograficoPt", delegate (Editor OrtograficoPt)
                        {
                            OrtograficoPt.LoadFormData("");
                            OrtograficoPt.DisabledItems = "save";
                            OrtograficoPt.Width = Unit.Percentage(100);
                            OrtograficoPt.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO == null) ?
                                                  publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RDC :
                                                  publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO.ToString();
                            if (ViewBag.cargoSigla != "RVO")
                                OrtograficoPt.SetConfig("readonly", (revisao != "0"));
                            OrtograficoPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            OrtograficoPt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_CONTEUDO_RVO = obj.MvcGetString();
                    }
                }
            }

            // id da publicação a editar
            ViewBag.publicacaoId = publicacaoId;

            // define que os registros serão alterados...
            ViewBag.lIncluir = "N";

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes2(int publicacaoId, int? colecionadorId)
        {
            // carrega viewbag...
            this.InicializaViewBag();

            // identificando o colecionador
            ViewBag.colecionadorId = colecionadorId;

            // veio de uma revisão?
            string revisao = "0";
            ViewBag.revisao = revisao;

            // buscando a publicacaoAreaConsultoria...
            var publicacaoAreaConsultoria = _service.FindById(publicacaoId, colecionadorId);

            //publicacaoAreaConsultoria.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO = (ICollection<PublicacaoAlteracaoRevogacaoDTO>)_servicePublicacaoAlteracaoRevogacao.PublicacaoAlteracaoRevogacao(null, publicacaoAreaConsultoria.PUB_ID).lista;

            if (publicacaoAreaConsultoria != null && publicacaoAreaConsultoria.ARE_CONS_ID != null && publicacaoAreaConsultoria.PUB_ID != null)
            {
                string txtCabecalho = "";
                string txtComRemissao = "";

                // resenha
                var obj =
                PrepararEditor("Resenha", delegate (Editor Resenha)
                {
                    Resenha.LoadFormData("");
                    Resenha.DisabledItems = "save";
                    Resenha.Width = Unit.Percentage(100);
                    Resenha.Text = publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA;
                    Resenha.SetConfig("readonly", (revisao == "0"));
                    Resenha.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                    Resenha.Font.Name = "Helvetica";
                });
                ViewBag.PUB_CONTEUDO_RESENHA = obj.MvcGetString();

                // remissões...
                if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null)
                {
                    // substituindo as remissões...
                    txtComRemissao = _service.RemissaoMateriaImpressa(publicacaoAreaConsultoria).MATERIA_IMPRESSA.ToString();

                    // substituindo o cabeçalho da matéria...
                    txtCabecalho = _service.CabecaMateriaImpressa(publicacaoAreaConsultoria).ToString();
                }
                obj =
                PrepararEditor("Remissao", delegate (Editor Remissao)
                {
                    Remissao.LoadFormData("");
                    Remissao.DisabledItems = "save";
                    Remissao.Width = Unit.Percentage(100);
                    Remissao.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA == null) ? "" : "Matéria: " + publicacaoAreaConsultoria.PUB_ID.ToString() + "<br>" + txtCabecalho + txtComRemissao;
                    Remissao.SetConfig("readonly", (revisao == "0"));
                    Remissao.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                    Remissao.Font.Name = "Helvetica";
                });
                ViewBag.PRE_REMISSAO = obj.MvcGetString();

                if (this.cargoSigla != "DGT")
                {
                    if (this.cargoSigla != "RVO")
                    {
                        // ementa
                        obj =
                        PrepararEditor("Ementa", delegate (Editor Ementa)
                        {
                            Ementa.LoadFormData("");
                            Ementa.DisabledItems = "save";
                            Ementa.Width = Unit.Percentage(100);
                            Ementa.Text = publicacaoAreaConsultoria.PUB_EMENTA;
                            Ementa.SetConfig("readonly", (revisao == "0"));
                            Ementa.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            Ementa.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_EMENTA = obj.MvcGetString();

                        // manchete
                        obj =
                        PrepararEditor("Manchete", delegate (Editor Manchete)
                        {
                            Manchete.LoadFormData("");
                            Manchete.DisabledItems = "save";
                            Manchete.Width = Unit.Percentage(100);
                            Manchete.Text = publicacaoAreaConsultoria.PUB_MANCHETE;
                            Manchete.SetConfig("readonly", (revisao == "0"));
                            Manchete.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            Manchete.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_MANCHETE = obj.MvcGetString();

                        // ementa portal
                        obj =
                        PrepararEditor("EmentaPt", delegate (Editor EmentaPt)
                        {
                            EmentaPt.LoadFormData("");
                            EmentaPt.DisabledItems = "save";
                            EmentaPt.Width = Unit.Percentage(100);
                            EmentaPt.Text = publicacaoAreaConsultoria.PUB_EMENTA_PORTAL;
                            EmentaPt.SetConfig("readonly", (revisao == "0"));
                            EmentaPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            EmentaPt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_EMENTA_PORTAL = obj.MvcGetString();

                        // manchete portal
                        obj =
                        PrepararEditor("ManchetePt", delegate (Editor ManchetePt)
                        {
                            ManchetePt.LoadFormData("");
                            ManchetePt.DisabledItems = "save";
                            ManchetePt.Width = Unit.Percentage(100);
                            ManchetePt.Text = publicacaoAreaConsultoria.PUB_MANCHETE_PORTAL;
                            ManchetePt.SetConfig("readonly", (revisao == "0"));
                            ManchetePt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            ManchetePt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_MANCHETE_PORTAL = obj.MvcGetString();
                    }

                    // integra
                    obj =
                    PrepararEditor("Integra", delegate (Editor Integra)
                    {
                        Integra.LoadFormData("");
                        Integra.DisabledItems = "save";
                        Integra.Width = Unit.Percentage(100);
                        Integra.Text = publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO;
                        Integra.SetConfig("readonly", (revisao == "0"));
                        Integra.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                        Integra.Font.Name = "Helvetica";
                    });
                    ViewBag.PUB_CONTEUDO = obj.MvcGetString();

                    // publicar...
                    if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO != null)
                    {
                        // substituindo o cabeçalho da matéria...
                        txtCabecalho = _service.CabecaMateriaImpressa(publicacaoAreaConsultoria, true).ToString();

                        // montando a matéria...
                        publicacaoAreaConsultoria.PUBLICAR_PORTAL = "Matéria: " + publicacaoAreaConsultoria.PUB_ID.ToString() + "<br>" + txtCabecalho + publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO;
                    }

                    obj =
                    PrepararEditor("Publicar", delegate (Editor Publicar)
                    {
                        Publicar.LoadFormData("");
                        Publicar.DisabledItems = "save";
                        Publicar.Width = Unit.Percentage(100);
                        Publicar.Text = (publicacaoAreaConsultoria.PUBLICAR_PORTAL == null) ? "" : publicacaoAreaConsultoria.PUBLICAR_PORTAL.ToString();
                        Publicar.SetConfig("readonly", (revisao == "0"));
                        Publicar.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                        Publicar.Font.Name = "Helvetica";
                    });
                    ViewBag.Publicar = obj.MvcGetString();
                }

                if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null)
                {
                    // digitador
                    obj =
                    PrepararEditor("Digitador", delegate (Editor Digitador)
                    {
                        Digitador.LoadFormData("");
                        Digitador.DisabledItems = "save";
                        Digitador.Width = Unit.Percentage(100);
                        Digitador.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT == null) ?
                                          txtCabecalho + txtComRemissao :
                                          publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT.ToString();
                        if (ViewBag.cargoSigla != "DGT")
                            Digitador.SetConfig("readonly", (revisao != "0"));
                        Digitador.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                        Digitador.Font.Name = "Helvetica";
                    });
                    ViewBag.PUB_CONTEUDO_RESENHA_DGT = obj.MvcGetString();

                    if (this.cargoSigla != "DGT")
                    {
                        // ortográfico
                        obj =
                        PrepararEditor("Ortografico", delegate (Editor Ortografico)
                        {
                            Ortografico.LoadFormData("");
                            Ortografico.DisabledItems = "save";
                            Ortografico.Width = Unit.Percentage(100);
                            Ortografico.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO == null) ?
                                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT :
                                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO.ToString();
                            if (ViewBag.cargoSigla != "RVO")
                                Ortografico.SetConfig("readonly", (revisao == "0"));
                            Ortografico.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            Ortografico.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_CONTEUDO_RESENHA_RVO = obj.MvcGetString();
                    }
                }

                if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO != null)
                {
                    if (this.cargoSigla == "RVO")
                    {
                        // ortográfico Portal
                        obj =
                        PrepararEditor("OrtograficoPt", delegate (Editor OrtograficoPt)
                        {
                            OrtograficoPt.LoadFormData("");
                            OrtograficoPt.DisabledItems = "save";
                            OrtograficoPt.Width = Unit.Percentage(100);
                            OrtograficoPt.Text = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO == null) ?
                                                  publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RDC :
                                                  publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO.ToString();
                            if (ViewBag.cargoSigla != "RVO")
                                OrtograficoPt.SetConfig("readonly", (revisao != "0"));
                            OrtograficoPt.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                            OrtograficoPt.Font.Name = "Helvetica";
                        });
                        ViewBag.PUB_CONTEUDO_RVO = obj.MvcGetString();
                    }
                }
            }

            // id da publicação a editar
            ViewBag.publicacaoId = publicacaoId;

            // define que os registros serão alterados...
            ViewBag.lIncluir = "N";

            return View("Detalhes");
        }

        // retorna o Texto Atualizado...
        [AjaxExceptionFilter]
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult textoAtualizado(int? publicacaoId = null, int? colecionadorId = null, string cargo = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (publicacaoId != null && colecionadorId != null)
                {
                    var publicacaoAreaConsultoria = _service.FindById(publicacaoId, colecionadorId);

                    publicacaoAreaConsultoria.PUBLICAR_PORTAL = _service.PublicarPortalTexto(publicacaoAreaConsultoria);
                    publicacaoAreaConsultoria.MATERIA_IMPRESSA = _service.MateriaImpressaTexto(publicacaoAreaConsultoria);

                    // eliminando objetos pesados
                    publicacaoAreaConsultoria.PUBLICACAO = null;
                    publicacaoAreaConsultoria.PUBLICACAO_UF = null;
                    publicacaoAreaConsultoria.PUBLICACAO_TITULACAO = null;
                    publicacaoAreaConsultoria.PUBLICACAO_REVISAO_COLABORADOR = null;
                    publicacaoAreaConsultoria.PUBLICACAO_REVISAO = null;
                    publicacaoAreaConsultoria.PUBLICACAO_REMISSIVO = null;
                    publicacaoAreaConsultoria.PUBLICACAO_REMISSAO = null;
                    publicacaoAreaConsultoria.PUBLICACAO_PALAVRA_CHAVE = null;
                    publicacaoAreaConsultoria.PUBLICACAO_CONFIG = null;

                    response.Add("txt", publicacaoAreaConsultoria);
                }
            }
            catch (Exception e)
            {
                SessionUtil.HandleException(e);
            }
            return Json(response);
        }

        // paginar matéria - tela principal...
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult PaginarMateria()
        {
            //List<SelectListItem> informativo = new List<SelectListItem>();
            //informativo.AddRange(new[] { new SelectListItem() { Text = "Nenhum", Value = "" } });

            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.colaboradores = _serviceColaborador.FindAll().OrderBy(x => x.COL_NOME).Where(x => x.COL_ATIVO == 1).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });
            //ViewBag.tpMateria = _serviceTpMateria.FindAll().OrderBy(x => x.TIP_MAT_DESCRICAO).Where(x => x.TIP_MAT_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });
            //ViewBag.tpAto = _serviceTpAto.FindAll().OrderBy(x => x.TIP_ATO_DESCRICAO).Where(x => x.TIP_ATIVO == 1).Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });
            //ViewBag.uf = _serviceUf.FindAll().OrderBy(x => x.UF_ID).Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });
            //ViewBag.informativo = new SelectList(informativo, "Value", "Text");
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");
            ViewBag.colecionadorId = _serviceColaborador.BuscarColecionadorDoColaborador(SessionContext.autenticado.USU_LOGIN);
            ViewBag.areas = _serviceAreas.FindAll().OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View();
        }

        // salvando paginação da matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult salvarPaginacaoMateria(IList<PublicacaoAreaConsultoriaDTO> publicacaoAreaConsultoria)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarPaginacaoMateria(publicacaoAreaConsultoria);
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result);
            }
        }

        // lendo dados para o index...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult PublicacoesAreaConsultoria(string uf = null, string faseMateria = null, int?[] tpMateria = null, DateTime? dtCadastro = null, string coadgedBI = null, int? nrMateria = null, string anoAto = null, int? tpAto = null, string nrAto = null, int? nrInformativo = null, string anoInformativo = null, int? colecionadorId = null, int? colaboradorId = null, int? gg = null, int? vb = null, int? svb = null, DateTime? dtAto = null, int? ativoId = null, int pagina = 1)
        {
            Pagina<PublicacaoAreaConsultoriaDTO> page = _service.PublicacoesAreaConsultoria(uf, faseMateria, dtCadastro, coadgedBI, tpMateria, nrMateria, anoAto, tpAto, nrAto, nrInformativo, anoInformativo, colecionadorId, colaboradorId, gg, vb, svb, dtAto, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("publicacoesAreaConsultoria", page);

            return Json(response);
        }

        // buscando o ato a ser revogado/alterado...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult BuscarAtoRevogado(int colecionadorId, string tipoR, int? tpAto = null, string nrAto = null, DateTime? dtAto = null, Boolean importarSeNecessario = false)
        {
            JSONResponse response = new JSONResponse();

            var tipo = "Revigoração";
            if (tipoR == "R")
                tipo = "Revogação";
            else if (tipoR == "A")
                tipo = "Alteração";

            // buscando no COADGED \\
            var revogado = _servicePublicacao.Publicacoes(tpAto, nrAto, dtAto).lista;
            if (revogado.Count() == 0)
            {
                // buscando no PORTAL \\
                if (colecionadorId == 4) // DP=30 \\
                {
                    var revogado30 = _serviceTab30.Coad(null, null, _serviceTpAto.FindById(tpAto).TIP_ATO_DESCRICAO, nrAto, dtAto).lista;
                    if (revogado30.Count() == 0)
                        response.message = Message.Success("ATENÇÃO! O Ato informado para (" + tipo +
                                                           ") não está registrado em nossas bases (COADGED ou PORTAL). Por favor, efetue o cadastramento deste Ato antes de proceder sua " + tipo + ".");
                    else if (importarSeNecessario)
                    {
                        try // importando...
                        {
                            int[] retorno = _service.ImportarMateriaTab30(revogado30.FirstOrDefault());
                            response.Add("revogado", retorno);
                        }
                        catch (Exception e)
                        {
                            response.message = Message.Success("ATENÇÃO! Ocorreu um erro durante a importação da matéria. Verifique se o Portal DP está online e tente novamente.");
                            SessionUtil.HandleException(e);
                        }
                    }
                    else
                        response.message = Message.Success("ATENÇÃO! Esta matéria consta apenas no PORTAL e para sofrer (" + tipo + ") precisa ser importada.");

                    if (response.result.Count() == 0) // se não importou...
                        response.Add("revogado", revogado30.FirstOrDefault());
                }
                else // ATC=31 \\
                {
                    var revogado31 = _serviceTab31.Coad(null, null, _serviceTpAto.FindById(tpAto).TIP_ATO_DESCRICAO, nrAto, dtAto).lista;
                    if (revogado31.Count() == 0)
                        response.message = Message.Success("ATENÇÃO! O Ato informado para (" + tipo +
                                                           ") não está registrado em nossas bases (COADGED ou PORTAL). Por favor, efetue o cadastramento deste Ato antes de proceder sua " + tipo + ".");
                    else if (importarSeNecessario)
                    {
                        try // importando...
                        {
                            int[] retorno = _service.ImportarMateriaTab31(revogado31.FirstOrDefault(), colecionadorId);
                            response.Add("revogado", retorno);
                        }
                        catch (Exception e)
                        {
                            response.message = Message.Success("ATENÇÃO! Ocorreu um erro durante a importação da matéria. Verifique se o Portal ATC está online e tente novamente.");
                            SessionUtil.HandleException(e);
                        }
                    }
                    else
                        response.message = Message.Success("ATENÇÃO! Esta matéria consta apenas no PORTAL e para sofrer (" + tipo + ") precisa ser importada.");

                    if (response.result.Count() == 0) // se não importou...
                        response.Add("revogado", revogado31.FirstOrDefault());
                }
            }
            else
            {
                response.Add("revogado", revogado.FirstOrDefault());
            }

            return Json(response);
        }

        // COADGED-BI...
        [AjaxExceptionFilter]
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult CoadGedBI(bool COADGED = true, string uf = null, string faseMateria = null, int?[] tpMateria = null, DateTime? dtCadastro = null, string coadgedBI = null, int? nrMateria = null, string anoAto = null, int? tpAto = null, string nrAto = null, int? nrInformativo = null, string anoInformativo = null, int? colecionadorId = null, int? colaboradorId = null, int? ggId = null, int? vbId = null, int? svbId = null, DateTime? dtAto = null, int? ativoId = null, int pagina = 1, int? carregarMais = null, int? apartirDe = null)
        {
            ActionResult retorno = null;
            try
            {
                JSONResponse response = new JSONResponse();

                Pagina<PublicacaoAreaConsultoriaDTO> page = new Pagina<PublicacaoAreaConsultoriaDTO>();

                if (carregarMais == null && apartirDe == null && COADGED)
                {
                    page = _service.PublicacoesAreaConsultoria(uf, faseMateria, dtCadastro, coadgedBI, tpMateria, nrMateria, anoAto, tpAto, nrAto, nrInformativo, anoInformativo, colecionadorId, colaboradorId, ggId, vbId, svbId, dtAto, ativoId: ativoId, pagina: pagina, itensPorPagina: 7);

                    if (page.lista.Count() > 0)
                    {
                        foreach (var p in page.lista)
                        {
                            foreach (var v in p.PUBLICACAO_REVISAO)
                            {
                                int i = 1;
                                foreach (var r in p.PUBLICACAO_REVISAO_COLABORADOR)
                                {
                                    v.REPROVADA = v.REPROVADA + i.ToString() + ". " + r.MOTIVO.ToString() + "; ";
                                    i++;
                                }
                            }

                            p.PUBLICAR_PORTAL = _service.PublicarPortalTexto(p);

                            // eliminando objetos pesados
                            p.PUBLICACAO.PUB_CONTEUDO = String.IsNullOrWhiteSpace(p.PUBLICACAO.PUB_CONTEUDO) ? "false" : "true"; // para controlar a liberação apenas quando houver matéria
                            p.PUBLICACAO.PUB_CONTEUDO_RDC = null;
                            p.PUBLICACAO.PUB_CONTEUDO_RESENHA = String.IsNullOrWhiteSpace(p.PUBLICACAO.PUB_CONTEUDO_RESENHA) ? "false" : "true"; // para controlar a liberação apenas quando houver matéria
                            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = null;
                            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_RDC = null;
                            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO = null;
                            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT = null;
                            p.PUBLICACAO.PUB_CONTEUDO_RVT = null;
                            p.PUBLICACAO.CABECALHO = null;
                        }
                    }
                    else
                    {
                        response.message = Message.Success("Não foram encontradas matérias com os parâmetros especificados!");
                    }

                    response.AddPage("coadgedBIretorno", page);
                }

                // Caso não tenha encontrado no COADGED... buscando no Portal...
                if ((page.lista == null || page.lista.Count() == 0) && !COADGED)
                {
                    string buscar = null;

                    if (!String.IsNullOrWhiteSpace(coadgedBI))
                        buscar = _service.RetornarPalavraPorPalavra(coadgedBI);

                    if (colecionadorId == 4)
                    {
                        if (buscar == null)
                        {
                            var expressao_ato = (tpAto != null) ? _serviceTpAto.FindById(tpAto).TIP_ATO_DESCRICAO : "";
                            var gg = (ggId != null) ? _serviceTitulacao.FindById(ggId).TIT_DESCRICAO : "";
                            var vb = (vbId != null) ? _serviceTitulacao.FindById(vbId).TIT_DESCRICAO : "";
                            var svb = (svbId != null) ? _serviceTitulacao.FindById(svbId).TIT_DESCRICAO : "";

                            var tab30 = _serviceTab30.Coad(null, nrMateria, expressao_ato, nrAto, dtAto, anoAto, anoInformativo, nrInformativo.ToString(), uf, gg, vb, svb, dtCadastro, buscar, carregarMais, apartirDe, pagina, 7);
                            if (tab30 == null || tab30.lista.Count() == 0)
                            {
                                response.message = Message.Success("Não foram encontradas matérias com os parâmetros especificados!");
                            }
                            response.AddPage("coadgedBIretorno", tab30);
                            response.Add("total", null);
                        }
                        else
                        {
                            var tab30 = _serviceTab30html.Coad(null, null, null, buscar, carregarMais, apartirDe, pagina, 3);
                            if (tab30 == null || tab30.lista.Count() == 0)
                            {
                                response.message = Message.Success("Não foram encontradas matérias com os parâmetros especificados!");
                            }
                            response.AddPage("coadgedBIretorno", tab30);
                            response.Add("total", _serviceTab30html.Totalizar(buscar, apartirDe));
                        }
                    }
                    else
                    {
                        if (buscar == null)
                        {
                            var expressao_ato = (tpAto != null) ? _serviceTpAto.FindById(tpAto).TIP_ATO_DESCRICAO : "";
                            var gg = (ggId != null) ? _serviceTitulacao.FindById(ggId).TIT_DESCRICAO : "";
                            var vb = (vbId != null) ? _serviceTitulacao.FindById(vbId).TIT_DESCRICAO : "";
                            var svb = (svbId != null) ? _serviceTitulacao.FindById(svbId).TIT_DESCRICAO : "";

                            var tab31 = _serviceTab31.Coad(null, nrMateria, expressao_ato, nrAto, dtAto, anoAto, anoInformativo, nrInformativo.ToString(), uf, gg, vb, svb, dtCadastro, colaboradorId, buscar, carregarMais, apartirDe, pagina: pagina, itensPorPagina: 7);
                            if (tab31 == null || tab31.lista.Count() == 0)
                            {
                                response.message = Message.Success("Não foram encontradas matérias com os parâmetros especificados!");
                            }
                            response.AddPage("coadgedBIretorno", tab31);
                            response.Add("total", null);
                        }
                        else
                        {
                            var tab31 = _serviceTab31html.Coad(null, null, null, buscar, carregarMais, apartirDe, pagina, 3);
                            if (tab31 == null || tab31.lista.Count() == 0)
                            {
                                response.message = Message.Success("Não foram encontradas matérias com os parâmetros especificados!");
                            }
                            response.AddPage("coadgedBIretorno", tab31);
                            response.Add("total", _serviceTab31html.Totalizar(buscar, apartirDe));
                        }
                    }
                }

                retorno = Json(response);
            }
            catch (Exception e)
            {
                SessionUtil.HandleException(e);
                throw new Exception(SysException.Show(e));
            }
            return retorno;
        }

        public ActionResult HtmlDaMateria(int modulo, int colecionadorId)
        {
            JSONResponse response = new JSONResponse();
            if (colecionadorId != 4)
            {
                var tab31 = _serviceTab31html.Coad(null, null, modulo).lista.FirstOrDefault();
                if (tab31 != null)
                    response.Add("html", tab31.html);
                else
                    response.Add("html", "Matéria não encontrada!");
            }
            else
            {
                var tab30 = _serviceTab30html.Coad(null, null, modulo).lista.FirstOrDefault();
                if (tab30 != null)
                    response.Add("html", tab30.html);
                else
                    response.Add("html", "Matéria não encontrada!");
            }
            return Json(response);
        }

        //
        public ActionResult atualizarMateriaImpressaJson(PublicacaoAreaConsultoriaDTO _pubArea)
        {
            JSONResponse response = new JSONResponse();
            response.Add("txt", _service.MateriaImpressaTexto(_pubArea));
            return Json(response);
        }

        //
        public ActionResult atualizarPublicarPortalJson(PublicacaoAreaConsultoriaDTO _pubArea)
        {
            JSONResponse response = new JSONResponse();
            response.Add("txt", _service.PublicarPortalTexto(_pubArea));
            return Json(response);
        }

        //

        public ActionResult carregarTexto(string campo, PublicacaoAreaConsultoriaSRV _service, PublicacaoAreaConsultoriaDTO _pubArea, PublicacaoDTO _pub)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                response.Add("txt", new TxtPublicacao().lerTexto(campo, _service, _pubArea, _pub));
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
            }
            return Json(response);
        }

        //

        public class TxtPublicacao
        {
            private static PublicacaoAreaConsultoriaSRV service;
            private static PublicacaoAreaConsultoriaDTO pubArea;
            private static PublicacaoDTO pub;

            public string lerTexto(string _campo = "", PublicacaoAreaConsultoriaSRV _service = null, PublicacaoAreaConsultoriaDTO _pubArea = null, PublicacaoDTO _pub = null)
            {
                var texto = "";
                try
                {
                    if (!String.IsNullOrWhiteSpace(_campo))
                    {
                        if (_service == null)
                        {
                            if (service == null)
                                _service = new PublicacaoAreaConsultoriaSRV();
                            else
                                _service = service;
                        }

                        if (_pubArea == null)
                        {
                            if (pubArea == null)
                                _pubArea = new PublicacaoAreaConsultoriaDTO();
                            else
                                _pubArea = pubArea;
                        }

                        if (_pub == null)
                        {
                            if (pub == null)
                                _pub = new PublicacaoDTO();
                            else
                                _pub = pub;
                        }

                        if (pubArea.PUBLICACAO.PUB_CONTEUDO == null && pubArea.PUBLICACAO.PUB_CONTEUDO_RESENHA == null)
                        {
                            _pubArea.PUBLICACAO = pub;
                            pubArea.PUBLICACAO = pub;
                        }

                        System.Reflection.PropertyInfo cp = null;

                        if (_campo == "MATERIA_IMPRESSA")
                        {
                            var txtComRemissao = _service.RemissaoMateriaImpressa(_pubArea).MATERIA_IMPRESSA.ToString();
                            var txtCabecalho = _service.CabecaMateriaImpressa(_pubArea).ToString();
                            texto = "Matéria: " + _pub.PUB_ID.ToString() + "<br>" + txtCabecalho + txtComRemissao;
                            cp = _pubArea.GetType().GetProperty(_campo);
                        }
                        else
                        if (_campo == "PUBLICAR_PORTAL")
                        {
                            var txtCabecalho = _service.CabecaMateriaImpressa(_pubArea, true).ToString();
                            texto = "Matéria: " + _pub.PUB_ID.ToString() + "<br>" + txtCabecalho + _pubArea.PUBLICACAO.PUB_CONTEUDO;
                            cp = _pubArea.GetType().GetProperty(_campo);
                        }
                        else
                        {
                            if (_pub.GetType().GetProperty(_campo) != null)
                            {
                                cp = _pub.GetType().GetProperty(_campo);
                                texto = cp.GetValue(_pub, null) as string;
                            }
                            else
                            if (_pubArea.GetType().GetProperty(_campo) != null)
                            {
                                cp = _pubArea.GetType().GetProperty(_campo);
                                texto = cp.GetValue(_pubArea, null) as string;
                            }
                        }
                    }

                    // guardando os objetos
                    pub = CloneHelper.Clone(_pub);
                    pubArea = CloneHelper.Clone(_pubArea);
                    service = CloneHelper.Clone(_service);
                }
                catch
                {
                    texto = "";
                }

                return texto;
            }
        }

        //

        private PublicacaoAreaConsultoriaDTO limparTxtDTO(PublicacaoAreaConsultoriaDTO p)
        {
            p.MATERIA_IMPRESSA = null;
            p.PUBLICAR_PORTAL = null;
            p.PUB_MANCHETE = null;
            p.PUB_MANCHETE_PORTAL = null;
            p.PUB_EMENTA = null;
            p.PUB_EMENTA_PORTAL = null;
            p.PUBLICACAO.CABECALHO = null;
            p.PUBLICACAO.PUB_CONTEUDO = null;
            p.PUBLICACAO.PUB_CONTEUDO_RDC = null;
            p.PUBLICACAO.PUB_CONTEUDO_RVT = null;
            p.PUBLICACAO.PUB_CONTEUDO_RVO = null;
            p.PUBLICACAO.PUB_CONTEUDO_RESENHA = null;
            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = null;
            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_RDC = null;
            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO = null;
            p.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT = null;

            return p;
        }

        // buscando matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult buscarMateria(int mat_id, PublicacaoAreaConsultoriaDTO pubAreaCons)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var publicacaoAreaConsultoria = _service.PublicacoesAreaConsultoria(null, null, null, null, null, mat_id).lista.FirstOrDefault();

                if (publicacaoAreaConsultoria != null)
                {
                    if (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null)
                    {
                        publicacaoAreaConsultoria.PUBLICAR_PORTAL = _service.PublicarPortalTexto(publicacaoAreaConsultoria);
                        publicacaoAreaConsultoria.MATERIA_IMPRESSA = _service.MateriaImpressaTexto(publicacaoAreaConsultoria);

                        // digitador logado? carregue o seu texto...
                        if (@ViewBag.cargoSigla == "DGT" || @ViewBag.cargoSigla == "RVO")
                        {
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT =
                                !String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT) ?
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT :
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA;
                        }

                        // revisor ortográfico logado? carregue o seu texto...
                        if (@ViewBag.cargoSigla == "RVO")
                        {
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO =
                                !String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO) ?
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO :
                                !String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT) ?
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT :
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT;
                        }
                    }

                    publicacaoAreaConsultoria.PUB_ID = pubAreaCons.PUB_ID;
                    publicacaoAreaConsultoria.ARE_CONS_ID = pubAreaCons.ARE_CONS_ID;

                    new TxtPublicacao().lerTexto("", _service, publicacaoAreaConsultoria, publicacaoAreaConsultoria.PUBLICACAO);
                    publicacaoAreaConsultoria = limparTxtDTO(publicacaoAreaConsultoria);
                }

                response.Add("pub", publicacaoAreaConsultoria);
            }
            catch (Exception ex)
            {
                response.Add("pub", null);
                response.success = false;
                response.message = Message.Fail(ex);
            }

            return Json(response);
        }

        //

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadpublicacaoAreaConsultoria(int publicacaoId, int colecionadorId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var publicacaoAreaConsultoria = _service.FindById(publicacaoId, colecionadorId);
                if (publicacaoAreaConsultoria.ARE_CONS_ID != null && publicacaoAreaConsultoria.PUB_ID != null)
                {
                    Tab_31DTO tab31 = _serviceTab31.Coad(publicacaoAreaConsultoria.PUB_ID).lista.FirstOrDefault();
                    publicacaoAreaConsultoria.PUB_ID_PORTAL = (tab31 != null) ? tab31.id : null;

                    if ((publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null) || (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO != null))
                    {
                        publicacaoAreaConsultoria.PUBLICAR_PORTAL = _service.PublicarPortalTexto(publicacaoAreaConsultoria);
                        publicacaoAreaConsultoria.MATERIA_IMPRESSA = _service.MateriaImpressaTexto(publicacaoAreaConsultoria);

                        // digitador logado? carregue o seu texto...
                        if (@ViewBag.cargoSigla == "DGT" || @ViewBag.cargoSigla == "RVO")
                        {
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT =
                                !String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT) ?
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT :
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA;
                        }

                        // revisor ortográfico logado? carregue o seu texto...
                        if (@ViewBag.cargoSigla == "RVO")
                        {
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO =
                                !String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO) ?
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO :
                                !String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT) ?
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT :
                                publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT;
                        }
                    }
                }

                new TxtPublicacao().lerTexto("", _service, publicacaoAreaConsultoria, publicacaoAreaConsultoria.PUBLICACAO);

                publicacaoAreaConsultoria = limparTxtDTO(publicacaoAreaConsultoria);

                response.Add("pub", publicacaoAreaConsultoria);
            }
            catch (Exception ex)
            {
                response.Add("pub", null);
                response.success = false;
                response.message = Message.Fail(ex);
            }
            return Json(response);
        }

        // liberando a matéria para a produção técnica...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult liberarRevisaoTecnica(int publicacaoId, int colecionadorId)
        {
            JSONResponse result = new JSONResponse();

            // salvando a liberação em PublicacaoRevisão...
            try
            {
                _servicePublicacaoRevisao.SalvarLiberacaoParaRevisaoTecnica(publicacaoId, colecionadorId);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }

        // salvando...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(PublicacaoAreaConsultoriaDTO publicacaoAreaConsultoria)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    // registrando o usuário atual
                    publicacaoAreaConsultoria.PUBLICACAO.USU_LOGIN = publicacaoAreaConsultoria.PUBLICACAO.USU_LOGIN != null ? publicacaoAreaConsultoria.PUBLICACAO.USU_LOGIN : SessionContext.autenticado.USU_LOGIN;

                    // salvando e pegando o ID da matéria no COADGED e no PORTAL...
                    int[] retorno = _service.SalvarPublicacaoAreaConsultoria(publicacaoAreaConsultoria);

                    result.Add("PUB_ID", retorno[0]);
                    result.Add("PUB_ID_PORTAL", retorno[1]);

                    // retornando com as mensagens ocorridas
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result);
            }
        }

        // lendo dados para o Painel...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Painel(int? colaboradorId = null, int? colecionadorId = null, int? nrInformativo = null, string anoInformativo = null, int? ativoId = null, int pagina = 1)
        {
            IQueryable<PublicacaoAreaConsultoriaDTO> page = _service.Painel(colaboradorId, colecionadorId, nrInformativo, anoInformativo, pagina: pagina, itensPorPagina: 999999);

            JSONResponse response = new JSONResponse();
            response.Add("painel", page);

            return Json(response);
        }

        // salvarEditor...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult salvarEditor(string extensao, string campo, int? pub_id = null, int? area_id = null)
        {
            try
            {
                if (pub_id != null && area_id != null)
                {
                    var pub = _servicePublicacao.FindById(pub_id);
                    var pubArea = _service.FindById(pub_id, area_id);
                    var texto = "";

                    System.Reflection.PropertyInfo cp = null;

                    if (campo == "MATERIA_IMPRESSA")
                    {
                        texto = _service.MateriaImpressaTexto(pubArea);
                        cp = pubArea.GetType().GetProperty(campo);
                    }
                    else
                    if (campo == "PUBLICAR_PORTAL")
                    {
                        texto = _service.PublicarPortalTexto(pubArea);
                        cp = pubArea.GetType().GetProperty(campo);
                    }
                    if (pub_id == 0 && area_id == 0) // sumário e diários \\
                    {
                        texto = campo;
                    }
                    else
                    {
                        if (pub.GetType().GetProperty(campo) != null)
                        {
                            cp = pub.GetType().GetProperty(campo);
                            texto = cp.GetValue(pub, null) as string;
                        }
                        else
                        if (pubArea.GetType().GetProperty(campo) != null)
                        {
                            cp = pubArea.GetType().GetProperty(campo);
                            texto = cp.GetValue(pubArea, null) as string;
                        }
                    }

                    if ((cp != null || pub_id == 0) && !String.IsNullOrWhiteSpace(texto))
                    {
                        var obj =
                        PrepararEditor("editor", delegate (Editor objEditor)
                        {
                            objEditor.LoadFormData("");
                            objEditor.Text = texto;
                        });

                        string nomeArquivo = Path.GetRandomFileName();
                        nomeArquivo = nomeArquivo.Substring(0, nomeArquivo.Length - 3) + extensao;

                        string arq = "~/richtexteditor/doc/" + nomeArquivo;

                        if (extensao == "PDF")
                            obj.SavePDF(arq);
                        if (extensao == "RTF")
                            obj.SaveRTF(arq);
                        if (extensao == "HTML")
                            obj.SaveFile(arq);

                        string arqDel = HostingEnvironment.ApplicationPhysicalPath + "\\richtexteditor\\doc\\" + nomeArquivo;

                        var bytes = System.IO.File.ReadAllBytes(arqDel);
                        System.IO.File.Delete(arqDel);

                        //if (extensao == "_PDF")
                        //    return File(bytes, "application/pdf", Path.GetFileName(nomeArquivo));
                        //else

                        return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(nomeArquivo));

                        //return File(arq, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(nomeArquivo));
                    }
                    else
                    {
                        JSONResponse response = new JSONResponse();
                        response.message = Message.Success("Não há informação a ser salva no campo [" + campo + "].");
                        response.Add("pub", null);
                        return Json(response);
                    }
                }
            }
            catch
            {
                JSONResponse response = new JSONResponse();
                response.message = Message.Success("Ocorreu um erro ao salvar a informação do campo [" + campo + "].");
                response.Add("pub", null);
                return Json(response);
            }
            return null;
        }

        // Sumario
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Sumario(int? inf = null, string ano = null, int? colecionadorId = null, string uf = null, string baseDados = "COADGED", Boolean imprimir = false)
        {
            if (inf != null && ano != null && colecionadorId != null)
            {
                string txt = "";

                // matéria...
                var obj =
                PrepararEditor("Publicar", delegate (Editor Publicar)
                {
                    Publicar.LoadFormData("");
                    Publicar.DisabledItems = "save";
                    Publicar.Width = Unit.Percentage(100);
                    Publicar.Font.Name = "Courier New";
                    Publicar.Text = (inf != null && ano != null && colecionadorId != null) ? _service.Sumario((int)inf, ano, (int)colecionadorId, uf, baseDados) : "";
                    txt = Publicar.Text;
                });

                ViewBag.Publicar = obj.MvcGetString();

                // download...
                if (imprimir)
                    return this.salvarEditor("RTF", txt, 0, 0);
            }

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // UF
            var ufv = _serviceUf.FindAll();
            ViewBag.uf = ufv.OrderBy(x => x.UF_ID).Where(u => u.UF_ID != "TD").Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });

            return View();
        }

        // Indice Orientacoes
        [Autorizar(IsAjax = true)]
        public ActionResult IndiceOrientacoes(int? inf = null, string ano = null, int? colecionadorId = null, string uf = null, string baseDados = "COADGED", Boolean imprimir = false)
        {
            if (ano != null && colecionadorId != null)
            {
                string txt = "";

                // matéria...
                var obj =
                PrepararEditor("Publicar", delegate (Editor Publicar)
                {
                    Publicar.LoadFormData("");
                    Publicar.DisabledItems = "save";
                    Publicar.Width = Unit.Percentage(100);
                    Publicar.Font.Name = "Courier New";
                    Publicar.Text = (ano != null && colecionadorId != null) ? _service.IndiceOrientacoesLembretes(ano, (int)colecionadorId, uf, baseDados) : "";
                    txt = Publicar.Text;
                });

                ViewBag.Publicar = obj.MvcGetString();

                // download...
                if (imprimir)
                    return this.salvarEditor("RTF", txt, 0, 0);
            }

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // UF
            var ufv = _serviceUf.FindAll();
            ViewBag.uf = ufv.OrderBy(x => x.UF_ID).Where(u => u.UF_ID != "TD").Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });

            return View();
        }

        // Indice Remissivo
        [Autorizar(IsAjax = true)]
        public ActionResult IndiceRemissivo(int? inf = null, string ano = null, int? colecionadorId = null, string uf = null, string baseDados = "COADGED", Boolean imprimir = false)
        {
            if (ano != null && colecionadorId != null)
            {
                string txt = "";

                // matéria...
                var obj =
                PrepararEditor("Publicar", delegate (Editor Publicar)
                {
                    Publicar.LoadFormData("");
                    Publicar.DisabledItems = "save";
                    Publicar.Width = Unit.Percentage(100);
                    Publicar.Font.Name = "Courier New";
                    Publicar.Text = (ano != null && colecionadorId != null) ? _service.IndiceAlfabeticoRemissivo(ano, (int)colecionadorId, uf, baseDados) : "";
                    txt = Publicar.Text;
                });

                ViewBag.Publicar = obj.MvcGetString();

                // download...
                if (imprimir)
                    return this.salvarEditor("RTF", txt, 0, 0);
            }

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // UF
            var ufv = _serviceUf.FindAll();
            ViewBag.uf = ufv.OrderBy(x => x.UF_ID).Where(u => u.UF_ID != "TD").Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });

            return View();
        }

        // Indice Atos
        [Autorizar(IsAjax = true)]
        public ActionResult IndiceAtos(int? inf = null, string ano = null, int? colecionadorId = null, string uf = null, string baseDados = "COADGED", Boolean imprimir = false)
        {
            if (ano != null && colecionadorId != null)
            {
                string txt = "";

                // matéria...
                var obj =
                PrepararEditor("Publicar", delegate (Editor Publicar)
                {
                    Publicar.LoadFormData("");
                    Publicar.DisabledItems = "save";
                    Publicar.Width = Unit.Percentage(100);
                    Publicar.Font.Name = "Courier New";
                    Publicar.Text = (ano != null && colecionadorId != null) ? _service.IndiceNumericoAtos(ano, (int)colecionadorId, uf, baseDados) : "";
                    txt = Publicar.Text;
                });

                ViewBag.Publicar = obj.MvcGetString();

                // download...
                if (imprimir)
                    return this.salvarEditor("RTF", txt, 0, 0);
            }

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // UF
            var ufv = _serviceUf.FindAll();
            ViewBag.uf = ufv.OrderBy(x => x.UF_ID).Where(u => u.UF_ID != "TD").Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });

            return View();
        }

        // Indice Revogacoes
        [Autorizar(IsAjax = true)]
        public ActionResult IndiceRevogacoes(int? inf = null, string ano = null, int? colecionadorId = null, string uf = null, string baseDados = "COADGED", Boolean imprimir = false)
        {
            if (ano != null && colecionadorId != null)
            {
                string txt = "";

                // matéria...
                var obj =
                PrepararEditor("Publicar", delegate (Editor Publicar)
                {
                    Publicar.LoadFormData("");
                    Publicar.DisabledItems = "save";
                    Publicar.Width = Unit.Percentage(100);
                    Publicar.Font.Name = "Courier New";
                    Publicar.Text = (ano != null && colecionadorId != null) ? _service.IndiceNumericoAlteracoesRevogacoes(ano, (int)colecionadorId, uf, baseDados) : "";
                    txt = Publicar.Text;
                });

                ViewBag.Publicar = obj.MvcGetString();

                // download...
                if (imprimir)
                    return this.salvarEditor("RTF", txt, 0, 0);
            }

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // UF
            var ufv = _serviceUf.FindAll();
            ViewBag.uf = ufv.OrderBy(x => x.UF_ID).Where(u => u.UF_ID != "TD").Select(c => new SelectListItem() { Text = c.UF_NOME, Value = c.UF_ID.ToString() });

            return View();
        }

        // importando EXCEL...
        public ActionResult ImportaExcel()
        {
            _service.ImportouExcel(); // importando titulações ATC
            _service.ImportouExcel("C:\\COADGED\\Importacao\\Andrea limpou\\titulacoes_tab30.xlsx"); // DP
            return null;
        }
    }

}