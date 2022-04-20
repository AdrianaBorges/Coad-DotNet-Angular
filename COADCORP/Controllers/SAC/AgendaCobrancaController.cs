using System;
using System.Linq;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;

namespace COADCORP.Controllers.SAC
{
    public class AgendaCobrancaController : Controller
    {
        public AgendaSRV _agenda { get; set; }
        public HistAtendSRV _atendimento { get; set; }
        private void CarregarCombos()
        {
            var _SetorTelefones = new OpcaoAtendimentoSRV().BuscarSetorDeTelefones().ToList();
            var _tipotel = new TipoTelefoneSRV().BuscarTodos().ToList();

            ViewBag.ListaSetorTelefone = new SelectList(_SetorTelefones, "OPC_ID", "OPC_DESCRICAO");
            ViewBag.Listatipotel = new SelectList(_tipotel, "TIPO_TEL_ID", "TIPO_TEL_DESCRICAO");
        }
        public ActionResult Index()
        {
            this.CarregarCombos();

            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarHistorico(int _cli_id, int _pagina = 1, int _registroPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                ///--- Busca do corporativo antigo.
                var listaagenda = _agenda.BuscarPorCliente(_cli_id.ToString(), _pagina, _registroPorPagina);
                ///--------------------------------

                var listaatend = _atendimento.BuscarPorCliente(_cli_id, _pagina, _registroPorPagina);

                response.AddPage("listaagenda", listaagenda);
                response.AddPage("listaatend", listaatend);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        // parcelas liberadas com antigo código nove
        // atual parcela liberada em situação irregular
        [Autorizar(IsAjax = true)]
        public ActionResult ParcelaLiberadaSitIrregular(int _pagina = 1, int _registroPorPagina = 10)
        {
            var response = new JSONResponse();

            try
            {
                var lstliberacaoindevida = new ParcelasSRV().ParcelaLiberadaSitIrregular(_pagina);

                response.AddPage("lstliberacaoindevida", lstliberacaoindevida);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarAgendamento( string assinatura
                                             , string cliente
                                             , string atendente
                                             , string cnpj
                                             , DateTime? dataini = null
                                             , DateTime? datafim = null
                                             , bool pendente = true
                                             , int pagina = 1
                                             , int registroPorPagina = 20)
        {

            var response = new JSONResponse();

            try
            {
                var lstagendamento = new AgendaCobrancaSRV().BuscarAgendamento(assinatura, cliente, atendente, cnpj, dataini, datafim, pendente, pagina, registroPorPagina );

                response.AddPage("lstagendamento", lstagendamento);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        
        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTitulosVencidos(int? _cli_id, string _asn_num_assinatura)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (_cli_id == null)
                {
                    var _assinatura = new AssinaturaSRV().FindById(_asn_num_assinatura);
                    _cli_id = _assinatura.CLI_ID;
                }

                var _srvParcela = new ParcelasSRV();
                var _lstTitulosVencidos = _srvParcela.ListarTitulosVencidos((int)_cli_id);
                var _totaldebito = _lstTitulosVencidos.Sum(x => x.VLR_TOTAL_DEBITO);
                var _lstNegociacao = _srvParcela.ListarNegociacaoAtraso(_totaldebito, 6);
                
                response.Add("totaldebito", _totaldebito);
                response.Add("lstTitulosVencidos", _lstTitulosVencidos);
                response.Add("lstNegociacao", _lstNegociacao);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true)]
        public ActionResult BuscarDebitoDetalhadamente(string assinatura, int cliente)
        {
            var response = new JSONResponse();

            try
            {
                var _srv = new ParcelasSRV();

                var _lstTitulosVencidos = _srv.ListarDebitoDetalhadamente(assinatura, cliente);

                var _totaldebito = _lstTitulosVencidos.Sum(x => x.PAR_VLR_PARCELA);

                response.Add("totaldebito", _totaldebito);
                response.Add("lstTitulosVencidos", _lstTitulosVencidos);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTitulosAtraso( string assinatura
                                               , string cliente
                                               , string atendente
                                               , string cnpj
                                               , DateTime? dataini = null
                                               , DateTime? datafim = null
                                               , int pagina = 1
                                               , bool primeiraParcela = false)

        {
            var response = new JSONResponse();

            try
            {
                var lstagendacobranca = new ParcelasSRV()
                    .BuscarTitulosEmAtraso(assinatura, cliente, atendente, cnpj, dataini, datafim, pagina, primeiraParcela);

                response.AddPage("lstagendacobranca", lstagendacobranca);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTitulosAtrasoPrimeiraParcela(string assinatura
                                              , string cliente
                                              , string atendente
                                              , string cnpj
                                              , DateTime? dataini = null
                                              , DateTime? datafim = null
                                              , int pagina = 1
                                              , bool primeiraParcela = false)
        {
            var response = new JSONResponse();

            try
            {
                var lstprimeiraparcela = new ParcelasSRV()
                    .ListarTitulosEmAtrasoPrimeiraParcela(assinatura, cliente, atendente, cnpj, dataini, datafim, pagina, primeiraParcela);

                response.AddPage("lstprimeiraparcela", lstprimeiraparcela);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarTitulosComParcelaLiberada(string assinatura
                                              , string cliente
                                              , string atendente
                                              , string cnpj
                                              , DateTime? dataini = null
                                              , DateTime? datafim = null
                                              , int pagina = 1)
        {
            var response = new JSONResponse();

            try
            {
                var lstparcelaliberada = new ParcelasSRV()
                    .BuscarTitulosComParcelaLiberada(assinatura, cliente, atendente, cnpj, dataini, datafim, pagina);

                response.AddPage("lstparcelaliberada", lstparcelaliberada);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult BuscarParcelaComLiberacaorregular(int pagina)
        {
            var response = new JSONResponse();

            try
            {
                var lstliberacaoindevida = new ParcelasSRV()
                    .ParcelaLiberadaSitIrregular(pagina);

                response.AddPage("lstliberacaoindevida", lstliberacaoindevida);
                response.success = true;
                response.message = Message.Info("Ok");

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult GravarAgendamento(AgendaCobrancaDTO _agenda)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                new AgendaCobrancaSRV().GravarAgendamento(_agenda);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public void AtualizarStatusDeAgendamento()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                new AgendaCobrancaSRV().ExecutarUpdateEmAgendamentos();

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

        }
    }
}
