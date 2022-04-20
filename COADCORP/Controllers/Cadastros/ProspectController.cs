using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Exceptions.ErrorHandling;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.Filters;
using COAD.CORPORATIVO.Util;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using GenericCrud.Service;

namespace COADCORP.Areas.Controllers.Cadastros
{
    public class ProspectController : Controller
    {
        private const int UEN_ID = 1;
        public ClienteSRV _service { get; set; } //= new ClienteSRV();
        public HistoricoPedidoSRV _serviceHistPedido { get; set; } //= new HistoricoPedidoSRV();
        public OpcaoAtendimentoSRV _setor { get; set; } //= new OpcaoAtendimentoSRV();
        public TipoTelefoneSRV _tipotelefone { get; set; } //= new TipoTelefoneSRV();
        public TipoAtendimentoSRV _tipoAtendSRV { get; set; } //= new TipoAtendimentoSRV();
        public AcaoAtendimentoSRV _acaoAtendSRV { get; set; } //= new AcaoAtendimentoSRV();
        public TipoDeClienteSRVProxy _tipoDeCliente { get; set; } //= new TipoDeClienteSRV();
        public ClienteProspectSRV _cliProspSRV { get; set; } //= new ClienteProspectSRV();
        public ClassificacaoClienteSRV _classificacaoClienteSRV { get; set; } //= new ClassificacaoClienteSRV();
        public OrigemCadastroSRV _origemCadastro { get; set; } //= new OrigemCadastroSRV();
        public UFSRV _ufSRV { get; set; }

        private void _preencherCombos()
        {

            ViewBag.tiposDeCliente = _tipoDeCliente.RetornarTiposDeCliente(0);
            ViewBag.ListaUf = new SelectList(_ufSRV.FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");
            ViewBag.ufs = _ufSRV.BuscarSomenteUF();

        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            _preencherCombos();
            ViewBag.tipo = "cliente";
            return View("Editar");
        }
        //

        //[AutorizarCustomAttribute(SessionUtilMethodName = "PossuiPermissaoParaEditarProspect", PorMenu = false)]
        [Autorizar(PorMenu = false)]
        public ActionResult Editar(string clienteId, string tipo)
        {
            _preencherCombos();
            ViewBag.clienteId = clienteId;
            ViewBag.tipo = tipo;    
            ViewBag.abrirModal = false;
            return View();
        }
        //
        // GET: /franquia/Pedido/

        [Autorizar(Departamento = "TI, franquiado, franquiador",PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index()
        {
            int? uenId = SessionUtil.GetUenId();
            ViewBag.lstUen = new UENSRV().FindAll();

            if (SessionContext.IsGerenteDepartamento("franquiador") || SessionContext.IsGerenteDepartamento("ti"))
            {
                ViewBag.lstRegiao = new RegiaoSRV().FindAllByUen(uenId);
            }
            else if(SessionContext.IsAdmDepartamento("franquiador") || SessionContext.IsAdmDepartamento("TI"))
            {
                ViewBag.lstRegiao = new RegiaoSRV().FindAll();
            }
            return View();
        }

        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoProspect(int? clienteId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var prospect = _cliProspSRV.CarregarDadosDoProspectCorp(clienteId);
                response.Add("prospect", prospect);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDoProspectOriginal(string codigo)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var prospect = _cliProspSRV.CarregarDadosDoProspectOriginal(codigo);
                response.Add("prospect", prospect);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        

        [AutorizarCustomAttribute(SessionUtilMethodName = "PossuiPermissaoParaEditarProspect", IsAjax = true)]
        [HttpPost]
        public ActionResult SalvarClienteEProspect(ClienteProspectDTO clienteProspect)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    int? REP_ID = SessionContext.GetIdRepresentante();

                    var salvamentoResult = _cliProspSRV.SalvarClienteEProspect(clienteProspect, REP_ID);
                    result.Add("salvamentoResult", salvamentoResult);

                    if (salvamentoResult != null && salvamentoResult.Cliente != null)
                    {
                        result.Add("cliId", salvamentoResult.Cliente.CLI_ID);
                        result.message = Message.Info("Dados do prospects atualizados com sucesso!!");
                        
                    }
                    
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (WarningException e)
            {
                result.success = false;
                SessionUtil.HandleException(e);
                result.message = Message.Fail(e);
                result.Add("requisicaoForcar", true);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e);
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

        [Autorizar(PorMenu = false)]
        public ActionResult Buscar()
        {
            if (SessionUtil.FranquiadoOuGerenteOuTI())
            {
                var lstClassificacaoCliente = _classificacaoClienteSRV.FindAll();
                ViewBag.lstClassificacaoCliente = lstClassificacaoCliente;
            }

            var lstOrigemCadastro = _origemCadastro.FindAll();
            ViewBag.lstOrigemCadastro = lstOrigemCadastro;
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarProspectCorp(
            string cpf_cnpj = null,
            string nome = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 30,
            int? codigoCliente = null)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                   var rgId = SessionUtil.GetRegiao();

                    //if (REP_ID != null && (!SessionContext.IsGerenteDepartamento("FRANQUIADO", true) && !SessionContext.IsGerenteDepartamento("FRANQUIADOR", true) && !SessionContext.IsGerenteDepartamento("TI", true)))
                    //{
                    //    throw new Exception("Seu nivelRepresentante não tem acesso a pesquisa por representante.");
                    //}

                    var uenId = SessionUtil.GetUenId();
                    var lstProspectCorp = _cliProspSRV.BuscarProspectsCorp(
                        cpf_cnpj,
                        nome,
                        email,
                        dddTelefone,
                        telefone,
                        pesquisaCpfCnpjPorIqualdade,
                        pagina,
                        registroPorPagina,
                        codigoCliente);


                    response.AddPage("lstProspectCorp", lstProspectCorp);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
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
        public ActionResult BuscarProspect(
            string cpf_cnpj = null,
            string nome = null,
            string email = null,
            string dddTelefone = null,
            string telefone = null,
            int? classificacaoClienteId = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registroPorPagina = 50       
            )
        {

            JSONResponse response = new JSONResponse();

            try
            {


                var lstProspect = _cliProspSRV.BuscarProspects(cpf_cnpj,
                    nome,
                    email,
                    dddTelefone,
                    telefone,
                    pesquisaCpfCnpjPorIqualdade,
                    pagina,
                    registroPorPagina); 


                response.AddPage("lstProspect", lstProspect);
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult BuscarCodigosCarteiramento(string carDescricao)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstStrCarteiramentos = new RepresentanteLegadoSRV().BuscarCodigosCarteiramento(carDescricao);
                IList<SelectListItem> lstCarteiramentos = new List<SelectListItem>();
                
                if (lstStrCarteiramentos != null)
                {
                    lstCarteiramentos =     
                        lstStrCarteiramentos
                        .Select(x => new SelectListItem() { Text = x, Value = x })
                        .ToList();
                }
                response.Add("lstCarteiramentos", lstCarteiramentos);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public JsonResult ListarOpcoesAtendimentos()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstOpcoesAtendimento = ServiceFactory
                    .RetornarServico<OpcaoAtendimentoSRV>()
                    .FindAll();

                response.Add("lstOpcoesAtendimento", lstOpcoesAtendimento);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChecarCarteiraValida(string carId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var valida = ServiceFactory
                    .RetornarServico<CarteiramentoSRV>()
                    .ChecarCarteiraValida(carId);

                response.Add("valida", valida);

            }
            catch (Exception e)
            {
                response.Add("valida", false);
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
