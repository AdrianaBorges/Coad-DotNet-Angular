using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Areas.franquia.Controllers
{
    public class SuspectController : Controller
    {
        //private const int uenId = 1;
        //
        // GET: /franquia/cliente
        private AssinaturaSRV _serviceAss = new AssinaturaSRV();
        private ClienteSRV _service = new ClienteSRV();
        private ProspectSRV _prospectSRV = new ProspectSRV();
        private TipoDeClienteSRVProxy _tipoDeCliente = new TipoDeClienteSRVProxy();
        private AgendaSRV _agenda = new AgendaSRV();
        private HistAtendSRV _atendimento = new HistAtendSRV();
        private OpcaoAtendimentoSRV _setor = new OpcaoAtendimentoSRV();
        private TipoTelefoneSRV _tipotelefone = new TipoTelefoneSRV();
        private TipoAtendimentoSRV _tipoAtendSRV = new TipoAtendimentoSRV();
        private AcaoAtendimentoSRV _acaoAtendSRV = new AcaoAtendimentoSRV();
        private LinhaProdutoInformativoSRV _lproinfSRV = new LinhaProdutoInformativoSRV();

        private void _preencherCombos()
        {

            var _SetorTelefones = _setor.BuscarSetorDeTelefones().ToList();
            var _SetorEmails = _setor.BuscarSetorDeEmails().ToList();
            var _tipotel = _tipotelefone.BuscarTodos().ToList();

            List<URA> ListaUras = new URASRV().BuscarTodos().ToList();
            List<SelectListItem> ListaPasta = new List<SelectListItem>();
            List<SelectListItem> ListaTipoPessoa = new List<SelectListItem>();
            List<SelectListItem> ListaMotivoTroca = new List<SelectListItem>();

            var ListaTipoAtendimento = _tipoAtendSRV.FindAll();
            var ListaAcaoAtendimento = _acaoAtendSRV.FindAll();

            var ufs = new UFSRV().BuscarSomenteUF();
            var _tipoLogradouro = new TipoLogradouroSRV().FindAll();
            var tipoEndereco = new TipoEnderecoSRV().FindAll();
            var municipio = new MunicipioSRV().FindAll();
            var ListaClassificacao = new ClassificacaoSRV().FindAll();

            ListaTipoPessoa.AddRange(new[]{
                            new SelectListItem() { Text = "Física",   Value = "F" },
                            new SelectListItem() { Text = "Jurídica", Value = "J" }
            });

            ListaMotivoTroca.AddRange(new[]{
                            new SelectListItem() { Text = "Uso por Terceiros",   Value = "UPT" },
                            new SelectListItem() { Text = "Extravio", Value = "EXT" },
                            new SelectListItem() { Text = "Cliente quis trocar", Value = "CQT" }
            });

            ListaPasta.AddRange(new[]{
                            new SelectListItem() { Text = "Vazia", Value = "VAZ" },
                            new SelectListItem() { Text = "Cheia", Value = "CHE" }
                            
            });

            ViewBag.ufs = ufs;
            ViewBag.tiposDeCliente = _tipoDeCliente.RetornarTiposDeCliente(0);
            ViewBag.tipoLogradouro = _tipoLogradouro;
            ViewBag.tipoEndereco = tipoEndereco;
            ViewBag.ListaPasta = new SelectList(ListaPasta, "Value", "Text");
            ViewBag.ListaTipoPessoa = new SelectList(ListaTipoPessoa, "Value", "Text");
            ViewBag.ListaMotivoTroca = new SelectList(ListaMotivoTroca, "Value", "Text");
            ViewBag.ListaTipoAtendimento = new SelectList(ListaTipoAtendimento, "TIP_ATEND_ID", "TIP_ATEND_DESCRICAO");
            ViewBag.ListaAcaoAtendimento = new SelectList(ListaAcaoAtendimento, "ACA_ID", "ACA_DESCRICAO");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");

            ViewBag.ListaClassificacao = new SelectList(ListaClassificacao, "CLA_ID", "CLA_DESCRICAO");
            ViewBag.ListaUras = new SelectList(ListaUras, "URA_ID ", "URA_ID ");
            ViewBag.ListaSetorTelefone = new SelectList(_SetorTelefones, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.ListaSetorEmail = new SelectList(_SetorEmails, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.Listatipotel = new SelectList(_tipotel, "TIPO_TEL_ID", "TIPO_TEL_ID");
            ViewBag.ListaTipoLogradouro = new SelectList(_tipoLogradouro, "TIPO_LOG_ID", "TIPO_LOG_DESCRICAO");
            ViewBag.ListaTipoEndereco = new SelectList(tipoEndereco, "TP_END_ID", "TP_END_DESCRICAO");


        }
       
        
        [Autorizar]
        public ActionResult Novo(string cpf_cnpj, string nome, string email, 
                                                        string REP_ID, string dddTelefone, string telefone, int? AREA_ID, int? CMP_ID)
        {
            _preencherCombos();

            ViewBag.filtrosAInserir = new CadastraSuspectComBaseNoFiltroDTO()
            {
                cpf_cnpj = cpf_cnpj,
                nome = nome,
                email = email,
                dddTelefone = dddTelefone,
                telefone = telefone,
                AREA_ID = AREA_ID,
                CMP_ID = CMP_ID
            };
                
            ViewBag.RG_ID = SessionUtil.GetRegiao();
            ViewBag.validarCPF_CNPJ = 1;
            return View();
        }

       
        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(ClienteDto cliente)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    
                    int? rep_id = null;

                    if (AuthUtil.TryGetRepId(out rep_id))
                    {
                        _service.SalvarClienteRodizio(cliente, rep_id);

                    }
                    else {

                        throw new Exception("O representante não pode ser encontrado.");
                    }
                    SysException.RegistrarLog("Dados do cliente atualizados com sucesso!!", "", SessionContext.autenticado);
                    
                    result.success = true;
                    result.message = Message.Info("Dados do suspect atualizados com sucesso!!");
                    
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.SetMessageFromValidacaoException(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }      
       
    }
}
