using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.COADGED.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Repositorios.Base;
using System.Globalization;
using RTE;
using System.Web.UI.WebControls;
using GenericCrud.Service;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoRevisaoController : Controller
    {
        // preparando objetos...
        public AreasSRV _serviceAreas {get; set;}
        public PublicacaoRevisaoSRV _service { get; set; }
        public PublicacaoAreaConsultoriaSRV _servicePublicacaoAreaConsultoria { get; set; }
        public PublicacaoRevisaoColaboradorSRV _servicePublicacaoRevisaoColaborador { get; set; }
        public PublicacaoSRV _servicePublicacao { get; set; }
        public PublicacaoRemissaoSRV _servicePublicacaoRemissao { get; set; }
        public PublicacaoTitulacaoSRV _servicePublicacaoTitulacao { get; set; }
        public PublicacaoUfSRV _servicePublicacaoUf { get; set; }

        // dados cadastrais...
        private ColaboradorSRV _serviceColaborador = new ColaboradorSRV();

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
            PrepararEditor(obj, delegate(Editor editor) { });
            return new EmptyResult();
        }

        //
        public ActionResult salvarPublicacao(PublicacaoDTO _pub, PublicacaoAreaConsultoriaDTO publicacaoAreaConsultoria)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ClassMetadata c = new ClassMetadata();

                var pub = _servicePublicacao.FindById(_pub.PUB_ID);

                if (!c.CompararAtributosDeObjetos(_pub, pub)) // se houve alteração, salve histórico
                    _service.SalvarAlteracaoDaMateria(publicacaoAreaConsultoria);
                
                _servicePublicacao.SalvarPublicacao(_pub);

                result.success = true;
                result.message = Message.Success("Matéria salva com sucesso!");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }
            return Json(result);
        }

        //
        public ActionResult buscarTextoDigitador(int pub_id)
        {
            JSONResponse response = new JSONResponse();
            response.Add("revisao", _servicePublicacao.FindById(pub_id).PUB_CONTEUDO_RESENHA_DGT);
            return Json(response);
        }

        //
        public ActionResult buscarTextoDiagramador(int pub_id)
        {
            JSONResponse response = new JSONResponse();
            response.Add("revisao", _servicePublicacao.FindById(pub_id).PUB_CONTEUDO_RESENHA_RVO);
            return Json(response);
        }

        //
        public void InicializaViewBag()
        {
            if (SessionContext.per_id == "REDACAO")
            {
                ViewBag.cargo = "Redator";
                ViewBag.cargoSigla = "RDC";
            }
            else if (SessionContext.per_id == "REVISAO_TEC")
            {
                ViewBag.cargo = "Revisor Técnico";
                ViewBag.cargoSigla = "RVT";
            }
            else if (SessionContext.per_id == "DIGITACAO")
            {
                var obj =
                PrepararEditor("EditorRapido", delegate(Editor EditorRapido)
                {
                    EditorRapido.LoadFormData("");
                    EditorRapido.DisabledItems = "save";
                    EditorRapido.Width = Unit.Percentage(100);
                    EditorRapido.SetConfig("fontnamelist", "Helvetica,Arial Narrow");
                    EditorRapido.Font.Name = "Helvetica";
                    EditorRapido.SetConfig("readonly", false);
                    EditorRapido.AutoFocus = true;
                });
                ViewBag.PUB_CONTEUDO_RESENHA_DGT = obj.MvcGetString();

                ViewBag.cargo = "Digitador";
                ViewBag.cargoSigla = "DGT";
            }
            else if (SessionContext.per_id == "REVISAO_ORT")
            {
                ViewBag.cargo = "Revisor Ortográfico";
                ViewBag.cargoSigla = "RVO";
            }
            else if (SessionContext.per_id == "DIAGRAMACAO_IMP" || SessionContext.per_id == "DIAGRAMACAO_WEB")
            {
                var obj =
                PrepararEditor("EditorRapido", delegate(Editor EditorRapido)
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

                ViewBag.cargo = "Diagramador";
                ViewBag.cargoSigla = "DIA";
            }

            ViewBag.colaborador = SessionContext.autenticado.USU_LOGIN;
            ViewBag.colecionadorId = _serviceColaborador.BuscarColecionadorDoColaborador(ViewBag.colaborador);
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.OrderBy(x => x.ARE_CONS_DESCRICAO).Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString(), Selected = (c.ARE_CONS_ID == ViewBag.colecionadorId) });
            var area = _serviceAreas.FindById(ViewBag.colecionadorId);
            ViewBag.colecionadorNome = area == null ? "" : area.ARE_CONS_DESCRICAO;
        }

        // REVISÃO TÉCNICA ----------------------------------------------------------------------------------------------------------------------//
        // tela principal...
        [Autorizar(PorMenu = false)]
        public ActionResult AprovacaoRevisaoTecnica()
        {
            this.InicializaViewBag();
            return View();
        }

        // serviço que lê as matérias...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult RevisaoTecnica(int? colecionadorId = null, int? informativo = null, string anoInformativo = null, int pagina = 1)
        {
            Pagina<PublicacaoRevisaoDTO> page = _service.PublicacaoRevisao(null, colecionadorId, "L", null, null, informativo, anoInformativo, pagina, 3);

            foreach (var p in page.lista)
            {
                int i = 1;
                foreach (var r in _servicePublicacaoRevisaoColaborador.PublicacaoRevisaoColaborador(p.PUB_ID, p.ARE_CONS_ID, null, null, null, null, 1, 99999).lista)
                {
                    p.REPROVADA = p.REPROVADA + i.ToString() + ". " + r.MOTIVO.ToString() + "; ";
                    i++;
                }
                p.PUBLICACAO_AREAS_CONSULTORIA = _servicePublicacaoAreaConsultoria.FindById(p.PUB_ID, p.ARE_CONS_ID);
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_CONFIG = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_PALAVRA_CHAVE = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_UF = (ICollection<PublicacaoUfDTO>)_servicePublicacaoUf.PublicacaoUf(p.PUB_ID, p.ARE_CONS_ID).lista;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSIVO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_TITULACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO_COLABORADOR = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.MATERIA_IMPRESSA = _servicePublicacaoAreaConsultoria.MateriaImpressaTexto(p.PUBLICACAO_AREAS_CONSULTORIA);
            }

            JSONResponse response = new JSONResponse();
            response.AddPage("revisao", page);

            return Json(response);
        }

        // aprovando a matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult AprovarRevisaoTecnica(int revId)
        {
            JSONResponse result = new JSONResponse();

            // salvando aprovação...
            try
            {
                _service.SalvarAprovacaoReprovacaoDaRevisaoTecnica(revId, "A");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }

        // reprovando a matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReprovarRevisaoTecnica(int revId, string motivo)
        {
            JSONResponse result = new JSONResponse();

            // salvando aprovação...
            try
            {
                _service.SalvarAprovacaoReprovacaoDaRevisaoTecnica(revId, "R", motivo);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        // DIGITAÇÃO ------------------------------------------------------------------------------------------------------------------------------//
        // tela principal...
        [Autorizar(PorMenu = false)]
        public ActionResult AprovacaoDigitacao()
        {
            this.InicializaViewBag();
            return View();
        }

        // serviço que lê as matérias...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Digitacao(int? colecionadorId = null, int? informativo = null, string anoInformativo = null, int pagina = 1)
        {
            Pagina<PublicacaoRevisaoDTO> page = _service.PublicacaoRevisao(null, colecionadorId, null, "L", null, informativo, anoInformativo, pagina, 3);

            foreach (var p in page.lista)
            {
                int i = 1;
                foreach (var r in _servicePublicacaoRevisaoColaborador.PublicacaoRevisaoColaborador(p.PUB_ID, p.ARE_CONS_ID, null, null, null, null, 1, 99999).lista)
                {
                    p.REPROVADA = p.REPROVADA + i.ToString() + ". " + r.MOTIVO.ToString() + "; ";
                    i++;
                }
                p.PUBLICACAO_AREAS_CONSULTORIA = _servicePublicacaoAreaConsultoria.FindById(p.PUB_ID, p.ARE_CONS_ID);
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_CONFIG = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_PALAVRA_CHAVE = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_UF = (ICollection<PublicacaoUfDTO>)_servicePublicacaoUf.PublicacaoUf(p.PUB_ID, p.ARE_CONS_ID).lista;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSIVO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_TITULACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO_COLABORADOR = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.MATERIA_IMPRESSA = _servicePublicacaoAreaConsultoria.MateriaImpressaTexto(p.PUBLICACAO_AREAS_CONSULTORIA);
            }

            JSONResponse response = new JSONResponse();
            response.AddPage("revisao", page);

            return Json(response);
        }

        // aprovando a matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult AprovarDigitacao(int revId)
        {
            JSONResponse result = new JSONResponse();

            // salvando aprovação...
            try
            {
                _service.SalvarAprovacaoReprovacaoDaDigitacao(revId, "A");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }

        // reprovando a matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReprovarDigitacao(int revId, string motivo)
        {
            JSONResponse result = new JSONResponse();

            // salvando aprovação...
            try
            {
                _service.SalvarAprovacaoReprovacaoDaDigitacao(revId, "R", motivo);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // REVISÃO ORTOGRÁFICA --------------------------------------------------------------------------------------------------------------------//
        // tela principal...
        [Autorizar(PorMenu = false)]
        public ActionResult AprovacaoRevisaoOrtografica()
        {
            this.InicializaViewBag();
            return View();
        }

        // serviço que lê as matérias...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult RevisaoOrtografica(int? colecionadorId = null, int? informativo = null, string anoInformativo = null, int pagina = 1)
        {
            Pagina<PublicacaoRevisaoDTO> page = _service.PublicacaoRevisao(null, colecionadorId, null, null, "L", informativo, anoInformativo, pagina, 3);

            foreach (var p in page.lista)
            {
                int i = 1;
                foreach (var r in _servicePublicacaoRevisaoColaborador.PublicacaoRevisaoColaborador(p.PUB_ID, p.ARE_CONS_ID, null, null, null, null, 1, 99999).lista)
                {
                    p.REPROVADA = p.REPROVADA + i.ToString() + ". " + r.MOTIVO.ToString() + "; ";
                    i++;
                }
                p.PUBLICACAO_AREAS_CONSULTORIA = _servicePublicacaoAreaConsultoria.FindById(p.PUB_ID, p.ARE_CONS_ID);
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_CONFIG = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_PALAVRA_CHAVE = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_UF = (ICollection<PublicacaoUfDTO>)_servicePublicacaoUf.PublicacaoUf(p.PUB_ID, p.ARE_CONS_ID).lista;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSIVO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_TITULACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO_COLABORADOR = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.MATERIA_IMPRESSA = _servicePublicacaoAreaConsultoria.MateriaImpressaTexto(p.PUBLICACAO_AREAS_CONSULTORIA);
            }

            JSONResponse response = new JSONResponse();
            response.AddPage("revisao", page);

            return Json(response);
        }

        // aprovando a matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult AprovarRevisaoOrtografica(int revId)
        {
            JSONResponse result = new JSONResponse();

            // salvando aprovação...
            try
            {
                _service.SalvarAprovacaoReprovacaoDaRevisaoOrtografica(revId, "A");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }

        // reprovando a matéria...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReprovarRevisaoOrtografica(int revId, string motivo)
        {
            JSONResponse result = new JSONResponse();

            // salvando aprovação...
            try
            {
                _service.SalvarAprovacaoReprovacaoDaRevisaoOrtografica(revId, "R", motivo);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
            }

            // retorne o erro...
            return Json(result);
        }

        // Diagramação - tela \\
        [Autorizar(PorMenu = false)]
        public ActionResult DiagramacaoTela()
        {
            System.Globalization.CultureInfo calendarioLocal = new CultureInfo("PT-BR");
            var semanaAtual = calendarioLocal.Calendar.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Sunday);

            this.InicializaViewBag();
            return View();
        }

        // serviço que lê as matérias da Diagramação...
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Diagramacao(int? colecionadorId = null, int? informativo = null, string anoInformativo = null, int pagina = 1)
        {
            Pagina<PublicacaoRevisaoDTO> page = _service.PublicacaoRevisao(null, colecionadorId, null, null, "A", informativo, anoInformativo, pagina, 3);

            foreach (var p in page.lista)
            {
                int i = 1;
                foreach (var r in _servicePublicacaoRevisaoColaborador.PublicacaoRevisaoColaborador(p.PUB_ID, p.ARE_CONS_ID, null, null, null, null, 1, 99999).lista)
                {
                    p.REPROVADA = p.REPROVADA + i.ToString() + ". " + r.MOTIVO.ToString() + "; ";
                    i++;
                }
                p.PUBLICACAO_AREAS_CONSULTORIA = _servicePublicacaoAreaConsultoria.FindById(p.PUB_ID, p.ARE_CONS_ID);
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_CONFIG = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_PALAVRA_CHAVE = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_UF = (ICollection<PublicacaoUfDTO>)_servicePublicacaoUf.PublicacaoUf(p.PUB_ID, p.ARE_CONS_ID).lista;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REMISSIVO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_TITULACAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.PUBLICACAO_REVISAO_COLABORADOR = null;
                p.PUBLICACAO_AREAS_CONSULTORIA.MATERIA_IMPRESSA = _servicePublicacaoAreaConsultoria.MateriaImpressaTexto(p.PUBLICACAO_AREAS_CONSULTORIA);
            }

            JSONResponse response = new JSONResponse();
            response.AddPage("revisao", page);

            return Json(response);
        }
    }
}