using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.COBRANCA.Bancos.Model.DTO;
using COAD.COBRANCA.Bancos.Service;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.DAO.Reflection;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.EnvioEmail;
using COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate;
using COAD.CORPORATIVO.Model.Dto.Custons.Historicos;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Service.Utils;
using COAD.CORPORATIVO.Util;
using COAD.CRYPT;
using COAD.SEGURANCA.Config.Email;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Exceptions;
using GenericCrud.Metadatas;
using GenericCrud.Service;
using GenericCrud.Util;
using Newtonsoft.Json;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PAR_NUM_PARCELA")]
    public class ParcelasSRV : GenericService<PARCELAS, ParcelasDTO, string>
    {
        // legado \\
        public ParcelasLegadoSRV _serviceParcelasLegado { get; set; }
        public LiquidacaoLegadoSRV _serviceLiquidacaoLegado { get; set; }

        // coadcorp \\


        public ParcelaPendenteSRV _serviceParcelaPendente { get; set; }
        public ParcelasDAO _dao { get; set; }
        public ContaSRV _srvConta { get; set; }
        public PedidoPagamentoSRV _pedidoPagamentoSRV { get; set; }
        public ParcelaLiquidacaoSRV _serviceLiquidacao { get; set; }

        public ParcelaAlocadaSRV _serviceParcelaAlocada { get; set; }
        public ParcelasRemessaSRV _serviceParcelasRemessa { get; set; }
        public ConfigAlocacaoContaSRV _configAlocacaoSRV { get; set; }
        public CnabArquivosSRV _srvcnabarq { get; set; }
        public CnabArquivosItemSRV _srvcnabitemarq { get; set; }
        public CnabArquivosItemErroSRV _srvcnabitemarqerro { get; set; }

        // ocorrências \\
        public OcorrenciaErroSRV _serviceOcorrenciaErro { get; set; }
        public OcorrenciaRetornoSRV _serviceOcorrenciaRetorno { get; set; }
        public OcorrenciaRemessaSRV _serviceOcorrenciaRemessa { get; set; }
        public CryptService _cryptSRV = new CryptService();
        public TabSeqSRV _sequenciaSRV { get; set; }
        public IEmailSRV EmailSRV { get; set; }
        public JobAgendamentoSRV jobAgendamento { get; set; }
        public AssinaturaSRV _asssrv { get; set; }

        private static bool _podeEnviar = true;
        public static bool podeProcessar
        {
            get
            {
                return _podeEnviar;
            }
            set
            {
                _podeEnviar = value;
            }
        }


        public ParcelasDTO BuscarParcelaNossoNumero(string _par_nosso_numero)
        {
            return _dao.BuscarParcelaNossoNumero(_par_nosso_numero);
        }
        public ParcelasDTO BuscarParcela(string _par_num_parcela, bool _baixamanual = false)
        {
            return _dao.BuscarParcela(_par_num_parcela, _baixamanual);
        }

        public ParcelasSRV()
        {
            _dao = new ParcelasDAO();

            _serviceParcelaPendente = new ParcelaPendenteSRV();
            _serviceParcelasLegado = new ParcelasLegadoSRV();
            _serviceLiquidacaoLegado = new LiquidacaoLegadoSRV();
            _pedidoPagamentoSRV = new PedidoPagamentoSRV();
            _serviceLiquidacao = new ParcelaLiquidacaoSRV();
            _serviceParcelaAlocada = new ParcelaAlocadaSRV();
            _serviceParcelasRemessa = new ParcelasRemessaSRV();
            _configAlocacaoSRV = new ConfigAlocacaoContaSRV();

            _serviceOcorrenciaErro = new OcorrenciaErroSRV();
            _serviceOcorrenciaRetorno = new OcorrenciaRetornoSRV();
            _serviceOcorrenciaRemessa = new OcorrenciaRemessaSRV();
            _sequenciaSRV = new TabSeqSRV();


            Dao = _dao;
            EmailActionContainer.AddSuccessCallback("envBoletoAuto", ctx => {

                MarcarParcelaBoletoComoEnviado(ctx);
            });

            EmailActionContainer.AddFailCallback("envBoletoAuto", ctx => {

                NotificarBoletoNaoEnviado(ctx);
            });
        }

        public ParcelasSRV(ParcelasDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;

            EmailActionContainer.AddSuccessCallback("envBoletoAuto", ctx => {

                MarcarParcelaBoletoComoEnviado(ctx);
            });

            EmailActionContainer.AddFailCallback("envBoletoAuto", ctx => {

                NotificarBoletoNaoEnviado(ctx);
            });
        }

        public IList<ParcelasDTO> CarregarItensAvulsos(string _ASN_NUM_ASSINATURA, string _CLI_NOME)
        {
            return _dao.CarregarItensAvulsos(_ASN_NUM_ASSINATURA, _CLI_NOME);
        }

        public IList<ParcelasDTO> ListarParcelasContrato(string codContrato)
        {
            return _dao.ListarParcelasContrato(codContrato);
        }
        public IList<BoletosAlocadosDTO> ListarTitulosParaAlocacao(int _emp_id)
        {
            return _dao.ListarTitulosParaAlocacao(_emp_id);
        }
        public Pagina<ParcelasDTO> ListarTitulosParaAlocacaoDet(int _emp_id, int pagina = 1, int numpaginas = 12)
        {
            return _dao.ListarTitulosParaAlocacaoDet(_emp_id, pagina, numpaginas);
        }

        public IList<ParcelasAtrasoCustomDTO> ListarDebitoDetalhadamente(string assinatura, int cliente)
        {
            return _dao.ListarDebitoDetalhadamente(assinatura, cliente);

        }

        public IList<ParcelasAtrasoCustomDTO> ListarDebitoDetalhado(string assinatura, int _cli_id)
        {
            return _dao.ListarTitulosVencidos(_cli_id);
        }

        public IList<ParcelasAtrasoCustomDTO> ListarTitulosVencidos(int _cli_id)
        {
            return _dao.ListarTitulosVencidos(_cli_id);
        }


        public IList<BoletosAlocadosDTO> ListarTitulosAlocados(DateTime _dtini, DateTime _dtfim, string _banid = null)
        {
            return _dao.ListarTitulosAlocados(_dtini, _dtfim, _banid);
        }

        public IList<ParcelasNegociacaoCustomDTO> ListarNegociacaoAtraso(Nullable<decimal> _valor, int _qtdeParcelas)
        {
            return _dao.ListarNegociacaoAtraso(_valor, _qtdeParcelas);
        }

        public ParcelasConciliacaoRemTotalDTO BuscarAuditoriaRetorno(int? _cnq_id
                                                                     , int? _rem_id
                                                                     , string _parNumParcela
                                                                     , string _parNossoNumero
                                                                     , string _ban_id
                                                                     , string _oct_codigo
                                                                     , int _ipe_id = 0
                                                                     , int _ppi_id = 0
                                                                     , string _cnqnome = null
                                                                     , int _pagina = 1)
        {
            return _dao.BuscarAuditoriaRetorno(_cnq_id, _rem_id, _parNumParcela, _parNossoNumero, _ban_id, _oct_codigo, _ipe_id, _ppi_id, _cnqnome, _pagina);
        }

        public void BaixaManual(ParcelasDTO _parcela)
        {

            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {

                ParcelaLiquidacaoDTO liquidacao = new ParcelaLiquidacaoDTO();

                liquidacao.PAR_NUM_PARCELA = _parcela.PAR_NUM_PARCELA;
                liquidacao.BAN_ID = _parcela.BAN_ID;
                liquidacao.PLI_DATA = _parcela.PAR_DATA_PAGTO;
                liquidacao.PLI_DATA_BAIXA = _parcela.PAR_DATA_BAIXA;
                liquidacao.PLI_DATA_BORDERO = _parcela.PAR_DATA_PAGTO;
                liquidacao.PLI_NUMERO = _parcela.PAR_NUM_PARCELA;
                liquidacao.PLI_ORIGEM_PGTO = "3";
                liquidacao.PLI_TIPO_DOC = _parcela.PAR_TIPO_DOC;
                liquidacao.PLI_VALOR = _parcela.PAR_VLR_PAGO;

                _serviceLiquidacao.Save(liquidacao);

                LiquidacaoLegadoDTO liqLegado = new LiquidacaoLegadoDTO();

                DateTime dtBaixa = (DateTime)liquidacao.PLI_DATA_BAIXA;
                DateTime dt = (DateTime)liquidacao.PLI_DATA;
                DateTime dtBordero = (DateTime)liquidacao.PLI_DATA_BORDERO;

                liqLegado.CONTRATO = _parcela.CTR_NUM_CONTRATO;
                liqLegado.LETRA = liquidacao.PAR_NUM_PARCELA.Substring(6, 1);
                liqLegado.CD = liquidacao.PAR_NUM_PARCELA.Substring(7, 1);
                liqLegado.BANCO = liquidacao.BAN_ID;
                liqLegado.DT_BORDERO = dtBordero.ToString("dd/MM/yyyy");
                liqLegado.IDENT_DOCTO = liquidacao.PAR_NUM_PARCELA;
                liqLegado.DATA_DA_BAIXA = dtBaixa.ToString("dd/MM/yyyy");
                liqLegado.DATA = dt.ToString("dd/MM/yyyy");
                liqLegado.NUMERO = liquidacao.PLI_NUMERO;
                liqLegado.ORIGEM_PGTO = liquidacao.PLI_ORIGEM_PGTO;
                liqLegado.TIPO_DOC = liquidacao.PLI_TIPO_DOC;
                liqLegado.VALOR = liquidacao.PLI_VALOR.ToString().Replace('.', ',');

                _serviceLiquidacaoLegado.Save(liqLegado);

                ParcelasLegadoUpdateDTO _parLegado = new ParcelasLegadoUpdateDTO();

                var parLegado = this.BuscarLegado(_parcela.PAR_NUM_PARCELA);
                if (parLegado != null)
                {
                    parLegado.DT_PAGTO = dtBaixa.ToString("dd/MM/yyyy");
                    parLegado.VLR_PAGO = liquidacao.PLI_VALOR.ToString().Replace('.', ',');
                    _serviceParcelasLegado.Merge(parLegado);
                }

                _parcela.PAR_DATA_PAGTO = DateTime.Now;
                _parcela.PAR_BAIXA_MANUAL = true;

                this.Merge(_parcela);

                scope.Complete();
            }
        }
        public IList<ParcelasDTO> buscarPorNossoNumero(string nn)
        {
            return _dao.buscarPorNossoNumero(nn);
        }
        public Pagina<ParcelasRetornoCustomDTO> BuscarparcelasRetorno(int? _cnq_id, string _parNumParcela, int pagina = 1, int numpaginas = 12)
        {
            return _dao.BuscarparcelasRetorno(_cnq_id, _parNumParcela, pagina, numpaginas);
        }

        public Pagina<ParcelasRetornoCustomDTO> BuscarparcelasRetorno(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int numpaginas = 12)
        {
            return _dao.BuscarparcelasRetorno(_data_ini, _data_fim, _ban_id, _nome, pagina, numpaginas);
        }

        public IList<ParcelasDTO> buscarParcelaPorParte(string titulo)
        {
            return _dao.buscarParcelaPorParte(titulo);
        }

        // ALT: 07/08/2017 - titulos impagos da assinatura
        public IList<ParcelasBoletoDTO> obterTitulosDaAssinatura(string idAssinatura)
        {
            return _dao.obterTitulosDaAssinatura(idAssinatura);
        }

        public IQueryable<ParcelasConciliacaoRemDTO> BuscarParcelasRemessa(int _rem_id)
        {
            return _dao.BuscarParcelasRemessa(_rem_id);
        }

        public Pagina<ParcelasConciliacaoRemDTO> BuscarParcelasRemessa(int _rem_id, int _pagina, int _numpaginas)
        {
            return _dao.BuscarParcelasRemessa(_rem_id, _pagina, _numpaginas);
        }
        public Pagina<ParcelasAtrasoCobrancaDTO> BuscarTitulosAtrasoCobranca(string assinatura
                                                                           , string cnpj
                                                                           , DateTime? dataini = null
                                                                           , DateTime? datafim = null
                                                                           , int atrasoini = 7
                                                                           , int atrasofim = 90
                                                                           , bool todos = false
                                                                           , int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.BuscarTitulosAtrasoCobranca(assinatura, cnpj, dataini, datafim, atrasoini, atrasofim, todos, pagina, registroPorPagina);
        }

        /// <summary>
        /// ALT: 07/07/2017 - retornar sempre o cliente exato do título informado
        /// </summary>
        /// <param name="idTitulo"></param>
        /// <returns></returns>
        public ClienteDto RetornarClienteDaParcela(string idTitulo)
        {
            var parcela = FindById(idTitulo);
            if (parcela != null && parcela.EMP_ID != null)
            {
                int idCliente = 0;

                if (parcela.CTR_NUM_CONTRATO != null) // cliente com contrato/assinatura
                {
                    var assinatura = new AssinaturaSRV().BuscarAssinaturaPorContrato(parcela.CTR_NUM_CONTRATO);
                    if (assinatura != null)
                    {
                        idCliente = (int)assinatura.CLI_ID;
                    }
                }
                else if (parcela.IPE_ID != null) // cliente com pedido
                {
                    var itemPedido = new ItemPedidoSRV().FindById(parcela.IPE_ID);
                    if (itemPedido != null)
                    {
                        var pedidoCRM = new PedidoCRMSRV().FindById(itemPedido.PED_CRM_ID);
                        if (pedidoCRM != null)
                        {
                            idCliente = (int)pedidoCRM.CLI_ID;
                        }
                    }
                }
                else if (parcela.PPI_ID != null) // cliente com proposta
                {
                    var propostaItem = new PropostaItemSRV().FindById(parcela.PPI_ID);
                    if (propostaItem != null)
                    {
                        var proposta = new PropostaSRV().FindById(propostaItem.PRT_ID);
                        if (proposta != null)
                        {
                            idCliente = (int)proposta.CLI_ID;
                        }
                    }
                }

                if (idCliente > 0)
                {
                    var cliente = new ClienteSRV().FindById(idCliente);
                    if (cliente != null)
                    {
                        return cliente;
                    }
                }
            }

            return null;
        }

        public BradescoCobrancaResponse RegistrarBoleto(string _par_num_parcela, DateTime dtVencimento, Decimal vlrBoleto, string email, int? repId = null)
        {
            try
            {

                var titulo = ServiceFactory.RetornarServico<ParcelasSRV>().FindById(_par_num_parcela);
                var conta = new ContaSRV().BuscarContaBoletoAvulso((int)titulo.EMP_ID, "237");

                if (conta == null)
                {
                    throw new Exception($"Não é possível registrar o boleto. A conta para boleto avulso para a empresa {titulo.EMP_ID} não foi encontrado");
                }
                var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(conta.EMP_ID);
                var cliente = ServiceFactory.RetornarServico<ClienteSRV>().BuscarClientePorBoleto(_par_num_parcela);
                var endereco = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);

                //-------------
                var avalista = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(2);
                var munavalista = ServiceFactory.RetornarServico<MunicipioSRV>().FindById(avalista.CID_ID);
                var cepavalista = avalista.EMP_CEP.Substring(0, 5);
                var cepdigavalista = avalista.EMP_CEP.Substring(5, 3);
                //-------------

                if (String.IsNullOrWhiteSpace(cliente.CNPJ_CPF))
                    throw new Exception("CPF/CNPJ não informado!!");

                if (endereco == null)
                    throw new Exception("Cliente sem endereço");

                var _registrar = new RegistrarBoletoSRV();
                var _boleto = new BradescoCobrancaRequest();
                var _cep = endereco.END_CEP.Substring(0, 5);
                var _cepdig = endereco.END_CEP.Substring(5, 3);
                var _cnpj = empresa.EMP_CNPJ.Substring(0, 8);
                var _cnpjfilial = empresa.EMP_CNPJ.Substring(8, 4);
                var _digcnpj = empresa.EMP_CNPJ.Substring(12, 2);
                var _agencia = conta.CTA_AGENCIA.PadLeft(4, '0');
                var _zeros = "0000000";
                var _conta = StringUtil.RetirarCaractereEspecial(conta.CTA_CONTA, true);
                _conta = _conta.Substring(0, _conta.Length - 1).PadLeft(7, '0');

                var _dtemissao = DateTime.Now.Day.ToString().PadLeft(2, '0') + "." +
                                 DateTime.Now.Month.ToString().PadLeft(2, '0') + "." +
                                 DateTime.Now.Year.ToString();
                var _dtvencto = dtVencimento.Day.ToString().PadLeft(2, '0') + "." +
                                 dtVencimento.Month.ToString().PadLeft(2, '0') + "." +
                                 dtVencimento.Year.ToString();

                var _vlrTirulo = 0;

                if (vlrBoleto > 0)
                    _vlrTirulo = (int)(vlrBoleto * 100);
                else
                    _vlrTirulo = (int)(titulo.PAR_VLR_PARCELA * 100);

                //var _nossonumero = ServiceFactory.RetornarServico<BoletoSRV>().GerarNossoNumero(conta.BAN_ID, titulo.PAR_NUM_PARCELA, true, true);

                _boleto.idProduto = conta.CTA_CARTEIRA_BOLETO;   ///  Código da carteira utilizada.  
				_boleto.nuCPFCNPJ = _cnpj.ToString();
                _boleto.filialCPFCNPJ = _cnpjfilial.ToString();
                _boleto.ctrlCPFCNPJ = _digcnpj.ToString();
                _boleto.nuNegociacao = _agencia + _zeros + _conta;

                _boleto.nuTitulo = "0"; // _nossonumero;
                _boleto.nuCliente = titulo.PAR_NUM_PARCELA;
                _boleto.dtEmissaoTitulo = _dtemissao;
                _boleto.dtVencimentoTitulo = _dtvencto;
                _boleto.vlNominalTitulo = _vlrTirulo.ToString();
                _boleto.cdEspecieTitulo = "04";

                _boleto.nomePagador = cliente.CLI_NOME;
                _boleto.logradouroPagador = endereco.END_LOGRADOURO;
                _boleto.nuLogradouroPagador = endereco.END_NUMERO;
                _boleto.cepPagador = _cep.ToString();
                _boleto.complementoCepPagador = _cepdig.ToString();
                _boleto.bairroPagador = endereco.END_BAIRRO;
                _boleto.municipioPagador = endereco.MUNICIPIO.MUN_DESCRICAO;
                _boleto.ufPagador = endereco.END_UF;
                _boleto.cdIndCpfcnpjPagador = (cliente.CNPJ_CPF.ToString().Length == 11) ? "1" : "2";
                _boleto.nuCpfcnpjPagador = cliente.CNPJ_CPF.ToString().PadLeft(14, '0');

                //if (conta.BAN_ID == "999")
                //{
                //    _boleto.nomeSacadorAvalista = avalista.EMP_RAZAO_SOCIAL;
                //    _boleto.logradouroSacadorAvalista = avalista.EMP_LOGRADOURO;
                //    _boleto.nuLogradouroSacadorAvalista = avalista.EMP_NUMERO;
                //    _boleto.cepSacadorAvalista = cepavalista.ToString();
                //    _boleto.complementoCepSacadorAvalista = cepdigavalista.ToString();
                //    _boleto.bairroSacadorAvalista = avalista.EMP_BAIRRO;
                //    _boleto.municipioSacadorAvalista = munavalista.MUN_DESCRICAO;
                //    _boleto.ufSacadorAvalista = munavalista.UF;
                //    _boleto.cdIndCpfcnpjSacadorAvalista = (avalista.EMP_CNPJ.ToString().Length == 11) ? "1" : "2";
                //    _boleto.nuCpfcnpjSacadorAvalista = avalista.EMP_CNPJ.ToString().PadLeft(14, '0');
                //}


                var _jsontxt = JsonConvert.SerializeObject(_boleto);

                var a = (BradescoCobrancaResponse)_registrar.Request(false, _boleto, "Post", "", typeof(BradescoCobrancaResponse));

                if (a.cdErro != "" && a.cdErro != "0")
                    throw new Exception(a.msgErro);

                titulo.PAR_NOSSO_NUMERO = a.nuTituloGerado;
                titulo.PAR_VENC_BOLETO = dtVencimento;
                titulo.PAR_VLR_BOLETO = vlrBoleto;
                titulo.CTA_ID = conta.CTA_ID;
                titulo.BAN_ID = "237";
                titulo.PAR_DATA_ALOC = DateTime.Now;
                titulo.PAR_ALOC_AUTOMATICA = true;
                titulo.EMP_ID = conta.EMP_ID;

                this.Merge(titulo);

                var _histAtend = new HistoricoAtendimentoDTO();

                _histAtend.CLI_ID = cliente.CLI_ID;
                _histAtend.HAT_DESCRICAO = "Boleto nº " + titulo.PAR_NUM_PARCELA + " Empresa " + titulo.EMP_ID.ToString() + " Venc: " + dtVencimento.ToString("dd/MM/yyyy") +
                                           " Valor: " + vlrBoleto.ToString() + " registrado e enviado para o email " + email;
                _histAtend.HAT_DATA_HIST = DateTime.Now;
                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_SOLICITANTE = SessionContext.autenticado.USU_LOGIN;
                _histAtend.UEN_ID = 3;

                new HistAtendSRV().Save(_histAtend);

                return a;

            }
            catch (Exception e)
            {

                throw new Exception(string.Format("Erro ao registrar boleto."), e);

            }
        }

        public void EnviarBoletoAvulso(string idTitulo, DateTime dtVencimento, Decimal vlrBoleto, int idConta, string email, int? repId = null)
        {
            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {

                this.RegistrarBoleto(idTitulo, dtVencimento, vlrBoleto, email, repId);

                var parcela = FindById(idTitulo);

                if (parcela != null)
                {
                    parcela.PAR_BOLETO_AVULSO = true;
                    Merge(parcela);
                }

                var lstEmail = new List<EmailDTO>() {

                    new EmailDTO(){ Email = email}

                };

                this.EnviarEmailComBoleto(lstEmail, idTitulo, "emailPropostaBoleto", repId);

                scope.Complete();
            }



        }

        public void EnviarEmailComBoleto(List<EmailDTO> lstEmails = null, string numeroParcela = null, string actionName = null, int? repId = null)
        {
            if (lstEmails != null)
            {
                string url = null;

                var templateEmail = @"<div id=':lr' class='a3s aXjCH m15cf5173643ab5e2'>
                            Prezado(a) {0},<br><br>
                                Realize o pagamento utilizando o boleto bancário em anexo.
                                <br><br>

                                * Caso algum dos dados acima esteja incorreto, favor entrar em contato conosco através do e-mail <a href='mailto:comercial@coad.com.br' target='_blank'>comercial@coad.com.br</a><br></div><div class='gmail_default' style='font-family:tahoma,sans-serif'><br></div><div class='gmail_default' style='font-family:tahoma,sans-serif'><br></div><div class='gmail_default' style='font-family:tahoma,sans-serif'><br></div><div class='gmail_default' style='font-family:tahoma,sans-serif'><br></div><div class='gmail_quote'><div dir='ltr'><div><div><div><div><div><div><div class='gmail_quote'><br></div><br></div></div></div></div></div></div></div>

                            <br>
                            <font face='Arial' size='1'>* Mantenha em segurança sua senha de email.</font>
                            <font face='Arial' size='1'> Não forneça dados indevidamente.&nbsp;</font>
                            <div><font face='Arial' size='1'>
                            * Qualquer dúvida ou anormalidade comunique ao administrador de sua conta.</font>
                            </div>
                            </div>";

                var _cli = this.RetornarClienteDaParcela(numeroParcela);
                if (_cli != null)
                {
                    string nomeCliente = _cli.CLI_NOME;
                    templateEmail = string.Format(templateEmail, nomeCliente);

                    string emailCC = ServiceFactory.RetornarServico<RepresentanteSRV>().RetornarEmailCCRepresentante(repId);
                    IEmailSRV _enviarEMAIL = ServiceFactory.RetornarServico<IEmailSRV>();

                    foreach (var email in lstEmails)
                    {
                        email.Email = SysUtils.DecidirEnderecoDeEmail(email.Email);
                        _enviarEMAIL.EnviarEmailParaCliente(email.Email, "Efetue o pagamento do seu boleto COAD", templateEmail, url, actionName, numeroParcela, emailCC, 3);
                    }
                }
            }
        }

        //----------Novo Processo de Baixa 

        public void ProcessarRetorno(int _cnq_id)
        {
            _dao.ProcessarRetorno(_cnq_id);

        }

        public void ProcessarArquivoRetornoNovo(HttpPostedFileBase _conteudo, string nomeArquivo, StreamReader arquivo = null)
        {

            CnabArquivosDTO _arq = new CnabArquivosDTO();
            var _arqlinhasErro = new List<CnabArquivosItemErroDTO>();
            var _listaparcelas = new List<ParcelasDTO>();

            string arq = arquivo.ReadToEnd();
            string[] linhas = arq.Split(new char[] { '\n' });

            Stream fs = _conteudo.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytesArquivo = br.ReadBytes((Int32)fs.Length);

            int linhaTrailler = linhas.Count() - 1;

            string ArquivoRetorno = "02";
            string nrTituloAqui = "";
            string nrTituloBco = "";
            string codRetorno;
            var banco = "";
            var cta_id = 0;
            var bancoPesquisa = "";

            bool arquivoSeraProcessado = false;

            int ln = 1;

            String nrTitulo;
            ParcelasDTO _parcela = null;

            String valorprincipal = "";
            String datavencimento = "";

            foreach (string linha in linhas)
            {
                if (!String.IsNullOrWhiteSpace(linha))
                {
                    if (ln == 1)
                    {

                        if ((linha.Length - 1 != 400) && (linha.Length - 1 != 240))
                            throw new Exception("Arquivo com largura inválida");

                        if (linha.Length - 1 == 400)
                        {
                            arquivoSeraProcessado = (ArquivoRetorno == linha.Substring(0, 2));

                            if (!arquivoSeraProcessado)
                                throw new Exception("Arquivo com formato incorreto. Verifique!!");

                            banco = linha.Substring(76, 3);

                            bancoPesquisa = banco;

                            if ((banco == "999") || (banco == "998"))
                                bancoPesquisa = "237";

                            var agencia = linha.Substring(26, 4);
                            var conta = linha.Substring(30, 8);

                            var contapesquisa = _srvConta.Listar(banco).FirstOrDefault();

                            if (contapesquisa == null)
                                throw new Exception("Conta não encontrada para o banco (" + banco + "). Verifique !! ");

                            cta_id = (int)contapesquisa.CTA_ID;

                            var dtCreditoHSBC = "";

                            if (banco == "399")
                                dtCreditoHSBC = "20" + linha.Substring(119, 2) + "/" + linha.Substring(121, 2) + "/" + linha.Substring(123, 2);

                            DateTime? dataarq = DateTime.Now;

                            var _dtvalida = linha.Substring(94, 6);

                            if (_dtvalida != "000000" &&
                                _dtvalida.Trim() != null)
                            {
                                var dataarquivo = "20" + linha.Substring(98, 2) + "/" + linha.Substring(96, 2) + "/" + linha.Substring(94, 2);
                                dataarq = Convert.ToDateTime(dataarquivo);
                            }


                            var TipoArquivo = linha.Substring(0, 9);
                            var CodigoCedente = linha.Substring(110, 7);
                            var EmpresaCedente = linha.Substring(46, 30);
                            var linha01 = linha;


                            //-------------------------------------------

                            EmpresaSRV _emp = new EmpresaSRV();

                            var emp = _emp.FindById(2);

                            if (emp == null)
                                throw new Exception("Empresa não encontrada. Verifique!!");

                            _arq.EMP_ID = emp.EMP_ID;
                            _arq.BAN_ID = banco;
                            _arq.CTA_ID = (int)cta_id;
                            _arq.CNQ_QTD_LINHAS = linhaTrailler;
                            _arq.CNQ_DATA_ARQUIVO = (DateTime)dataarq;
                            _arq.CNQ_NOME = nomeArquivo;
                            _arq.CNQ_DATA_LIDO = DateTime.Now;
                            _arq.CNQ_ARQUIVO = bytesArquivo;
                            _arq.USU_LOGIN = SessionContext.login;
                            _arq.DATA_CADASTRO = DateTime.Now;
                            _arq.CNQ_COD_CEDENTE = CodigoCedente;
                            _arq.CNQ_EMP_CEDENTE = EmpresaCedente;
                            _arq.CNQ_TIPO_ARQUIVO = TipoArquivo;
                            _arq.CNQ_LINHA_ARQUIVO = linha.Trim();

                            //-------------------------------------------
                        }
                        else
                        {

                            linhaTrailler = linhas.Count() - 2;

                            banco = linha.Substring(0, 3);

                            bancoPesquisa = banco;

                            var agencia = linha.Substring(53, 4);
                            var conta = linha.Substring(59, 12);

                            var contapesquisa = _srvConta.RetornarContaPorAgenciaEConta( int.Parse(agencia), int.Parse(conta));

                            if (contapesquisa == null)
                                throw new Exception("Conta não encontrada para o banco (" + banco + "). Verifique !! ");

                            cta_id = (int)contapesquisa.CTA_ID;

                            DateTime? dataarq = DateTime.Now;

                            var _dtvalida = linha.Substring(143, 8);

                            if (_dtvalida != "00000000" &&
                                _dtvalida.Trim() != null)
                            {
                                var dataarquivo = _dtvalida.Substring(4, 4) + "/" + _dtvalida.Substring(2, 2) + "/" + _dtvalida.Substring(0, 2);
                                dataarq = Convert.ToDateTime(dataarquivo);
                            }


                            var TipoArquivo = "02RETORNO";
                            var CodigoCedente = (int.Parse(conta)).ToString(); //linha.Substring(52, 19); // agencia + (int.Parse(conta)).ToString() ou banco
                            var EmpresaCedente = linha.Substring(72, 30);
                            var linha01 = linha;

                            //-------------------------------------------

                            EmpresaSRV _emp = new EmpresaSRV();

                            var emp = _emp.BuscarPorCNPJ(linha.Substring(18, 14));

                            if (emp == null)
                                throw new Exception("Empresa não encontrada. Verifique!!");

                            _arq.EMP_ID = emp.EMP_ID;
                            _arq.BAN_ID = banco;
                            _arq.CTA_ID = (int)cta_id;
                            _arq.CNQ_QTD_LINHAS = linhaTrailler;
                            _arq.CNQ_DATA_ARQUIVO = (DateTime)dataarq;
                            _arq.CNQ_NOME = nomeArquivo;
                            _arq.CNQ_DATA_LIDO = DateTime.Now;
                            _arq.CNQ_ARQUIVO = bytesArquivo;
                            _arq.USU_LOGIN = SessionContext.login;
                            _arq.DATA_CADASTRO = DateTime.Now;
                            _arq.CNQ_COD_CEDENTE = CodigoCedente;
                            _arq.CNQ_EMP_CEDENTE = EmpresaCedente;
                            _arq.CNQ_TIPO_ARQUIVO = TipoArquivo;
                            _arq.CNQ_LINHA_ARQUIVO = linha.Trim();

                            //-------------------------------------------

                        }

                    }
                    else
                    if (ln != linhaTrailler)
                    {

                        if (linha.Length - 1 == 400)
                        {

                            //var nrTitulo = banco == "104" ? linha.Substring(31, 25).Trim() : linha.Substring(37, 25).Trim();
                            //nrTituloBco = banco == "237" ? linha.Substring(70, 12).Trim() : linha.Substring(62, 11).Trim();
                            nrTitulo = "";
                            nrTituloBco = "";


                            // 001 - Banco do Brasil 
                            // 033 - Santander 
                            // 041 - Banrisul 
                            // 104 - CEF
                            // 237 - Bradesco
                            // 341 - ITAU
                            // 604 - Banco Industrial

                            // 399 - HSBC 


                            switch (banco)
                            {
                                case "033":
                                    nrTitulo = linha.Substring(37, 25).Trim();
                                    nrTituloBco = linha.Substring(62, 08).Trim();
                                    if (nrTituloBco == "00000000")
                                        nrTituloBco = "";
                                    break;
                                case "041":
                                    nrTitulo = linha.Substring(37, 25).Trim();
                                    nrTituloBco = linha.Substring(62, 11).Trim();
                                    break;
                                case "104":
                                    nrTitulo = linha.Substring(31, 25).Trim();
                                    nrTituloBco = linha.Substring(56, 18).Trim();
                                    if (nrTituloBco == "00000000000000000")
                                        nrTituloBco = "";
                                    break;
                                case "237":
                                    nrTitulo = linha.Substring(116, 10).Trim();
                                    nrTituloBco = linha.Substring(70, 12).Trim();
                                    break;
                                case "341":
                                    nrTitulo = linha.Substring(116, 10).Trim();
                                    nrTituloBco = linha.Substring(86, 9).Trim();
                                    break;
                                case "399":
                                    nrTitulo = linha.Substring(116, 10).Trim();
                                    nrTituloBco = linha.Substring(126, 11).Trim();
                                    break;
                                case "422":
                                    nrTitulo = linha.Substring(116, 10).Trim();
                                    nrTituloBco = linha.Substring(126, 11).Trim();
                                    break;
                                default:
                                    nrTitulo = linha.Substring(37, 25).Trim();
                                    nrTituloBco = linha.Substring(62, 11).Trim();
                                    break;
                            }


                            //if (banco == "237" && String.IsNullOrWhiteSpace(nrTitulo))
                            //{
                            //    string letra = (Convert.ToChar(Convert.ToInt32(linha.Substring(78, 2)))).ToString();
                            //    string dv = linha.Substring(80, 1);
                            //    nrTitulo = linha.Substring(72, 6) + letra + dv;
                            //}

                            if (nrTitulo.Length >= 8)
                            {
                                nrTituloAqui = nrTitulo.Substring(0, 8);

                                if (nrTituloAqui == "00000000" && banco == "237")
                                    nrTituloAqui = nrTitulo.Substring(17, 8);

                            }
                            else
                                nrTituloAqui = nrTitulo;

                            if (banco == "041")
                            {
                                if (nrTituloAqui.Length < 8)
                                    nrTituloAqui += linha.Substring(125, 1).Trim();
                            }

                            _parcela = this.FindById(nrTituloAqui);

                            if (_parcela != null)
                            {

                                valorprincipal = linha.Substring(152, 11) + "." + linha.Substring(163, 2);
                                var vlPago = Convert.ToDecimal(linha.Substring(253, 11) + "." + linha.Substring(264, 2));
                                decimal juros = 0;

                                // juros \\
                                if (banco != "041")
                                {
                                    if (vlPago > 0)
                                        juros = vlPago - Convert.ToDecimal(valorprincipal);
                                }
                                else
                                {
                                    juros = Convert.ToDecimal(linha.Substring(266, 11) + "." + linha.Substring(277, 2));
                                }

                                codRetorno = linha.Substring(108, 2);

                                if (bancoPesquisa == "399")
                                    bancoPesquisa = "237";

                                var ocor = _serviceOcorrenciaRetorno.LerOcorrenciaRetorno(bancoPesquisa, codRetorno).lista.FirstOrDefault();

                                DateTime? dtPgto = null;

                                var _dtvalida = linha.Substring(110, 6);

                                if (_dtvalida != "000000" &&
                                    _dtvalida.Trim() != null)
                                {
                                    var datapgto = "20" + linha.Substring(114, 2) + "-" + linha.Substring(112, 2) + "-" + linha.Substring(110, 2);
                                    dtPgto = Convert.ToDateTime(datapgto);
                                }

                                var deveBaixar = false;
                                var deveDesAlocar = false;
                                var entradaConfirmada = false;

                                if (ocor != null)
                                {
                                    deveBaixar = ocor.OCT_BAIXAR_TITULO; /// 1
                                    deveDesAlocar = ocor.OCT_DESALOCAR_TITULO;  // 2
                                    entradaConfirmada = ocor.OCT_REGISTRAR_TITULO; // 3
                                }

                                var dataocorrencia = linha.Substring(110, 2) + "-" + linha.Substring(112, 2) + "-20" + linha.Substring(114, 2);
                                datavencimento = linha.Substring(146, 2) + "-" + linha.Substring(148, 2) + "-20" + linha.Substring(150, 2);
                                var valortitulo = string.Format("{0:0,0.00}", Convert.ToDecimal(valorprincipal));
                                var valorpago = string.Format("{0:0,0.00}", vlPago + juros);

                                //-------------------------------------------
                                CnabArquivosItemDTO _itemret = new CnabArquivosItemDTO();

                                _itemret.CNI_DATA_PAGTO = dtPgto;
                                _itemret.CNI_VLR_JUROS = juros;
                                _itemret.CNI_VLR_PAGO = vlPago;
                                _itemret.CNI_VLR_PARCELA = Convert.ToDecimal(valorprincipal);
                                _itemret.PAR_NUM_PARCELA = nrTituloAqui;
                                _itemret.PAR_NOSSO_NUMERO = nrTituloBco;
                                _itemret.BAN_ID = _arq.BAN_ID;
                                _itemret.CTA_ID = _arq.CTA_ID;
                                _itemret.OCT_CODIGO = ocor.OCT_CODIGO;
                                _itemret.CNI_LINHA_ARQUIVO = linha.Trim();

                                if (deveBaixar)
                                    _itemret.CNI_ACAO = 1;
                                if (deveDesAlocar)
                                    _itemret.CNI_ACAO = 2;
                                if (entradaConfirmada)
                                    _itemret.CNI_ACAO = 3;

                                /*
                                if (deveBaixar)
                                {

                                    _parcela.PAR_DATA_PAGTO = dtPgto;
                                    _parcela.PAR_VLR_PAGO = vlPago + juros;

                                }
							    */

                                _arq.CNAB_ARQUIVOS_ITEM.Add(_itemret);

                                //-------------------------------------------
                                _parcela.PAR_NOSSO_NUMERO = nrTituloBco;

                                _listaparcelas.Add(_parcela);
                                //-------------------------------------------
                            }
                            else
                            {
                                var _msgerro = "";
                                if (_parcela == null)
                                    _msgerro = "Parcela não localizada no banco de dados";

                                if (String.IsNullOrWhiteSpace(nrTituloAqui))
                                    _msgerro = "Numero da parcela não informado no arquivo";

                                var _erro = new CnabArquivosItemErroDTO();

                                _erro.CNE_NUM_LINHA = ln.ToString();
                                _erro.CNE_LINHA_ERRO = linha.Trim();
                                _erro.CNE_NUM_PARCELA = nrTituloAqui;
                                _erro.CNE_ERRO = _msgerro;

                                _arqlinhasErro.Add(_erro);

                            }

                        }
                        else 
                        {

                            if (linha.Substring(13, 1) == "T")
                            {

                                nrTituloBco = "";

                                // 756 - SICOOB

                                switch (banco)
                                {
                                    case "756":
                                        nrTitulo = linha.Substring(58, 15).Trim();
                                        nrTituloBco = linha.Substring(37, 10).Trim();
                                        if (nrTituloBco == "00000000")
                                            nrTituloBco = "";
                                        break;
                                    default:
                                        nrTitulo = linha.Substring(58, 15).Trim();
                                        nrTituloBco = linha.Substring(37, 10).Trim();
                                        break;
                                }

                                if (nrTitulo.Length >= 8)
                                    nrTituloAqui = nrTitulo.Substring(0, 8);
                                else
                                    nrTituloAqui = nrTitulo;

                                _parcela = this.FindById(nrTituloAqui);

                                valorprincipal = linha.Substring(81, 13) + "." + linha.Substring(94, 2);

                                datavencimento = linha.Substring(73, 2) + "-" + linha.Substring(75, 2) + "-" + linha.Substring(77, 2);

                            }

                            if (linha.Substring(13, 1) == "U")
                                if(_parcela != null)
                                {

                                    var vlPago = Convert.ToDecimal(linha.Substring(77, 13) + "." + linha.Substring(90, 2));
                                    decimal juros = 0;

                                    // juros \\
                                    if (vlPago > 0)
                                        juros = vlPago - Convert.ToDecimal(valorprincipal);

                                    codRetorno = linha.Substring(15, 2);

                                    var ocor = _serviceOcorrenciaRetorno.LerOcorrenciaRetorno(bancoPesquisa, codRetorno).lista.FirstOrDefault();

                                    DateTime? dtPgto = null;

                                    var _dtvalida = linha.Substring(145, 8);

                                    if (_dtvalida != "00000000" &&
                                        _dtvalida.Trim() != null)
                                    {
                                        var datapgto = linha.Substring(141, 4) + "-" + linha.Substring(139, 2) + "-" + linha.Substring(137, 2);
                                        dtPgto = Convert.ToDateTime(datapgto);
                                    }

                                    var deveBaixar = false;
                                    var deveDesAlocar = false;
                                    var entradaConfirmada = false;

                                    if (ocor != null)
                                    {
                                        deveBaixar = ocor.OCT_BAIXAR_TITULO; /// 1
                                        deveDesAlocar = ocor.OCT_DESALOCAR_TITULO;  // 2
                                        entradaConfirmada = ocor.OCT_REGISTRAR_TITULO; // 3
                                    }

                                    var dataocorrencia = linha.Substring(137, 2) + "-" + linha.Substring(139, 2) + "-" + linha.Substring(141, 4);

                                    var valortitulo = string.Format("{0:0,0.00}", Convert.ToDecimal(valorprincipal));
                                    var valorpago = string.Format("{0:0,0.00}", vlPago + juros);

                                    //-------------------------------------------
                                    CnabArquivosItemDTO _itemret = new CnabArquivosItemDTO();

                                    _itemret.CNI_DATA_PAGTO = dtPgto;
                                    _itemret.CNI_VLR_JUROS = juros;
                                    _itemret.CNI_VLR_PAGO = vlPago;
                                    _itemret.CNI_VLR_PARCELA = Convert.ToDecimal(valorprincipal);
                                    _itemret.PAR_NUM_PARCELA = nrTituloAqui;
                                    _itemret.PAR_NOSSO_NUMERO = nrTituloBco;
                                    _itemret.BAN_ID = _arq.BAN_ID;
                                    _itemret.CTA_ID = _arq.CTA_ID;
                                    _itemret.OCT_CODIGO = ocor.OCT_CODIGO;
                                    _itemret.CNI_LINHA_ARQUIVO = linha.Trim();

                                    if (deveBaixar)
                                        _itemret.CNI_ACAO = 1;
                                    if (deveDesAlocar)
                                        _itemret.CNI_ACAO = 2;
                                    if (entradaConfirmada)
                                        _itemret.CNI_ACAO = 3;

                                    _arq.CNAB_ARQUIVOS_ITEM.Add(_itemret);

                                    //-------------------------------------------
                                    _parcela.PAR_NOSSO_NUMERO = nrTituloBco;

                                    _listaparcelas.Add(_parcela);
                                    //-------------------------------------------
                                }
                                else
                                {
                                    var _msgerro = "";
                                    if (_parcela == null)
                                        _msgerro = "Parcela não localizada no banco de dados";

                                    if (String.IsNullOrWhiteSpace(nrTituloAqui))
                                        _msgerro = "Numero da parcela não informado no arquivo";

                                    var _erro = new CnabArquivosItemErroDTO();

                                    _erro.CNE_NUM_LINHA = ln.ToString();
                                    _erro.CNE_LINHA_ERRO = linha.Trim();
                                    _erro.CNE_NUM_PARCELA = nrTituloAqui;
                                    _erro.CNE_ERRO = _msgerro;

                                    _arqlinhasErro.Add(_erro);

                                }

                        }

                    }

                }

                ln++;
            }

            //---------

            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {

                var tam = _arq.CNQ_LINHA_ARQUIVO.Trim().Length;

                var _retorno = _srvcnabarq.Save(_arq);

                foreach (var _item in _arq.CNAB_ARQUIVOS_ITEM)
                {
                    _item.CNQ_ID = _retorno.CNQ_ID;
                }

                foreach (var _item in _arqlinhasErro)
                {
                    _item.CNQ_ID = _retorno.CNQ_ID;
                }

                foreach (var _item in _listaparcelas)
                {
                    _item.CNQ_ID = _retorno.CNQ_ID;
                }

                _srvcnabitemarq.SaveAll(_arq.CNAB_ARQUIVOS_ITEM);
                _srvcnabitemarqerro.SaveAll(_arqlinhasErro);
                this.MergeAll(_listaparcelas);

                scope.Complete();
            }


        }
        ////----------Novo Processo de Baixa 



        /// <summary>
        /// Identifica erros, realiza a leitura, o processamento e retorna um HTML do arquivo de retorno.
        /// </summary>
        /// <param name="linhas"></param>
        /// <returns></returns>
        public string ProcessarArquivoRetorno(HttpPostedFileBase _conteudo, string nomeArquivo, StreamReader arquivo = null, bool processar = false)
        {
            List<ParcelasDTO> listaDeParcelas = new List<ParcelasDTO>();
            List<ParcelaLiquidacaoDTO> listaDeLiquidacao = new List<ParcelaLiquidacaoDTO>();
            List<ParcelaAlocadaUpdateDTO> listaDeParcelasAlocadas = new List<ParcelaAlocadaUpdateDTO>();

            // ALT: 15/02/2017 - DTO de Parcelas para realizar UPDATE direto no BANCO.
            List<ParcelaUpdateDTO> listaDeParcelasUpdateDB = new List<ParcelaUpdateDTO>();
            List<ParcelasLegadoUpdateDTO> listaDeParcelasLegadoUpdateDB = new List<ParcelasLegadoUpdateDTO>();

            // ALT: 18/07/2017 - metodo para salvar arquivo CNAB e LOGs...
            CnabArquivosSRV _servicoCnabArquivos = new CnabArquivosSRV(new CnabArquivosDAO());

            string arq = arquivo.ReadToEnd();
            string[] linhas = arq.Split(new char[] { '\n' });

            Stream fs = _conteudo.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytesArquivo = br.ReadBytes((Int32)fs.Length);

            int linhaTrailler = linhas.Count() - 1;

            string cabecalho = "<table class=\"table table-hover table-bordered\" border=\"1\">";
            string corpo = "<table class=\"table table-hover table-bordered\" border=\"1\">";
            string ArquivoRetorno = "02";

            string nrTituloAqui = "";
            string nrTituloBco = "";

            string codRetorno;
            string codErro;
            string retDescricao;

            string cnpjCedente = "";

            int? cta_id = null;

            string banco = "";
            string agencia = "";
            string conta = "";
            string dt = "";
            string dtCreditoHSBC = "";
            string jaProcessado = "0";
            string tempo;

            bool deveBaixar;
            bool deveDesAlocar;
            bool entradaConfirmada;
            bool arquivoSeraProcessado = false;

            DateTime? dtPgto = null;
            DateTime? dtBordero = null;
            DateTime? dtOcorrencia = null;
            DateTime d = DateTime.Now;

            decimal ttPrevisto = 0;
            decimal ttRecebido = 0;
            decimal ttJuros = 0;
            decimal juros = 0;
            decimal vlPago = 0;

            int qtBaixar = 0;
            int qtBaixou = 0;
            int qtDesAlocou = 0;
            int qtRegistrou = 0;

            int ln = 1;

            tempo = d.ToString("hh:mm:ss");

            foreach (string linha in linhas)
            {
                if (!String.IsNullOrWhiteSpace(linha))
                {
                    if (ln == 1)
                    {
                        // header \\
                        banco = linha.Substring(76, 3);
                        agencia = linha.Substring(26, 4);
                        conta = linha.Substring(30, 8);

                        /*
                        if (banco == "237")
                            banco = "999";
                        */

                        if (banco == "399")
                            dtCreditoHSBC = "20" + linha.Substring(119, 2) + "-" + linha.Substring(121, 2) + "-" + linha.Substring(123, 2);

                        dtBordero = null;
                        dt = "20" + linha.Substring(98, 2) + "-" + linha.Substring(96, 2) + "-" + linha.Substring(94, 2);
                        if (dt != "2000-00-00" && dt != "20  -  -  ")
                            dtBordero = Convert.ToDateTime(dt);

                        arquivoSeraProcessado = (ArquivoRetorno == linha.Substring(0, 2));

                        cabecalho += "<tr><td style=\"text-align:right\">Arquivo:</td><td>" + nomeArquivo + "</td></tr>";

                        if (linha.Length - 1 != 400)
                            cabecalho += "<tr><td style=\"text-align:right\">ERRO:</td><td>[header com largura invalida!]</td></tr>";

                        cabecalho += "<tr><td style=\"text-align:right\">Data:</td><td>" + linha.Substring(94, 2) + "-" + linha.Substring(96, 2) + "-20" + linha.Substring(98, 2) + "</td></tr>";
                        cabecalho += "<tr><td style=\"text-align:right\">Tipo do Arquivo:</td><td>" + linha.Substring(0, 9) + "</td></tr>";
                        cabecalho += "<tr><td style=\"text-align:right\">Banco:</td><td>" + linha.Substring(76, 18) + "</td></tr>";
                        if (banco != "237")
                        {
                            cabecalho += "<tr><td style=\"text-align:right\">Agencia:</td><td>" + agencia + "</td></tr>";
                            cabecalho += "<tr><td style=\"text-align:right\">Conta:</td><td>" + conta + "</td></tr>";
                        }
                        cabecalho += "<tr><td style=\"text-align:right\">Codigo Cedente:</td><td>" + linha.Substring(110, 7) + "</td></tr>";
                        cabecalho += "<tr><td style=\"text-align:right\">Empresa Cedente:</td><td>" + linha.Substring(46, 30) + "</td></tr>";
                    }
                    else
                    if (ln != linhaTrailler)
                    {
                        if (banco == "237" && String.IsNullOrWhiteSpace(nrTituloAqui))
                        {
                            agencia = linha.Substring(25, 4);
                            conta = linha.Substring(29, 8);
                            cabecalho += "<tr><td style=\"text-align:right\">Agencia:</td><td>" + agencia + "</td></tr>";
                            cabecalho += "<tr><td style=\"text-align:right\">Conta:</td><td>" + conta + "</td></tr>";
                        }

                        // variáveis de baixa e totalização \\

                        nrTituloAqui = banco == "104" ? linha.Substring(31, 25).Trim() : linha.Substring(37, 25).Trim();
                        nrTituloBco = banco == "237" ? linha.Substring(70, 12).Trim() : linha.Substring(62, 11).Trim();

                        // compatibilidade Banrisul (sistema legado e atual)
                        if (banco == "041")
                        {
                            if (nrTituloAqui.Length < 8)
                                nrTituloAqui += linha.Substring(125, 1).Trim();
                        }

                        // identificando títulos BRADESCO sem registro \\
                        if (banco == "237" && String.IsNullOrWhiteSpace(nrTituloAqui))
                        {
                            string letra = (Convert.ToChar(Convert.ToInt32(linha.Substring(78, 2)))).ToString();
                            string dv = linha.Substring(80, 1);
                            nrTituloAqui = linha.Substring(72, 6) + letra + dv;
                        }

                        // valor principal \\
                        var valor = linha.Substring(152, 11) + "." + linha.Substring(163, 2);

                        // pagamento \\
                        if (!String.IsNullOrWhiteSpace(linha.Substring(253, 11) + linha.Substring(264, 2)))
                            vlPago = Convert.ToDecimal(linha.Substring(253, 11) + "." + linha.Substring(264, 2));

                        // juros \\
                        if (banco != "041") // ALT: 05/02/2018 - Banrisul - tem juros à parte do valor pago \\
                        {
                            juros = vlPago - Convert.ToDecimal(valor);
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(linha.Substring(266, 11) + linha.Substring(277, 2)))
                                juros = Convert.ToDecimal(linha.Substring(266, 11) + "." + linha.Substring(277, 2));
                        }

                        // tratando as ocorrências \\
                        codRetorno = linha.Substring(108, 2);
                        codErro = null;

                        // pegando os erros de cada banco \\
                        int tamErro = 2;
                        if (banco == "041")
                            codErro = linha.Substring(382, 10);
                        else if (banco == "604")
                            codErro = linha.Substring(377, 8);
                        else if (banco == "237" || banco == "999" || banco == "998")
                            codErro = linha.Substring(318, 10);
                        else if (banco == "341")
                            codErro = linha.Substring(377, 8);
                        else if (banco == "033")
                        {
                            tamErro = 3;
                            codErro = linha.Substring(136, 3) + linha.Substring(139, 3) + linha.Substring(142, 3); // codigos com 3 digitos cada
                        }
                        else if (banco == "104")
                        {
                            tamErro = 3;
                            codErro = linha.Substring(79, 3); // codigos com 3 digitos cada
                        }
                        else if (banco == "399")
                            codErro = linha.Substring(301, 2); // codigos com 2 digitos cada

                        deveBaixar = _serviceOcorrenciaRetorno.LerOcorrenciaRetorno(banco, codRetorno, true).lista.Count() > 0;
                        deveDesAlocar = _serviceOcorrenciaRetorno.LerOcorrenciaRetorno(banco, codRetorno, null, true).lista.Count() > 0;
                        entradaConfirmada = _serviceOcorrenciaRetorno.LerOcorrenciaRetorno(banco, codRetorno, null, null, true).lista.Count() > 0;

                        retDescricao = "";

                        if (!String.IsNullOrWhiteSpace(codErro))
                        {
                            var ocor = _serviceOcorrenciaRetorno.LerOcorrenciaRetorno(banco, codRetorno).lista.FirstOrDefault();
                            if (ocor != null)
                            {
                                retDescricao = ocor.OCT_DESCRICAO;

                                double retorno = 0;

                                if (double.TryParse(codErro, out retorno))
                                {
                                    int i = 0;
                                    string erro = codErro.Substring(i, tamErro);
                                    while (erro != StringUtil.PreencherZeroEsquerda('0', tamErro) && !String.IsNullOrWhiteSpace(erro))
                                    {
                                        var Erros = _serviceOcorrenciaErro.LerOcorrenciaErro(banco, erro, codRetorno, 1, 999999).lista;
                                        if (Erros != null && Erros.Count() > 0)
                                            retDescricao += " [" + Erros.FirstOrDefault().OCE_CODIGO + " - " + Erros.FirstOrDefault().OCE_DESCRICAO + "]";

                                        i += tamErro;

                                        if ((tamErro + i) <= codErro.Length)
                                            erro = codErro.Substring(i, tamErro);
                                        else
                                            break;
                                    }
                                }
                            }
                        }

                        dtPgto = null;
                        dt = "20" + linha.Substring(114, 2) + "-" + linha.Substring(112, 2) + "-" + linha.Substring(110, 2);
                        if (dt != "2000-00-00" && dt != "20  -  -  ")
                            dtPgto = Convert.ToDateTime(dt);

                        dtOcorrencia = dtPgto;

                        if (!String.IsNullOrWhiteSpace(linha.Substring(152, 11) + linha.Substring(163, 2)))
                            ttPrevisto += Convert.ToDecimal(valor);

                        ttRecebido += vlPago + juros;
                        ttJuros += juros > 0 ? juros : 0;
                        qtBaixar += deveBaixar ? 1 : 0;

                        // detalhe \\
                        if (linha.Length - 1 != 400)
                            cabecalho += "<tr><td style=\"text-align:right\">ERRO:</td><td>[detalhe com largura invalida!]" + "</td></tr>";

                        if (ln == 2)
                        {
                            cnpjCedente = linha.Substring(3, 14);
                            cabecalho += "<tr><td style=\"text-align:right\">CNPJ do Cedente:</td><td>" + linha.Substring(3, 14) + "</td></tr>";

                            corpo += "<tr>";
                            corpo += "<td> Tit.Aqui </td>";
                            corpo += "<td> Tit.Bco </td>";
                            corpo += "<td> Ocor </td>";
                            corpo += "<td> Dt.Ocor </td>";
                            corpo += "<td> Vencto </td>";
                            corpo += "<td style=\"text-align:right\"> Vlr.Tit </td>";
                            corpo += "<td> Bco </td>";
                            corpo += "<td> Age </td>";
                            corpo += "<td style=\"text-align:right\"> Vlr.Rec </td>";
                            corpo += "<td> Data </td>";
                            corpo += "<td> Acao </td>";
                            corpo += "</tr>";
                        }

                        corpo += "<tr>";
                        corpo += "<td>" + nrTituloAqui + "</td>";
                        corpo += "<td>" + nrTituloBco + "</td>";
                        if (!String.IsNullOrWhiteSpace(retDescricao))
                        {
                            corpo += "<td>" + codRetorno + " (" + retDescricao + ")</td>";
                        }
                        else
                        {
                            corpo += "<td>" + codRetorno + "</td>";
                        }
                        corpo += "<td>" + linha.Substring(110, 2) + "-" + linha.Substring(112, 2) + "-20" + linha.Substring(114, 2) + "</td>";
                        corpo += "<td>" + linha.Substring(146, 2) + "-" + linha.Substring(148, 2) + "-20" + linha.Substring(150, 2) + "</td>";
                        corpo += "<td style=\"text-align:right\">" + string.Format("{0:0,0.00}", Convert.ToDecimal(valor)) + "</td>";
                        corpo += "<td>" + linha.Substring(165, 3) + "</td>";
                        corpo += "<td>" + linha.Substring(168, 4) + "</td>";
                        corpo += "<td style=\"text-align:right\">" + string.Format("{0:0,0.00}", vlPago + juros) + "</td>";

                        if (banco == "604")
                            corpo += "<td>" + linha.Substring(385, 2) + "-" + linha.Substring(387, 2) + "-20" + linha.Substring(389, 2) + "</td>";
                        else
                            if ("033,341,041,237".Contains(banco))
                        {
                            if (linha.Substring(295, 2).Trim() == "")
                                corpo += "<td></td>";
                            else
                                corpo += "<td>" + linha.Substring(295, 2) + "-" + linha.Substring(297, 2) + "-20" + linha.Substring(299, 2) + "</td>";
                        }
                        else
                                if (banco == "104")
                            corpo += "<td>" + linha.Substring(293, 2) + "-" + linha.Substring(295, 2) + "-20" + linha.Substring(297, 2) + "</td>";
                        else
                                    if (banco == "399")
                            corpo += "<td>" + dtCreditoHSBC + "</td>";

                        ParcelasDTO parcela = new ParcelasDTO();

                        if (String.IsNullOrWhiteSpace(nrTituloAqui))
                        {
                            parcela = _dao.buscarPorNossoNumero(nrTituloBco).FirstOrDefault();
                            if (parcela == null)
                            {
                                ln++;
                                continue;
                            }
                            nrTituloAqui = parcela.PAR_NUM_PARCELA;
                        }

                        // não ler linha sem identificação da CAIXA \\
                        if (banco == "104" && linha.Substring(27, 2) == "00")
                        {
                            ln++;
                            continue;
                        }

                        // processar \\
                        dt = "NENHUMA";
                        if (arquivoSeraProcessado)
                        {
                            if (banco == "041")
                            {
                                if (nrTituloAqui.Length < 8)
                                {
                                    var parc = _dao.buscarParcelaPorParte(nrTituloAqui);
                                    if (parc != null && parc.Count() > 0)
                                        nrTituloAqui = parc.FirstOrDefault().PAR_NUM_PARCELA;
                                }
                            }

                            if (parcela == null || parcela.PAR_NUM_PARCELA != nrTituloAqui)
                            {
                                parcela = _dao.FindByIdConverted(nrTituloAqui);
                                if (parcela == null)
                                {
                                    parcela = _dao.buscarPorNossoNumero(nrTituloBco).FirstOrDefault();
                                    if (parcela == null)
                                    {
                                        ln++;
                                        continue;
                                    }
                                }
                            }

                            cta_id = cta_id == null ? parcela.CTA_ID : cta_id;

                            if (parcela != null && parcela.PAR_NUM_PARCELA == nrTituloAqui)
                            {
                                if (parcela.PAR_DATA_PAGTO == null) // garante que nenhum titulo pago será tocado \\
                                {
                                    // a parcela já consta na lista? \\
                                    var indice = listaDeParcelasUpdateDB.FindIndex(x => x.PAR_NUM_PARCELA == parcela.PAR_NUM_PARCELA);
                                    var indLeg = listaDeParcelasLegadoUpdateDB.FindIndex(x => x.CONTRATO + x.LETRA + x.CD == parcela.PAR_NUM_PARCELA);

                                    // inicializando DTO PARCELAS para executar direto no BANCO \\
                                    ParcelaUpdateDTO ParcelaUpdateDB = new ParcelaUpdateDTO();
                                    ParcelaUpdateDB.PAR_NUM_PARCELA = parcela.PAR_NUM_PARCELA;
                                    ParcelaUpdateDB.PAR_DATA_PAGTO = parcela.PAR_DATA_PAGTO;
                                    ParcelaUpdateDB.PAR_VLR_PAGO = parcela.PAR_VLR_PAGO;
                                    ParcelaUpdateDB.PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                    ParcelaUpdateDB.PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                    ParcelaUpdateDB.PAR_REMESSA = parcela.PAR_REMESSA;
                                    ParcelaUpdateDB.PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                    ParcelaUpdateDB.CTA_ID = parcela.CTA_ID;
                                    ParcelaUpdateDB.BAN_ID = parcela.BAN_ID;

                                    //
                                    ParcelasLegadoUpdateDTO ParcelaLegadoUpdateDB = new ParcelasLegadoUpdateDTO();
                                    var parcLegado = this.BuscarLegado(parcela.PAR_NUM_PARCELA);
                                    if (parcLegado != null)
                                    {
                                        ParcelaLegadoUpdateDB.CONTRATO = parcLegado.CONTRATO;
                                        ParcelaLegadoUpdateDB.LETRA = parcLegado.LETRA;
                                        ParcelaLegadoUpdateDB.CD = parcLegado.CD;
                                        ParcelaLegadoUpdateDB.ALOC_BANCO = parcLegado.ALOC_BANCO;
                                        ParcelaLegadoUpdateDB.BCO_ALOC = parcLegado.BCO_ALOC;
                                        ParcelaLegadoUpdateDB.CART_ALOC = parcLegado.CART_ALOC;
                                        ParcelaLegadoUpdateDB.CART_ALOC_2 = parcLegado.CART_ALOC_2;
                                        ParcelaLegadoUpdateDB.CEDENTE = parcLegado.cedente;
                                        ParcelaLegadoUpdateDB.DT_ALOC = parcLegado.DT_ALOC;
                                        ParcelaLegadoUpdateDB.DT_EMISSAO_BLQ = parcLegado.DT_EMISSAO_BLQ;
                                        ParcelaLegadoUpdateDB.NOSSO_NUMERO = parcLegado.nosso_numero;
                                        ParcelaLegadoUpdateDB.DT_PAGTO = parcLegado.DT_PAGTO;
                                        ParcelaLegadoUpdateDB.VLR_PAGO = parcLegado.VLR_PAGO;
                                    }

                                    // desalocar - data do arquivo maior que alocação e alocada para o banco deste arquivo \\
                                    if (deveDesAlocar)
                                    {
                                        var _desalocar = false;

                                        if (parcela.PAR_DATA_ALOC != null)
                                        {
                                            var _dtaloc = (DateTime)parcela.PAR_DATA_ALOC;
                                            var dtaloc = _dtaloc.ToString("dd/MM/yyyy");

                                            var _dtbord = (DateTime)dtBordero;
                                            var dtbord = _dtbord.ToString("dd/MM/yyyy");

                                            var _dtbordA = _dtbord.AddDays(-1);
                                            var dtbordA = _dtbordA.ToString("dd/MM/yyyy");

                                            _desalocar = ((dtaloc == dtbord) || (dtaloc == dtbordA)) && parcela.BAN_ID == banco;
                                        }

                                        if (parcela.PAR_DATA_ALOC != null && _desalocar)
                                        {
                                            if (processar)
                                            {
                                                // tá na lista? \\
                                                if (indice >= 0)
                                                {
                                                    listaDeParcelasUpdateDB[indice].CTA_ID = null;
                                                    listaDeParcelasUpdateDB[indice].BAN_ID = null;
                                                    listaDeParcelasUpdateDB[indice].PAR_DATA_ALOC = null;
                                                    listaDeParcelasUpdateDB[indice].PAR_REMESSA = null;
                                                    listaDeParcelasUpdateDB[indice].PAR_TRANSMITIDO = null;
                                                    listaDeParcelasUpdateDB[indice].PAR_NOSSO_NUMERO = null;
                                                    //
                                                    listaDeParcelas[indice].CTA_ID = null;
                                                    listaDeParcelas[indice].BAN_ID = null;
                                                    listaDeParcelas[indice].PAR_DATA_ALOC = null;
                                                    listaDeParcelas[indice].PAR_REMESSA = null;
                                                    listaDeParcelas[indice].PAR_TRANSMITIDO = null;
                                                    listaDeParcelas[indice].PAR_NOSSO_NUMERO = null;
                                                }
                                                else
                                                {
                                                    ParcelaUpdateDB.CTA_ID = null;
                                                    ParcelaUpdateDB.BAN_ID = null;
                                                    ParcelaUpdateDB.PAR_DATA_ALOC = null;
                                                    ParcelaUpdateDB.PAR_REMESSA = null;
                                                    ParcelaUpdateDB.PAR_TRANSMITIDO = null;
                                                    ParcelaUpdateDB.PAR_NOSSO_NUMERO = null;
                                                    // 
                                                    parcela.CTA_ID = null;
                                                    parcela.BAN_ID = null;
                                                    parcela.PAR_DATA_ALOC = null;
                                                    parcela.PAR_REMESSA = null;
                                                    parcela.PAR_TRANSMITIDO = null;
                                                    parcela.PAR_NOSSO_NUMERO = null;
                                                }
                                                //
                                                if (indLeg >= 0 && (parcLegado != null))
                                                {
                                                    listaDeParcelasLegadoUpdateDB[indLeg].ALOC_BANCO = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].BCO_ALOC = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].CART_ALOC = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].CART_ALOC_2 = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].CEDENTE = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].DT_ALOC = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].DT_EMISSAO_BLQ = null;
                                                    listaDeParcelasLegadoUpdateDB[indLeg].NOSSO_NUMERO = null;
                                                }
                                                else if (parcLegado != null)
                                                {
                                                    ParcelaLegadoUpdateDB.ALOC_BANCO = null;
                                                    ParcelaLegadoUpdateDB.BCO_ALOC = null;
                                                    ParcelaLegadoUpdateDB.CART_ALOC = null;
                                                    ParcelaLegadoUpdateDB.CART_ALOC_2 = null;
                                                    ParcelaLegadoUpdateDB.CEDENTE = null;
                                                    ParcelaLegadoUpdateDB.DT_ALOC = null;
                                                    ParcelaLegadoUpdateDB.DT_EMISSAO_BLQ = null;
                                                    ParcelaLegadoUpdateDB.NOSSO_NUMERO = null;
                                                }

                                                // parcela alocada - pegue último registro da última transmissão \\
                                                var parAlocada = _serviceParcelaAlocada.LerParcelaAlocada(nrTituloAqui, parcela.REM_ID).OrderByDescending(x => x.ALO_DATA_TRANSMISSAO).FirstOrDefault();
                                                if (parAlocada != null) // achou \\
                                                {
                                                    ParcelaAlocadaUpdateDTO parAloc = new ParcelaAlocadaUpdateDTO();
                                                    parAloc.ALO_DATA_ALOCACAO = parAlocada.ALO_DATA_ALOCACAO;
                                                    parAloc.ALO_DATA_DESALOCACAO = DateTime.Now;
                                                    parAloc.ALO_DATA_TRANSMISSAO = parAlocada.ALO_DATA_TRANSMISSAO;
                                                    parAloc.ALO_NOSSO_NUMERO = parAlocada.OCT_CODIGO != null ? null : parAlocada.ALO_NOSSO_NUMERO;
                                                    parAloc.ALO_REM_DATA_OCORRENCIA = parAlocada.ALO_REM_DATA_OCORRENCIA;
                                                    parAloc.ALO_RET_DATA_OCORRENCIA = dtOcorrencia;
                                                    parAloc.BAN_ID = parAlocada.BAN_ID;
                                                    parAloc.CTA_ID = parAlocada.CTA_ID;
                                                    parAloc.OCE_CODIGO = parAlocada.OCE_CODIGO;
                                                    parAloc.OCM_CODIGO = parAlocada.OCM_CODIGO;
                                                    parAloc.OCT_CODIGO = codRetorno;
                                                    parAloc.PAR_NUM_PARCELA = parAlocada.PAR_NUM_PARCELA;
                                                    parAloc.PAR_REMESSA = parAlocada.PAR_REMESSA;
                                                    parAloc.REM_ID = parAlocada.REM_ID;

                                                    listaDeParcelasAlocadas.Add(parAloc);
                                                }
                                            }
                                            dt = "DESALOCOU"; // efetuou a desalocação?
                                        }
                                        else
                                            dt = "DESALOCOU ANTES"; // efetuou a desalocação antes?
                                        qtDesAlocou++;
                                    }
                                    else
                                    // preparando para registrar o título \\
                                    if (entradaConfirmada)
                                    {
                                        if (parcela.PAR_NOSSO_NUMERO != nrTituloBco)
                                        {
                                            dt = "REGISTROU NS.NUM.";
                                            if (processar)
                                            {
                                                if (indice >= 0)
                                                {
                                                    listaDeParcelasUpdateDB[indice].PAR_NOSSO_NUMERO = nrTituloBco;
                                                    listaDeParcelasUpdateDB[indice].PAR_TRANSMITIDO = "A";
                                                    //
                                                    listaDeParcelas[indice].PAR_NOSSO_NUMERO = nrTituloBco;
                                                    listaDeParcelas[indice].PAR_TRANSMITIDO = "A";
                                                }
                                                else
                                                {
                                                    ParcelaUpdateDB.PAR_NOSSO_NUMERO = nrTituloBco;
                                                    ParcelaUpdateDB.PAR_TRANSMITIDO = "A";
                                                    //
                                                    parcela.PAR_NOSSO_NUMERO = nrTituloBco;
                                                    parcela.PAR_TRANSMITIDO = "A";
                                                }
                                                //

                                                if (indLeg >= 0 && (parcLegado != null))
                                                {
                                                    listaDeParcelasLegadoUpdateDB[indLeg].NOSSO_NUMERO = nrTituloBco;
                                                }
                                                else if (parcLegado != null)
                                                {
                                                    ParcelaLegadoUpdateDB.NOSSO_NUMERO = nrTituloBco;
                                                }

                                                // parcela alocada  - gravando o codigo, data e erro \\
                                                ParcelaAlocadaDTO parAlocada = _serviceParcelaAlocada.LerParcelaAlocada(nrTituloAqui, parcela.REM_ID).OrderByDescending(x => x.ALO_DATA_TRANSMISSAO).FirstOrDefault();
                                                if (parAlocada != null && parAlocada.OCT_CODIGO == null) // apenas se ainda não salvou \\
                                                {
                                                    ParcelaAlocadaUpdateDTO parAloc = new ParcelaAlocadaUpdateDTO();
                                                    parAloc.ALO_DATA_ALOCACAO = parAlocada.ALO_DATA_ALOCACAO;
                                                    parAloc.ALO_DATA_DESALOCACAO = parAlocada.ALO_DATA_DESALOCACAO;
                                                    parAloc.ALO_DATA_TRANSMISSAO = parAlocada.ALO_DATA_TRANSMISSAO;
                                                    parAloc.ALO_NOSSO_NUMERO = parAlocada.ALO_NOSSO_NUMERO;
                                                    parAloc.ALO_REM_DATA_OCORRENCIA = parAlocada.ALO_REM_DATA_OCORRENCIA;
                                                    parAloc.ALO_RET_DATA_OCORRENCIA = dtOcorrencia;
                                                    parAloc.BAN_ID = parAlocada.BAN_ID;
                                                    parAloc.CTA_ID = parAlocada.CTA_ID;
                                                    parAloc.OCE_CODIGO = parAlocada.OCE_CODIGO;
                                                    parAloc.OCM_CODIGO = parAlocada.OCM_CODIGO;
                                                    parAloc.OCT_CODIGO = codRetorno;
                                                    parAloc.PAR_NUM_PARCELA = parAlocada.PAR_NUM_PARCELA;
                                                    parAloc.PAR_REMESSA = parAlocada.PAR_REMESSA;
                                                    parAloc.REM_ID = parAlocada.REM_ID;

                                                    listaDeParcelasAlocadas.Add(parAloc);
                                                }
                                            }
                                        }
                                        else
                                            dt = "REGISTROU NS.NUM.ANTES";
                                        qtRegistrou++;
                                    }
                                    else
                                    // preparando para baixar o título \\
                                    if (deveBaixar)
                                    {
                                        if (dtPgto != null)
                                        {
                                            if (processar)
                                            {
                                                if (indice >= 0)
                                                {
                                                    listaDeParcelasUpdateDB[indice].CTA_ID = parcela.CTA_ID;
                                                    listaDeParcelasUpdateDB[indice].BAN_ID = parcela.BAN_ID;
                                                    listaDeParcelasUpdateDB[indice].PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                                    listaDeParcelasUpdateDB[indice].PAR_REMESSA = parcela.PAR_REMESSA;
                                                    listaDeParcelasUpdateDB[indice].PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                                    listaDeParcelasUpdateDB[indice].PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                                    //
                                                    listaDeParcelas[indice].CTA_ID = parcela.CTA_ID;
                                                    listaDeParcelas[indice].BAN_ID = parcela.BAN_ID;
                                                    listaDeParcelas[indice].PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                                    listaDeParcelas[indice].PAR_REMESSA = parcela.PAR_REMESSA;
                                                    listaDeParcelas[indice].PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                                    listaDeParcelas[indice].PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                                    //
                                                    listaDeParcelasUpdateDB[indice].PAR_VLR_PAGO = vlPago + juros;
                                                    listaDeParcelasUpdateDB[indice].PAR_DATA_PAGTO = dtPgto; //DateTime.Now;
                                                    //
                                                    listaDeParcelas[indice].PAR_VLR_PAGO = vlPago + juros;
                                                    listaDeParcelas[indice].PAR_DATA_PAGTO = dtPgto; //DateTime.Now;
                                                }
                                                else
                                                {
                                                    ParcelaUpdateDB.CTA_ID = parcela.CTA_ID;
                                                    ParcelaUpdateDB.BAN_ID = parcela.BAN_ID;
                                                    ParcelaUpdateDB.PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                                    ParcelaUpdateDB.PAR_REMESSA = parcela.PAR_REMESSA;
                                                    ParcelaUpdateDB.PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                                    ParcelaUpdateDB.PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                                    // 
                                                    ParcelaUpdateDB.PAR_VLR_PAGO = vlPago + juros;
                                                    ParcelaUpdateDB.PAR_DATA_PAGTO = dtPgto; //DateTime.Now;
                                                }

                                                //
                                                parcela.PAR_VLR_PAGO = vlPago + juros;
                                                parcela.PAR_DATA_PAGTO = dtPgto; //DateTime.Now;

                                                //
                                                if (parcLegado != null)
                                                {
                                                    var _dtpgto = (DateTime)parcela.PAR_DATA_PAGTO;
                                                    parcLegado.DT_PAGTO = _dtpgto.ToString("dd/MM/yyyy");
                                                    parcLegado.VLR_PAGO = parcela.PAR_VLR_PAGO.ToString().Replace('.', ',');
                                                    //
                                                    if (indLeg >= 0)
                                                    {
                                                        listaDeParcelasLegadoUpdateDB[indLeg].DT_PAGTO = parcLegado.DT_PAGTO;
                                                        listaDeParcelasLegadoUpdateDB[indLeg].VLR_PAGO = parcLegado.VLR_PAGO;
                                                    }
                                                    else
                                                    {
                                                        ParcelaLegadoUpdateDB.DT_PAGTO = parcLegado.DT_PAGTO;
                                                        ParcelaLegadoUpdateDB.VLR_PAGO = parcLegado.VLR_PAGO;
                                                    }
                                                }

                                                // parcela alocada  - gravando o codigo, data e erro \\
                                                ParcelaAlocadaDTO parAlocada = _serviceParcelaAlocada.LerParcelaAlocada(nrTituloAqui, parcela.REM_ID).OrderByDescending(x => x.ALO_DATA_TRANSMISSAO).FirstOrDefault();
                                                if (parAlocada != null) // apenas se ainda não salvou \\
                                                {
                                                    ParcelaAlocadaUpdateDTO parAloc = new ParcelaAlocadaUpdateDTO();
                                                    parAloc.ALO_DATA_ALOCACAO = parAlocada.ALO_DATA_ALOCACAO;
                                                    parAloc.ALO_DATA_DESALOCACAO = parAlocada.ALO_DATA_DESALOCACAO;
                                                    parAloc.ALO_DATA_TRANSMISSAO = parAlocada.ALO_DATA_TRANSMISSAO;
                                                    parAloc.ALO_NOSSO_NUMERO = (parAlocada.OCT_CODIGO == null) ? parAlocada.ALO_NOSSO_NUMERO : null;
                                                    parAloc.ALO_REM_DATA_OCORRENCIA = parAlocada.ALO_REM_DATA_OCORRENCIA;
                                                    parAloc.ALO_RET_DATA_OCORRENCIA = dtOcorrencia;
                                                    parAloc.BAN_ID = parAlocada.BAN_ID;
                                                    parAloc.CTA_ID = parAlocada.CTA_ID;
                                                    parAloc.OCE_CODIGO = parAlocada.OCE_CODIGO;
                                                    parAloc.OCM_CODIGO = parAlocada.OCM_CODIGO;
                                                    parAloc.OCT_CODIGO = codRetorno;
                                                    parAloc.PAR_NUM_PARCELA = parAlocada.PAR_NUM_PARCELA;
                                                    parAloc.PAR_REMESSA = parAlocada.PAR_REMESSA;
                                                    parAloc.REM_ID = parAlocada.REM_ID;

                                                    listaDeParcelasAlocadas.Add(parAloc);
                                                }

                                                // liquidando \\
                                                var temLiquidacao = _serviceLiquidacao.FindById(nrTituloAqui, "BL", nrTituloAqui);
                                                if (temLiquidacao == null)
                                                {
                                                    var naoLiquidou = listaDeLiquidacao.Where(x => x.PAR_NUM_PARCELA == nrTituloAqui).Count() == 0;
                                                    if (naoLiquidou)
                                                    {
                                                        ParcelaLiquidacaoDTO liquidacao = new ParcelaLiquidacaoDTO();

                                                        liquidacao.PAR_NUM_PARCELA = nrTituloAqui;
                                                        liquidacao.BAN_ID = banco;
                                                        liquidacao.PLI_DATA = dtPgto;
                                                        liquidacao.PLI_DATA_BAIXA = DateTime.Now;
                                                        liquidacao.PLI_DATA_BORDERO = dtBordero;
                                                        liquidacao.PLI_NUMERO = nrTituloAqui;
                                                        liquidacao.PLI_ORIGEM_PGTO = (nrTituloAqui.Where(c => char.IsLetter(c)).Count() > 0) && nrTituloAqui.Substring(6, 1) == "A" ? "3" : "1";
                                                        liquidacao.PLI_TIPO_DOC = "BL";
                                                        liquidacao.PLI_VALOR = vlPago + juros;

                                                        listaDeLiquidacao.Add(liquidacao);
                                                    }
                                                }
                                            }
                                            dt = "LIQ.HOJE"; // efetuou a baixa?
                                            qtBaixou++;
                                        }
                                        else
                                        {
                                            dt = "SEM DATA PGTO"; // sem data de pagamento
                                        }
                                    }
                                    else
                                    {   // registrando apenas \\
                                        if (indice >= 0)
                                        {
                                            listaDeParcelasUpdateDB[indice].PAR_TRANSMITIDO = "R";
                                            listaDeParcelas[indice].PAR_TRANSMITIDO = "R";
                                        }
                                        else
                                        {
                                            ParcelaUpdateDB.PAR_TRANSMITIDO = "R";
                                            parcela.PAR_TRANSMITIDO = "R";
                                        }

                                        dt = "NENHUMA"; // ação \\

                                        // parcela alocada  - gravando o codigo, data e erro \\
                                        ParcelaAlocadaDTO parAlocada = _serviceParcelaAlocada.LerParcelaAlocada(nrTituloAqui, parcela.REM_ID).OrderByDescending(x => x.ALO_DATA_TRANSMISSAO).FirstOrDefault();
                                        if (parAlocada != null) // apenas se ainda não salvou \\
                                        {
                                            if (!String.IsNullOrWhiteSpace(codErro.Trim()))
                                            {
                                                if (Convert.ToDouble(codErro) > 0) // se houver erro, leia \\
                                                {
                                                    if (processar)
                                                    {
                                                        int i = 0;
                                                        string erro = codErro.Substring(i, tamErro);
                                                        while (erro != StringUtil.PreencherZeroEsquerda('0', tamErro) && !String.IsNullOrWhiteSpace(erro))
                                                        {
                                                            var Erros = _serviceOcorrenciaErro.LerOcorrenciaErro(banco, erro, codRetorno, 1, 999999).lista;
                                                            if (Erros != null && Erros.Count() > 0)
                                                            {
                                                                ParcelaAlocadaUpdateDTO parAloc = new ParcelaAlocadaUpdateDTO();
                                                                parAloc.ALO_DATA_ALOCACAO = parAlocada.ALO_DATA_ALOCACAO;
                                                                parAloc.ALO_DATA_DESALOCACAO = parAlocada.ALO_DATA_DESALOCACAO;
                                                                parAloc.ALO_DATA_TRANSMISSAO = parAlocada.ALO_DATA_TRANSMISSAO;
                                                                parAloc.ALO_NOSSO_NUMERO = null;
                                                                parAloc.ALO_REM_DATA_OCORRENCIA = parAlocada.ALO_REM_DATA_OCORRENCIA;
                                                                parAloc.ALO_RET_DATA_OCORRENCIA = dtOcorrencia;
                                                                parAloc.BAN_ID = parAlocada.BAN_ID;
                                                                parAloc.CTA_ID = parAlocada.CTA_ID;
                                                                parAloc.OCE_CODIGO = erro;
                                                                parAloc.OCM_CODIGO = parAlocada.OCM_CODIGO;
                                                                parAloc.OCT_CODIGO = codRetorno;
                                                                parAloc.PAR_NUM_PARCELA = parAlocada.PAR_NUM_PARCELA;
                                                                parAloc.PAR_REMESSA = parAlocada.PAR_REMESSA;
                                                                parAloc.REM_ID = parAlocada.REM_ID;

                                                                listaDeParcelasAlocadas.Add(parAloc);

                                                                dt = "OCOR.ERRO"; // ação \\
                                                            }
                                                            // próximo erro \\
                                                            i += tamErro;
                                                            if (i + tamErro < codErro.Length)
                                                            {
                                                                erro = codErro.Substring(i, tamErro);
                                                            }
                                                            else
                                                            {
                                                                erro = "";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    // adicionar \\
                                    if (indice < 0)
                                    {
                                        listaDeParcelasUpdateDB.Add(ParcelaUpdateDB);
                                        listaDeParcelas.Add(parcela);
                                    }
                                    //
                                    if ((indLeg < 0) && (parcLegado != null))
                                    {
                                        listaDeParcelasLegadoUpdateDB.Add(ParcelaLegadoUpdateDB);
                                    }
                                }
                                else
                                {
                                    dt = "LIQ.ANTES"; // efetuou a baixa?
                                    qtBaixou++;

                                    // esta parcela foi baixada manualmente, então, atualize agora de acordo com o arquivo borderô do banco...
                                    if (parcela.PAR_BAIXA_MANUAL == true && deveBaixar == true && processar == true && dtPgto != null)
                                    {
                                        // a parcela já consta na lista? \\
                                        var indice = listaDeParcelasUpdateDB.FindIndex(x => x.PAR_NUM_PARCELA == parcela.PAR_NUM_PARCELA);
                                        var indLeg = listaDeParcelasLegadoUpdateDB.FindIndex(x => x.CONTRATO + x.LETRA + x.CD == parcela.PAR_NUM_PARCELA);

                                        // inicializando DTO PARCELAS para executar direto no BANCO \\
                                        ParcelaUpdateDTO ParcelaUpdateDB = new ParcelaUpdateDTO();
                                        ParcelaUpdateDB.PAR_NUM_PARCELA = parcela.PAR_NUM_PARCELA;
                                        ParcelaUpdateDB.PAR_DATA_PAGTO = parcela.PAR_DATA_PAGTO;
                                        ParcelaUpdateDB.PAR_VLR_PAGO = parcela.PAR_VLR_PAGO;
                                        ParcelaUpdateDB.PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                        ParcelaUpdateDB.PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                        ParcelaUpdateDB.PAR_REMESSA = parcela.PAR_REMESSA;
                                        ParcelaUpdateDB.PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                        ParcelaUpdateDB.CTA_ID = parcela.CTA_ID;
                                        ParcelaUpdateDB.BAN_ID = parcela.BAN_ID;

                                        //
                                        ParcelasLegadoUpdateDTO ParcelaLegadoUpdateDB = new ParcelasLegadoUpdateDTO();
                                        var parcLegado = this.BuscarLegado(parcela.PAR_NUM_PARCELA);
                                        if (parcLegado != null)
                                        {
                                            ParcelaLegadoUpdateDB.CONTRATO = parcLegado.CONTRATO;
                                            ParcelaLegadoUpdateDB.LETRA = parcLegado.LETRA;
                                            ParcelaLegadoUpdateDB.CD = parcLegado.CD;
                                            ParcelaLegadoUpdateDB.ALOC_BANCO = parcLegado.ALOC_BANCO;
                                            ParcelaLegadoUpdateDB.BCO_ALOC = parcLegado.BCO_ALOC;
                                            ParcelaLegadoUpdateDB.CART_ALOC = parcLegado.CART_ALOC;
                                            ParcelaLegadoUpdateDB.CART_ALOC_2 = parcLegado.CART_ALOC_2;
                                            ParcelaLegadoUpdateDB.CEDENTE = parcLegado.cedente;
                                            ParcelaLegadoUpdateDB.DT_ALOC = parcLegado.DT_ALOC;
                                            ParcelaLegadoUpdateDB.DT_EMISSAO_BLQ = parcLegado.DT_EMISSAO_BLQ;
                                            ParcelaLegadoUpdateDB.NOSSO_NUMERO = parcLegado.nosso_numero;
                                            ParcelaLegadoUpdateDB.DT_PAGTO = parcLegado.DT_PAGTO;
                                            ParcelaLegadoUpdateDB.VLR_PAGO = parcLegado.VLR_PAGO;
                                        }

                                        // preparando para baixar o título \\
                                        if (indice >= 0)
                                        {
                                            listaDeParcelasUpdateDB[indice].CTA_ID = parcela.CTA_ID;
                                            listaDeParcelasUpdateDB[indice].BAN_ID = parcela.BAN_ID;
                                            listaDeParcelasUpdateDB[indice].PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                            listaDeParcelasUpdateDB[indice].PAR_REMESSA = parcela.PAR_REMESSA;
                                            listaDeParcelasUpdateDB[indice].PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                            listaDeParcelasUpdateDB[indice].PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                            //
                                            listaDeParcelas[indice].CTA_ID = parcela.CTA_ID;
                                            listaDeParcelas[indice].BAN_ID = parcela.BAN_ID;
                                            listaDeParcelas[indice].PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                            listaDeParcelas[indice].PAR_REMESSA = parcela.PAR_REMESSA;
                                            listaDeParcelas[indice].PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                            listaDeParcelas[indice].PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                            //
                                            listaDeParcelasUpdateDB[indice].PAR_VLR_PAGO = vlPago + juros;
                                            listaDeParcelasUpdateDB[indice].PAR_DATA_PAGTO = DateTime.Now;

                                            //
                                            listaDeParcelas[indice].PAR_VLR_PAGO = vlPago + juros;
                                            listaDeParcelas[indice].PAR_DATA_PAGTO = DateTime.Now;

                                        }
                                        else
                                        {
                                            ParcelaUpdateDB.CTA_ID = parcela.CTA_ID;
                                            ParcelaUpdateDB.BAN_ID = parcela.BAN_ID;
                                            ParcelaUpdateDB.PAR_DATA_ALOC = parcela.PAR_DATA_ALOC;
                                            ParcelaUpdateDB.PAR_REMESSA = parcela.PAR_REMESSA;
                                            ParcelaUpdateDB.PAR_TRANSMITIDO = parcela.PAR_TRANSMITIDO;
                                            ParcelaUpdateDB.PAR_NOSSO_NUMERO = parcela.PAR_NOSSO_NUMERO;
                                            // 
                                            ParcelaUpdateDB.PAR_VLR_PAGO = vlPago + juros;
                                            ParcelaUpdateDB.PAR_DATA_PAGTO = DateTime.Now;

                                        }

                                        //
                                        parcela.PAR_VLR_PAGO = vlPago + juros;
                                        parcela.PAR_DATA_PAGTO = DateTime.Now;

                                        // adicionar \\
                                        if (indice < 0)
                                        {
                                            listaDeParcelasUpdateDB.Add(ParcelaUpdateDB);
                                            listaDeParcelas.Add(parcela);
                                        }
                                        //
                                        if ((indLeg < 0) && (parcLegado != null))
                                        {
                                            listaDeParcelasLegadoUpdateDB.Add(ParcelaLegadoUpdateDB);
                                        }
                                    }
                                }
                            }
                        }
                        corpo += "<td>" + dt + "</td>";
                        corpo += "</tr>";
                    }
                    ln++;
                }
            }

            if ((listaDeParcelasUpdateDB.Count() > 0) && (listaDeLiquidacao.Count() > 0))
            {
                try
                {
                    CnabArquivosDTO _arq = new CnabArquivosDTO();
                    EmpresaSRV _emp = new EmpresaSRV();
                    var emp = _emp.BuscarPorCNPJ(cnpjCedente);
                    if (emp == null)
                        throw new Exception("CNPJ (" + cnpjCedente + ") identificando a empresa do grupo COAD no interior do arquivo (" + nomeArquivo + ") não foi encontrado no COADCORP!");

                    var _status = _servicoCnabArquivos.buscarArquivosCNAB(emp.EMP_ID, banco, cta_id, (ln - 3), dtBordero, nomeArquivo);
                    if (_status.Count() > 0)
                    {
                        var _processou = _status.Where(x => x.CNQ_DATA_PROCESSADO != null);
                        if (_processou.Count() > 0 && processar && arquivoSeraProcessado)
                        {
                            var _dt = (DateTime)_processou.FirstOrDefault().CNQ_DATA_PROCESSADO;
                            return "#O Arquivo " + nomeArquivo + " já foi (processado) no dia " + _dt.ToString("dd/MM/yyyy hh:mm:ss") + " pelo usuário " + _processou.FirstOrDefault().USU_LOGIN;
                        }

                        var _estornou = _status.Where(x => x.CNQ_DATA_ESTORNADO != null);
                        if (_estornou.Count() > 0 && processar && arquivoSeraProcessado)
                        {
                            var _dt = (DateTime)_processou.FirstOrDefault().CNQ_DATA_ESTORNADO;
                            return "#O Arquivo " + nomeArquivo + " foi (estornado) no dia " + _dt.ToString("dd/MM/yyyy hh:mm:ss") + " pelo usuário " + _processou.FirstOrDefault().USU_LOGIN;
                        }
                    }

                    var txOpt = new TransactionOptions();
                    txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                    txOpt.Timeout = TransactionManager.MaximumTimeout;
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                    {
                        //Stream fs = _conteudo.InputStream;
                        //BinaryReader br = new BinaryReader(fs);
                        //Byte[] bytesArquivo = br.ReadBytes((Int32)fs.Length);

                        _arq.EMP_ID = emp.EMP_ID;
                        _arq.BAN_ID = banco;
                        _arq.CTA_ID = (int)cta_id;
                        _arq.CNQ_QTD_LINHAS = (ln - 3);
                        _arq.CNQ_DATA_ARQUIVO = (DateTime)dtBordero;
                        _arq.CNQ_NOME = nomeArquivo;
                        _arq.CNQ_DATA_LIDO = DateTime.Now;
                        _arq.CNQ_ARQUIVO = bytesArquivo;
                        _arq.USU_LOGIN = SessionContext.login;
                        _arq.DATA_CADASTRO = DateTime.Now;

                        if (processar)
                        {
                            _arq.CNQ_DATA_PROCESSADO = DateTime.Now;
                        }

                        _servicoCnabArquivos.Save(_arq);

                        if (processar && arquivoSeraProcessado)
                        {
                            this.BaixarTitulos(listaDeParcelas, listaDeLiquidacao, listaDeParcelasAlocadas, false, listaDeParcelasUpdateDB, listaDeParcelasLegadoUpdateDB);
                        }

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    return "#" + ex;
                }
            }

            corpo += "</table>";

            d = DateTime.Now;
            tempo += " a " + d.ToString("hh:mm:ss");

            jaProcessado = (qtBaixar > 0 || qtDesAlocou > 0 || qtRegistrou > 0) ? jaProcessado : "1";

            cabecalho = jaProcessado + cabecalho; // flag indicando se o arquivo já foi processado \\
            cabecalho += "<tr><td style=\"text-align:right\">SOMATORIO do valor principal dos títulos:</td><td>" + string.Format("{0:0,0.00}", ttPrevisto) + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">SOMATORIO do valor na coluna de pagamento:</td><td>" + string.Format("{0:0,0.00}", ttRecebido) + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">SOMATORIO do valor na coluna de multa e juros:</td><td>" + string.Format("{0:0,0.00}", ttJuros) + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">QUANTIDADE de baixas:</td><td>" + qtBaixar.ToString() + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">QUANTIDADE de baixas efetuadas:</td><td>" + qtBaixou.ToString() + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">QUANTIDADE de Titulos:</td><td>" + (ln - 3).ToString() + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">QUANTIDADE de Titulos Desalocados:</td><td>" + qtDesAlocou.ToString() + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">QUANTIDADE de Titulos Registrados:</td><td>" + qtRegistrou.ToString() + "</td></tr>";
            cabecalho += "<tr><td style=\"text-align:right\">Tempo de Processamento:</td><td>" + tempo + "</td></tr>";
            cabecalho += "</table>";

            return cabecalho + corpo;
        }

        /// <summary>
        /// ALT: 21/11/2016 - Efetua a Baixa de títulos (método único de baixas)
        ///                   RECORRENTE passará apenas o parâmetro [listaDeParcelas]; [listaDeLiquidacao]=null; [listaDeParcelasAlocadas]=null e [lRecorrente]=true
        /// </summary>
        /// <returns></returns>

        public void BaixarTitulos(List<ParcelasDTO> listaDeParcelas,                        // obrigatório
                                  List<ParcelaLiquidacaoDTO> listaDeLiquidacao = null,      // não exigido para recorrência
                                  List<ParcelaAlocadaUpdateDTO> listaDeParcelasAlocadas = null,   // não exigido para recorrência
                                  bool lPrepararLiquidacaoDTO = false,                      // preparar TRUE/FALSE
                                  List<ParcelaUpdateDTO> listaDeParcelaUpdateDTO = null,    // lista de parcelas para UPDATE direto no BANCO
                                  List<ParcelasLegadoUpdateDTO> listaDeParcelasLegadoUpdateDTO = null) // lista de parcelas legadas para UPDATE direto no BANCO
        {
            if ((listaDeParcelas != null && listaDeParcelas.Count() > 0) || (listaDeParcelasAlocadas != null && listaDeParcelasAlocadas.Count() > 0))
            {
                if (listaDeLiquidacao == null)
                    listaDeLiquidacao = new List<ParcelaLiquidacaoDTO>();

                if (listaDeParcelasAlocadas == null)
                    listaDeParcelasAlocadas = new List<ParcelaAlocadaUpdateDTO>();

                if (listaDeParcelaUpdateDTO == null)
                    listaDeParcelaUpdateDTO = new List<ParcelaUpdateDTO>();

                if (listaDeParcelasLegadoUpdateDTO == null)
                    listaDeParcelasLegadoUpdateDTO = new List<ParcelasLegadoUpdateDTO>();

                if (lPrepararLiquidacaoDTO)
                    listaDeLiquidacao = _prepararListaDeLiquidacao(listaDeParcelas);

                // salvando COADCORP ----------------------------------------------------------------------------------------------------------------------\\
                if (listaDeParcelaUpdateDTO != null && listaDeParcelaUpdateDTO.Count() > 0) // parcelas
                    this.ExecutarUpdateEmParcelas(listaDeParcelaUpdateDTO);

                if (listaDeLiquidacao.Count() > 0)
                    _serviceLiquidacao.SaveOrUpdateNonIdentityKeyEntity(listaDeLiquidacao); // liquidacao

                if (listaDeParcelasAlocadas != null && listaDeParcelasAlocadas.Count() > 0) // parcela alocada
                    _serviceParcelaAlocada.ExecutarInsertUpdateEmParcelaAlocada(listaDeParcelasAlocadas);

                // salvando legado ------------------------------------------------------------------------------------------------------------------------\\
                if (listaDeParcelasLegadoUpdateDTO != null && listaDeParcelasLegadoUpdateDTO.Count() > 0) // parcelas
                    _serviceParcelasLegado.ExecutarUpdateEmParcelas(listaDeParcelasLegadoUpdateDTO);

                var lstLiqLegado = _prepararListaDeLiquidacaoLegado(listaDeLiquidacao, listaDeParcelas); // liquidacao
                if (lstLiqLegado.Count() > 0)
                    _serviceLiquidacaoLegado.SalvarLiquidacaoLegado(lstLiqLegado);
            }
        }

        //
        private List<LiquidacaoLegadoDTO> _prepararListaDeLiquidacaoLegado(List<ParcelaLiquidacaoDTO> listaDeLiquidacao, List<ParcelasDTO> listaDeParcelas)
        {
            List<LiquidacaoLegadoDTO> lstLiqLegado = new List<LiquidacaoLegadoDTO>();
            foreach (var liq in listaDeLiquidacao)
            {
                var parcela = listaDeParcelas.Where(x => x.PAR_NUM_PARCELA == liq.PAR_NUM_PARCELA && x.CTR_NUM_CONTRATO != null).FirstOrDefault();
                if (parcela != null)
                {
                    var existe = lstLiqLegado.Where(x => x.PAR_NUM_PARCELA == parcela.PAR_NUM_PARCELA).Count() > 0;
                    if (!existe)
                    {
                        string codigoParcelaLegado = (!string.IsNullOrWhiteSpace(parcela.PAR_COD_LEGADO)) ?
                            parcela.PAR_COD_LEGADO :
                            parcela.PAR_NUM_PARCELA;
                        LiquidacaoLegadoDTO liqLegado = new LiquidacaoLegadoDTO();

                        DateTime dtBaixa = (DateTime)liq.PLI_DATA_BAIXA;
                        DateTime dt = (DateTime)liq.PLI_DATA;
                        DateTime dtBordero = liq.PLI_DATA_BORDERO != null ? (DateTime)liq.PLI_DATA_BORDERO : DateTime.Now;

                        liqLegado.CONTRATO = codigoParcelaLegado.Substring(0, 6);
                        liqLegado.LETRA = codigoParcelaLegado.Substring(6, 1);
                        liqLegado.CD = codigoParcelaLegado.Substring(7, 1);
                        liqLegado.BANCO = liq.BAN_ID;
                        liqLegado.DT_BORDERO = dtBordero.ToString("dd/MM/yyyy");
                        liqLegado.IDENT_DOCTO = liq.PAR_NUM_PARCELA;
                        liqLegado.DATA_DA_BAIXA = dtBaixa.ToString("dd/MM/yyyy");
                        liqLegado.DATA = dt.ToString("dd/MM/yyyy");
                        liqLegado.NUMERO = liq.PLI_NUMERO;
                        liqLegado.ORIGEM_PGTO = liq.PLI_ORIGEM_PGTO;
                        liqLegado.TIPO_DOC = liq.PLI_TIPO_DOC;
                        liqLegado.VALOR = liq.PLI_VALOR.ToString().Replace('.', ',');
                        liqLegado.PAR_NUM_PARCELA = liq.PAR_NUM_PARCELA;
                        liqLegado.atualizarCodigo = liq.atualizaCodigo;
                        lstLiqLegado.Add(liqLegado);
                    }
                }
            }
            return lstLiqLegado;
        }
        //
        private List<ParcelaLiquidacaoDTO> _prepararListaDeLiquidacao(List<ParcelasDTO> listaDeParcelas)
        {
            List<ParcelaLiquidacaoDTO> listaDeLiquidacao = new List<ParcelaLiquidacaoDTO>();
            foreach (var p in listaDeParcelas)
            {
                var existe = listaDeLiquidacao.Where(x => x.PAR_NUM_PARCELA == p.PAR_NUM_PARCELA).Count() > 0;
                if (p.PAR_DATA_PAGTO != null && !existe)
                {
                    var _tipo = "BL";
                    if (p.TPG_ID != null)
                    {
                        var _tipoPagamento = new TipoPagamentoSRV().FindById(p.TPG_ID);
                        if (_tipoPagamento != null)
                            _tipo = _tipoPagamento.DLI_SIGLA;
                    }
                    ParcelaLiquidacaoDTO liquidacao = new ParcelaLiquidacaoDTO();

                    liquidacao.PAR_NUM_PARCELA = p.PAR_NUM_PARCELA;
                    liquidacao.BAN_ID = p.BAN_ID;
                    liquidacao.PLI_DATA = p.PAR_DATA_PAGTO;
                    liquidacao.PLI_DATA_BAIXA = DateTime.Now;
                    liquidacao.PLI_DATA_BORDERO = p.PAR_DATA_PAGTO;
                    liquidacao.PLI_NUMERO = p.PAR_NUM_PARCELA;
                    liquidacao.PLI_ORIGEM_PGTO = (p.PAR_NUM_PARCELA.Where(c => char.IsLetter(c)).Count() > 0) && p.PAR_NUM_PARCELA.Substring(6, 1) == "A" ? "3" : "1";
                    liquidacao.PLI_TIPO_DOC = _tipo;
                    liquidacao.PLI_VALOR = p.PAR_VLR_PAGO;
                    liquidacao.atualizaCodigo = (string.IsNullOrWhiteSpace(p.CTR_NUM_CONTRATO));

                    listaDeLiquidacao.Add(liquidacao);
                }
            }
            return listaDeLiquidacao;
        }

        /// <summary>
        /// ALT: 23/11/2016 - Estornar baixa de título
        /// </summary>
        /// <param name="PAR_NUM_PARCELA"></param>
        /// <returns></returns>
        public bool EstornarBaixa(string PAR_NUM_PARCELA)
        {
            try
            {
                List<ParcelaUpdateDTO> listaDeParcelasUpdateDB = new List<ParcelaUpdateDTO>();
                List<ParcelasLegadoUpdateDTO> listaDeParcelasLegadoUpdateDB = new List<ParcelasLegadoUpdateDTO>();

                List<ParcelasDTO> lstParcelas = new List<ParcelasDTO>();
                List<ParcelaLiquidacaoDTO> lstParcelaLiquidacao = new List<ParcelaLiquidacaoDTO>();
                List<ParcelasLegadoDTO> lstParcLegado = new List<ParcelasLegadoDTO>();

                var par = _dao.FindByIdConverted(PAR_NUM_PARCELA);
                if (par != null)
                {
                    par.PAR_DATA_PAGTO = null;
                    par.PAR_VLR_PAGO = null;

                    lstParcelas.Add(par);

                    ParcelaUpdateDTO ParcelaUpdateDB = new ParcelaUpdateDTO();
                    ParcelaUpdateDB.PAR_NUM_PARCELA = par.PAR_NUM_PARCELA;
                    ParcelaUpdateDB.PAR_DATA_PAGTO = par.PAR_DATA_PAGTO;
                    ParcelaUpdateDB.PAR_VLR_PAGO = par.PAR_VLR_PAGO;
                    ParcelaUpdateDB.PAR_DATA_ALOC = par.PAR_DATA_ALOC;
                    ParcelaUpdateDB.PAR_NOSSO_NUMERO = par.PAR_NOSSO_NUMERO;
                    ParcelaUpdateDB.PAR_REMESSA = par.PAR_REMESSA;
                    ParcelaUpdateDB.PAR_TRANSMITIDO = par.PAR_TRANSMITIDO;
                    ParcelaUpdateDB.CTA_ID = par.CTA_ID;
                    ParcelaUpdateDB.BAN_ID = par.BAN_ID;

                    listaDeParcelasUpdateDB.Add(ParcelaUpdateDB);
                }

                var liq = _serviceLiquidacao.BuscarPorParcela(PAR_NUM_PARCELA);
                if (liq != null)
                {
                    foreach (var l in liq)
                    {
                        l.PLI_DATA_EXCLUSAO = DateTime.Now;
                        l.BANCOS = null;
                        l.DOC_LIQUIDACAO = null;
                        l.PARCELAS = null;

                        lstParcelaLiquidacao.Add(l);
                    }
                }

                var parLeg = this.BuscarLegado(PAR_NUM_PARCELA);
                if (parLeg != null)
                {
                    parLeg.DT_PAGTO = null;
                    parLeg.VLR_PAGO = null;

                    lstParcLegado.Add(parLeg);

                    ParcelasLegadoUpdateDTO ParcelaLegadoUpdateDB = new ParcelasLegadoUpdateDTO();
                    ParcelaLegadoUpdateDB.CONTRATO = parLeg.CONTRATO;
                    ParcelaLegadoUpdateDB.LETRA = parLeg.LETRA;
                    ParcelaLegadoUpdateDB.CD = parLeg.CD;
                    ParcelaLegadoUpdateDB.ALOC_BANCO = parLeg.ALOC_BANCO;
                    ParcelaLegadoUpdateDB.BCO_ALOC = parLeg.BCO_ALOC;
                    ParcelaLegadoUpdateDB.CART_ALOC = parLeg.CART_ALOC;
                    ParcelaLegadoUpdateDB.CART_ALOC_2 = parLeg.CART_ALOC_2;
                    ParcelaLegadoUpdateDB.CEDENTE = parLeg.cedente;
                    ParcelaLegadoUpdateDB.DT_ALOC = parLeg.DT_ALOC;
                    ParcelaLegadoUpdateDB.DT_EMISSAO_BLQ = parLeg.DT_EMISSAO_BLQ;
                    ParcelaLegadoUpdateDB.NOSSO_NUMERO = parLeg.nosso_numero;
                    ParcelaLegadoUpdateDB.DT_PAGTO = parLeg.DT_PAGTO;
                    ParcelaLegadoUpdateDB.VLR_PAGO = parLeg.VLR_PAGO;

                    listaDeParcelasLegadoUpdateDB.Add(ParcelaLegadoUpdateDB);
                }

                var lstLiqLegado = _serviceLiquidacaoLegado.LerLiquidacaoLegado(PAR_NUM_PARCELA.Substring(0, 6), PAR_NUM_PARCELA.Substring(6, 1));

                // abrindo a transação...
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    // salvando COADCORP -----------------------------------------------------------\\
                    this.ExecutarUpdateEmParcelas(listaDeParcelasUpdateDB);
                    _serviceLiquidacao.SaveOrUpdateNonIdentityKeyEntity(lstParcelaLiquidacao);

                    // salvando legado -------------------------------------------------------------\\
                    _serviceParcelasLegado.ExecutarUpdateEmParcelas(listaDeParcelasLegadoUpdateDB);
                    _serviceLiquidacaoLegado.DeleteAll(lstLiqLegado);

                    scope.Complete();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Seleciona os registros para uma alocação
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="vencI"></param>
        /// <param name="vencF"></param>
        /// <param name="vlrI"></param>
        /// <param name="vlrF"></param>
        /// <param name="vlrT"></param>
        /// <param name="pagina"></param>
        /// <param name="itensPorPagina"></param>
        /// <returns></returns>
        public List<ParcelasDTO> SelecionarTitulos(string titulo, ContaDTO conta, DateTime? vencI = null, DateTime? vencF = null, Decimal vlrI = 0, Decimal vlrF = 0, Decimal vlrT = 0)
        {
            List<ParcelasDTO> ret = new List<ParcelasDTO>();

            //var sel = _dao.Selecionar(conta.EMP_ID, vencI, vencF, vlrI, vlrF);

            int empID = conta.CTA_ALOCAR_TITULO_DA_EMP_ID == null ? conta.EMP_ID : (int)conta.CTA_ALOCAR_TITULO_DA_EMP_ID;

            var sel = _dao.SelecionarProcedureRegistrosDeAlocacao(titulo, empID, vencI, vencF, vlrI, vlrF, conta.BAN_ID);

            Decimal soma = 0;
            foreach (var reg in sel)
            {
                reg.CTA_ID = conta.CTA_ID;
                ret.Add(reg);
                soma += (Decimal)reg.PAR_VLR_PARCELA;
                if (soma >= vlrT)
                    break;
            }

            return ret;
        }
        public List<ParcelasDTO> SelecionarTitulos(ContaDTO conta, decimal vlrT)
        {
            List<ParcelasDTO> ret = new List<ParcelasDTO>();

            var sel = _dao.ListarTitulosParaAlocacaoDet((int)conta.CTA_ALOCAR_TITULO_DA_EMP_ID);

            Decimal soma = 0;
            foreach (var reg in sel)
            {
                reg.CTA_ID = (int)conta.CTA_ID;
                reg.EMP_ID = (int)conta.CTA_ALOCAR_TITULO_DA_EMP_ID;
                reg.BAN_ID = conta.BAN_ID;
                ret.Add(reg);
                soma += (Decimal)reg.PAR_VLR_PARCELA;
                if (soma >= vlrT)
                    break;
            }

            return ret;
        }

        public IList<ParcelasDTO> listarParcelasAlocacaoAvulsa()
        {
            var _conta = new ContaSRV().BuscarContaBoletoAvuso();

            return _dao.listarParcelasAlocacaoAvulsa(_conta);
        }

        public IList<ParcelasDTO> listarParcelasAvulsas(int _REM_ID)
        {
            return _dao.listarParcelasAvulsas(_REM_ID);
        }

        public List<ParcelasDTO> EfetuarAlocacaoAvulsa(IList<ParcelasDTO> alocar, int _tre_id)
        {

            var _lista = new List<ParcelasDTO>();

            foreach (var p in alocar)
            {
                var _item = this.FindById(p.PAR_NUM_PARCELA);
                _item.BAN_ID = p.BAN_ID;
                _item.CTA_ID = p.CTA_ID;
                _item.EMP_ID = p.EMP_ID;

                _lista.Add(_item);
            }

            return this.EfetuarAlocacao(_lista, _tre_id, false);
        }

        public List<ParcelasDTO> EfetuarAlocacao(IList<ParcelasDTO> alocar, int _tre_id, bool? sacadorAvalista)
        {

            if (alocar[0].CTA_ID == null || alocar[0].CTA_ID == 0)
                throw new Exception("Conta para a alocação não informada !!!");

            DateTime _dataRemessa = DateTime.Now;
            var dt = _dataRemessa.ToString("yyyy-MM-dd hh:mm:ss");

            var _serviceConta = new ContaSRV();
            var lstParcLegado = new List<ParcelasLegadoDTO>();

            //-----------------------

            var _cta = (int)alocar[0].CTA_ID;
            var conta = _serviceConta.FindById(_cta);
            var _ban_id = conta.BAN_ID;
            var _emp_id = alocar[0].EMP_ID;
            var _cta_id = alocar[0].CTA_ID.ToString().PadLeft(2, '0');
            var _data = dt.ToString();
            var _remessaref = _emp_id.ToString() + "-" + _ban_id + "-" + alocar[0].CTA_ID.ToString().PadLeft(2, '0') + " " + dt.ToString();

            //-----------------------
            var _tipoRemessa = ServiceFactory.RetornarServico<TipoRemessaSRV>().FindById(_tre_id);
            var _boletosrv = ServiceFactory.RetornarServico<BoletoSRV>();

            ParcelasRemessaDTO _remessa = new ParcelasRemessaDTO();

            _remessa.REM_REF = _remessaref;
            _remessa.REM_DATA = _dataRemessa;
            _remessa.REM_TRANSMITIDO = null;
            _remessa.BAN_ID = _ban_id;
            _remessa.CTA_ID = _cta;
            _remessa.EMP_ID = _emp_id;
            _remessa.REM_QTDE = alocar.Count();
            _remessa.REM_TOTAL_REMESSA = alocar.Sum(x => x.PAR_VLR_BOLETO > 0 ? x.PAR_VLR_BOLETO : x.PAR_VLR_PARCELA);
            _remessa.TRE_ID = _tre_id;
            _remessa.REM_AVULSA = _tipoRemessa.TRE_AVULSA;
            _remessa.REM_SACADOR_AVALISTA = sacadorAvalista;

            _remessa = new ParcelasRemessaSRV().Save(_remessa);

            //------------------------

            foreach (var p in alocar)
            {
                p.CTA_ID = _cta;
                p.BAN_ID = _ban_id;
                p.PAR_DATA_ALOC = _dataRemessa;
                p.USU_LOGIN = SessionContext.login;
                p.PAR_TIPO_ALOC = 1;
                p.REM_ID = _remessa.REM_ID;
                p.PAR_REMESSA = _remessaref;
                p.EMP_ID = _emp_id;

                if(conta.CTA_GERA_NOSSO_NUMERO == true)
                {
                    p.PAR_NOSSO_NUMERO = _boletosrv.GerarProximoNossoNumero(conta);
                }

                if (_remessa.TRE_ID == 3)
                    p.BAN_ID = "999";

                if (_remessa.TRE_ID == 4)
                    p.BAN_ID = "998";
            }

            this.SaveOrUpdateNonIdentityKeyEntity(alocar);

            return alocar.ToList();
        }

        private void _gerarNossoNumero(ParcelasDTO parcela, ContaDTO conta)
        {
            if(parcela != null && conta.CTA_GERA_NOSSO_NUMERO == null)
            {
                
            }
        }

        /// <summary>
        /// efetua a desalocação dos títulos da remessa informada.
        /// </summary>
        /// <returns></returns>
        public void EfetuarDesAlocacao(int _rem_id)
        {
            try
            {
                // abrindo a transação...
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var parcelaRemessa = _serviceParcelasRemessa.FindById(_rem_id);

                    parcelaRemessa.REM_DATA_DESALOCACAO = DateTime.Now;

                    var parcelaAlocada = _serviceParcelaAlocada.LerParcelaAlocada(_rem_id);

                    foreach (var p in parcelaAlocada)
                    {
                        p.ALO_DATA_DESALOCACAO = DateTime.Now;
                    }

                    //List<ParcelasLegadoDTO> lstParcLegado = new List<ParcelasLegadoDTO>();

                    var _listaParcelas = this.LerParcelaAlocada(_rem_id);

                    foreach (var p in _listaParcelas)
                    {
                        p.CTA_ID = null;
                        p.BAN_ID = null;
                        p.PAR_DATA_ALOC = null;
                        p.USU_LOGIN = SessionContext.login;
                        p.PAR_TIPO_ALOC = null;
                        p.REM_ID = null;
                        p.PAR_REMESSA = null;
                        p.PAR_TRANSMITIDO = null;


                        //var parcLegado = this.BuscarLegado(p.PAR_NUM_PARCELA);
                        //if (parcLegado != null)
                        //{
                        //    parcLegado.BCO_ALOC = null;
                        //    parcLegado.DT_ALOC = null;
                        //    parcLegado.DT_EMISSAO_BLQ = null;
                        //    parcLegado.ALOC_BANCO = null;
                        //    parcLegado.REM_ID = null;
                        //    parcLegado.CART_ALOC = null;
                        //    parcLegado.CART_ALOC_2 = null;
                        //    parcLegado.nosso_numero = null;
                        //    parcLegado.cedente = null;

                        //    lstParcLegado.Add(parcLegado);
                        //}
                    }

                    _serviceParcelasRemessa.Merge(parcelaRemessa);
                    _serviceParcelaAlocada.SalvarParcelaAlocada(parcelaAlocada);

                    //_serviceParcelasLegado.SalvarParcelaLegado(lstParcLegado);

                    this.SaveOrUpdateNonIdentityKeyEntity(_listaParcelas);

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// efetua a transmissão dos títulos da remessa informada.
        /// </summary>
        /// <returns></returns>
        public void EfetuarTransmissao(int _rem_id, int _cta_id)
        {
            _dao.EfetuarTransmissao(_rem_id, _cta_id);
        }

        public IList<ParcelasDTO> LerParcelaAlocada(int _rem_id)
        {
            return _dao.LerParcelaAlocada(_rem_id);
        }

        public void SalvarRemessaParcelaAlocada(int rem_id, string tipoRemessa)
        {
            try
            {
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var _srvremessa = new ParcelasRemessaSRV();
                    var _remessa = _srvremessa.FindById(rem_id);

                    if (_remessa == null)
                        throw new Exception("Remessa não encontrada!! Verifique!!");

                    if (_serviceParcelaAlocada.LerParcelaAlocada(rem_id).Count() == 0)
                    {
                        var lstParcelaAlocada = new List<ParcelaAlocadaDTO>();

                        var parcela = this.LerTitulos(null, null, rem_id, null);

                        foreach (var p in parcela)
                        {
                            ParcelaAlocadaDTO parcelaAlocada = new ParcelaAlocadaDTO();
                            parcelaAlocada.ALO_DATA_ALOCACAO = DateTime.Now;
                            parcelaAlocada.BAN_ID = p.BAN_ID;
                            parcelaAlocada.CTA_ID = (int)p.CTA_ID;
                            parcelaAlocada.PAR_NUM_PARCELA = p.PAR_NUM_PARCELA;
                            parcelaAlocada.PAR_REMESSA = p.PAR_REMESSA;
                            parcelaAlocada.OCM_CODIGO = tipoRemessa;
                            parcelaAlocada.ALO_REM_DATA_OCORRENCIA = DateTime.Now;
                            parcelaAlocada.REM_ID = p.REM_ID;

                            lstParcelaAlocada.Add(parcelaAlocada);
                        }

                        _serviceParcelaAlocada.SalvarParcelaAlocada(lstParcelaAlocada);

                        _remessa.REM_QTDE = parcela.Count();
                        _remessa.REM_TOTAL_REMESSA = parcela.Sum(x => x.PAR_VLR_BOLETO > 0 ? x.PAR_VLR_BOLETO : x.PAR_VLR_PARCELA);

                    }

                    _remessa.REM_DATA_REMESSA = DateTime.Now;

                    _srvremessa.Merge(_remessa);

                    scope.Complete();

                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// ALT: 14/02/2017 - Procedure executando UPDATE em PARCELAS
        /// </summary>
        /// <param name="parcelas"></param>
        public void ExecutarUpdateEmParcelas(List<ParcelaUpdateDTO> parcelas)
        {
            using (var context = new COADCORPEntities())
            {
                context.ExecutarProcedure(parcelas, "PARCELA_BAIXAS_REGISTRONN", "@parcelas", "tp_Parcela_Baixas_RegistroNN");
            }
        }

        // registros rejeitados para restauração...
        public IList<ParcelasDTO> RestaurarRejeitados(int remessa)
        {
            return _dao.RestaurarRejeitados(remessa);
        }

        // registros de títulos disponíveis...
        public IList<ParcelasDTO> LerTitulosDisponiveis(int empresa = 2)
        {
            return _dao.LerTitulosDisponiveis(empresa);
        }

        // registros de títulos transmitidos ou remetido ao banco...
        public IList<ParcelasDTO> LerTitulosTransmitidos(int remessa)
        {
            return _dao.LerTitulosTransmitidos(remessa);
        }

        // registros de títulos rejeitados pelo banco...
        public IList<ParcelasDTO> LerTitulosRejeitados(int remessa)
        {
            return _dao.LerTitulosRejeitados(remessa);
        }

        // registros de títulos alocados no banco...
        public IList<ParcelasDTO> LerTitulosAlocados(int remessa)
        {
            return _dao.LerTitulosAlocados(remessa);
        }

        // registros de títulos para gerar boletos...
        public List<ParcelasDTO> LerTitulosGerarBoletos(int remessa)
        {
            return _dao.LerTitulosGerarBoletos(remessa);
        }

        // registros de títulos vencidos a receber...
        public IList<ParcelasDTO> LerTitulosVencidosReceber(int empresa = 2, DateTime? de = null, DateTime? ate = null)
        {
            return _dao.LerTitulosVencidosReceber(empresa, de, ate);
        }

        // registros de títulos vincendos a receber...
        public IList<ParcelasDTO> LerTitulosVincendosReceber(int empresa = 2, DateTime? de = null, DateTime? ate = null)
        {
            return _dao.LerTitulosVincendosReceber(empresa, de, ate);
        }

        // registros de títulos recebidos...
        public IList<ParcelasDTO> LerTitulosRecebidos(int empresa = 2, DateTime? de = null, DateTime? ate = null)
        {
            return _dao.LerTitulosRecebidos(empresa, de, ate);
        }

        // registros de títulos expirando...
        public IList<ParcelasDTO> LerTitulosExpirando(int empresa = 2, DateTime? dataBase = null)
        {
            return _dao.LerTitulosExpirando(empresa, dataBase);
        }

        // registros de títulos expirados...
        public IList<ParcelasDTO> LerTitulosExpirados(int empresa = 2, DateTime? dataBase = null)
        {
            return _dao.LerTitulosExpirados(empresa, dataBase);
        }

        // filtro comum...
        public IList<ParcelasDTO> LerTitulos(int? empresa = null, string banco = null, int? remessa = null, string transmitido = null)
        {
            return _dao.LerTitulos(empresa, banco, remessa, transmitido);
        }

        //----------------------------------------------------------------------------

        public IList<ParcelasDTO> BuscarPorContrato(string _numcontrato)
        {
            return _dao.BuscarPorContrato(_numcontrato);
        }
        public List<BoletosAlocadosDTO> BuscarTitulosAlocados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarTitulosAlocados(_dtini, _dtfim).ToList();
        }
        public Pagina<ParcelasDTO> BuscarTitulosProrrogados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _situacao = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.BuscarTitulosProrrogados(_dtini, _dtfim, _situacao, pagina, registroPorPagina);
        }
        public IList<ParcelasDTO> BuscarTitulosProrrogados(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _situacao = null)
        {
            return _dao.BuscarTitulosProrrogados(_dtini, _dtfim, _situacao);
        }
  
       
        public void SalvarParcela(ParcelasDTO _parcela, string _tipo)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (_tipo == "P")
                {
                    // Prorroga Parcela
                    _parcela.USU_LOGIN_PRORROGACAO = SessionContext.autenticado.USU_LOGIN;
                    _parcela.DATA_PRORROGACAO = DateTime.Now;
                    _parcela.PAR_SITUACAO = "PRO";

                    ProrrogarParcelaAgendada(_parcela.PAR_NUM_PARCELA);

                }

                if (_tipo == "L")
                {
                    _parcela.DATA_ALTERA = DateTime.Now;
                    _parcela.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                }

                if (_tipo == "C")
                {
                    _parcela.PAR_SITUACAO = "CON";
                    _parcela.DATA_ALTERA = DateTime.Now;
                    _parcela.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                }


                this.Merge(_parcela, "PAR_NUM_PARCELA");

                AssinaturaDTO _assinatura = new AssinaturaSRV().BuscarAssinaturaPorContrato(_parcela.CTR_NUM_CONTRATO);

                if (_tipo == "L")
                    new ClienteSRV().GravarHistorico(3, _assinatura.CLI_ID, _assinatura.ASN_NUM_ASSINATURA, "Liberação do titulo vencido (" + _parcela.PAR_NUM_PARCELA + ")" + "(" + _parcela.PAR_SITUACAO + ")", 109);
                else if (_tipo == "C")
                    new ClienteSRV().GravarHistorico(3, _assinatura.CLI_ID, _assinatura.ASN_NUM_ASSINATURA, "Prorrogação de titulo - Confirmação (" + _parcela.PAR_NUM_PARCELA + ")", 108);
                else
                    new ClienteSRV().GravarHistorico(3, _assinatura.CLI_ID, _assinatura.ASN_NUM_ASSINATURA, "Prorrogação de titulo (" + _parcela.PAR_NUM_PARCELA + ")", 108);


                scope.Complete();
            }
        }


        #region Prorrogar Parcela na Agenda de Cobrança

        private void ProrrogarParcelaAgendada(string _parcela)
        {

            // var lst = ServiceFactory.RetornarServico<ClientePassivelCobrancaSRV>().ListarClientePorParcela(_parcela);

            new ParcelaPendenteSRV().ExcluirPorParcela(_parcela);

        }

        #endregion

        public void AlterarSituacaoParcela(ICollection<ParcelasDTO> lst)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var lstParcelas = new List<ParcelasDTO>();

                foreach (ParcelasDTO item in lst)
                {
                    item.PAR_SITUACAO = "NOR";

                    lstParcelas.Add(item);

                    //_cobrancaSrv.DeletarParcelaEmCobranca(item.PAR_NUM_PARCELA, item.ASN_NUM_ASSINATURA);

                }

                this.SaveOrUpdateNonIdentityKeyEntity(lstParcelas);

            }

        }

        public IList<ParcelasConciliacaoDTO> BuscarConciliacaoTitulos(DateTime _dtini, DateTime _dtfim, int _emp_id, string _ban_id = null, string _parcela = null)
        {
            return _dao.BuscarConciliacaoTitulos(_dtini, _dtfim, _emp_id, _ban_id, _parcela);
        }

        public Pagina<ParcelasConciliacaoDTO> BuscarConciliacaoTitulos(DateTime _dtini, DateTime _dtfim, int _emp_id, string _ban_id = null, string _parcela = null, int _tipodata = 0, int? _tipoBaixa = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.BuscarConciliacaoTitulos(_dtini, _dtfim, _emp_id, _ban_id, _parcela, _tipodata, _tipoBaixa, pagina, registroPorPagina);
        }
        public IList<ParcelasConciliacaoDTO> BuscarConciliacao(Nullable<DateTime> _dtini = null,
                                        Nullable<DateTime> _dtfim = null,
                                        int? _emp_id = null,
                                        string _ban_id = null,
                                        string _parcela = null,
                                        int _tipodata = 0,
                                        int? _tipoBaixa = null)
        {
            return _dao.BuscarConciliacao((DateTime)_dtini, (DateTime)_dtfim, (int)_emp_id, _ban_id, _parcela, _tipodata, _tipoBaixa);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------Atividades de faturamento e geração de parcelas --------------------------------------

        /// <summary>
        /// Método que gera parcela para o faturamento. Método de entrada.
        /// </summary>
        /// <param name="contrato"></param>
        /// <param name="faturamento"></param>
        /// <returns></returns>
        public IEnumerable<ParcelasDTO> GerarParcelasFaturamento(IEnumerable<ContratoDTO> lstContratos, ContextoFaturamentoDTO faturamento)
        {
            IEnumerable<ParcelasDTO> lstContratosRetorno = new List<ParcelasDTO>();

            if (faturamento == null)
            {
                throw new FaturamentoException("As informações de faturamento não foram encontradas.");
            }

            foreach (var contrato in lstContratos)
            {
                var lstContratosGerados = GerarParcelasFaturamento(contrato, faturamento);
                lstContratosRetorno = lstContratosRetorno.Concat(lstContratosGerados);
            }

            return lstContratosRetorno;

        }

        /// <summary>
        /// Método que gera parcela para o faturamento.
        /// </summary>
        /// <param name="contrato"></param>
        /// <param name="faturamento"></param>
        /// <returns></returns>
        public ICollection<ParcelasDTO> GerarParcelasFaturamento(ContratoDTO contrato, ContextoFaturamentoDTO faturamento)
        {
            if (contrato != null && faturamento != null)
            {
                var lstParcelas = _processarGeracaoParcela(contrato, faturamento);
                this.SaveOrUpdateNonIdentityKeyEntity(lstParcelas);
                this.SalvarParcelasLegado(lstParcelas);

                var ultimaParcelaGerada = lstParcelas.
                    OrderByDescending(x => x.PAR_DATA_VENCTO).
                    Select(x => x.PAR_NUM_PARCELA).
                    FirstOrDefault();

                contrato.CTR_COD_ULTIMA_PARCELA_GERADA = ultimaParcelaGerada;

                return lstParcelas;
            }

            return null;
        }

        /// <summary>
        /// Processa a geração da parcela
        /// </summary>
        /// <param name="contrato"></param>
        /// <param name="faturamento"></param>
        /// <returns></returns>
        private IList<ParcelasDTO> _processarGeracaoParcela(ContratoDTO contrato, ContextoFaturamentoDTO faturamento)
        {
            IList<ParcelasDTO> lstParcelas = new List<ParcelasDTO>();

            var codigoDoItemPedido = contrato.IPE_ID;
            var itemPedido = faturamento.itemPedido;
            var formaPagamento = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(codigoDoItemPedido);
            var formaPagamentoRestante = _pedidoPagamentoSRV.RetornarPagamentoTirandoAEntrada(codigoDoItemPedido);

            TipoPeriodoDTO tipoPeriodo = new ItemPedidoSRV().ChecaEAdicionaTipoPeriodo(itemPedido);
            int? ipeId = itemPedido.IPE_ID;
            var vendaRecorrente = tipoPeriodo.TTP_RECORRENTE;

            bool podeAlocar = (formaPagamento.TPG_ID == 7);
            var lstParcelasEntradas = GerarVariasParcelas(itemPedido, contrato, null, faturamento, formaPagamento, podeAlocar: podeAlocar);
            lstParcelas = lstParcelas.Concat(lstParcelasEntradas).ToList();

            if (formaPagamentoRestante != null)
            {
                bool podeAlocarPagRest = (formaPagamentoRestante.TPG_ID == 7);
                var lstParcelasRestantes = GerarVariasParcelas(itemPedido, contrato, null, faturamento, formaPagamentoRestante, podeAlocar: podeAlocarPagRest);
                //var lstParcelasRestantes = GerarParcelas(itemPedido, contrato, faturamento, formaPagamentoParaGeracaoDeParcelas, parcelaEntrada);
                lstParcelas = lstParcelas.Concat(lstParcelasRestantes).ToList();
            }

            return lstParcelas;
        }

        public void SalvarParcelasLegado(IEnumerable<ParcelasDTO> lstParcelas)
        {
            IList<ParcelasLegadoDTO> lstParcelaLegado = new List<ParcelasLegadoDTO>();
            List<ParcelaLiquidacaoDTO> lstParcelaLiquidacao = new List<ParcelaLiquidacaoDTO>();

            var _parcelaLiquidacaoLegadoSRV = ServiceFactory
                    .RetornarServico<LiquidacaoLegadoSRV>();

            foreach (var parcela in lstParcelas)
            {
                var codParcela = (!string.IsNullOrWhiteSpace(parcela.PAR_COD_LEGADO)) ? parcela.PAR_COD_LEGADO : parcela.PAR_NUM_PARCELA;

                var parcelaLegado = CriarParcelaLegado(parcela);
                lstParcelaLegado.Add(parcelaLegado);

                var lstLiquidacao = ServiceFactory
                    .RetornarServico<ParcelaLiquidacaoSRV>()
                    .BuscarPorParcela(parcela.PAR_NUM_PARCELA);

                lstParcelaLiquidacao = lstParcelaLiquidacao.Concat(lstLiquidacao).ToList();

            }

            ServiceFactory.RetornarServico<ParcelasLegadoSRV>().SalvarParcelaLegado(lstParcelaLegado);
            var lstLiquidacaoLegado = _prepararListaDeLiquidacaoLegado(lstParcelaLiquidacao, lstParcelas.ToList());

            ServiceFactory.RetornarServico<LiquidacaoLegadoSRV>().SalvarLiquidacaoLegado(lstLiquidacaoLegado);
        }

        public ParcelasLegadoDTO CriarParcelaLegado(ParcelasDTO parcela)
        {
            var codigoParcela = parcela.PAR_NUM_PARCELA;
            var codigoNumContrato = parcela.CTR_NUM_CONTRATO;
            var composicaoDoCodigo = StringUtil.Truncate(codigoParcela, 2, true);
            var letra = composicaoDoCodigo.Substring(0, 1);
            var verificador = composicaoDoCodigo.Substring(1, 1);


            var codigoTeste = codigoNumContrato + letra + verificador;

            if (parcela.PAR_NUM_PARCELA != codigoTeste)
            {
                parcela.PAR_COD_LEGADO = codigoTeste;
                SaveOrUpdateNonIdentityKeyEntity(parcela);
            }

            var parcelaLegado = new ParcelasLegadoDTO()
            {
                CONTRATO = codigoNumContrato,
                LETRA = letra,
                CD = verificador,
                DATA_INSERT = DateTime.Now,
                DH_SUBIR = DateTime.Now.ToString("dd/MM/yyyy"),
                BCO_ALOC = parcela.BAN_ID,
                PAR_NUM_PARCELA = parcela.PAR_NUM_PARCELA
            };

            if (parcela.PAR_DATA_ALOC != null)
                parcelaLegado.DT_ALOC = parcela.PAR_DATA_ALOC.Value.ToString("dd/MM/yyyy");

            parcelaLegado.DT_EMISSAO_BLQ = parcelaLegado.DT_ALOC;
            parcelaLegado.ALOC_BANCO = (!string.IsNullOrWhiteSpace(parcelaLegado.DT_ALOC)) ? "S" : "N";


            if (parcela.TPG_ID != 7)
            {
                parcelaLegado.BCO_ALOC = "2";
                parcelaLegado.ALOC_BANCO = "S";
                parcelaLegado.DT_EMISSAO_BLQ = DateTime.Now.ToString("dd/MM/yyyy");
                parcelaLegado.DT_ALOC = DateTime.Now.ToString("dd/MM/yyyy");
            }

            if (parcela.PAR_DATA_VENCTO != null)
            {
                parcelaLegado.DATA_VENCTO = ((DateTime)parcela.PAR_DATA_VENCTO).ToString("dd/MM/yyyy");
            }

            if (parcela.PAR_VLR_PARCELA != null)
            {
                parcelaLegado.VLR_PARCELA = StringUtil.Truncate(StringUtil.FormatarDinheiro(parcela.PAR_VLR_PARCELA, false), 11);
            }

            if (parcela.PAR_MORA_MES != null)
            {
                parcelaLegado.MORA_MES = StringUtil.FormatarDinheiro(parcela.PAR_MORA_MES, false);
            }

            if (parcela.PAR_VLR_PAGO != null)
            {
                parcelaLegado.VLR_PAGO = StringUtil.Truncate(StringUtil.FormatarDinheiro(parcela.PAR_VLR_PAGO, false), 11);
            }

            if (parcela.PAR_DATA_PAGTO != null)
            {
                parcelaLegado.DT_PAGTO = ((DateTime)parcela.PAR_DATA_PAGTO).ToString("dd/MM/yyyy");
            }

            return parcelaLegado;
        }

        public ParcelasDTO BuscarUltimaParcela(string numeroDoContrato, bool dispararErroSeNaoEncontrar = false)
        {
            var parcela = _dao.BuscarUltimaParcela(numeroDoContrato);

            if (parcela == null && dispararErroSeNaoEncontrar == true)
            {
                throw new PagamentoNaoRealizadoException("Não existe nenhuma parcela paga");
            }

            return parcela;
        }

        /// <summary>
        /// Insere uma nova parcela se baseando na data da ultima parcela gerada
        /// </summary>
        /// <param name="contrato"></param>
        /// <param name="parcelaBase">Opcional</param>
        /// <returns></returns>
        public void GerarProximaParcelaContratoRecorrente(ParcelasDTO parcela = null)
        {
            if (parcela != null && ChecarParcelaPertenceAPedidoRecorrente(parcela))
            {
                // Verifica se já existe uma parcela paga. Se tiver gero a próxima parcela há ser paga
                ParcelasDTO parcelaBase = ObterUltimaParcelaDoPedido(parcela.IPE_ID, parcela.PGT_ID);

                if ((parcelaBase != null) && (parcelaBase.PAR_NUM_PARCELA.Equals(parcela.PAR_NUM_PARCELA)) || (parcelaBase.PAR_VLR_PAGO != null && parcelaBase.PAR_DATA_PAGTO != null))
                {
                    DateTime? dataVencimento = DateUtil.AdicionaMes(parcelaBase.PAR_DATA_VENCTO, 1, parcelaBase.PAR_DATA_VENCTO.Day);
                    var _itemPedidoSRV = new ItemPedidoSRV();

                    ContratoDTO contrato = null;
                    ItemPedidoDTO itemPedido = _itemPedidoSRV.FindById(parcelaBase.IPE_ID);
                    PedidoPagamentoDTO pedidoPagamento = _pedidoPagamentoSRV.FindById(parcelaBase.PGT_ID);
                    pedidoPagamento.PGT_ENTRADA = false;
                    pedidoPagamento.PGT_PAGO = false;

                    int? numeroParcela = parcelaBase.PAR_SEQ_PARCELA + 1;

                    if (!string.IsNullOrWhiteSpace(parcelaBase.CTR_NUM_CONTRATO))
                    {
                        contrato = new ContratoSRV().FindById(parcelaBase.CTR_NUM_CONTRATO);
                    }

                    var lstParcelas = GerarVariasParcelas(itemPedido, contrato, null, null, pedidoPagamento, numeroParcela, dataVencimento);
                    SaveOrUpdateAll(lstParcelas);

                    if (contrato != null)
                    {
                        SalvarParcelasLegado(lstParcelas);
                    }

                    var pagamentoGateway = (itemPedido.IPE_PAGAMENTO_GATEWAY == true);

                    PrepararParcelaGateway(itemPedido, null, pagamentoGateway, null, lstParcelas.FirstOrDefault());
                }
            }
        }


        public Pagina<ParcelasDTO> ListarPorContratos(string numeroContrato, int pagina = 1, int registrosPorPagina = 7)
        {
            var listapartcelasDTO = _dao.ListarPorContratos(numeroContrato, pagina, registrosPorPagina);

            foreach (var item in listapartcelasDTO.lista)
            {
                item.PAR_DIAS_ATRASO = 0;
                if (item.PAR_DATA_PAGTO == null)
                {
                    var i = (DateTime.Now - item.PAR_DATA_VENCTO).TotalDays;
                    if (i > 0)
                        item.PAR_DIAS_ATRASO = (int)i;
                }
            }

            return listapartcelasDTO;

        }

        public IList<ParcelasDTO> ObterParcelasDoPedidoPagamentoEmAberto(int? PGT_ID, int? IPE_ID)
        {
            return ObterParcelasDoPedidoPagamento(PGT_ID, IPE_ID, false);
        }

        public IList<ParcelasDTO> ObterParcelasDoPedidoPagamento(int? PGT_ID, int? IPE_ID, bool? paga = null)
        {
            return _dao.ObterParcelasDoPedidoPagamento(PGT_ID, IPE_ID, paga);
        }

        public ParcelasDTO ObterParcelaDoPedidoPagamento(int? PGT_ID)
        {
            return _dao.ObterParcelaDoPedidoPagamento(PGT_ID);
        }

        [MetodoAuxiliar]
        public IList<ParcelasDTO> ObterParcelasDoPedidoPagamento(PedidoPagamentoDTO pedidoPagamento)
        {
            if (pedidoPagamento != null)
            {
                var pgtId = pedidoPagamento.PGT_ID;
                return ObterParcelasDoPedidoPagamento(pgtId, null);
            }
            return new List<ParcelasDTO>();
        }

        [MetodoAuxiliar]
        public ParcelasDTO ObterParcelaDoPedidoPagamento(PedidoPagamentoDTO pedidoPagamento)
        {
            if (pedidoPagamento != null)
            {
                var pgtId = pedidoPagamento.PGT_ID;
                return ObterParcelaDoPedidoPagamento(pgtId);
            }
            return null;
        }

        [MetodoAuxiliar]
        public ParcelasDTO ObterProximaParcelaDoPedidoEmAberto(int? IPE_ID)
        {
            // Retorna a parcela.
            ParcelasDTO parcela = _dao.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);

            // Se não possuí nenhum parcela verifico o motivo. (Não há mais parcelas a serem pagas? Ou elas simplesmente não foram geradas?)
            if (parcela == null)
            {
                ChecarParcelasCriadasNoPedidoECriar(IPE_ID);
                parcela = _dao.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);

            }
            return parcela;
        }

        /// <summary>
        /// Retorna qual(s) é(são) a(s) parcela(s) do pedido baseado nas regras (R1) de seleção das parcelas. <para></para>
        /// (R1) - <para></para>
        ///     1) - Verifica qual é a forma de pagamento válida (verificar regras no método PedidoPagamentoSRV.CompararEntradaComPagamentoRestante(args, args). <para></para>
        ///     2) - Verifica se o tipo de pagamento é cartão. Se for retorna todas as parcelas associadas a essa forma de pagamento para dar baixa. <para></para>
        ///     Se não não for cartão retorna apenas uma das parcelas. <para></para>
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <param name="pagamentoEntrada"></param>
        /// <param name="pagamentoRestante"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public IList<ParcelasDTO> ObterProximasParcelasAPagar(int? IPE_ID, PedidoPagamentoDTO pagamentoEntrada, PedidoPagamentoDTO pagamentoRestante)
        {
            IList<ParcelasDTO> lstParcelas = null;
            var pagamentoValido = PedidoPagamentoSRV.CompararEntradaComPagamentoRestante(pagamentoEntrada, pagamentoRestante);
            if (pagamentoValido.TPG_ID == 9)
            {
                lstParcelas = ObterParcelasDoPedidoEPedidoPagamento(IPE_ID, pagamentoValido.PGT_ID);
                if (lstParcelas == null || lstParcelas.Count() <= 0)
                {
                    ChecarParcelasCriadasNoPedidoECriar(IPE_ID);
                    lstParcelas = ObterParcelasDoPedidoEPedidoPagamento(IPE_ID, pagamentoValido.PGT_ID);
                }
            }
            else
            {
                lstParcelas = new List<ParcelasDTO>();


                // Retorna a parcela.
                ParcelasDTO parcela = _dao.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);

                // Se não possuí nenhuma parcela verifico o motivo. (Não há mais parcelas a serem pagas? Ou elas simplesmente não foram geradas?)
                if (parcela == null)
                {
                    ChecarParcelasCriadasNoPedidoECriar(IPE_ID);
                    parcela = _dao.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);
                    lstParcelas.Add(parcela);
                }
                else
                {
                    lstParcelas.Add(parcela);
                }
            }
            return lstParcelas;
        }

        /// <summary>
        /// Retorna qual(s) é(são) a(s) parcela(s) do pedido baseado nas regras (R1) de seleção das parcelas. <para></para>
        /// (R1) - <para></para>
        ///     1) - Verifica se o tipo de pagamento é cartão. Se for retorna todas as parcelas associadas a essa forma de pagamento para dar baixa. <para></para>
        ///     Se não não for cartão retorna apenas uma das parcelas. <para></para>
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <param name="pagamentoEntrada"></param>
        /// <param name="pagamentoRestante"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public IList<ParcelasDTO> ObterProximasParcelasAPagar(int? IPE_ID)
        {
            IList<ParcelasDTO> lstParcelas = null;
            var pagamentoValido = new ItemPedidoSRV().ObterPedidoPagamentoAtual(IPE_ID);

            if (pagamentoValido != null)
            {
                if (pagamentoValido.TPG_ID == 8 || pagamentoValido.TPG_ID == 9)
                {
                    lstParcelas = ObterParcelasDoPedidoEPedidoPagamento(IPE_ID, pagamentoValido.PGT_ID);
                    if (lstParcelas == null || lstParcelas.Count() <= 0)
                    {
                        ChecarParcelasCriadasNoPedidoECriar(IPE_ID);
                        lstParcelas = ObterParcelasDoPedidoEPedidoPagamento(IPE_ID, pagamentoValido.PGT_ID);
                    }
                }
                else
                {
                    lstParcelas = new List<ParcelasDTO>();


                    // Retorna a parcela.
                    ParcelasDTO parcela = _dao.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);

                    // Se não possuí nenhuma parcela verifico o motivo. (Não há mais parcelas a serem pagas? Ou elas simplesmente não foram geradas?)
                    if (parcela == null)
                    {
                        ChecarParcelasCriadasNoPedidoECriar(IPE_ID);
                        parcela = _dao.ObterProximaParcelaDoPedidoEmAberto(IPE_ID);

                        if (parcela != null)
                            lstParcelas.Add(parcela);
                    }
                    else
                    {
                        if (parcela != null)
                            lstParcelas.Add(parcela);
                    }
                }
                return lstParcelas;
            }

            return null;
        }

        /// <summary>
        /// Verifica se o pedido por alguma eventualidade foi gerado sem parcelas. 
        /// Se foi gerado sem parcelas então gera os parcelas necessárias
        /// </summary>
        /// <param name="IPE_ID"></param>
        [MetodoAuxiliar]
        private void ChecarParcelasCriadasNoPedidoECriar(int? IPE_ID)
        {
            var itemPedido = new ItemPedidoSRV().FindById(IPE_ID);

            // Se o pedido é status 1 (pendente) significa que não é o caso de todas as parcelas já estarem quitadas.
            // Provavelmente elas não foram geradas como deveriam.
            // ChecarPedidoPendentePossuiParcelas(IPE_ID) verifica se o pedido é pendente e se ele possui alguma parcela associado a ele.
            if (itemPedido.PST_ID == 1 && !ChecarPedidoPendentePossuiParcelas(IPE_ID))
            {
                // se não tiver gero as parcelas que não foram geradas.
                this.GerarVariasParcelas(itemPedido, null);
            }
        }

        public bool ChecarParcelaPertenceAPedidoRecorrente(ParcelasDTO parcela)
        {
            if (parcela != null)
            {
                if (!string.IsNullOrWhiteSpace(parcela.CTR_NUM_CONTRATO))
                {
                    var contrato = new ContratoSRV().FindById(parcela.CTR_NUM_CONTRATO);
                    return contrato.CTR_VENDA_RECORRENTE;
                }
                else
                {
                    var _itemPedidoSRV = new ItemPedidoSRV();
                    var itemPedido = _itemPedidoSRV.FindById(parcela.IPE_ID);
                    var tipoPeriodo = _itemPedidoSRV.ChecaEAdicionaTipoPeriodo(itemPedido);

                    if (tipoPeriodo == null)
                        return false;

                    return tipoPeriodo.TTP_RECORRENTE;
                }
            }

            return false;
        }


        /// <summary>
        /// Insere as informações na parcela que caracterizam seu pagamento e faz a liquidação necessária.
        /// </summary>
        /// <param name="parcela"></param>
        /// <param name="requisicao"></param>
        /// <param name="baixarTitulo"></param>
        /// <param name="marcaPedidoPagamento"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public bool DarBaixaNaParcela(ParcelasDTO parcela, RequisicaoPagamentoDTO requisicao, bool baixarTitulo = true, bool marcaPedidoPagamento = false)
        {
            if (parcela != null)
            {
                DarBaixaNaParcela(new List<ParcelasDTO>() { parcela }, requisicao, baixarTitulo, marcaPedidoPagamento);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Insere as informações na parcela que caracterizam seu pagamento e faz a liquidação necessária.
        /// </summary>
        /// <param name="lstParcela"></param>
        /// <param name="requisicao"></param>
        /// <param name="baixarTitulo"></param>
        /// <param name="marcaPedidoPagamento"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public bool DarBaixaNaParcela(IList<ParcelasDTO> lstParcela, RequisicaoPagamentoDTO requisicao, bool baixarTitulo = true, bool marcaPedidoPagamento = false)
        {
            if (lstParcela != null && lstParcela.Count() > 0)
            {
                List<ParcelaLiquidacaoDTO> lstParcelasLiquidacao = new List<ParcelaLiquidacaoDTO>();
                List<ParcelaAlocadaUpdateDTO> lstParcelasAlocadas = new List<ParcelaAlocadaUpdateDTO>();

                foreach (var parcela in lstParcela)
                {
                    parcela.PAR_VLR_PAGO = parcela.PAR_VLR_PARCELA;
                    parcela.PAR_DATA_PAGTO = (requisicao != null && requisicao.DataLiquidacao != null) ? requisicao.DataLiquidacao : DateTime.Now;

                    if (marcaPedidoPagamento)
                    {
                        _pedidoPagamentoSRV.MarcarPedidoPagamentoDaParcelaComoPago(parcela, requisicao);
                    }

                    if (requisicao != null)
                    {
                        if (requisicao.Recorrente)
                        {
                            GerarProximaParcelaContratoRecorrente(parcela);
                        }

                        lstParcelasLiquidacao = requisicao.lstParcelasLiquidacao;
                        lstParcelasAlocadas = requisicao.lstParcelasAlocadas;

                        parcela.PAR_CODIGO_DE_BARRAS = requisicao.CodigoBarras;
                        parcela.PAR_URL_BOLETO = requisicao.UrlBoleto;
                        parcela.PAR_STATUS_TRANSACAO = requisicao.StatusTransacao;
                        parcela.ORDER_KEY = requisicao.OrderKey;
                        parcela.ORDER_KEY_REF = requisicao.OrderReference;
                        parcela.AUTHORIZATION_CODE = requisicao.AuthorizationCode;

                        if (parcela.TPG_ID == 7 && requisicao.ChaveTransacaoBoleto != null)
                        {
                            parcela.PAR_CHAVE_TRANSACAO = requisicao.ChaveTransacaoBoleto.ToString();
                        }
                        else if (parcela.TPG_ID == 9 && requisicao.ChaveTransacaoCartao != null)
                        {
                            parcela.PAR_CHAVE_TRANSACAO = requisicao.ChaveTransacaoCartao.ToString();
                        }
                        else
                        {
                            var chave = (requisicao.ChaveTransacaoCartao != null) ? requisicao.ChaveTransacaoCartao : requisicao.ChaveTransacaoBoleto;

                            if (chave != null)
                            {
                                parcela.PAR_CHAVE_TRANSACAO = chave.ToString();
                            }
                        }
                    }
                }

                SaveOrUpdateAll(lstParcela);

                if (baixarTitulo)
                {
                    BaixarTitulos(lstParcela.ToList(), lstParcelasLiquidacao, lstParcelasAlocadas, true);
                }
            }
            else if (requisicao.Recorrente && baixarTitulo)
            {
                var parcela = ObterUltimaParcelaDoPedido(requisicao.CodigoItemPedido);
                GerarProximaParcelaContratoRecorrente(parcela);
            }
            return true;
        }




        /// <summary>
        /// Dá baixa na parcela de acordo com os dados no parâmetro de requisição
        /// </summary>
        /// <param name="codParcela"></param>
        /// <param name="requisicao"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public bool DarBaixaNaParcela(string codParcela, RequisicaoPagamentoDTO requisicao, bool baixarTitulo = true, bool marcaPedidoPagamento = false)
        {
            var parcela = FindById(codParcela);
            return DarBaixaNaParcela(parcela, requisicao, baixarTitulo, marcaPedidoPagamento);
        }


        /// <summary>
        /// Insere as informações na parcela que caracterizam seu pagamento e faz a liquidação necessária.
        /// </summary>
        /// <param name="ipeId"></param>
        /// <param name="requisicao"></param>
        /// <param name="baixarTitulo"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public bool PagarProximaParcelaDoPedidoEmAberto(int? ipeId, RequisicaoPagamentoDTO requisicao, bool baixarTitulo = true)
        {
            var parcela = ObterProximaParcelaDoPedidoEmAberto(ipeId);
            return DarBaixaNaParcela(parcela, requisicao, baixarTitulo);
        }

        /// <summary>
        /// Gera várias parcelas baseada no pedido e alguns outros dados
        /// </summary>
        /// <param name="itemPedido"></param>
        /// <param name="pedidoPagamento"></param>
        /// <param name="ultimaParcelaGerada"></param>
        /// <param name="qtdParcelas"></param>
        /// <returns></returns>
        public IList<ParcelasDTO> GerarVariasParcelas(ItemPedidoDTO itemPedido, ContratoDTO contrato, int? qtdParcelas = null,
            ContextoFaturamentoDTO faturamento = null, PedidoPagamentoDTO pagamento = null, int? numeroDaParcela = null, DateTime? dataVencimento = null, bool podeAlocar = false)
        {
            if (itemPedido == null && contrato == null)
            {
                throw new ArgumentNullException("Informe o item pedido ou o contrato para gerar a parcela");
            }

            TipoPeriodoDTO tipoPeriodo = ServiceFactory
                .RetornarServico<ItemPedidoSRV>()
                .ChecaEAdicionaTipoPeriodo(itemPedido);

            int? ipeId = itemPedido.IPE_ID;

            var pedidoPagamento = DataUtil.ReturnNotNull(pagamento, _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(ipeId));
            var primeiraParcelaGratis = itemPedido.IPE_PRIMEIRA_PARCELA_CORTERIA;
            var vendaRecorrente = tipoPeriodo.TTP_RECORRENTE;
            var entrada = (bool)DataUtil.ReturnNotNull(pedidoPagamento.PGT_ENTRADA, false);
            var gerarParcelaComoEntrada = (vendaRecorrente) ? false : entrada;
            var tipoPagamento = pedidoPagamento.TPG_ID;
            var indexInicial = 1;
            var pago = false;
            var diaVencimentoVendaRecorrente = itemPedido.IPE_DIA_VENCIMENTO_VENDA_RECORRENTE;

            decimal? valorEntrada = null;
            decimal? valorParcela = null;

            // Se for cartão gero apenas 1 (uma) parcela
            if (tipoPagamento == 9)
                qtdParcelas = 1;

            /// Verifico se no momento dos pedidos ele possui um pagamento sem entrada, nesse caso todas as parcelas necessárias já estão criadas
            /// Necessita apenas que as mesmas sejam associadas ao contrado gerado.
            if (faturamento != null)
            {
                var lstParcelasGeradas = ProcessarParcelasGeradasPreFaturamento(itemPedido, pedidoPagamento, contrato);

                if (lstParcelasGeradas != null)
                {
                    return lstParcelasGeradas;
                }

            }

            if (vendaRecorrente == true)
            {
                DateTime? dataInicial = null;

                if (numeroDaParcela != null)
                {
                    indexInicial = (int)numeroDaParcela;
                    qtdParcelas = (int)numeroDaParcela;
                }
                else
                {
                    qtdParcelas = 2;
                }

                if (diaVencimentoVendaRecorrente != null)
                {
                    dataInicial = DateUtil.AlteraDia(dataVencimento, (int)diaVencimentoVendaRecorrente); // altera o dia da data

                    if (((DateTime)dataInicial).Date < DateTime.Now.Date) // se a data resultante for menor que a data atual adiciono 1 mês
                    {
                        dataInicial = DateUtil.AdicionaMes(dataInicial, 1, dataInicial.Value.Day);
                    }
                }
                else
                {
                    dataInicial = (numeroDaParcela > 0) ? DateUtil.AdicionaMes(dataVencimento, 1, dataVencimento.Value.Day) : DateUtil.AdicionaDia(DateTime.Now, 2);
                }

                dataVencimento = dataInicial;
            }
            else
            {

                dataVencimento = pedidoPagamento.PGT_DATA_VENCIMENTO;

                if (qtdParcelas == null)
                {
                    qtdParcelas = pedidoPagamento.PGT_QTDE_PARCELAS;
                }
            }

            if (entrada == true)
            {
                indexInicial = 0;
                qtdParcelas--;
                valorEntrada = pedidoPagamento.PGT_VLR_PARCELA;
            }

            if (dataVencimento == null)
            {
                if (faturamento != null && faturamento.ultimaDataVencimentoGerada != null)
                {
                    dataVencimento = faturamento.ultimaDataVencimentoGerada;
                }
                else
                {
                    dataVencimento = (tipoPagamento == 9) ? DateTime.Now : DateUtil.AdicionaDia(DateTime.Now, 2);
                }
            }

            IList<ParcelasDTO> lstParcelas = new List<ParcelasDTO>();

            int? dia = dataVencimento.Value.Day;
            for (var index = indexInicial; index <= qtdParcelas; index++)
            {
                var valorAtualDaParcela = valorParcela;
                pago = false;

                if (index != indexInicial)
                {
                    dataVencimento = DateUtil.AdicionaMes(dataVencimento, 1, dia);
                }
                else
                {
                    if (primeiraParcelaGratis == true)
                    {
                        valorAtualDaParcela = 0.00m;
                    }

                    if ((valorEntrada == null || valorEntrada <= 0 || vendaRecorrente == true) && pedidoPagamento.PGT_PAGO == true)
                    {
                        pago = true;
                    }
                }

                if (faturamento != null)
                {
                    faturamento.ultimaDataVencimentoGerada = dataVencimento;
                }

                var parcelaGerada = GerarParcelaPorDados(itemPedido, contrato, pedidoPagamento, index, gerarParcelaComoEntrada, valorAtualDaParcela, dataVencimento, pago, null, null, podeAlocar: podeAlocar);

                SaveOrUpdateNonIdentityKeyEntity(parcelaGerada);
                if (pago)
                {
                    _serviceLiquidacao.InserirLiquidacao(parcelaGerada);
                }

                lstParcelas.Add(parcelaGerada);

            }

            return lstParcelas;
        }


        public IList<ParcelasDTO> ObterParcelasDoPedidoEPedidoPagamento(int? IPE_ID, int? PGT_ID)
        {
            return _dao.ObterParcelasDoPedidoEPedidoPagamento(IPE_ID, PGT_ID);
        }

        public bool PossuiParcelasDoPedidoEPedidoPagamento(int? IPE_ID, int? PGT_ID)
        {
            return _dao.PossuiParcelasDoPedidoEPedidoPagamento(IPE_ID, PGT_ID);
        }


        /// <summary>
        /// Processa as parcelas geradas pelo pedido antes do faturamento. Associando-o ao contrato gerado no processo de faturamento.
        /// </summary>
        /// <param name="itemPedido"></param>
        /// <param name="pagamento"></param>
        /// <returns></returns>
        public IList<ParcelasDTO> ProcessarParcelasGeradasPreFaturamento(ItemPedidoDTO itemPedido, PedidoPagamentoDTO pagamento, ContratoDTO contrato)
        {

            if (contrato != null)
            {
                int? codigoDoItem = itemPedido.IPE_ID;
                int? codigoDaFormaDePagamento = pagamento.PGT_ID;
                string codigoDoContrato = contrato.CTR_NUM_CONTRATO;

                if (PossuiParcelasDoPedidoEPedidoPagamento(codigoDoItem, codigoDaFormaDePagamento))
                {
                    var lstParcelas = ObterParcelasDoPedidoEPedidoPagamento(codigoDoItem, codigoDaFormaDePagamento);

                    foreach (var parcela in lstParcelas)
                    {
                        parcela.PAR_PARCELA_DO_PEDIDO = false;
                        parcela.CTR_NUM_CONTRATO = codigoDoContrato;
                    }

                    return lstParcelas;

                }
            }

            return null;
        }

        /// <summary>
        /// Gera o código da parcela usando o código do contrato como parte do código
        /// </summary>
        /// <param name="itemPedido"></param>
        /// <param name="numeroDaParcela"></param>
        /// <returns></returns>
        public string ProcessarCodigoDaParcelaBaseadoNoPedido(ItemPedidoDTO itemPedido, int numeroDaParcela)
        {
            return ProcessarCodigoDaParcela(itemPedido, null, numeroDaParcela);
        }

        /// <summary>
        /// Gera o código da parcela usando o código do contrato como parte do código
        /// </summary>
        /// <param name="contrato"></param>
        /// <param name="numeroDaParcela"></param>
        /// <returns></returns>
        public string ProcessarCodigoDaParcelaBaseadoNoContrato(ContratoDTO contrato, int numeroDaParcela)
        {
            return ProcessarCodigoDaParcela(null, contrato, numeroDaParcela);
        }

        private string ProcessarCodigoDaParcela(ItemPedidoDTO itemPedido, ContratoDTO contrato, int numeroDaParcela)
        {
            if (itemPedido == null && contrato == null)
            {
                throw new ArgumentNullException("Informe o item pedido ou o contrato para gerar a parcela");
            }

            var verificador = (numeroDaParcela > 9) ? 0 : numeroDaParcela;

            if (contrato == null)
            {
                var codigoItemPedido = itemPedido.IPE_COD_LEGADO;
                var letra = AssinaturaUtil.GetLetraDoAlfabeto((int)numeroDaParcela);
                var codigoDaParcela = codigoItemPedido + letra + verificador;
                return codigoDaParcela;
            }
            else
            {
                var codigoContrato = contrato.CTR_NUM_CONTRATO;
                var letra = AssinaturaUtil.GetLetraDoAlfabeto((int)numeroDaParcela);
                var codigoDaParcela = codigoContrato + letra + verificador;

                return codigoDaParcela;
            }

        }


        private string ProcessarCodigoDaParcela(PropostaItemDTO propostaItem, int numeroDaParcela)
        {
            var verificador = (numeroDaParcela > 9) ? 0 : numeroDaParcela;

            if (propostaItem != null)
            {
                var codigoItemPedido = propostaItem.COD_LEGADO;
                var letra = AssinaturaUtil.GetLetraDoAlfabeto((int)numeroDaParcela);
                var codigoDaParcela = codigoItemPedido + letra + verificador;
                return codigoDaParcela;
            }

            return null;
        }
        /// <summary>
        /// Gera uma parcela baseado nos dados passados.
        /// Os dados padrões serão extraidos no contrato.
        /// Na ausência de contrado usa o item pedido. 
        /// Usa também os dados da forma de pagamento.
        /// Qualquer sobreposição dos dados podem ser realizados utilizando os 
        /// demais campos do DadosDeParcelaDTO
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public ParcelasDTO GerarParcela(DadosDeParcelaDTO dados)
        {
            if (dados.itemPedido != null && dados.pedidoPagamento != null)
            {
                var itemPedido = dados.itemPedido;
                var contrato = dados.contrato;
                var propostaItem = dados.propostaItem;
                var pedidoPagamento = dados.pedidoPagamento;
                var numeroDaParcela = dados.numeroDaParcela;
                var parcelaZerada = dados.parcelaZerada;
                var dataPagamento = dados.dataPagamento;
                var valorPago = dados.valorPago;
                var paga = dados.paga;
                var podeAlocar = dados.PodeAlocar;

                var entrada = DataUtil.ReturnNotNull(dados.entrada, pedidoPagamento.PGT_ENTRADA);
                var valorParcela = DataUtil.ReturnNotNull(dados.valorParcela, pedidoPagamento.PGT_VLR_PARCELA);
                var dataVencimentoParcela = DataUtil.ReturnNotNull(dados.dataVencimento, pedidoPagamento.PGT_DATA_VENCIMENTO);


                int? tipoPagamento = pedidoPagamento.TPG_ID;
                int? codigoPedidoPagamento = pedidoPagamento.PGT_ID;
                int? empId = ServiceFactory.RetornarServico<ItemPedidoSRV>().RetornaEmpIdDoItemPedido(itemPedido);
                int? iffId = dados.iffId;

                if (tipoPagamento != 7)
                {
                    podeAlocar = false;
                }

                string codigoDaParcela = null;

                if (itemPedido == null && contrato == null && propostaItem != null)
                    codigoDaParcela = ProcessarCodigoDaParcela(propostaItem, numeroDaParcela);
                else
                    codigoDaParcela = ProcessarCodigoDaParcela(itemPedido, contrato, numeroDaParcela);

                var parcela = new ParcelasDTO()
                {
                    PAR_NUM_PARCELA = codigoDaParcela,
                    EMP_ID = empId,
                    PAR_VLR_PARCELA = (valorParcela != null) ? valorParcela : pedidoPagamento.PGT_VLR_PARCELA,
                    PAR_MORA_MES = 4.0M,
                    PAR_SITUACAO = "NOR",
                    TPG_ID = tipoPagamento,
                    PGT_ID = codigoPedidoPagamento,
                    IPE_ID = itemPedido.IPE_ID,
                    PAR_PARCELA_DO_PEDIDO = true,
                    PAR_DATA_PAGTO = dataPagamento,
                    PAR_SEQ_PARCELA = numeroDaParcela,
                    PAR_PODE_ALOCAR = podeAlocar,
                    IFF_ID = iffId
                };

                if (contrato != null)
                {
                    parcela.CTR_NUM_CONTRATO = contrato.CTR_NUM_CONTRATO;
                    parcela.PAR_PARCELA_DO_PEDIDO = false;
                }

                if (dataVencimentoParcela != null)
                {
                    parcela.PAR_DATA_VENCTO = (DateTime)dataVencimentoParcela;
                }

                if (paga == true)
                {
                    parcela.PAR_CHAVE_TRANSACAO = pedidoPagamento.PGT_CHAVE_TRANSACAO;
                    parcela.PAR_CODIGO_DE_BARRAS = pedidoPagamento.PGT_CODIGO_DE_BARRAS;
                    parcela.PAR_URL_BOLETO = pedidoPagamento.PGT_URL_BOLETO;
                    parcela.PAR_STATUS_TRANSACAO = pedidoPagamento.PGT_STATUS_TRANSACAO;

                    parcela.PAR_VLR_PAGO = DataUtil.ReturnNotNull(valorPago, valorParcela);

                }
                return parcela;
            }

            return null;

        }

        /// <summary>
        /// Gera as parcelas baseadas nos dados informados sem levar em conta a entidade pedido pagamento.
        /// Geralmente utilizado por parcelas de proposta que não possui a entidade pedido pagamento.
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public ParcelasDTO GerarParcelaDadosSemPedidoPagamento(DadosDeParcelaDTO dados)
        {
            if (dados != null)
            {
                var itemPedido = dados.itemPedido;
                var contrato = dados.contrato;
                var propostaItem = dados.propostaItem;
                var numeroDaParcela = dados.numeroDaParcela;
                var dataPagamento = dados.dataPagamento;
                var paga = dados.paga;
                var dataVencimentoParcela = dados.dataVencimento;
                var valorParcela = dados.valorParcela;
                var podeAlocar = dados.PodeAlocar;
                var alocacaoAutomatica = dados.alocAutomatica;
                var iffId = dados.iffId;

                int? ipeId = null;
                int? seqParcelaAtual = _sequenciaSRV.GetSeqParcela();
                seqParcelaAtual++;

                string codigoDaParcela = StringUtil.PreencherZeroEsquerda((int)seqParcelaAtual, 8);

                dados.empId = (dados.empId != null) ? dados.empId : 1;

                if (dados.itemPedido != null)
                    ipeId = dados.itemPedido.IPE_ID;

                if (dados.tipoPagamento != 7)
                {
                    podeAlocar = false;
                    alocacaoAutomatica = false;
                }

                var parcela = new ParcelasDTO()
                {
                    PAR_NUM_PARCELA = codigoDaParcela,
                    EMP_ID = dados.empId,
                    PAR_VLR_PARCELA = dados.valorParcela,
                    PAR_MORA_MES = 4.0M,
                    PAR_SITUACAO = "NOR",
                    TPG_ID = dados.tipoPagamento,
                    PGT_ID = dados.PGT_ID,
                    IPE_ID = ipeId,
                    PAR_PARCELA_DO_PEDIDO = true,
                    PAR_DATA_PAGTO = dados.dataPagamento,
                    PAR_SEQ_PARCELA = numeroDaParcela,
                    PAR_ALOC_AUTOMATICA = alocacaoAutomatica,
                    PAR_PODE_ALOCAR = podeAlocar,
                    IFF_ID = iffId
                };

                _sequenciaSRV.AcrescentaSeqParcela((int)seqParcelaAtual);

                if (contrato != null)
                {
                    parcela.CTR_NUM_CONTRATO = contrato.CTR_NUM_CONTRATO;
                    parcela.PAR_PARCELA_DO_PEDIDO = false;
                }

                if (propostaItem != null)
                {
                    parcela.PPI_ID = propostaItem.PPI_ID;
                }

                if (dataVencimentoParcela != null)
                {
                    parcela.PAR_DATA_VENCTO = (DateTime)dataVencimentoParcela;
                }

                if (paga == true)
                {
                    parcela.PAR_CHAVE_TRANSACAO = dados.CHAVE_TRANSACAO;
                    parcela.PAR_CODIGO_DE_BARRAS = dados.CODIGO_DE_BARRAS;
                    parcela.PAR_URL_BOLETO = dados.URL_BOLETO;
                    parcela.PAR_STATUS_TRANSACAO = dados.STATUS_TRANSACAO;

                    parcela.PAR_VLR_PAGO = dados.valorPago;

                }
                return parcela;
            }

            return null;

        }


        /// <summary>
        /// Gera e inclui várias parcelas baseadas nos dados informados sem levar em conta a entidade pedido pagamento.
        /// </summary>
        /// <param name="dados"></param>
        public void GerarVariasParcelaDadosSemPedidoPagamento(IEnumerable<DadosDeParcelaDTO> dados)
        {
            if (dados != null && dados.Count() > 0)
            {
                var lstParcelas = new List<ParcelasDTO>();

                foreach (var dadoPar in dados)
                {
                    var par = GerarParcelaDadosSemPedidoPagamento(dadoPar);
                    lstParcelas.Add(par);
                }

                SaveOrUpdateNonIdentityKeyEntity(lstParcelas);
            }

        }

        public ParcelasDTO GerarParcelaPorDados(
            ItemPedidoDTO itemPedido,
            ContratoDTO contrato,
            PedidoPagamentoDTO pedidoPagamento,
            int numeroDaParcela,
            bool entrada = false,
            decimal? valorDaParcela = null,
            DateTime? dataVencimento = null,
            bool paga = false,
            decimal? valorPago = null,
            DateTime? dataPagamento = null,
            bool valorZerado = false,
            PropostaItemDTO propostaItem = null,
            bool podeAlocar = true)
        {
            if (itemPedido == null && contrato == null)
            {
                throw new ArgumentNullException("Informe o item de pedido ou contrato para processar as parcelas.");
            }


            int? iffId = (entrada == true) ? itemPedido.IFF_ID_ENTRADA : itemPedido.IFF_ID;

            if (iffId == null && entrada == true)
                iffId = itemPedido.IFF_ID;

            DadosDeParcelaDTO dados = new DadosDeParcelaDTO()
            {
                contrato = contrato,
                dataPagamento = dataPagamento,
                dataVencimento = dataVencimento,
                entrada = entrada,
                itemPedido = itemPedido,
                numeroDaParcela = numeroDaParcela,
                paga = paga,
                parcelaZerada = valorZerado,
                pedidoPagamento = pedidoPagamento,
                valorPago = valorPago,
                valorParcela = valorDaParcela,
                propostaItem = propostaItem,
                PodeAlocar = podeAlocar,
                iffId = iffId
            };

            var parcela = GerarParcela(dados);

            return parcela;
        }

        public bool ChecarParcelaJaAlocada(ParcelasDTO parcela)
        {
            if (parcela == null)
            {
                throw new NullReferenceException("A parcela passada é nula.");
            }

            return ChecarParcelaJaAlocada(parcela.PAR_NUM_PARCELA);
        }

        public string GerarLinkBoleto(string numParcela)
        {
            if (string.IsNullOrWhiteSpace(numParcela))
            {
                throw new ArgumentNullException("Informe o código da parcela");
            }

            string codBoleto = _cryptSRV.CriptografarTripleDES(numParcela);
            return codBoleto;
        }

        public bool ChecarParcelaJaAlocada(string codParcela)
        {
            return _dao.ChecarParcelaJaAlocada(codParcela);
        }


        [Obsolete("Não será mais utilizado")]
        public void LiberarParcelaParaAlocacao(ItemPedidoDTO itemPedido, ParcelasDTO parcela, RegiaoDTO regiao, ContaDTO conta, bool pagamentoGateway = false)
        {
            LiberarParcelaParaAlocacao(itemPedido, new List<ParcelasDTO>() { parcela }, regiao, conta, pagamentoGateway);
        }

        /// <summary>
        /// Adiciona as informações necessárias sobre banco para uma parcela ser alocada.
        /// </summary>
        /// <param name="propostaItem"></param>
        public void AdicionarContaAsParcelas(PropostaItemDTO propostaItem)
        {
            if (propostaItem != null)
            {
                int? empId = 1;

                if (propostaItem.PROPOSTA != null && propostaItem.PROPOSTA.EMP_ID != null)
                    empId = propostaItem.PROPOSTA.EMP_ID;

                var lstParcelas = ListarParcelaPorProposta(propostaItem.PPI_ID);

                ContaDTO conta = ServiceFactory.RetornarServico<ContaSRV>().BuscarContaBoletoAvuso();

                if (conta == null)
                {
                    throw new Exception(string.Format(
                            "Não é possível adicionar a conta. Nenhuma conta de primeira parcela foi encontrada para a empresa Informada {0}", empId
                        )
                    );
                }

                if (lstParcelas != null)
                {
                    foreach (var par in lstParcelas)
                    {
                        if (conta != null)
                        {
                            par.CTA_ID = conta.CTA_ID;
                            par.BAN_ID = conta.BAN_ID;
                            par.EMP_ID = conta.EMP_ID;
                        }
                    }

                    MergeAll(lstParcelas);
                }
            }
        }

        /// <summary>
        /// Adiciona as informações necessárias sobre banco para uma parcela ser alocada.
        /// </summary>
        /// <param name="parcela"></param>
        //public void AdicionarContaEGerarNossoNumero(ParcelasDTO parcela, PropostaItemDTO propostaItem)
        //{
        //    if (parcela != null)
        //    {
        //        int? empId = 1;
        //        if (propostaItem.PROPOSTA != null && propostaItem.PROPOSTA.EMP_ID != null)
        //            empId = propostaItem.PROPOSTA.EMP_ID;

        //        ContaDTO conta = ServiceFactory.RetornarServico<ContaSRV>().BuscarContaBoletoAvuso();

        //        if (conta == null)
        //        {
        //            throw new Exception(string.Format(
        //                    "Não é possível adicionar a conta. Nenhuma conta de primeira parcela foi encontrada para a empresa Informada {0}", empId
        //                )
        //            );
        //        }

        //        parcela.CTA_ID = conta.CTA_ID;
        //        parcela.BAN_ID = conta.BAN_ID;
        //        parcela.EMP_ID = conta.EMP_ID;

        //        if (string.IsNullOrWhiteSpace(parcela.PAR_NOSSO_NUMERO))
        //        {
        //            parcela.PAR_SEG_VIA = false;
        //            string nossoNumero = ServiceFactory.RetornarServico<BoletoSRV>().GerarNossoNumero(parcela.BAN_ID, parcela.PAR_NUM_PARCELA, true, true);
        //            parcela.PAR_NOSSO_NUMERO = nossoNumero;
        //        }
        //        else
        //        {
        //            parcela.PAR_SEG_VIA = true;
        //        }
        //        Merge(parcela);

        //    }
        //}

        /// <summary>
        /// Libera a parcela colocando os dados necessários para a sua alocação
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <param name="numeroParcela"></param>
        /// <param name="regiao"></param>
        /// <param name="conta"></param>
        /// <param name="pagamentoGateway"></param>
        [Obsolete("Não será mais utilizado")]
        public void LiberarParcelaParaAlocacao(ItemPedidoDTO itemPedido, IList<ParcelasDTO> lstParcelas, RegiaoDTO regiao, ContaDTO conta, bool pagamentoGateway = false)
        {
            if (lstParcelas == null)
            {
                throw new NullReferenceException("A parcela não pode ser nula.");
            }

            if (regiao == null)
            {
                throw new NullReferenceException("A região não pode ser nula.");
            }

            if (conta == null)
            {
                throw new NullReferenceException("A conta não pode ser nula.");
            }

            foreach (var parcela in lstParcelas)
            {
                var alocada = ChecarParcelaJaAlocada(parcela);
                if (!alocada)
                {
                    parcela.PAR_PODE_ALOCAR = !pagamentoGateway;
                    parcela.CTA_ID = conta.CTA_ID;

                    var contrato = parcela.CONTRATOS;
                    pagamentoGateway = DataUtil.ReturnNotNull(pagamentoGateway, false);
                    if (contrato != null)
                    {
                        contrato.CTR_PAGAMENTO_GATEWAY = pagamentoGateway;
                        new ContratoSRV().SaveOrUpdate(contrato);
                    }
                    else
                    {
                        itemPedido.IPE_PAGAMENTO_GATEWAY = pagamentoGateway;
                        new ItemPedidoSRV().SaveOrUpdate(itemPedido);
                    }
                }
            }

        }

        [MetodoTopLevelReferenciavel]
        public ParcelasDTO
            PrepararParcelaGateway(ItemPedidoDTO itemPedido, RegiaoDTO regiao,
            bool pagamentoGateway = false,
            ContaDTO conta = null,
            ParcelasDTO parcela = null)
        {
            if (parcela != null)
            {
                return PrepararParcelasGateway(itemPedido, regiao, pagamentoGateway, conta, new List<ParcelasDTO>() { parcela }).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Recupera a conta necessária para a alocação do pagamento. <para></para>
        /// Libera a parcela colocando os dados necessários para a seu pagamento.
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <param name="regiao"></param>
        /// <param name="conta"></param>
        /// <param name="pagamentoGateway"></param>
        /// <returns></returns>
        [MetodoTopLevelReferenciavel]
        public IList<ParcelasDTO> PrepararParcelasGateway(ItemPedidoDTO itemPedido, RegiaoDTO regiao,
            bool pagamentoGateway = false,
            ContaDTO conta = null,
            IList<ParcelasDTO> lstParcelas = null)
        {
            int? IPE_ID = itemPedido.IPE_ID;

            var tipoMeioPagamento = (pagamentoGateway == true) ? TipoAmbientePagamento.GATEWAY_PAGAMENTO : TipoAmbientePagamento.INTERNO;
            if (regiao == null)
            {
                var pedido = new PedidoCRMSRV().ChecarEPreencherPedido(itemPedido);
                regiao = new RegiaoSRV().FindById(pedido.RG_ID);
            }

            conta = _configAlocacaoSRV.RetornaPorTipo(tipoMeioPagamento);

            var tipoPagamento = new ItemPedidoSRV().ObterPedidoPagamentoAtual(IPE_ID);
            if (lstParcelas == null)
            {
                lstParcelas = ObterProximasParcelasAPagar(IPE_ID);
            }

            if (lstParcelas != null)
            {

                foreach (var parcela in lstParcelas)
                {
                    parcela.CTA_ID = (conta != null) ? conta.CTA_ID : null;

                    if (conta != null)
                    {
                        parcela.CTA_ID = conta.CTA_ID;
                        parcela.BAN_ID = conta.BAN_ID;
                    }
                }

                SaveOrUpdateAll(lstParcelas);
                return lstParcelas;
            }

            return null;
        }


        public IList<ParcelasDTO> PrepararParcelasGateway(
            IList<ParcelasDTO> lstParcelas, bool pagamentoGateway = false)
        {
            var tipoMeioPagamento = (pagamentoGateway == true) ? TipoAmbientePagamento.GATEWAY_PAGAMENTO : TipoAmbientePagamento.INTERNO;

            ContaDTO conta = _configAlocacaoSRV.RetornaPorTipo(tipoMeioPagamento);

            if (lstParcelas != null)
            {
                foreach (var parcela in lstParcelas)
                {
                    parcela.CTA_ID = (conta != null) ? conta.CTA_ID : null;

                    if (conta != null)
                    {
                        parcela.CTA_ID = conta.CTA_ID;
                        parcela.BAN_ID = conta.BAN_ID;
                    }
                }

                SaveOrUpdateAll(lstParcelas);
                return lstParcelas;
            }

            return null;
        }
        /// <summary>
        /// Verifica se um pedido pendente (status = 1) possui alguma parcela. 
        /// Se não possuí. 
        /// Provavelmente foi gerado errado. 
        /// E as mesmas devem ser criadas.
        /// </summary>
        /// <param name="IPE_ID"></param>
        /// <returns></returns>
        public bool ChecarPedidoPendentePossuiParcelas(int? IPE_ID)
        {
            return _dao.ChecarPedidoPendentePossuiParcelas(IPE_ID);
        }

        public ParcelasLegadoDTO BuscarLegado(string numParcela)
        {
            var parcelaCorp = FindById(numParcela);
            var parNumeroArg = numParcela;

            if (!string.IsNullOrWhiteSpace(parcelaCorp.PAR_COD_LEGADO))
            {
                parNumeroArg = parcelaCorp.PAR_COD_LEGADO;
            }
            var parcelaLegada = _serviceParcelasLegado.FindById(parNumeroArg.Substring(0, 6), parNumeroArg.Substring(6, 1), parNumeroArg.Substring(7, 1));
            return parcelaLegada;
        }

        public void PagarVariasParcelasPorCodigo(IList<string> lstNumeroParcelas, List<ParcelaLiquidacaoDTO> lstParcelasLiquidacao, List<ParcelaAlocadaUpdateDTO> lstParcelasAlocadas, bool darBaixa = false)
        {
            using (var scope = new TransactionScope())
            {
                var lstParcela = FindByIdList(lstNumeroParcelas);
                PagarVariasParcelas(lstParcela, lstParcelasLiquidacao, lstParcelasAlocadas, darBaixa);

                scope.Complete();
            }
        }


        public ParcelasDTO ObterProximaParcelaDaPropostaEmAberto(int? ppiId)
        {
            return _dao.ObterProximaParcelaDaPropostaEmAberto(ppiId);
        }

        /// <summary>
        /// Utilizado para dar notificar a baixa de várias parcelas. 
        /// Pode ser utilizado na baixa das parcelas no momento do retorno do banco.
        /// </summary>
        /// <param name="lstParcelas"></param>
        /// <param name="darBaixa"></param>
        [MetodoTopLevel]
        public void PagarVariasParcelas(IList<ParcelasDTO> lstParcelas, List<ParcelaLiquidacaoDTO> lstParcelasLiquidacao, List<ParcelaAlocadaUpdateDTO> lstParcelasAlocadas, bool darBaixa = false,
                                        List<ParcelaUpdateDTO> listaDeParcelasUpdateDB = null) // ALT: 16/02/2017 - novo parâmetro para salvar direto no BANCO \\
        {
            if (lstParcelas != null)
            {
                // Obter todos os códigos de propostas agrupados.
                var lstParProposta = lstParcelas
                    .Where(x => x.IPE_ID == null &&
                        x.PPI_ID != null)
                    .OrderBy(or => or.PPI_ID);

                // Obter todos os códigos de pedido agrupados.
                var lstIpe = lstParcelas
                    .Where(x => x.IPE_ID != null)
                    .OrderBy(or => or.IPE_ID)
                    .Select(sel => sel.IPE_ID)
                    .Distinct();

                var _itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();

                if (lstParProposta != null && lstParProposta.Count() > 0)
                {
                    PropostaItemSRV propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();
                    propostaItemSRV.PagarPropostas(lstParProposta, 1, "CoadCorp");
                }

                foreach (var ipe in lstIpe)
                {
                    var dadosDePagamento = _itemPedidoSRV.RetornarDadosBasicosDePagamento(ipe);
                    dadosDePagamento.darBaixar = false;
                    dadosDePagamento.lstParcelasAPagar = lstParcelas.Where(x => x.IPE_ID == ipe).ToList();
                    dadosDePagamento.tipoPagamentoGateway = TipoPagamentoGateway.PARCELA_INFORMADA;
                    dadosDePagamento.lstParcelasAlocadas = lstParcelasAlocadas;
                    dadosDePagamento.lstParcelasLiquidacao = lstParcelasLiquidacao;

                    _itemPedidoSRV.PagarPedido(dadosDePagamento);

                }

                // dando baixa nas parcelas que não pertence a nenhum pedido.
                var lstParcelasSemPedidos = lstParcelas.Where(x => x.IPE_ID == null && x.PPI_ID == null).ToList();
                List<ParcelaUpdateDTO> lstParcelasUpdateDB = null;

                if (listaDeParcelasUpdateDB != null)
                    lstParcelasUpdateDB = listaDeParcelasUpdateDB.Where(x => lstParcelasSemPedidos.Any(y => x.PAR_NUM_PARCELA == y.PAR_NUM_PARCELA)).ToList();

                BaixarTitulos(lstParcelasSemPedidos, lstParcelasLiquidacao, lstParcelasAlocadas, true, lstParcelasUpdateDB);
            }
        }

        public ParcelasDTO ObterUltimaParcelaDoPedido(int? IPE_ID, int? PGT_ID = null)
        {
            return _dao.ObterUltimaParcelaDoPedido(IPE_ID, PGT_ID);
        }

        public IList<ParcelasDTO> FindByIdList(IEnumerable<string> lstNumeroParcelas)
        {
            return _dao.FindByIdList(lstNumeroParcelas);
        }

        /// <summary>
        /// Pega a parcela e prepara ela para ser paga pelo gateway
        /// </summary>
        /// <param name="IPE_ID"></param>
        public void PrepararParcelaGateway(int? IPE_ID)
        {
            var itemPedido = new ItemPedidoSRV().FindById(IPE_ID);
            if (itemPedido != null)
            {
                PrepararParcelaGateway(itemPedido, null, true);
            }
        }

        public IList<ParcelasDTO> ListarParcelaPorProposta(int? ppiId)
        {
            return _dao.ListarParcelaPorProposta(ppiId);
        }

        /// <summary>
        /// Checa se a proposta possui uma ou várias parcelas
        /// Se houver exclui.
        /// </summary>
        public void ChecarEExcluirParcelaNaProposta(int? ppiId)
        {
            var lstParcelas = ListarParcelaPorProposta(ppiId);

            var lstCodParcela = lstParcelas.Select(x => x.PAR_NUM_PARCELA);

            //// Separando as parcelas que ainda não foram alocadas das que já foram e não podem ser excluídas fisicamente
            //var lstExclusaoFisica = lstParcelas.Where(x => x.REM_ID == null && (x.PAR_VLR_PAGO == null || x.PAR_DATA_PAGTO == null));
            //var lstExclusaoLogica = lstParcelas.Where(x => x.REM_ID != null && (x.PAR_VLR_PAGO != null || x.PAR_DATA_PAGTO != null));

            //if (lstExclusaoFisica != null && lstExclusaoFisica.Count() > 0)
            //    DeleteAll(lstParcelas);

            if (lstParcelas != null && lstParcelas.Count() > 0)
            {
                MarcarParcelasComoExcluidas(lstParcelas);
            }
        }

        /// <summary>
        /// Insere uma data de exclusão na parcela. Isso indica ao sistema que essa parcela foi excluída.
        /// </summary>
        /// <param name="lstParcelas"></param>
        public void MarcarParcelasComoExcluidas(IEnumerable<ParcelasDTO> lstParcelas)
        {

            if (lstParcelas != null)
            {
                foreach (var par in lstParcelas)
                {
                    par.DATA_EXCLUSAO = DateTime.Now;
                }

                MergeAll(lstParcelas);
            }
        }

        public bool HasParcelaNaProposta(int? ppiId)
        {
            return _dao.HasParcelaNaProposta(ppiId);
        }

        /// <summary>
        /// Verifica se o valor da parcela no item foi alterado, e promove a alteração nas parcelas geradas.
        /// </summary>
        /// <param name="propostaItem"></param>
        public void AtualizarValorParcela(PropostaItemDTO propostaItem)
        {
            var valor = (propostaItem.TIPO_PAGAMENTO.TPG_TIPO == 1)
                ? propostaItem.PPI_VALOR_ENTRADA :
                propostaItem.PPI_VALOR_PARCELA;

            var iffId = (propostaItem.TIPO_PAGAMENTO.TPG_TIPO == 1)
                ? propostaItem.IFF_ID_ENTRADA :
                propostaItem.IFF_ID;

            var lstParcelas = ListarParcelaPorProposta(propostaItem.PPI_ID);
            var dataVencimento = propostaItem.PPI_DATA_VENCIMENTO;

            if (propostaItem.PST_ID == 1)
            {
                var lstParcelasParaAlterar = new List<ParcelasDTO>();
                if (lstParcelas != null && lstParcelas.Count() > 0)
                {
                    foreach (var par in lstParcelas)
                    {
                        if (par.PAR_VLR_PARCELA != valor)
                        {
                            par.IFF_ID = iffId;
                            par.PAR_VLR_PARCELA = valor;

                            if (dataVencimento != null)
                                par.PAR_DATA_VENCTO = (DateTime)dataVencimento;

                            lstParcelasParaAlterar.Add(par);
                        }
                    }
                    MergeAll(lstParcelasParaAlterar);
                }
            }
            else if (propostaItem.PST_ID == 7)
            {
                foreach (var par in lstParcelas)
                {
                    if (par.PAR_VLR_PARCELA != valor)
                        throw new InvalidOperationException("Erro. O pedido está pago, mas o valor da parcela não corresponde ao valor do item da proposta. Não é possível alterar o valor da proposta após a mesma já está paga.");
                }
            }
        }


        public string ValidarCodigoERetornarHashBoleto(string codigo)
        {
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                var codigoSemVerificador = codigo.Substring(0, codigo.Length - 1);
                var verificadorPassado = int.Parse(codigo.Substring(codigo.Length - 1, 1));
                var verificadorCalculado = MathUtil.CalcularDigitoVerificador32Digitos(codigoSemVerificador);

                if (verificadorPassado == verificadorCalculado)
                {
                    return codigoSemVerificador;
                }
                throw new Exception("O código informado não é consistente.");
            }
            else
            {
                throw new ArgumentNullException("Informe o código");
            }
        }

        public int? RetornarCodigoDoCliente(ParcelasDTO parcela)
        {
            if (parcela != null)
            {
                if (parcela.IPE_ID != null)
                {
                    return ServiceFactory
                        .RetornarServico<PedidoCRMSRV>()
                        .RetornarCliIdDoPedidoPorItemPedido(parcela.IPE_ID);
                }
                else if (parcela.PPI_ID != null)
                {
                    return ServiceFactory
                        .RetornarServico<PropostaSRV>()
                        .RetornarCliIdDaPropostaPorPropostaItem(parcela.PPI_ID);
                }
            }

            return null;
        }

        public void PagarParcelaDaProposta(int? ppiId, bool baixaManual = false)
        {
            if (!ExisteParcelasPagas(null, ppiId))
            {
                var parcela = ObterProximaParcelaDaPropostaEmAberto(ppiId);

                if (parcela != null)
                {
                    if (baixaManual)
                        parcela.PAR_BAIXA_MANUAL = true;
                    parcela.PAR_VLR_PAGO = parcela.PAR_VLR_PARCELA;
                    parcela.PAR_DATA_PAGTO = DateTime.Now;

                    Merge(parcela);
                    BaixarTitulos(new List<ParcelasDTO>() { parcela }, new List<ParcelaLiquidacaoDTO>(), new List<ParcelaAlocadaUpdateDTO>(), true);
                }
            }
        }


        public void PagarParcelaDaProposta(ParcelasDTO parcela, bool baixaManual = false)
        {
            if (!ExisteParcelasPagas(null, parcela.PPI_ID))
            {
                if (parcela != null)
                {
                    if (parcela.PAR_VLR_PAGO == null)
                        parcela.PAR_VLR_PAGO = parcela.PAR_VLR_PARCELA;

                    if (parcela.PAR_DATA_PAGTO == null)
                        parcela.PAR_DATA_PAGTO = DateTime.Now;

                    if (baixaManual)
                        parcela.PAR_BAIXA_MANUAL = true;

                    Merge(parcela);
                    BaixarTitulos(new List<ParcelasDTO>() { parcela }, new List<ParcelaLiquidacaoDTO>(), new List<ParcelaAlocadaUpdateDTO>(), true);
                }
            }
        }

        public string GerarPDFDoBoleto(string hash, string path = null)
        {
            try
            {
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    var numParcela = _cryptSRV.DescriptografarTripleDES(hash);


                    var parcela = FindById(numParcela);
                    int? cliId = RetornarCodigoDoCliente(parcela);

                    ParametroDTO paramBoleto = new ParametroDTO()
                    {
                        idCliente = (int)cliId,
                        idConta = (int)parcela.CTA_ID,
                        idEmpresa = (int)parcela.EMP_ID,
                        idTitulo = parcela.PAR_NUM_PARCELA,
                        preAlocado = true,
                        idRemessa = "01",
                    };

                    var lstBytes = ServiceFactory
                        .RetornarServico<BoletoSRV>()
                        .GerarVariosBoletosPDF(new List<ParametroDTO> { paramBoleto });

                    string fileName = string.Format(@"boleto_{0:yyyy-MM-ddTH-mm}.xml", DateTime.Now);

                    string fullPath = Path.Combine(path, "temp", fileName);

                    File.WriteAllBytes(fullPath, lstBytes);
                    scope.Complete();


                    return fullPath;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível gerar o boleto.", e);
            }
        }

        public ParametroDTO RetornarDadosDoBoleto(string numParcela)
        {
            return _dao.RetornarDadosDoBoleto(numParcela);
        }

        public IList<ParcelasDTO> ListarParcelasALiberarAcesso()
        {
            return _dao.ListarParcelasALiberarAcesso();
        }

        public void GerarParcelaLiquidacaoLegado(IEnumerable<ParcelaLiquidacaoDTO> lstParcelaLiquidacao)
        {
            var lstLiqLegado = new List<LiquidacaoLegadoDTO>();

            // liquidacao legada \\
            foreach (var liq in lstParcelaLiquidacao)
            {
                LiquidacaoLegadoDTO liqLegado = new LiquidacaoLegadoDTO();

                DateTime dtBaixa = (DateTime)liq.PLI_DATA_BAIXA;
                DateTime dt = (DateTime)liq.PLI_DATA;
                DateTime dtBordero = liq.PLI_DATA_BORDERO != null ? (DateTime)liq.PLI_DATA_BORDERO : DateTime.Now;

                liqLegado.CONTRATO = liq.PAR_NUM_PARCELA.Substring(0, 6);
                liqLegado.LETRA = liq.PAR_NUM_PARCELA.Substring(6, 1);
                liqLegado.CD = liq.PAR_NUM_PARCELA.Substring(7, 1);
                liqLegado.BANCO = liq.BAN_ID;
                liqLegado.DT_BORDERO = dtBordero.ToString("dd/MM/yyyy");
                liqLegado.IDENT_DOCTO = liq.PAR_NUM_PARCELA;
                liqLegado.DATA_DA_BAIXA = dtBaixa.ToString("dd/MM/yyyy");
                liqLegado.DATA = dt.ToString("dd/MM/yyyy");
                liqLegado.NUMERO = liq.PLI_NUMERO;
                liqLegado.ORIGEM_PGTO = liq.PLI_ORIGEM_PGTO;
                liqLegado.TIPO_DOC = liq.PLI_TIPO_DOC;
                liqLegado.VALOR = liq.PLI_VALOR.ToString().Replace('.', ',');

                lstLiqLegado.Add(liqLegado);
            }

            // salvando legado \\
            _serviceLiquidacaoLegado.SalvarLiquidacaoLegado(lstLiqLegado);
        }


        /// <summary>
        /// Pega todas as parcelas pendentes e dá baixa na proposta
        /// </summary>
        public void ProcessarBaixaPropostaPedido(bool considerarData = true, BatchContext batchContext = null)
        {
            if (batchContext == null)
                batchContext = new BatchContext();

            HistoricoExecucaoSRV _historicoExecucao = ServiceFactory.RetornarServico<HistoricoExecucaoSRV>();

            DateTime? hoje = null; //= DateTime.Now;
            DateTime? inicial = null;// = hoje.Subtract(TimeSpan.FromDays(2));

            if (considerarData)
            {
                hoje = DateTime.Now;
                inicial = hoje.Value.Subtract(TimeSpan.FromDays(2));
            }

            try
            {
                var lstParcelas = ListarParcelasALiberarAcesso();

                var propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();
                var type = GetType();
                var nomeClasse = type.FullName;
                var nomeAssembly = type.AssemblyQualifiedName;

                if (lstParcelas != null && lstParcelas.Count() > 0)
                {
                    batchContext.IniciarPassoBatch("Baixando as propostas", true, lstParcelas.Count);
                    foreach (var par in lstParcelas)
                    {
                        int? codigoPropostaItem = par.PPI_ID;
                        int? codigoProposta = null;
                        string codigoParcela = par.PAR_NUM_PARCELA;

                        var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(par.PPI_ID);
                        if (propostaItem != null)
                        {
                            codigoProposta = propostaItem.PRT_ID;
                        }

                        try
                        {
                            if (par.PPI_ID != null)
                            {

                                using (var scope = new TransactionScope())
                                {
                                    _historicoExecucao.Incluir("Processamento da parcela", string.Format("Processando baixa na proposta/pedido: PropostaItem de código'{0}'; Parcela de Código {1}", par.PPI_ID, par.PAR_NUM_PARCELA), DateTime.Now, nomeClasse, nomeAssembly);
                                    propostaItemSRV.PagarProposta(par.PPI_ID, 1, "COADSYS", par, false);
                                    _historicoExecucao.Incluir("Processamento da parcela", string.Format("Baixa processada para a proposta/pedido PropostaItem de código'{0}'; Parcela de Código {1}", par.PPI_ID, par.PAR_NUM_PARCELA), DateTime.Now, nomeClasse, nomeAssembly);
                                    scope.Complete();
                                }
                            }
                        }

                        catch (Exception e)
                        {
                            string message = string.Format("Ocorreu uma falha na execução! Proposta Item {0} Proposta {1} Código da parcela processada. {2}", codigoPropostaItem, codigoProposta, codigoParcela);
                            _historicoExecucao.Incluir("Processamento da parcela", message, DateTime.Now, nomeClasse, nomeAssembly, e);
                        }
                        finally
                        {
                            batchContext.IncrementarPassoBatch();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ocorreu um erro ao tentar processar a baixa das propostas.", e);
            }

        }

        /// <summary>
        /// Verifica no pedido ou na proposta se há parcelas pagas.
        /// </summary>
        /// <param name="ipeId"></param>
        /// <param name="ppiId"></param>
        /// <returns></returns>
        public bool ExisteParcelasPagas(int? ipeId = null, int? ppiId = null)
        {
            return _dao.ExisteParcelasPagas(ipeId, ppiId);
        }

        public void CancelarParcelasDoContrato(string codContrato)
        {
            var lstParcelas = BuscarPorContrato(codContrato);
            MarcarParcelasComoExcluidas(lstParcelas);
        }

        public ICollection<ParcelasDTO> ListarParcelasPreFaturamentoDoPedido(int? ipeId)
        {
            return _dao.ListarParcelasPreFaturamentoDoPedido(ipeId);
        }

        public void CancelarParcelasPreFaturamentoDoPedido(int? ipeId)
        {
            if (ipeId != null)
            {
                var lstParcelas = ListarParcelasPreFaturamentoDoPedido(ipeId);
                MarcarParcelasComoExcluidas(lstParcelas);
            }
        }

        /// <summary>
        /// Retorna as parcelas pagas do contrato antigo e baixa as parcelas do contrato de transferência
        /// </summary>
        /// <param name="contratoAntigo"></param>
        /// <param name="lstParcelasContratoNovo"></param>
        public void PagarParcelasDoContratoTransferido(ContratoDTO contratoAntigo, ContratoDTO contratoTransferencia)
        {
            if (contratoAntigo == null)
                throw new ArgumentNullException("Informe o contrato antigo.");
            if (contratoTransferencia == null)
                throw new ArgumentNullException("Informe o contrato de transferência");

            var lstParcelasPagas = ListarParcelasPagasDoContrato(contratoAntigo.CTR_NUM_CONTRATO);

            if (lstParcelasPagas != null && lstParcelasPagas.Count() > 0)
            {
                var lstSeqParcelas = lstParcelasPagas.Select(x => x.PAR_SEQ_PARCELA);
                var lstParcelasABaixar = ListarParcelasContratoPorSequenciaParcela(contratoTransferencia.CTR_NUM_CONTRATO, lstSeqParcelas);

                if (lstParcelasABaixar != null && lstParcelasABaixar.Count() > 0)
                {
                    PagarVariasParcelas(lstParcelasABaixar.ToList(), null, null, true);
                }

            }
        }

        public ICollection<ParcelasDTO> ListarParcelasPagasDoContrato(string codContrato)
        {
            return _dao.ListarParcelasPagasDoContrato(codContrato);
        }

        public ICollection<ParcelasDTO> ListarParcelasContratoPorSequenciaParcela(string codContrato, IEnumerable<int?> lstSeqParcela)
        {
            return _dao.ListarParcelasContratoPorSequenciaParcela(codContrato, lstSeqParcela);
        }

        public void ExtornarVariasParcelas(ICollection<ItemParcelaExtornoDTO> lstCodParcelas, int? repId, string usuLogin)
        {
            if (lstCodParcelas != null)
            {
                var lstCodigos = lstCodParcelas.Select(x => x.numParcela).ToList();
                var lstParcelas = FindByIdList(lstCodigos);
                ExtornarVariasParcelas(lstParcelas, repId, usuLogin);
            }
        }

        public void ExtornarVariasParcelas(ICollection<string> lstCodParcelas, int? repId, string usuLogin)
        {
            var lstParcelas = FindByIdList(lstCodParcelas);
            ExtornarVariasParcelas(lstCodParcelas, repId, usuLogin);
        }

        public void ExtornarVariasParcelas(ICollection<ParcelasDTO> lstParcelas, int? repId, string usuLogin)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (lstParcelas != null && lstParcelas.Count() > 0)
                    {
                        var pedidosDaParcela = lstParcelas.Where(x => x.IPE_ID != null).Select(x => x.IPE_ID);
                        var propostasDaParcela = lstParcelas.Where(x => x.IPE_ID == null && x.PPI_ID != null).Select(x => x.PPI_ID);
                        var contratosParSemPedidoEProposta = lstParcelas.Where(x => x.IPE_ID == null && x.PPI_ID == null).Select(x => x.CTR_NUM_CONTRATO);

                        if (pedidosDaParcela != null && pedidosDaParcela.Count() > 0)
                        {
                            foreach (var ipeId in pedidosDaParcela)
                            {
                                var itemPedido = ServiceFactory.RetornarServico<ItemPedidoSRV>().FindById(ipeId);
                                if (itemPedido != null)
                                {
                                    var pedido = ServiceFactory.RetornarServico<PedidoCRMSRV>().FindById(itemPedido.PED_CRM_ID);
                                    var parcelasDoPedido = lstParcelas.Where(x => x.IPE_ID == ipeId).ToList();

                                    ExtornarVariasParcelasPorPedidoOuProposta(parcelasDoPedido, repId, pedido.CLI_ID, usuLogin, ipeId, itemPedido.PPI_ID, itemPedido.PST_ID);
                                }

                            }
                        }

                        if (propostasDaParcela != null && propostasDaParcela.Count() > 0)
                        {
                            foreach (var ppiId in propostasDaParcela)
                            {
                                var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(ppiId);
                                if (propostaItem != null)
                                {
                                    var proposta = ServiceFactory.RetornarServico<PropostaSRV>().FindById(propostaItem.PRT_ID);
                                    var parcelasDaProposta = lstParcelas.Where(x => x.PPI_ID == ppiId).ToList();

                                    ExtornarVariasParcelasPorPedidoOuProposta(parcelasDaProposta, repId, proposta.CLI_ID, usuLogin, null, ppiId, proposta.PST_ID);
                                }

                            }
                        }

                        if (contratosParSemPedidoEProposta != null && contratosParSemPedidoEProposta.Count() > 0)
                        {
                            foreach (var ctr in contratosParSemPedidoEProposta)
                            {
                                var contratos = ServiceFactory.RetornarServico<ContratoSRV>().FindById(ctr);
                                if (contratos != null)
                                {
                                    var assinatura = ServiceFactory.RetornarServico<AssinaturaSRV>().FindById(contratos.ASN_NUM_ASSINATURA);
                                    var parcelasDoContrato = lstParcelas.Where(x => x.CTR_NUM_CONTRATO == ctr).ToList();

                                    ExtornarVariasParcelasPorPedidoOuProposta(parcelasDoContrato, repId, assinatura.CLI_ID, usuLogin, null, null, null);
                                }

                            }
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new ExtornoException("Ocorreu um erro ao tentar extornar o pagamento da parcela", e);
            }
        }

        public void ExtornarVariasParcelasPorPedidoOuProposta(ICollection<ParcelasDTO> lstParcelas, int? repId, int? cliId, string usuLogin, int? ipeId, int? ppiId, int? pstId)
        {
            if (lstParcelas != null && lstParcelas.Count() > 0)
            {
                ExtornoParcelaHistoricoDTO historicoExtornoParcela = new ExtornoParcelaHistoricoDTO();

                foreach (var par in lstParcelas)
                {
                    if (par.PAR_VLR_PAGO == null && par.PAR_DATA_PAGTO == null)
                    {
                        throw new ExtornoException("A parcela '{0}' não contém informações de pagamento");
                    }

                    historicoExtornoParcela.Usuario = usuLogin;
                    historicoExtornoParcela.CliId = cliId;
                    historicoExtornoParcela.RepId = repId;
                    historicoExtornoParcela.PpiId = ppiId;
                    historicoExtornoParcela.IpeId = ipeId;
                    historicoExtornoParcela.PstId = pstId;

                    historicoExtornoParcela.Items.Add(new ExtornoParcelaHistoricoItemDTO()
                    {
                        CodParcela = par.PAR_NUM_PARCELA,
                        DataPagamento = par.PAR_DATA_PAGTO,
                        ValorPago = par.PAR_VLR_PAGO

                    });

                    par.PAR_DATA_PAGTO = null;
                    par.PAR_VLR_PAGO = null;
                    Merge(par);
                    _serviceLiquidacao.RemoverLiquidacoes(par.PAR_NUM_PARCELA);

                    var parLegado = this.BuscarLegado(par.PAR_NUM_PARCELA);
                    if (parLegado != null)
                    {
                        parLegado.DT_PAGTO = null;
                        parLegado.VLR_PAGO = null;
                        _serviceParcelasLegado.Merge(parLegado);
                        _serviceLiquidacaoLegado.RemoverLiquidacaoLegado(parLegado.CONTRATO, parLegado.LETRA, parLegado.CD);
                    }
                }

                ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>().RegistrarHistoricoParcelaExtornada(historicoExtornoParcela);
            }
        }

        public Pagina<ParcelasDTO> ListarParcelasPagas(string contrato = null,
           int? ppiId = null,
           int? ipeId = null,
           int pagina = 1,
           int registrosPorPagina = 7)
        {
            return _dao.ListarParcelasPagas(contrato, ppiId, ipeId, pagina, registrosPorPagina);
        }

        /// <summary>
        /// Retorna todas as parcelas que já foram faturadas (estão associadas a um contrato) e que não possuem mais de 5 anos
        /// </summary>
        /// <param name="cliId"></param>
        /// <param name="data"></param>
        /// <param name="prtIdExcluir"></param>
        /// <returns></returns>
        public IList<ParcelasDTO> ListarParcelasFaturadasEmAberto(int cliId, DateTime data, int? prtIdExcluir = null)
        {
            return _dao.ListarParcelasFaturadasEmAberto(cliId, data, prtIdExcluir);
        }

        public IList<ParcelasDTO> ListarParcelasEmAbertoDaAssinatura(string assinatura, DateTime data)
        {
            return _dao.ListarParcelasEmAbertoDaAssinatura(assinatura, data);
        }

        public IList<ParcelasDTO> ListarParcelaPorPedido(int? ipeID)
        {
            return _dao.ListarParcelaPorPedido(ipeID);
        }

        public IList<ParcelasDTO> ListarParcelasParaEnvioBoletoDasContas(IList<int> lstCTAId)
        {
            return _dao.ListarParcelasParaEnvioBoletoDasContas(lstCTAId);
        }

        public IList<ParcelasDTO> ListarParcelasParaEnvioBoleto()
        {
            var lstCtaIDValidos = _srvConta.ListarIDContasEnviaBoleto();
            var lstPar = ListarParcelasParaEnvioBoletoDasContas(lstCtaIDValidos);
            return lstPar;
        }

        public DetalhesEnvioBoletoDTO RetornarDetalhesDoEnvioDeBoleto(string codParcela)
        {
            return _dao.RetornarDetalhesDoEnvioDeBoleto(codParcela);
        }

        public void EnviarBoletosEmAberto(BatchContext batchContext = null)
        {
            try
            {
                if (batchContext == null)
                {
                    batchContext = new BatchContext();
                    batchContext.JobID = 14;
                }

                var lstParcelas = ListarParcelasParaEnvioBoleto();

               if (lstParcelas != null && lstParcelas.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Enviando o boleto por e-mail...", true, lstParcelas.Count);
                    foreach (var par in lstParcelas)
                    {
                        try
                        {
                            var detalhes = RetornarDetalhesDoEnvioDeBoleto(par.PAR_NUM_PARCELA);

                            if(detalhes != null)
                            {
                                var _templateHTMLSRV = ServiceFactory.RetornarServico<TemplateHTMLSRV>();
                                var _assinaturaEmail = ServiceFactory.RetornarServico<AssinaturaEmailSRV>();

                                using (var scope = new TransactionScope())
                                {
                                    string email = null;
                                    var emailObj = _assinaturaEmail.RetornarEmailDeContato(detalhes.CliId);

                                    if (emailObj != null)
                                        email = emailObj.AEM_EMAIL;

                                    string assunto = "[COAD] - Boleto Disponível";
                                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(13, detalhes);

                                    email = SysUtils.DecidirEnderecoDeEmail(email);

                                    var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                                    emailSRV.EnviarEmail(new EmailRequestDTO()
                                    {
                                        Assunto = assunto,
                                        CorpoEmail = corpoEmail,
                                        EmailDestino = email,
                                        codSMTP = 3,
                                        ActionArg = par.PAR_NUM_PARCELA,
                                        ActionName = "emailPropostaBoleto",
                                        usuario = "COADSYS",
                                        CallbackContextKeyStr = par.PAR_NUM_PARCELA,
                                        SuccessCallback = "envBoletoAuto",
                                        FailCallback = "envBoletoAuto"
                                    });

                                    par.PAR_DATA_AGEN_ENVIO = DateTime.Now;
                                    Merge(par);
                                    //EnviarLote(par);
                                    scope.Complete();
                                }
                            }
                            
                            batchContext.IncrementarPassoBatch();
                            batchContext.AdicionarContagemSucesso();
                        }
                        catch (Exception e)
                        {
                            var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                            if (par != null)
                            {
                                batchContext.AdicionarContagemFalha();
                            }
                        }

                        batchContext.IniciarPassoBatch("Boletos Agendados para envio com sucesso!!!", false);
                    }
                }


            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro enviar boletos por E-Mail.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Envio de Boletos para o Cliente",
                    projeto = "CORPORATIVO",
                    servico = "ParcelasSRV",
                    tipoJob = 7,
                    descricaoCodigoReferencia = "Não existe",
                    codReferencia = 0,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,

                });

            }
            finally
            {
                //_jobAgendamento.MarcarFimExecucao(8);
            }
        }
        
        public void MarcarParcelaBoletoComoEnviado(EmailContext emailContext)
        {
            if (!string.IsNullOrWhiteSpace(emailContext.ContextIdStr))
            {
                var parcela = FindById(emailContext.ContextIdStr);

                var email = FindById(emailContext.ContextIdStr);

                if (parcela != null)
                {
                    parcela.PAR_DATA_ENVIO = DateTime.Now;
                    Merge(parcela);

                    if (!string.IsNullOrWhiteSpace(parcela.PAR_NUM_PARCELA))
                    {
                        var cliente = ServiceFactory.RetornarServico<ClienteSRV>().RetornarClienteDaParcela(parcela.PAR_NUM_PARCELA);

                        if(cliente != null)
                        {
                            ServiceFactory.RetornarServico<HistAtendSRV>()
                                .RegistrarHistoricoEnvioAutomaricoBoleto("COADSYS", parcela.PAR_NUM_PARCELA, cliente.CLI_ID, emailContext.Email);
                        }
                    }
                }
            }
        }

        public void NotificarBoletoNaoEnviado(EmailContext emailContext)
        {
            if (!string.IsNullOrWhiteSpace(emailContext.ContextIdStr))
            {
                var job = ServiceFactory.RetornarServico<JobAgendamentoSRV>().FindById(14);
                if(emailContext.CodFila != null)
                {
                    var filaEmailSRV = ServiceFactory
                        .RetornarServico<FilaEmailSRV>();
                    var filaEmail = filaEmailSRV.FindById(emailContext.CodFila);

                    //if(filaEmail != null)
                    //{
                    //    filaEmail.FLE_DATA_CANCELAMENTO = DateTime.Now;
                    //    filaEmailSRV.Merge(filaEmail);
                    //}
                }

                if (job != null && job.JOB_EMAIL_ENVIO != null)
                {
                    // TODO: Enviará email com erro.
                     
                }
            }
        }

        public IQueryable<string> ListarArquivosRemessaParaOZip(int? REM_ID = null)
        {
            return _dao.ListarArquivosRemessaParaOZip(REM_ID);
        }

        public void AtualizaAgendaDeCobranca()
        {
            //_dao.ExecutarClientePassivelDeCobranca();

        }

        public Pagina<PARCELA_PENDENTE> BuscarTitulosEmAtraso(string assinatura, string cliente, string atendente, string cnpj, DateTime? dataini, DateTime? datafim, int pagina, bool primeiraParcela)
        {
            return _dao.ListarTitulosEmAtraso(assinatura, cliente, atendente, cnpj, dataini, datafim, pagina, primeiraParcela);

        }

        public Pagina<CLIENTE_PASSIVEL_COBRANCA> ListarTitulosEmAtrasoPrimeiraParcela(string assinatura, string cliente, string atendente, string cnpj, DateTime? dataini, DateTime? datafim, int pagina, bool primeiraParcela)
        {
            return _dao.ListarTitulosEmAtrasoPrimeiraParcela(assinatura, cliente, atendente, cnpj, dataini, datafim, pagina, primeiraParcela);

        }

        public Pagina<ParcelasDTO> BuscarTitulosComParcelaLiberada(string assinatura, string cliente, string atendente, string cnpj, DateTime? dataini, DateTime? datafim, int pagina)
        {
            return _dao.ListarTitulosComParcelaLiberada(assinatura, cliente, atendente, cnpj, dataini, datafim, pagina);

        }

        public Pagina<PARCELA_PENDENTE> ParcelaLiberadaSitIrregular(int pagina)
        {
            return _dao.ListarTitulosComParcelaLiberada(pagina);

        }

    }

}
