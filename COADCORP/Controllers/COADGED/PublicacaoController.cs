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

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    [ValidateInput(false)]
    public class PublicacaoController : Controller
    {
        private PublicacaoSRV _service = new PublicacaoSRV();

        private InformativoSRV _serviceInformativo = new InformativoSRV();
        private UfSRV _serviceUf = new UfSRV();
        private ColaboradorSRV _serviceColaborador = new ColaboradorSRV();
        private TitulacaoSRV _serviceTitulacao = new TitulacaoSRV();
        private TipoMateriaSRV _serviceTpMateria = new TipoMateriaSRV();
        private TipoAtoSRV _serviceTpAto = new TipoAtoSRV();
        private VeiculoSRV _serviceVeiculo = new VeiculoSRV();
        private OrgaoSRV _serviceOrgao = new OrgaoSRV();
        private SecoesSRV _serviceSecao = new SecoesSRV();
        private LabelsSRV _serviceLabel = new LabelsSRV();
        //private GrupoConsultoriaSRV _serviceGrpConsultoria = new GrupoConsultoriaSRV();
        private AreasSRV _serviceAreas = new AreasSRV();


        // ALT: 21/07/2015 - instanciando o Editor de Textos
        private Editor Manchete = new Editor(System.Web.HttpContext.Current, "Manchete");
        private Editor Ementa = new Editor(System.Web.HttpContext.Current, "Ementa");
        private Editor Integra = new Editor(System.Web.HttpContext.Current, "Integra");
        private Editor Resenha = new Editor(System.Web.HttpContext.Current, "Resenha");

        [Autorizar]
        public ActionResult Index()
        {
            // redator
            ViewBag.redator = SessionContext.autenticado.USU_LOGIN;

            // ativo=1 ou inativo=0
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // Publicação/Ato revogado ou alterado
            var atoRevogado = _service.FindAll();
            ViewBag.atoRevogado = atoRevogado.Select(c => new SelectListItem() { Text = c.TIPO_ATO.TIP_ATO_DESCRICAO+" [Nº "+c.PUB_NUMERO_ATO+"] Data: "+c.PUB_DATA_ATO.ToString(), Value = c.PUB_ID.ToString() });

            // UF
            var uf = _serviceUf.FindAll();
            ViewBag.uf = uf.Select(c => new SelectListItem() { Text = c.UF_ID, Value = c.UF_ID.ToString() });

            // revisor
            var revisor = _serviceColaborador.FindAll();
            ViewBag.revisor = revisor.Where(s => s.CRG_ID == 2).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });

            // gg
            var titulacao = _serviceTitulacao.FindAll();
            ViewBag.gg = titulacao.Where(tit => tit.TIT_TIPO == "G").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() }); // grande grupo

            // tipo de matéria
            var tpMateria = _serviceTpMateria.FindAll();
            ViewBag.tpMateria = tpMateria.Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });

            // tipo de ato
            var tpAto = _serviceTpAto.FindAll();
            ViewBag.tpAto = tpAto.Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });

            // veículo
            var veiculo = _serviceVeiculo.FindAll();
            ViewBag.veiculo = veiculo.Select(c => new SelectListItem() { Text = c.TVI_DESCRICAO, Value = c.TVI_ID.ToString() });

            // órgão
            var orgao = _serviceOrgao.FindAll();
            ViewBag.orgao = orgao.Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() });

            // seção
            var secao = _serviceSecao.FindAll();
            ViewBag.secao = secao.Select(c => new SelectListItem() { Text = c.SEC_DESCRICAO, Value = c.SEC_ID.ToString() });

            // label
            var label = _serviceLabel.FindAll();
            ViewBag.label = label.Select(c => new SelectListItem() { Text = c.LBL_DESCRICAO, Value = c.LBL_ID.ToString() });

            // grupo da consultoria
            //var grpConsultoria = _serviceGrpConsultoria.FindAll();
            //ViewBag.grpConsultoria = grpConsultoria.Select(c => new SelectListItem() { Text = c.GRD_CONS_DESCRICAO, Value = c.GRD_CONS_ID.ToString() });

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Verbetes(int ggId)
        {
            // verbetes
            var verbetes = _serviceTitulacao.Verbetes(ggId).lista;
            //ViewBag.vb = verbetes.Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() });

            JSONResponse response = new JSONResponse();
            response.Add("verbetes", verbetes);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Subverbetes(int vbId)
        {
            // subverbetes
            var subverbetes = _serviceTitulacao.Subverbetes(vbId).lista;
            //ViewBag.svb = subverbetes.Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() });

            JSONResponse response = new JSONResponse();
            response.Add("subverbetes", subverbetes);

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Publicacoes(int? tpAto = null, string nrAto = null, DateTime? dtAto = null, int pagina = 1, int itensPorPagina = 10)
        {
            Pagina<PublicacaoDTO> page = _service.Publicacoes(tpAto, nrAto, dtAto, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("publicacoes", page);

            return Json(response);
        }

        [ValidateInput(false)]
        [Autorizar]
        public ActionResult Novo()
        {
            // redator
            ViewBag.redator = SessionContext.autenticado.USU_LOGIN;

            // ativo=1 ou inativo=0
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // Publicação/Ato revogado ou alterado
            var atoRevogado = _service.FindAll();
            ViewBag.atoRevogado = atoRevogado.Select(c => new SelectListItem() { Text = c.TIPO_ATO.TIP_ATO_DESCRICAO + " [Nº " + c.PUB_NUMERO_ATO + "] Data: " + c.PUB_DATA_ATO.ToString().Substring(1,10), Value = c.PUB_ID.ToString() });

            // UF
            var uf = _serviceUf.FindAll();
            ViewBag.uf = uf.Select(c => new SelectListItem() { Text = c.UF_ID, Value = c.UF_ID.ToString() });

            // revisor
            var revisor = _serviceColaborador.FindAll();
            ViewBag.revisor = revisor.Where(s => s.CRG_ID == 2).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });

            // gg
            var titulacao = _serviceTitulacao.FindAll();
            ViewBag.gg = titulacao.Where(tit => tit.TIT_TIPO == "G").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() }); // grande grupo

            // tipo de matéria
            var tpMateria = _serviceTpMateria.FindAll();
            ViewBag.tpMateria = tpMateria.Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });

            // tipo de ato
            var tpAto = _serviceTpAto.FindAll();
            ViewBag.tpAto = tpAto.Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });

            // veículo
            var veiculo = _serviceVeiculo.FindAll();
            ViewBag.veiculo = veiculo.Select(c => new SelectListItem() { Text = c.TVI_DESCRICAO, Value = c.TVI_ID.ToString() });

            // órgão
            var orgao = _serviceOrgao.FindAll();
            ViewBag.orgao = orgao.Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() });

            // seção
            var secao = _serviceSecao.FindAll();
            ViewBag.secao = secao.Select(c => new SelectListItem() { Text = c.SEC_DESCRICAO, Value = c.SEC_ID.ToString() });

            // label
            var label = _serviceLabel.FindAll();
            ViewBag.label = label.Select(c => new SelectListItem() { Text = c.LBL_DESCRICAO, Value = c.LBL_ID.ToString() });

            // grupo da consultoria
            //var grpConsultoria = _serviceGrpConsultoria.FindAll();
            //ViewBag.grpConsultoria = grpConsultoria.Select(c => new SelectListItem() { Text = c.GRD_CONS_DESCRICAO, Value = c.GRD_CONS_ID.ToString() });

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // informativos em produção
            Pagina<InformativoDTO> informativosEmProducao;
            try
            {
                informativosEmProducao = _serviceInformativo.LerInformativosEmProducao();
                ViewBag.informativo = informativosEmProducao.lista.Select(c => new SelectListItem() { Text = c.INF_ANO + "/" + c.INF_NUMERO.ToString(), Value = c.INF_ANO + "/" + c.INF_NUMERO.ToString() });
            }
            catch 
            {
                List<SelectListItem> informativo = new List<SelectListItem>();
                informativo.AddRange(new[]{ new SelectListItem() { Text = "Nenhum", Value = "" } });
                ViewBag.informativo = new SelectList(informativo, "Value", "Text");
            }

            // ementa
            Ementa.LoadFormData("");
            Ementa.MvcInit();
            Ementa.DisabledItems = "save";
            ViewBag.PUB_EMENTA = Ementa.MvcGetString();

            // manchete
            Manchete.LoadFormData("");
            Manchete.MvcInit();
            Manchete.DisabledItems = "save";
            ViewBag.PUB_MANCHETE = Manchete.MvcGetString();

            // integra
            Integra.LoadFormData("");
            Integra.MvcInit();
            Integra.DisabledItems = "save";
            ViewBag.PUB_CONTEUDO = Integra.MvcGetString();

            // resenha
            Resenha.LoadFormData("");
            Resenha.MvcInit();
            Resenha.DisabledItems = "save";
            ViewBag.PUB_CONTEUDO_RESENHA = Resenha.MvcGetString();

            return View("Edit");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int publicacaoId)
        {
            // redator
            ViewBag.redator = SessionContext.autenticado.USU_LOGIN;

            // ativo=1 ou inativo=0
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // Publicação/Ato revogado ou alterado
            var atoRevogado = _service.FindAll();
            ViewBag.atoRevogado = atoRevogado.Where(c => c.PUB_ID != publicacaoId).Select(c => new SelectListItem() { Text = c.TIPO_ATO.TIP_ATO_DESCRICAO + " [Nº " + c.PUB_NUMERO_ATO + "] Data: " + c.PUB_DATA_ATO.ToString().Substring(1, 10), Value = c.PUB_ID.ToString() });

            // UF
            var uf = _serviceUf.FindAll();
            ViewBag.uf = uf.Select(c => new SelectListItem() { Text = c.UF_ID, Value = c.UF_ID.ToString() });

            // UFs selecionadas
            //var ufsSelecionadas = _serviceUfsSelecionadas
            //ViewBag.ufsSelecionadas = uf.Select(c => new SelectListItem() { Text = c.UF_ID, Value = c.UF_ID.ToString() });

            // revisor
            var revisor = _serviceColaborador.FindAll();
            ViewBag.revisor = revisor.Where(s => s.CRG_ID == 2).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });

            // gg
            var titulacao = _serviceTitulacao.FindAll();
            ViewBag.gg = titulacao.Where(tit => tit.TIT_TIPO == "G").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() }); // grande grupo

            // tipo de matéria
            var tpMateria = _serviceTpMateria.FindAll();
            ViewBag.tpMateria = tpMateria.Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });

            // tipo de ato
            var tpAto = _serviceTpAto.FindAll();
            ViewBag.tpAto = tpAto.Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });

            // veículo
            var veiculo = _serviceVeiculo.FindAll();
            ViewBag.veiculo = veiculo.Select(c => new SelectListItem() { Text = c.TVI_DESCRICAO, Value = c.TVI_ID.ToString() });

            // órgão
            var orgao = _serviceOrgao.FindAll();
            ViewBag.orgao = orgao.Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() });

            // seção
            var secao = _serviceSecao.FindAll();
            ViewBag.secao = secao.Select(c => new SelectListItem() { Text = c.SEC_DESCRICAO, Value = c.SEC_ID.ToString() });

            // label
            var label = _serviceLabel.FindAll();
            ViewBag.label = label.Select(c => new SelectListItem() { Text = c.LBL_DESCRICAO, Value = c.LBL_ID.ToString() });

            // grupo da consultoria
            //var grpConsultoria = _serviceGrpConsultoria.FindAll();
            //ViewBag.grpConsultoria = grpConsultoria.Select(c => new SelectListItem() { Text = c.GRD_CONS_DESCRICAO, Value = c.GRD_CONS_ID.ToString() });

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // informativos em produção
            Pagina<InformativoDTO> informativosEmProducao;
            try
            {
                informativosEmProducao = _serviceInformativo.LerInformativosEmProducao();
                ViewBag.informativo = informativosEmProducao.lista.Select(c => new SelectListItem() { Text = c.INF_ANO + "/" + c.INF_NUMERO.ToString(), Value = c.INF_ANO + "/" + c.INF_NUMERO.ToString() });
            }
            catch
            {
                List<SelectListItem> informativo = new List<SelectListItem>();
                informativo.AddRange(new[] { new SelectListItem() { Text = "Nenhum", Value = "" } });
                ViewBag.informativo = new SelectList(informativo, "Value", "Text");
            }

            // ementa
            Ementa.LoadFormData("");
            Ementa.MvcInit();
            Ementa.DisabledItems = "save";
            ViewBag.PUB_EMENTA = Ementa.MvcGetString();

            // manchete
            Manchete.LoadFormData("");
            Manchete.MvcInit();
            Manchete.DisabledItems = "save";
            ViewBag.PUB_MANCHETE = Manchete.MvcGetString();

            // integra
            Integra.LoadFormData("");
            Integra.MvcInit();
            Integra.DisabledItems = "save";
            ViewBag.PUB_CONTEUDO = Integra.MvcGetString();

            // resenha
            Resenha.LoadFormData("");
            Resenha.MvcInit();
            Resenha.DisabledItems = "save";
            ViewBag.PUB_CONTEUDO_RESENHA = Resenha.MvcGetString();

            // id da publicação a editar
            ViewBag.publicacaoId = publicacaoId;

            return View("Edit");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(PublicacaoDTO publicacao)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    _service.SalvarPublicacao(publicacao);
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

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readpublicacao(int publicacaoId)
        {
            var publicacao = _service.FindById(publicacaoId);
            
            JSONResponse response = new JSONResponse();
            response.Add("publicacao", publicacao);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int publicacaoId)
        {
            // redator
            ViewBag.redator = SessionContext.autenticado.USU_LOGIN;

            // ativo=1 ou inativo=0
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // Publicação/Ato revogado ou alterado
            var atoRevogado = _service.FindAll();
            ViewBag.atoRevogado = atoRevogado.Select(c => new SelectListItem() { Text = c.TIPO_ATO.TIP_ATO_DESCRICAO + " [Nº " + c.PUB_NUMERO_ATO + "] Data: " + c.PUB_DATA_ATO.ToString(), Value = c.PUB_ID.ToString() });

            // UF
            var uf = _serviceUf.FindAll();
            ViewBag.uf = uf.Select(c => new SelectListItem() { Text = c.UF_ID, Value = c.UF_ID.ToString() });

            // revisor
            var revisor = _serviceColaborador.FindAll();
            ViewBag.revisor = revisor.Where(s => s.CRG_ID == 2).Select(c => new SelectListItem() { Text = c.COL_NOME, Value = c.COL_ID.ToString() });

            // gg
            var titulacao = _serviceTitulacao.FindAll();
            ViewBag.gg = titulacao.Where(tit => tit.TIT_TIPO == "G").Select(c => new SelectListItem() { Text = c.TIT_DESCRICAO, Value = c.TIT_ID.ToString() }); // grande grupo

            // tipo de matéria
            var tpMateria = _serviceTpMateria.FindAll();
            ViewBag.tpMateria = tpMateria.Select(c => new SelectListItem() { Text = c.TIP_MAT_DESCRICAO, Value = c.TIP_MAT_ID.ToString() });

            // tipo de ato
            var tpAto = _serviceTpAto.FindAll();
            ViewBag.tpAto = tpAto.Select(c => new SelectListItem() { Text = c.TIP_ATO_DESCRICAO, Value = c.TIP_ATO_ID.ToString() });

            // veículo
            var veiculo = _serviceVeiculo.FindAll();
            ViewBag.veiculo = veiculo.Select(c => new SelectListItem() { Text = c.TVI_DESCRICAO, Value = c.TVI_ID.ToString() });

            // órgão
            var orgao = _serviceOrgao.FindAll();
            ViewBag.orgao = orgao.Select(c => new SelectListItem() { Text = c.ORG_DESCRICAO, Value = c.ORG_ID.ToString() });

            // seção
            var secao = _serviceSecao.FindAll();
            ViewBag.secao = secao.Select(c => new SelectListItem() { Text = c.SEC_DESCRICAO, Value = c.SEC_ID.ToString() });

            // label
            var label = _serviceLabel.FindAll();
            ViewBag.label = label.Select(c => new SelectListItem() { Text = c.LBL_DESCRICAO, Value = c.LBL_ID.ToString() });

            // grupo da consultoria
            //var grpConsultoria = _serviceGrpConsultoria.FindAll();
            //ViewBag.grpConsultoria = grpConsultoria.Select(c => new SelectListItem() { Text = c.GRD_CONS_DESCRICAO, Value = c.GRD_CONS_ID.ToString() });

            // areas da consultoria
            var areas = _serviceAreas.FindAll();
            ViewBag.areas = areas.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            // id da publicação
            ViewBag.publicacaoId = publicacaoId;

            // informativos em produção
            Pagina<InformativoDTO> informativosEmProducao;
            try
            {
                informativosEmProducao = _serviceInformativo.LerInformativosEmProducao();
                ViewBag.informativo = informativosEmProducao.lista.Select(c => new SelectListItem() { Text = c.INF_ANO + "/" + c.INF_NUMERO.ToString(), Value = c.INF_ANO + "/" + c.INF_NUMERO.ToString() });
            }
            catch
            {
                List<SelectListItem> informativo = new List<SelectListItem>();
                informativo.AddRange(new[] { new SelectListItem() { Text = "Nenhum", Value = "" } });
                ViewBag.informativo = new SelectList(informativo, "Value", "Text");
            }

            return View("Detalhes");
        }
    }
}