using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COADCORP.Models;
using ACBrFramework.Sped;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.UTIL.Ferramentas;
using COAD.CORPORATIVO.Model.Dto;
using GenericCrud.Excel;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using GenericCrud.Service;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Util;
using COAD.FISCAL.Service.Integracoes;
using COAD.FISCAL.Model.DTO.Requests;
using COAD.CORPORATIVO.Model.Dto.FiltersInfo;
using GenericCrud.Controllers;
using COAD.SEGURANCA.Model.Custons;
using Coad.GenericCrud.Exceptions;

namespace COADCORP.Controllers.Lancamentos
{
    public class NotaFiscalController : GenericController<NOTA_FISCAL, NotaFiscalDTO, object>
    {
        public List<SelectListItem> ListaMeses = new List<SelectListItem>();
        private NotaFiscalSRV NotaFiscalSRV { get; set; }
        public ImpostoSRV _impostoSRV { get; set; }

        public NotaFiscalController()
        {

        }

        public NotaFiscalController(NotaFiscalSRV notaFiscalSRV) : base(notaFiscalSRV)
        {
            NotaFiscalSRV = notaFiscalSRV;
        }

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            ViewBag.Title = " Nota Fiscal";

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            this.Carregarlistas2();

            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();

            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");

            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult RptNotasPeriodo()
        {
            ViewBag.Title = "Relatório de Nota Fiscal por período";
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            this.Carregarlistas2();

            return View();

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarNotaFiscal(int? _nfnumero
                                           , string _cpfCnpj
                                           , int? _mesatual
                                           , string _anoatual
                                           , int? _emp_id
                                           , int? _nf_tipo
                                           , bool? antecipada = null
                                           , bool? avulsa = null
                                           , int numpagina = 1
                                           , int linhas = 15
                                           )
        {



            ViewBag.MesAtual = _mesatual;
            ViewBag.AnoAtual = _anoatual;

            DateTime _dtinicial = new DateTime(Convert.ToInt32(_anoatual), (int)_mesatual, 1);
            DateTime _dtfinal = new DateTime(Convert.ToInt32(_anoatual), (int)_mesatual, DateTime.DaysInMonth(Convert.ToInt32(_anoatual), (int)_mesatual));
            
            JSONResponse response = new JSONResponse();

            try
            {
                Pagina<NotaFiscalDTO> _listanf = null;

                if (String.IsNullOrWhiteSpace(_cpfCnpj))
                    _cpfCnpj = null;

                if (_nfnumero != null || _cpfCnpj != null)
                    _listanf = new NotaFiscalSRV().BuscarPorPeriodo(_nfnumero, _cpfCnpj, numpagina, linhas, antecipada, avulsa);
                else if (_dtinicial != null && _dtfinal != null && _emp_id != null && _nf_tipo != null)
                    _listanf = new NotaFiscalSRV().BuscarPorPeriodo((int)_nf_tipo, 0, (int)_emp_id, (DateTime)_dtinicial, (DateTime)_dtfinal, numpagina, linhas, antecipada, avulsa);

                if (_listanf.lista.Count() == 0)
                    throw new Exception("Nenhum item encontrado para o período selecionado.");

                response.success = true;
                response.AddPage("listanf", _listanf);
                
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
                
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ListarNfeCliente(int? _cli_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (_cli_id == null)
                    throw new Exception("Cliente não informado!!");

                var _listaNotaFiscal = new NotaFiscalSRV().BuscarNfeCliente((int)_cli_id);
                response.success = true;
                response.Add("listaNotaFiscal", _listaNotaFiscal);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult ListarNotasPeriodo(int? _mesatual, string _anoatual, int? _emp_id, int? _nf_tipo, int numpagina = 1, int linhas = 10)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                if (_mesatual == null || _anoatual == null)
                    throw new Exception("Informe Mes/Ano para realizar a consulta!! ");

                DateTime _dtini = new DateTime(Convert.ToInt32(_anoatual), (int)_mesatual, 1);
                DateTime _dtfim = new DateTime(Convert.ToInt32(_anoatual), (int)_mesatual, DateTime.DaysInMonth(Convert.ToInt32(_anoatual), (int)_mesatual));

                var listaNotaFiscalSintetico = new NotaFiscalSRV().BuscarNotasPeriodoSintetico((DateTime)_dtini, (DateTime)_dtfim);
                var listaNotaFiscal = new NotaFiscalSRV().BuscarNotasPeriodo((int)_emp_id, (int)_nf_tipo, (DateTime)_dtini, (DateTime)_dtfim, numpagina, linhas);
                response.success = true;
                response.message = Message.Info("Ok");
                response.AddPage("listaNotaFiscal", listaNotaFiscal);
                response.Add("listaNotaFiscalSintetico", listaNotaFiscalSintetico);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [Autorizar(IsAjax = true)]
        public ActionResult RelatorioNotasPeriodo(List<NotaFiscalDTO> _ListaNotaFiscal)
        {
            return View(_ListaNotaFiscal);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost, ValidateInput(false)]
        public ActionResult RelatorioNotasPeriodo(string campoform)
        {
            string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            return new PdfResult(campoform, curDir);
        }
        [Autorizar(IsAjax = true)]
        public void Carregarlistas()
        {
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            List<CFOTableDTO> Listacfop = new CFOPSRV().Buscar("SAI").ToList();
            List<SelectListItem> Lista = new List<SelectListItem>();
            List<TIPO_DOC_FISCAL> _listatipodoc = new TipoDocFiscalSRV().BuscarTodos().ToList();

            Lista.Add(new SelectListItem { Text = "Emitente", Value = "0" });
            Lista.Add(new SelectListItem { Text = "Destinatário", Value = "1" });



            ViewBag.ListaTipoDoc = new SelectList(_listatipodoc, "TDF_ID", "TDF_DESCRICAO");
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaTipoFrete = new SelectList(Lista, "Value", "Text");
            ViewBag.ListaUf = new SelectList(new UFSRV().FindAll().ToList(), "UF_SIGLA", "UF_DESCRICAO");
            ViewBag.Listacfop = new SelectList(Listacfop, "CFOP", "CFOP_DESCRICAO");

        }
        [Autorizar(IsAjax = true)]
        public ActionResult ExportarXLS(int? _nfnumero, int? _mesatual, string _anoatual, int? _emp_id, int? _nf_tipo)
        {

            JSONResponse response = new JSONResponse();
            try
            {
                ViewBag.MesAtual = _mesatual;
                ViewBag.AnoAtual = _anoatual;

                DateTime _dtinicial = new DateTime(Convert.ToInt32(_anoatual), (int)_mesatual, 1);
                DateTime _dtfinal = new DateTime(Convert.ToInt32(_anoatual), (int)_mesatual, DateTime.DaysInMonth(Convert.ToInt32(_anoatual), (int)_mesatual));

                string _nomearquivo = null;

                var _lista = new NotaFiscalSRV().BuscarPorPeriodo((int)_nf_tipo, 0, (int)_emp_id, _dtinicial, _dtfinal);

                if (_nomearquivo == null)
                    _nomearquivo = DateTime.Now.Day.ToString() +
                                   DateTime.Now.Month.ToString() +
                                   DateTime.Now.Year.ToString() +
                                   DateTime.Now.Millisecond.ToString() +
                                   DateTime.Now.Minute.ToString();
                //  throw new Exception("Nome do arquivo não informado!!");

                if (_lista.Count == 0)
                    throw new Exception("Nenhum item encontrado para o período selecionado.");

                string _retorno = null;

                _retorno = new ExcelLoad().Export(_lista, _nomearquivo, System.Web.HttpContext.Current);

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
        public void Carregarlistas2()
        {
            //-----

            Array itemValues = System.Enum.GetValues(typeof(GrupoTensao));
            Array itemNames = System.Enum.GetNames(typeof(GrupoTensao));

            List<SelectListItem> _lista = new List<SelectListItem>();

            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                SelectListItem _item = new SelectListItem() { Text = itemNames.GetValue(i).ToString(), Value = itemValues.GetValue(i).ToString() };
                _lista.Add(_item);
            }

            SelectList _ListaGrupoTensao = new SelectList(_lista, "Value", "Text");

            //-----

            _lista = new List<SelectListItem>();

            itemValues = System.Enum.GetValues(typeof(TipoLigacao));
            itemNames = System.Enum.GetNames(typeof(TipoLigacao));

            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                SelectListItem _item = new SelectListItem() { Text = itemNames.GetValue(i).ToString(), Value = itemValues.GetValue(i).ToString() };
                _lista.Add(_item);
            }

            SelectList _ListaTipoLigacao = new SelectList(_lista, "Value", "Text");

            //-----

            _lista = new List<SelectListItem>();

            _lista.Add(new SelectListItem() { Text = "Entrada", Value = "0", Selected = true });
            _lista.Add(new SelectListItem() { Text = "Saida", Value = "1" });
            _lista.Add(new SelectListItem() { Text = "Entrada Servico", Value = "2" });
            _lista.Add(new SelectListItem() { Text = "Saida Servico", Value = "3" });

            SelectList _ListaTipoNF = new SelectList(_lista, "Value", "Text");

            //-----

            List<CLASSICACAO_CONSUMO> _listaclass = new ClassificacaoConsumoSRV().BuscarPorTipo(0);

            //-----
            var ListaEmpresa = new EmpresaSRV().FindAll().ToList();
            //-----

            //-----

            ListaMeses.AddRange(new[]{
                            new SelectListItem() { Text = "Janeiro", Value = "1" },
                            new SelectListItem() { Text = "Fevereiro", Value = "2" },
                            new SelectListItem() { Text = "Março", Value = "3" },
                            new SelectListItem() { Text = "Abril", Value = "4" },
                            new SelectListItem() { Text = "Maio", Value = "5" },
                            new SelectListItem() { Text = "Junho", Value = "6" },
                            new SelectListItem() { Text = "Julho", Value = "7" },
                            new SelectListItem() { Text = "Agosto", Value = "8" },
                            new SelectListItem() { Text = "Setembro", Value = "9" },
                            new SelectListItem() { Text = "Outubro", Value = "10" },
                            new SelectListItem() { Text = "Novembro", Value = "11" },
                            new SelectListItem() { Text = "Dezembro", Value = "12" }
            });

            //-----

            ViewBag.ListaMes = new SelectList(ListaMeses, "Value", "Text");
            ViewBag.AnoAtual = DateTime.Now.Year;
            ViewBag.ListaEmpresa = new SelectList(ListaEmpresa, "EMP_ID", "EMP_NOME_FANTASIA");
            ViewBag.ListaClassificao = new SelectList(_listaclass, "CLC_COD_CLASSIFICACAO", "CLC_DESC_CLASSIFICACAO");
            ViewBag.ListaTipoNF = _ListaTipoNF;
            ViewBag.ListaTipoLigacao = _ListaTipoLigacao;
            ViewBag.ListaGrupoTensao = _ListaGrupoTensao;

        }
        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int? _nf_id)
        {
            ViewBag.Title = " Nota Fiscal (Editar) ";

            if (_nf_id == null)
                ViewBag.Title = " Nota Fiscal (Novo)";


            this.Carregarlistas();
            this.Carregarlistas2();

            ViewBag.nf_id = _nf_id;

            return View();
        }

        [Autorizar(IsAjax = true, Acesso = "Incluir")]
        public override ActionResult Salvar(NotaFiscalDTO _nota_fiscal)
        {

            JSONResponse resultado = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    new NotaFiscalSRV().SalvarNf(_nota_fiscal);

                    resultado.success = true;
                    resultado.message = Message.Info("Dados da nota fiscal atualizados com sucesso!!");

                    return Json(resultado, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    resultado.success = false;
                    resultado.SetMessageFromModelState(ModelState);
                    return Json(resultado, JsonRequestBehavior.AllowGet);
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

                resultado.success = false;
                resultado.message = Message.Fail(ex);
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarFornecedor(string _for_cnpj)
        {
            //--- For_Tipo (0 - Todos , 1 - Fornecedor , 2 - Transportador)
            try
            {
                FornecedorDTO a = new FornecedorDTO();

                if (_for_cnpj.Trim() != "")
                    a = new FornecedorSRV().BuscarPorCNPJ(_for_cnpj);

                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", SysException.Show(ex));

                TempData.Add("Erro", SysException.Show(ex));

                return Json(SysException.Show(ex), JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTransportador(string _tra_cnpj)
        {
            try
            {
                TransportadorDTO a = new TransportadorDTO();

                if (_tra_cnpj.Trim() != "")
                    a = new TransportadorSRV().BuscarPorCNPJ(_tra_cnpj);

                return Json(a, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", SysException.Show(ex));

                TempData.Add("Erro", SysException.Show(ex));

                return View("Index");
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarNotas(int? _nf_id)
        {
            JsonResponse resultado = new JsonResponse();

            try
            {
                if (_nf_id == null)
                    _nf_id = 0;


                NotaFiscalDTO _notafiscal = new NotaFiscalSRV().Buscar((int)_nf_id);

                return Json(_notafiscal, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                resultado.Message = "Erro ao carregar os dados ( " + SysException.Show(ex) + " )";

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarProduto(string _pro_nome)
        {
            try
            {
                List<Produto> p = new List<Produto>();

                if (_pro_nome.Trim() != "")
                    p = new ProdutosSRV().BuscarPorDescricao(_pro_nome);

                return Json(p, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(SysException.Show(ex), JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult MostrarProduto(int _pro_id)
        {
            try
            {
                Produto p = new Produto();

                if (_pro_id > 0)
                    p = new ProdutosSRV().BuscarPorID(_pro_id);

                return Json(p, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(SysException.Show(ex), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BuscarCfopEntrada(string _cfopsaida)
        {
            try
            {
                CFOTableDTO p = new CFOTableDTO();

                if (_cfopsaida.Trim() != "")
                    p = new CFOPSRV().BuscarCFOPEntrada(_cfopsaida);

                return Json(p, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(SysException.Show(ex), JsonRequestBehavior.AllowGet);
            }
        }
        [Autorizar(IsAjax = true)]
        public ActionResult ImportarXML()
        {
            this.Carregarlistas();

            return View();
        }
        [Autorizar(Acesso = "Editar")]
        [HttpPost]
        public ActionResult ImportarXML(HttpPostedFileBase uploadFile, int? _emp_id)
        {
            try
            {
                if (_emp_id == null)
                    throw new Exception("Empresa não informada!!");


                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    string _nomearquivo = Path.GetFileName(uploadFile.FileName);
                    string _arquivo = HttpContext.Server.MapPath("~/temp/") + Path.GetFileName(uploadFile.FileName);
                    string _filePath = Path.Combine(HttpContext.Server.MapPath("~/temp/"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(_filePath);

                    var _nf = new NotaFiscalSRV().ImportarXML(_nomearquivo, (int)_emp_id);

                    if (_nf != null)
                    {
                        TempData.Add("Resultado", "Nota Fiscal incluída com sucesso !!");
                    }
                    else
                    {
                        TempData.Add("Resultado", "Nota Fiscal não incluída. Verifique!!");
                    }

                }
                else
                    TempData.Add("Resultado", "Arquivo não informado!");


                this.Carregarlistas();

                return View();

            }
            catch (Exception ex)
            {
                this.Carregarlistas();

                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                TempData.Add("Resultado", SysException.Show(ex));

                return View();
            }
        }
        [Autorizar(Acesso = "Excluir")]
        [HttpPost]
        public ActionResult Excluir(NotaFiscalDTO _notafiscal)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                new NotaFiscalSRV().ExcluirNf(_notafiscal);

                result.success = true;
                result.message = Message.Info("Operação realizada com sucesso !!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                result.success = false;
                result.SetMessageFromModelState(ModelState);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult EnviarNotaFiscal(NotaFiscalDTO _notafiscal)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<NotaFiscalSRV>().EnviarNotaFiscal(_notafiscal);
                result.success = true;
                result.message = Message.Info("Email enviado com sucesso !!");
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [Autorizar(IsAjax = true)]
        public FileResult GerarDanfe(int id)
        {

            var _path = ServiceFactory.RetornarServico<NotaFiscalSRV>().GerarDanfe(id);

            var info = new FileInfoDTO(_path);

            if (info != null)
                return File(info.Bytes, System.Net.Mime.MediaTypeNames.Application.Pdf, info.FileName);
                                
            throw new HttpException(404, "O arquivo não foi encontrado");

        }
 
        public ActionResult CancelarNotaFiscal(int? nfID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().CancelarNotaFiscal(nfID);
                result.success = true;
                result.message = Message.Info("Nota Fiscal Cancelada com sucesso!!");
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelarNotaFiscalServico(int? nfID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().CancelarNotaFiscal(nfID);
                result.success = true;
                result.message = Message.Info("Nota Fiscal Cancelada com sucesso!!");
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GerarDevolucaoNotaFiscal(int? nfID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ServiceFactory.RetornarServico<IntegrNfeSRV>().GerarDevolucao(nfID);
                result.success = true;
                result.message = Message.Info("Nota Fiscal agendada para devolução com sucesso!!");
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GerarCartaCorrecao(CartaCorrecaoRequestDTO cartaCorrecaoRequest)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceFactory.RetornarServico<IntegrNfeSRV>().GerarCartaDeCorrecao(cartaCorrecaoRequest);
                    result.success = true;
                    result.message = Message.Info("Carta enviada com sucesso!!");
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarEventosNotaFiscal(int? nfID, int pagina = 1, int registrosPorPagina = 4)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var lstEventos = ServiceFactory.RetornarServico<NotaFiscalEventoSRV>().ListarEventosNotaFiscal(nfID, pagina, registrosPorPagina);
                response.AddPage("lstEventos", lstEventos);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public FileResult DownloadNFe(int? nfID)
        {
            var fileInfo = ServiceFactory.RetornarServico<NotaFiscalSRV>().RetornarArquivoDaNota(nfID);
            if (fileInfo != null)
            {
                return File(fileInfo.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileInfo.FileName);

            }
            throw new HttpException(404, "O arquivo não foi encontrado");
        }

        [Autorizar(IsAjax = true)]
        public FileResult DownloadEventoNFe(int? nfEveID)
        {

            var fileInfo = ServiceFactory.RetornarServico<NotaFiscalEventoSRV>().RetornarArquivoEventoNota(nfEveID);            
            if(fileInfo != null)
            {
                return File(fileInfo.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileInfo.FileName);

            }
            throw new HttpException(404, "O arquivo não foi encontrado");
        }

        
        public ActionResult ListarItensDeLote(int? nfID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstLoteItem = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>().ListarItensDoLoteDaNota(nfID);
                response.Add("lstLoteItem", lstLoteItem);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListarItensDeLoteService(int? nfID)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstLoteItem = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>().ListarItensDoLoteDaNotaService(nfID);
                response.Add("lstLoteItemService", lstLoteItem);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(PorMenu = false)]
        public ActionResult PesquisarNotaFiscal(
           PesquisaNotaFiscalDTO filtros)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listanf = NotaFiscalSRV.PesquisarNotaFiscal(filtros);
                response.AddPage("listanf", listanf);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public FileResult DownloadNFeDoLote(int? loteItemID)
        {
            var fileInfo = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>().RetornarArquivoDaNota(loteItemID);
            if (fileInfo != null)
            {
                return File(fileInfo.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileInfo.FileName);

            }
            throw new HttpException(404, "O arquivo não foi encontrado");
        }

        [Autorizar(IsAjax = true)]
        public JsonResult ListarNotaFiscalTipo()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var lstNfTipo = ServiceFactory.RetornarServico<NotaFiscalTipoSRV>().FindAll();
                response.Add("lstNfTipo", lstNfTipo);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(PorMenu = false)]
        public ActionResult Avulsa(int? nfId)
        {
            ViewBag.nfId = nfId;
            return View();
        }


        [Autorizar(IsAjax = true)]
        public JsonResult RecuperarDadosDaNotaFiscal(int nfId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var nf = NotaFiscalSRV.FindByIdFullLoaded(nfId, true);
                response.Add("nf", nf);

            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CalcularDescontoDosImpostos(
                NotaFiscalDTO notaFiscal)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                _impostoSRV.CalcularDescontosImpostoNotaServico(notaFiscal);
                response.Add("notaFiscal", notaFiscal);
                response.success = true;
                
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
        public ActionResult SalvarNotaFiscalAvulsa(NotaFiscalDTO notaFiscal)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    NotaFiscalSRV.SalvarNotaFiscalAvulsa(notaFiscal);

                    result.message = Message.Info("Nota Fiscal Avulsa Salva com sucesso!!");
                    SysException.RegistrarLog("Nota Fiscal Avulsa Salva com sucesso!!", "", SessionContext.autenticado);                 

                    result.success = true;

                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException e)
            {
                result.success = false;
                result.SetMessageFromValidacaoException(e, false, true);
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


        [Autorizar(IsAjax = true)]
        public JsonResult GerarLinkDanfe(int? nfeId)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var link = NotaFiscalSRV.GerarLinkDanfe(nfeId);
                response.Add("link", link);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExecutarCallBacksLoteItem(int? nflId)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                NotaFiscalSRV.ExecutarCallBacksLoteItem(nflId);
                result.success = true;
                result.message = Message.Info("Callback executado com sucesso!!");
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
