using System;
using System.Linq;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.DAO;
using Coad.GenericCrud.Repositorios.Base;
using COAD.UTIL.Grafico;
using COAD.CORPORATIVO.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Config.DataAttributes;
using COAD.UTIL.Grafico.Base;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.LEGADO.Service;
using GenericCrud.Util;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.Util;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Service;
using COAD.CORPORATIVO.LEGADO.Service.Utils;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.SEGURANCA.Service;
using System.Transactions;
using COAD.CORPORATIVO.Model.Dto.Custons.Atc;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CTR_NUM_CONTRATO")]
    public class ContratoSRV : ServiceAdapter<CONTRATOS, ContratoDTO, string>
    {
        private ContratoDAO _dao;
        public TabSeqSRV _tabSeq { get; set; }
        public ParamSRV _paramSRV { get; set; }
        public PedidoPagamentoSRV _pedidoPagamentoSRV { get; set; }
        public RepresentanteSRV _representanteSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicao { get; set; }
        public ContratoLegadoSRV _contratoLegadoSRV { get; set; }
        [ServiceProperty("CTR_NUM_CONTRATO", Name = "parcela", PropertyName = "PARCELAS")]
        public ParcelasSRV _parcelasSRV { get; set; }
        public SenhaSRV _senhaSRV { get; set; }
        public ContratoSRV(ContratoDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }
        public ContratoSRV()
        {
            this._dao = new ContratoDAO();
            this._tabSeq = new TabSeqSRV();
            this._paramSRV = new ParamSRV();
            this._pedidoPagamentoSRV = new PedidoPagamentoSRV();
            this._representanteSRV = new RepresentanteSRV();
            this._produtoComposicao = new ProdutoComposicaoSRV();
            this._contratoLegadoSRV = new ContratoLegadoSRV();
            this._parcelasSRV = new ParcelasSRV();

            SetDao(_dao);
        }
        public JsonGraficoResponse ContratosPorRepresentante(int _ano, int _mes, string _regiao_uf)
        {
            return _dao.ContratosPorRepresentante(_ano, _mes, _regiao_uf);

        }
        public string BuscarUltimoContrato(string _asn)
        {
            return _dao.BuscarUltimoContrato(_asn);
        }
        public ContratoDTO BuscarUltimoContrato(string _asn, bool _ativos)
        {
            return _dao.BuscarUltimoContrato(_asn, _ativos);
        }
        public ContratoDTO BuscarContratoNF(int _nf_numero, int _cli_id)
        {
            return _dao.BuscarContratoNF(_nf_numero, _cli_id);
        }
        public List<RepresentanteDTO> ListarApuracaoVendas(int _mes, int _ano, int? _repid)
        {
            return _dao.ListarApuracaoVendas(_mes, _ano, _repid);
        }
        public JsonGraficoDataSource BuscarContratosEvolucaoAnual(int _mes, int _anoini, int _ano, int? _emp_id, int? _grupo)
        {
            return _dao.BuscarContratosEvolucaoAnual(_mes, _anoini, _ano, _emp_id, _grupo);
        }
        public JsonGraficoDataSource BuscarVendasEvolucao(int _mes, int _anoini, int _ano, int? _emp_id, int? _grupo)
        {
            return _dao.BuscarVendasEvolucao(_mes, _anoini, _ano, _emp_id, _grupo);
        }
        public JsonGraficoDataSource BuscarVendasRepresentante(int _mes, int _ano, int? _emp_id,  int? _grupo_id = null)
        {
            return _dao.BuscarVendasRepresentante(_mes, _ano, _emp_id, _grupo_id);
        }
        public JsonGraficoDataSource BuscarVendasGrupo(int _mes, int _ano, int? _emp_id)
        {
            return _dao.BuscarVendasGrupo(_mes, _ano, _emp_id);
        }
       
        public void CancelarContrato(ContratoDTO _contrato)
        {
            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
            {

                var _mesFat = new ParametrosSRV().BuscarMesFaturamento();
                var _assinatura = new AssinaturaDAO().FindById(_contrato.ASN_NUM_ASSINATURA);
                var _parsrv = new ParcelasSRV();
                var _parcelas = _parsrv.ListarParcelasContrato(_contrato.CTR_NUM_CONTRATO);

                if ((_contrato.CTR_DATA_FAT.Value.Month == _mesFat &&
                     _contrato.CTR_DATA_FAT.Value.Year == DateTime.Now.Year))
                    _contrato.CTR_DATA_CANC = _contrato.CTR_DATA_FAT;
                else
                    _contrato.CTR_DATA_CANC = DateTime.Now;

                this.Merge(_contrato);

                foreach(var _item in _parcelas)
                {
                    _item.DATA_EXCLUSAO = DateTime.Now;
                }

                _parsrv.MergeAll(_parcelas);

                var _histAtend = new HistoricoAtendimentoDTO();

                _histAtend.HAT_DATA_HIST = DateTime.Now;
                _histAtend.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_SOLICITANTE = SessionContext.autenticado.USU_LOGIN;
                _histAtend.HAT_DESCRICAO = _contrato.HISTORICO_CANCELAMENTO + " (Contrato foi cancelado e as parcelas foram excluídas).";
                _histAtend.TIP_ATEND_ID = 113;
                _histAtend.ACA_ID = 1;
                _histAtend.CLI_ID = _assinatura.CLI_ID;
                _histAtend.UEN_ID = 3;
                
                new HistAtendSRV().Save(_histAtend);

                scope.Complete();
 
            }

        }
        public RelApuracaoRecebimentoTotalCustomDTO BuscarApuraRecebimento(Nullable<DateTime> _dtini
                                                                          , Nullable<DateTime> _dtfim
                                                                          , int _emp_id = 0
                                                                          , String _ban_id = null
                                                                          , int _gru_id = 0
                                                                          , int _tipobaixa = 0)
            
        {

            var _query = _dao.BuscarApuraRecebimento(_dtini, _dtfim, _emp_id, _ban_id, _gru_id, _tipobaixa);

            return _query;

        }
        public CONFERENCIA_FINANCEIRA_HEADER BuscarConferenciaFinanceira(DateTime _Date01, DateTime _Date02, int _emp_id, int? _grupo_id, int _tipodata = 0)
        {
            return _dao.BuscarConferenciaFinanceira(_Date01, _Date02, _emp_id, _grupo_id, _tipodata);
        }
        public JsonGraficoDataSource BuscarFaturamentoAnualSint(int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id)
        {
            return _dao.BuscarFaturamentoAnualSint(_ano,_emp_id, _rep_id, _grupo_id);
        }
        public JsonGraficoDataSource BuscarQtdeValor(DateTime _dtini, DateTime _dtfim, int _emp_id, int? _qtdeParcelas, int? _grupo_id = 0)
        {
            return _dao.BuscarQtdeValor(_dtini, _dtfim, _emp_id, _qtdeParcelas, _grupo_id);
        }
        public JsonGraficoDataSource BuscarProdutosTipoPgto(DateTime _dtini, DateTime _dtfim, int _emp_id, int _qtdeParcelas, int? _grupo_id = 0)
        {
            return _dao.BuscarProdutosTipoPgto(_dtini, _dtfim, _emp_id, _qtdeParcelas, _grupo_id);
        }
        public JsonGraficoDataSource BuscarContratoTipoPgto(int _mes, int _ano, int _emp_id, int _qtdeParcelas, int? _grupo_id = 0)
        {
            return _dao.BuscarContratoTipoPgto(_mes, _ano, _emp_id, _qtdeParcelas, _grupo_id);
        }
        public IList<RelPrevisaoReceitaProdutoDTO> BuscarPrevisaoReceita(int? _ano, int? _emp_id, int? _grupo_id)
        {
            return _dao.BuscarPrevisaoReceita(_ano, _emp_id, _grupo_id);
        }

        public IList<RPT_CONTRATOS_TIPO_PGTO_Result> ListarContratoTipoPgto(DateTime _dtini
                                                                          , DateTime _dtfim
                                                                          , int _emp_id
                                                                          , bool _tipodata
                                                                          , int _qtdeParcelas = 0
                                                                          , int _grupo_id = 0)

        {
            return _dao.ListarContratoTipoPgto(_dtini, _dtfim, _emp_id, _tipodata, _qtdeParcelas, _grupo_id);
        }

        public IList<ContratoDTO> BuscarPorAssinatura(string _asn_num_assinatura)
        {
            return _dao.BuscarPorAssinatura(_asn_num_assinatura);
        }
        public JsonGraficoDataSource RelatorioFaturamentoFranquia(DateTime? data, int? UEN_ID = 1)
        {
            if (data == null)
                data = DateTime.Now;

            var ano = ((DateTime)data).Year;
            var mes = ((DateTime)data).Month;
            var relatorio = _dao.RelatorioFaturamentoFranquia(ano, mes, UEN_ID);

            relatorio.chart.caption = "Relatorio de faturamento por região";
            relatorio.chart.subCaption = "Lista o faturamento de um mês seguimentado por região";
            
            return relatorio;
        }
        /// <summary>
        /// Gera os contratos no processo de faturamento (método base)
        /// </summary>
        /// <param name="faturamentoDTO"></param>
        /// <returns></returns>
        public IEnumerable<ContratoDTO> GerarContratoFaturamento(ContextoFaturamentoDTO faturamentoDTO)
        {
            if (faturamentoDTO == null)
            {
                throw new FaturamentoException("As informações de faturamento não foram encontradas.");
            }

            if (faturamentoDTO.itemPedido == null)
            {
                throw new FaturamentoException("Não é possível encontrar o item do pedido para faturar");

            }

            if (faturamentoDTO.assinatura == null)
            {
                throw new FaturamentoException("Não é possível encontrar assinatura para faturar. Isso deve ser causado por falha ao gerar a assinatura.");
            }

            if (faturamentoDTO.REGIAO == null)
            {
                throw new FaturamentoException("Não é possível obter as informações da Empresa.");
            }

            if (faturamentoDTO.EMP_ID == null && faturamentoDTO.REGIAO.EMP_ID == null)
            {
                throw new FaturamentoException("Não é possível obter as informações da Regiao.");
            }

            var lstDadosDeContrato = PreProcessarContratos(faturamentoDTO);
            var lstContratos = _processarCriacaoContrato(lstDadosDeContrato, faturamentoDTO);

            SalvarContrato(lstContratos);

            return lstContratos;
        }
        /// <summary>
        /// Prepara/processa os dados do contrato para que o processo subsequente tenha apenas que se 
        /// encarregar da criação propriamente dita do contrato.
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>
        private IList<DadosDeContratoDTO> PreProcessarContratos(ContextoFaturamentoDTO contexto)
        {
            if (contexto == null)
            {
                throw new FaturamentoException("O objeto contextual não foi encontrado.");
            }

            if (contexto.produto == null)
            {
                throw new FaturamentoException("O produto não foi encontrado.");
            }

            //if (contexto.tabelaPreco == null)
            //{
            //    throw new FaturamentoException("A tabela de preço não foi encontrado.");
            //}

            var produto = contexto.produto;
            _produtoComposicao.ChecaEMarcaProdutoCurso(produto);

            //var tabela = contexto.tabelaPreco;
            var itemPedido = contexto.itemPedido;
            var entrada = contexto.entrada;
            var pagamentoRestante = (contexto.pagamentoSemEntrada) ? contexto.entrada : contexto.pagamentoRestante;

            IList<DadosDeContratoDTO> lstDadosDeContrato = null;

            //if (produto.EhCurso == true)
            //{
            //    lstDadosDeContrato = RetornaDadosPreProcessadosDeContratoDeCurso(tabela, itemPedido, entrada, pagamentoRestante, contexto.pagamentoSemEntrada);
            //}
            //else
            //{
            lstDadosDeContrato = RetornarDadosPreProcessadosDeContratoDeProduto(itemPedido, entrada, pagamentoRestante, contexto.pagamentoSemEntrada);
            //}

            return lstDadosDeContrato;

        }
        /// <summary>
        /// Extrai os dados necessário do pedido, dos pagamentos e etc. para criação do contrato de curso
        /// </summary>
        /// <param name="tabela"></param>
        /// <param name="itemPedido"></param>
        /// <param name="entrada"></param>
        /// <param name="pagamentoRestante"></param>
        /// <returns></returns>
        private IList<DadosDeContratoDTO> RetornaDadosPreProcessadosDeContratoDeCurso(
            TabelaPrecoDTO tabela,
            ItemPedidoDTO itemPedido,
            PedidoPagamentoDTO entrada,
            PedidoPagamentoDTO pagamentoRestante,
            bool semEntrada)
        {
            if (tabela == null)
            {
                throw new FaturamentoException("O produto não foi encontrado.");
            }

            if (itemPedido == null)
            {
                throw new FaturamentoException("A tabela de preço não foi encontrado.");
            }

            if (entrada == null)
            {
                throw new FaturamentoException("Os dados de pagamento não foram encontrados.");
            }

            var porcentagemDeCurso = (tabela.TP_PORCENTAGEM_SERVICO > 0) ? tabela.TP_PORCENTAGEM_SERVICO : 70;

            var totalPedido = itemPedido.IPE_TOTAL;
            decimal? valorEntrada = 0.00m;

            var diaVencimentoVendaRecorrente = itemPedido.IPE_DIA_VENCIMENTO_VENDA_RECORRENTE;
            var numeroParcelas = itemPedido.IPE_PARCELA;
            var numeroParcelaParaCalculo = numeroParcelas;
            var valorParcelas = pagamentoRestante.PGT_VLR_PARCELA;
            var valorRestante = pagamentoRestante.PGT_VLR_TOTAL;
            // se possuir entrada
            if (semEntrada == false)
            {
                valorEntrada = entrada.PGT_VLR_TOTAL;
                numeroParcelaParaCalculo--;
            }


            var porcentagem = (porcentagemDeCurso / 100);
            var valorServico = (porcentagem * totalPedido);
            var valorProduto = totalPedido - valorServico;

            var valorEntradaServico = (porcentagem * valorEntrada);
            var valorEntradaProduto = valorEntrada - valorEntradaServico;

            var valorParcelasServico = ((valorServico - valorEntradaServico) / numeroParcelaParaCalculo);
            var valorParcelasProduto = ((valorProduto - valorEntradaProduto) / numeroParcelaParaCalculo);

            if(itemPedido != null)
            {
                if (itemPedido.PEDIDO_CRM != null)
                    itemPedido.PEDIDO_CRM = ServiceFactory.RetornarServico<PedidoCRMSRV>().FindById(itemPedido.PED_CRM_ID);

                if(itemPedido.PEDIDO_CRM.PED_CEM_POR_CENTO_FATURADO == true)
                {
                    valorServico = 0;
                    valorProduto = 0;
                }
            }
            var restanteServico = (porcentagem * valorRestante);
            var restanteProduto = valorRestante - restanteServico;

            IList<DadosDeContratoDTO> dadosDoContrato = new List<DadosDeContratoDTO>();

            var dadosContratoServico = new DadosDeContratoDTO()
            {
                GerarNotaFiscal = false,
                NumeroParcelas = numeroParcelas,
                ValorContrato = valorServico,
                ValorDeEntrada = valorEntradaServico,
                ValorParcela = valorParcelasServico,
                ValorRestante = restanteServico,
                DiaVencimentoVendaRecorrente = diaVencimentoVendaRecorrente
            };

            var dadosContratoProduto = new DadosDeContratoDTO()
            {
                GerarNotaFiscal = true,
                NumeroParcelas = numeroParcelas,
                ValorContrato = valorProduto,
                ValorDeEntrada = valorEntradaProduto,
                ValorParcela = valorParcelasProduto,
                ValorRestante = restanteProduto,
                DiaVencimentoVendaRecorrente = diaVencimentoVendaRecorrente
            };

            dadosDoContrato.Add(dadosContratoServico);
            dadosDoContrato.Add(dadosContratoProduto);

            return dadosDoContrato;
        }
        /// <summary>
        /// Extrai os dados necessário do pedido, dos pagamentos e etc. para criação do contrato de prdutos
        /// </summary>
        /// <param name="tabela"></param>
        /// <param name="itemPedido"></param>
        /// <param name="entrada"></param>
        /// <param name="pagamentoRestante"></param>
        /// <returns></returns>
        private IList<DadosDeContratoDTO> RetornarDadosPreProcessadosDeContratoDeProduto(
            ItemPedidoDTO itemPedido,
            PedidoPagamentoDTO entrada,
            PedidoPagamentoDTO pagamentoRestante,
            bool semEntrada)
        {
            if (itemPedido == null)
            {
                throw new FaturamentoException("A dado do item de pedido foi encontrado.");
            }

            if (itemPedido.IPE_CORTESIA != true && pagamentoRestante == null)
            {
                throw new FaturamentoException("A pagamentoRestante não foi encontrado.");
            }

            decimal? valorEntrada = 0.00m;
            decimal? valorBruto = (itemPedido.IPE_PRECO_UNITARIO * itemPedido.IPE_QTD);
            decimal? valorServico = null;
            decimal? valorProduto = null;

            string carId = null;
            var totalPedido = itemPedido.IPE_TOTAL;
            var diaVencimentoVendaRecorrente = itemPedido.IPE_DIA_VENCIMENTO_VENDA_RECORRENTE;
            var numeroParcelas = itemPedido.IPE_PARCELA;
            var valorParcelas = (itemPedido.IPE_CORTESIA != true) ? pagamentoRestante.PGT_VLR_PARCELA : 0;
            var valorRestante = (itemPedido.IPE_CORTESIA != true) ? pagamentoRestante.PGT_VLR_TOTAL : 0;


            if (semEntrada == false)
            {
                valorEntrada = entrada.PGT_VLR_TOTAL;
            }

            if (itemPedido.IPE_CORTESIA == true)
            {
                totalPedido = 0.00m;
                numeroParcelas = 0;
                valorParcelas = 0.00m;
                valorRestante = 0.00m;
            }

            if (itemPedido.TPG_ID == 9)
                numeroParcelas = 1;

            ServiceFactory.RetornarServico<ProdutoComposicaoSRV>().ChecaEMarcaProdutoCurso(itemPedido.PRODUTO_COMPOSICAO);

            if (itemPedido.PRODUTO_COMPOSICAO.EhCurso)
            {
                var valorDivisao = ServiceFactory.RetornarServico<ImpostoSRV>().DividirMaterialServico(valorBruto);

                if(valorDivisao != null)
                {
                    valorServico = valorDivisao.ValorServico;
                    valorProduto = valorDivisao.ValorProduto;
                }

            }

            if (itemPedido.PEDIDO_CRM == null)
                itemPedido.PEDIDO_CRM = ServiceFactory.RetornarServico<PedidoCRMSRV>().FindById(itemPedido.PED_CRM_ID);

            if (!string.IsNullOrWhiteSpace(itemPedido.PEDIDO_CRM.CAR_ID))
                carId = itemPedido.PEDIDO_CRM.CAR_ID;

            IList<DadosDeContratoDTO> dadosDoContrato = new List<DadosDeContratoDTO>();

            var dadosContratoProduto = new DadosDeContratoDTO()
            {
                GerarNotaFiscal = true,
                NumeroParcelas = numeroParcelas,
                ValorContrato = totalPedido,
                ValorDeEntrada = valorEntrada,
                ValorParcela = valorParcelas,
                ValorRestante = valorRestante,
                DiaVencimentoVendaRecorrente = diaVencimentoVendaRecorrente,
                Cortesia = itemPedido.IPE_CORTESIA,
                CarId = carId,
                PeriodoMesBonus = itemPedido.IPE_PERIODO_MES_BONUS,
                ValorBruto = valorBruto,
                ValorServico = valorServico,
                ValorProduto = valorProduto
            };

            dadosDoContrato.Add(dadosContratoProduto);

            return dadosDoContrato;
        }
        /// <summary>
        /// Cria o processamento da criação do contrato baseado nas informações já disponibilizadas.
        /// </summary>
        /// <param name="faturamentoDTO"></param>
        /// <param name="dadosContrato"></param>
        /// <returns></returns>
        private IList<ContratoDTO> _processarCriacaoContrato(IEnumerable<DadosDeContratoDTO> dadosContrato, ContextoFaturamentoDTO faturamentoDTO)
        {
            IList<ContratoDTO> lstContrato = new List<ContratoDTO>();

            if (dadosContrato != null)
            {
                foreach (var dado in dadosContrato)
                {
                    var contrato = _processarCriacaoContrato(faturamentoDTO, dado);
                    lstContrato.Add(contrato);
                }
            }

            return lstContrato;
        }
        /// <summary>
        /// Cria o processamento da criação do contrato baseado nas informações já disponibilizadas.
        /// </summary>
        /// <param name="faturamentoDTO"></param>
        /// <param name="dadosContrato"></param>
        /// <returns></returns>
        private ContratoDTO _processarCriacaoContrato(ContextoFaturamentoDTO faturamentoDTO, DadosDeContratoDTO dadosContrato)
        {
            try
            {
                if (faturamentoDTO == null)
                {
                    throw new FaturamentoException("O objeto contextual não foi encontrado.");
                }

                var itemPedido = faturamentoDTO.itemPedido;
                var assinatura = faturamentoDTO.assinatura;
                var codigoDoItem = itemPedido.IPE_ID;
                var pedido = faturamentoDTO.PEDIDO;
                var regiao = faturamentoDTO.REGIAO;
                var codigoDoContrato = _paramSRV.GetCodigoContrato();
                var empresa = (faturamentoDTO.EMP_ID != null) ? faturamentoDTO.EMP_ID : regiao.EMP_ID;
                var composicao = faturamentoDTO.produto;
                var area = 1;
                var parcelas = dadosContrato.NumeroParcelas;
                var valorContrato = dadosContrato.ValorContrato;
                var valorParcela = dadosContrato.ValorParcela;
                var valorEntrada = dadosContrato.ValorDeEntrada;
                var pagamentoPorGateway = itemPedido.IPE_PAGAMENTO_GATEWAY;
                var gerarNotaFiscal = dadosContrato.GerarNotaFiscal;
                var cortesia = dadosContrato.Cortesia;
                var usuLogin = faturamentoDTO.USU_LOGIN;
                var repIdExecutouAcao = faturamentoDTO.REP_ID_QUE_EXECUTOU_ACAO;
                var cliId = pedido.CLI_ID;
                var carId = dadosContrato.CarId;
                var periodoMesBonus = dadosContrato.PeriodoMesBonus;
                var produtoCurso = false;

                decimal? valorParcelasRestantes = dadosContrato.ValorRestante;
                short? diaVencimentoVendaRecorrente = dadosContrato.DiaVencimentoVendaRecorrente;

                int? codigoTipoPeriodo = null;
                int? pedidoId = 0;
                int? repId = null;
                int? rgId = null;
                int? cmpId = null;

                string repOperId = null;
                string codigoContratoSTR = null;

                DateTime? inicioVigencia = null;
                DateTime? fimDeVigencia = null;
                DateTime? dataAnoVicencia = null;
                AssinaturaDTO assinaturaMigracao = null;

                if(pedido.TPD_ID == 3 && !string.IsNullOrWhiteSpace(itemPedido.IPE_ASN_NUM_ASS_CANC))
                {
                    assinaturaMigracao = ServiceFactory.RetornarServico<AssinaturaSRV>().FindById(itemPedido.IPE_ASN_NUM_ASS_CANC);
                }
                else if(pedido.TPD_ID == 3 && string.IsNullOrWhiteSpace(itemPedido.IPE_ASN_NUM_ASS_CANC))
                {
                    throw new Exception("O pedido é do tipo migração. Mas não possui uma assinatura de origem.");
                }


                if (string.IsNullOrWhiteSpace(carId))
                    throw new Exception("Não é possível achar a carteira para faturar.");

                if (faturamentoDTO.RequisicaoFaturamento != null &&
                    faturamentoDTO.RequisicaoFaturamento.LstRequisicaoFaturamento != null)
                {
                    var requiFat = faturamentoDTO.RequisicaoFaturamento;
                    var requiFatDetalhe = faturamentoDTO
                        .RequisicaoFaturamento
                        .LstRequisicaoFaturamento
                        .Where(x => x.IpeId == codigoDoItem)
                        .FirstOrDefault();

                    if (requiFatDetalhe != null)
                    {
                        if (gerarNotaFiscal == true)
                            gerarNotaFiscal = requiFatDetalhe.GerarNotaFiscal;

                        if (requiFatDetalhe.EmpId != null)
                        {
                            empresa = requiFatDetalhe.EmpId;
                        }
                    }

                }

                codigoDoContrato++;
                _paramSRV.AtualizarCodigoContrato(codigoDoContrato);

                if (codigoDoContrato != null)
                {
                    codigoContratoSTR = codigoDoContrato.ToString();
                }

                if (pedido != null)
                {
                    pedidoId = pedido.PED_CRM_ID;
                    repId = pedido.REP_ID;
                    rgId = pedido.RG_ID;
                    repOperId = _representanteSRV.GetRepOperIdDoRepresentante(repId, carId);
                }

                TipoPeriodoDTO tipoPeriodo = ServiceFactory.RetornarServico<ItemPedidoSRV>().ChecaEAdicionaTipoPeriodo(itemPedido);

                var objetoPagamentoEntrada = _pedidoPagamentoSRV.RetornarTipoPagamentoDeEntrada(codigoDoItem);

                if (composicao != null)
                {
                    cmpId = composicao.CMP_ID;
                    ServiceFactory.RetornarServico<ProdutoComposicaoSRV>().ChecaEMarcaProdutoCurso(composicao);
                    if (composicao.EhCurso)
                    {
                        area = 4;
                        produtoCurso = true;
                        inicioVigencia = DateUtil.RetornaDiaPrimeiro(DateTime.Now);
                    }
                }
                else
                {
                    throw new FaturamentoException("Erro ao gerar contrato. Não é possível localizar as informações do produto a ser faturado.");
                }

                if (!produtoCurso)
                {
                    //var assinaturaVigencia = (pedido.TPD_ID == 3) ? assinaturaMigracao : assinatura;
                    if (!TentarObterProximaDataInicioVigencia(assinatura, out inicioVigencia))
                    {
                        inicioVigencia = DateUtil.RetornaDiaPrimeiro(DateTime.Now);
                    }
                }

                dataAnoVicencia = inicioVigencia;

                if (tipoPeriodo != null)
                {
                    if(periodoMesBonus != null)
                    {
                        dataAnoVicencia = DateUtil.AdicionaMes(inicioVigencia, (int) periodoMesBonus);
                    }

                    fimDeVigencia = ServiceFactory.RetornarServico<TipoPeriodoSRV>().CalcularPeriodoDeFimDeVigencia(inicioVigencia, tipoPeriodo, periodoMesBonus);
                    codigoTipoPeriodo = tipoPeriodo.TTP_ID;
                }

                // Se não existe um uf dessa região
                string regiaoUf = StringUtil.Truncate(regiao.RG_DESCRICAO, 2);
                var uf = ServiceFactory.RetornarServico<UFSRV>().FindById(regiaoUf);

                if (uf == null)
                {
                    regiaoUf = null;
                }

                var contrato = new ContratoDTO()
                {
                    AREA_ID = area,
                    ITEM_PEDIDO = itemPedido,
                    IPE_ID = itemPedido.IPE_ID,
                    ASN_NUM_ASSINATURA = assinatura.ASN_NUM_ASSINATURA,
                    CTR_ANO_VIGENCIA = ((DateTime)dataAnoVicencia).Year.ToString(),
                    //CTR_ANO_FAT = ((DateTime)inicioVigencia).Year.ToString(),
                    PED_CRM_ID = pedidoId,
                    PEDIDO_CRM = pedido,
                    EMP_ID = empresa,
                    CTR_NUM_CONTRATO = codigoContratoSTR,
                    CTR_VLR_CONTRATO = valorContrato,
                    CTR_VENDA_RECORRENTE = tipoPeriodo.TTP_RECORRENTE,
                    CTR_VLR_ENTRADA = valorEntrada,
                    CTR_QTE_PARCELAS = parcelas,
                    //CTR_ANO_PROD = ((DateTime)inicioVigencia).Year.ToString(),
                    CTR_VLR_PARCELAS = valorParcela,
                    CTR_DATA_INI_VIGENCIA = inicioVigencia,
                    REGIAO_UF = regiaoUf,
                    CTR_DATA_FIM_VIGENCIA = fimDeVigencia,
                    REP_ID = repId,
                    RG_ID = rgId,
                    CMP_ID = cmpId,
                    CTR_DATA_FATURAMENTO_EFETIVO = DateTime.Now,
                    CTR_GERA_NOTA_FISCAL = gerarNotaFiscal,
                    CTR_SERVICO = produtoCurso,
                    //CTR_SERVICO = true,
                    CTR_DIA_VENCIMENTO_VENDA_RECORRENTE = diaVencimentoVendaRecorrente,
                    CTR_PAGAMENTO_GATEWAY = (bool)DataUtil.ReturnNotNull(itemPedido.IPE_PAGAMENTO_GATEWAY, false),
                    CTR_CORTESIA = (cortesia == true) ? (int?)1 : null,
                    TTP_ID = codigoTipoPeriodo,
                    CTR_PERIODO_MES_BONUS = periodoMesBonus,
                    CAR_ID = carId,
                    REP_OPER_ID = repOperId,
                    CTR_PRORROGADO = 1,
                    CTR_VLR_SERVICO = dadosContrato.ValorServico,
                    CTR_VLR_PRODUTO = dadosContrato.ValorProduto,
                    CTR_VLR_BRUTO = dadosContrato.ValorBruto
                };

                InserirPeriodoDeFaturamento(contrato, itemPedido);
                ValidarDataFaturamento(contrato);
                return contrato;
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível gerar o contrato", e);
            }
        }

        public ContratoDTO CriarContratoTransferencia(string codContrato, ProcessoTransferenciaAssinaturaDTO transferenciaDTO)
        {
            var contrato = FindById(codContrato);
            return CriarContratoBaseadoEmOutro(contrato, transferenciaDTO);
        }
        /// <summary>
        /// Pega o contrato informado e gera um contrato com os mesmos dados mas com o código Diferente.
        /// </summary>
        /// <param name="contrato"></param>
        public ContratoDTO CriarContratoBaseadoEmOutro(ContratoDTO contrato, ProcessoTransferenciaAssinaturaDTO transferenciaDTO)
        {
            var codAssinatura = transferenciaDTO.NovoCodAssinatura;
            var cmdId = transferenciaDTO.CodProduto;
            var periodoBonus = transferenciaDTO.AcrescimoNoMes;
            var contratoNovo = contrato.Clone();
            contratoNovo.ASN_NUM_ASSINATURA = null;
            contratoNovo.CMP_ID = cmdId;
            contratoNovo.CTR_DATA_CANC = null;
            contratoNovo.CTR_NUMERO_NOTA = null;
            contratoNovo.CTR_NUM_CONTRATO = null;
            contratoNovo.NFX_ID = null;

            var codigoDoContrato = _paramSRV.GetCodigoContrato();
            string codigoContratoSTR = null;

            codigoDoContrato++;
            _paramSRV.AtualizarCodigoContrato(codigoDoContrato);

            if (codigoDoContrato != null)
            {
                codigoContratoSTR = codigoDoContrato.ToString();
            }

            contratoNovo.ASN_NUM_ASSINATURA = codAssinatura;
            contratoNovo.CTR_NUM_CONTRATO = codigoContratoSTR;

            var dataFinalVigencia = contratoNovo.CTR_DATA_FIM_VIGENCIA;

            DateTime? dataFinalVigenciaAtualizada = (periodoBonus != null) ?
                DateUtil.AdicionaMes(dataFinalVigencia, (int)periodoBonus, dataFinalVigencia.Value.Day) :
                dataFinalVigencia;

            contratoNovo.CTR_DATA_FIM_VIGENCIA = dataFinalVigenciaAtualizada;
            contratoNovo.CTR_PERIODO_MES_BONUS = periodoBonus;

            SaveOrUpdateNonIdentityKeyEntity(contratoNovo);
            var lstParcelas = _parcelasSRV.GerarParcelasFaturamento(contrato, null);
            _parcelasSRV.PagarParcelasDoContratoTransferido(contrato, contratoNovo);

            return contratoNovo;
        }

        private void ValidarDataFaturamento(ContratoDTO contrato)
        {
            if (contrato != null)
            {
                int? empId = contrato.EMP_ID;
                DateTime? dataFaturamento = contrato.CTR_DATA_FAT;

                var ultimaDataFaturamentoComNota = RetornarDataDoUltimoFaturamentoPorEmpresa(empId);

                if (dataFaturamento < ultimaDataFaturamentoComNota)
                {
                    string msg =
                        "Não é possível gerar um contrato com a data de faturamento no dia {0:dd/MM/yyyy}. " +
                        "Já existem contratos com notas emitidas com data superior a {1:dd/MM/yyyy}. " +
                        "Última data de faturamento gerado em uma nota. {2:dd/MM/yyyy}.";

                    throw new FaturamentoException(string.Format(msg, dataFaturamento, dataFaturamento, ultimaDataFaturamentoComNota));
                }
            }
        }

        public void InserirPeriodoDeFaturamento(ContratoDTO contrato, ItemPedidoDTO itemPedido)
        {

            var dataFaturamentoSRV = ServiceFactory.RetornarServico<DatasFaturamentoSRV>();
            if (contrato != null && itemPedido != null)
            {
                if (itemPedido.IPE_DATA_FATURAMENTO == null)
                {
                    throw new FaturamentoException("Data de Faturamento do Pedido não é válida.");
                }

                DateTime? dataFaturamento = itemPedido.IPE_DATA_FATURAMENTO;
                DateTime? dataProducao = itemPedido.IPE_DATA_PRODUCAO;

                string semana = null;
                string periodo = null;

                if (dataProducao == null)
                    dataProducao = DateTime.Now;

                if (dataFaturamento != null)
                {
                    var semanaFaturamento = dataFaturamentoSRV.FindByDate(dataFaturamento);
                    if (semanaFaturamento != null)
                    {
                        dataFaturamento = semanaFaturamento.Data;
                        semana = semanaFaturamento.SEMANA;
                        periodo = semanaFaturamento.PERIODO;
                    }
                    else
                    {
                        var periodoFaturamento = dataFaturamentoSRV.GetUltimoPeriodoFaturamento();
                        dataFaturamento = periodoFaturamento.Data;
                        periodo = periodoFaturamento.PERIODO;
                        semana = periodoFaturamento.SEMANA;
                    }
                }
                else
                {
                    var periodoFaturamento = dataFaturamentoSRV.GetUltimoPeriodoFaturamento();
                    dataFaturamento = periodoFaturamento.Data;
                    periodo = periodoFaturamento.PERIODO;
                    semana = periodoFaturamento.SEMANA;
                }

                if (itemPedido.IPE_DATA_FATURAMENTO == null)
                {
                    itemPedido.IPE_DATA_FATURAMENTO = dataFaturamento;
                    ServiceFactory.RetornarServico<ItemPedidoSRV>().SaveOrUpdate(itemPedido);
                }

                contrato.CTR_DATA_FAT = dataFaturamento;
                contrato.CTR_SEMANA_FAT = semana;
                contrato.CTR_PERIODO_FAT = periodo;
                contrato.CTR_ANO_FAT = ((DateTime)dataFaturamento).Year.ToString();
                contrato.CTR_ANO_PROD = ((DateTime)dataProducao).Year.ToString();
                contrato.CTR_DATA_FATURAMENTO_EFETIVO = dataProducao;
            }
        }

        public ContratoLegadoDTO ConverterParaContratoLegado(ContratoDTO contrato)
        {
            if (contrato != null)
            {
                string dataFatStr = null;
                string dataVigStr = null;
                DateTime? dataFatura = null;
                DateTime? dataVigencia = null;
                DateTime? dataProducao = contrato.CTR_DATA_FATURAMENTO_EFETIVO;
                string anoProd = null;
                string dataProducaoStr = null;
                int? repId = contrato.REP_ID;
                string codRepresentante = null;
                string periodoProducao = null;

                if (contrato.CTR_DATA_FAT != null)
                {
                    dataFatura = contrato.CTR_DATA_FAT;
                    dataFatStr = ((DateTime)dataFatura).ToString("dd/MM/yyyy");
                }

                if (dataProducao != null)
                {
                    dataProducaoStr = dataProducao.Value.ToString("dd/MM/yyyy");
                    anoProd = ((DateTime)dataProducao).Year.ToString();
                    periodoProducao = AssinaturaUtil.GetLetraFromMes(dataProducao.Value.Month).ToString();
                }

                if (contrato.CTR_DATA_INI_VIGENCIA != null)
                {
                    dataVigencia = contrato.CTR_DATA_INI_VIGENCIA;
                    dataVigStr = ((DateTime)dataVigencia).ToString("dd/MM/yyyy");
                }

                //codRepresentante = _representanteSRV.GetCodRepresentanteNoCoorporativo(repId);
                codRepresentante = contrato.REP_OPER_ID;

                var contratoLegado = new ContratoLegadoDTO()
                {
                    CONTRATO = contrato.CTR_NUM_CONTRATO,
                    ASSINATURA = contrato.ASN_NUM_ASSINATURA,
                    ANO_VIGENCIA = (contrato.CTR_CORTESIA == 1) ? "9999" : contrato.CTR_ANO_VIGENCIA,
                    PEDIDO = contrato.PED_CRM_ID.ToString(),
                    ANO_FAT = contrato.CTR_ANO_FAT,
                    PERIODO_FAT = contrato.CTR_PERIODO_FAT,
                    SEMANA_FAT = contrato.CTR_SEMANA_FAT,
                    DATA_FAT = dataFatStr,
                    DATA_PRODUCAO = dataProducaoStr,
                    ANO_PROD = anoProd,
                    PERIODO_PROD = periodoProducao,
                    AREA = contrato.AREA_ID.ToString(),
                    REGIAO = contrato.REGIAO_UF,
                    EMPRESA_ID = contrato.EMP_ID,
                    DATA_INSERT = DateTime.Now,
                    SEM_ENTRADA = "N",
                    REPRESENTANTE = codRepresentante,
                    VENDA_RECORRENTE = contrato.CTR_VENDA_RECORRENTE,
                    DIA_VENCIMENTO = contrato.CTR_DIA_VENCIMENTO_VENDA_RECORRENTE

                };
                
                decimal? valorEntrada = contrato.CTR_VLR_ENTRADA;
                decimal? valorParcelas = contrato.CTR_VLR_PARCELAS;
                int? qtdParcelas = contrato.CTR_QTE_PARCELAS;


                if (valorEntrada != null && valorEntrada > 0)
                {
                    contratoLegado.VLR_ENTRADA = StringUtil.Truncate(valorEntrada.ToString(), 11).Replace(".", ",");
                }
                else
                {

                    valorEntrada = valorParcelas;
                    qtdParcelas = --qtdParcelas;
                    if (qtdParcelas <= 0)
                        valorParcelas = null;

                    contratoLegado.VLR_ENTRADA = StringUtil.Truncate(valorEntrada.ToString(), 11).Replace(".", ",");
                }

                if (valorParcelas != null)
                {
                    contratoLegado.VLR_PARC_REST = StringUtil.Truncate(valorParcelas.ToString(), 11).Replace(".", ",");
                }

                if (qtdParcelas != null && qtdParcelas > 0)
                {
                    contratoLegado.QTE_PARC_REST = qtdParcelas.ToString().Replace(".", ",");
                }

                return contratoLegado;

            }

            return null;
        }
        public IList<ContratoDTO> SalvarContrato(IEnumerable<ContratoDTO> lstContrato)
        {
            IList<ContratoDTO> lstContratos = new List<ContratoDTO>();

            if (lstContrato != null)
            {
                foreach (var contrato in lstContrato)
                {
                    var contratoObj = SalvarContrato(contrato);
                    lstContratos.Add(contratoObj);
                }
            }

            return lstContratos;
        }
        public ContratoDTO SalvarContrato(ContratoDTO contrato)
        {
            if (contrato != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(contrato);
                var contratoLegado = ConverterParaContratoLegado(contrato);

                if (contratoLegado != null)
                {

                    new ContratoLegadoSRV().SalvarContrato(contratoLegado);
                }
                return contrato;
            }

            return null;
        }
        /// <summary>
        /// Busca o último contrato que ainda não terminou sua vigência
        /// </summary>
        /// <param name="numeroAssinatura"></param>
        /// <returns></returns>
        public ContratoDTO BuscarUltimoContratoValido(string numeroAssinatura)
        {
            return _dao.BuscarUltimoContratoValido(numeroAssinatura);
        }
        /// <summary>
        /// Busca o último contrato vigente, pega o fim de sua vigência e calcula a próxima data inicial de vigência do próximo contrato.
        /// </summary>
        /// <param name="numeroAssinatura"></param>
        /// <returns></returns>
        public DateTime? RetornarProximaDataInicioVigencia(AssinaturaDTO assinatura)
        {
            if (assinatura != null)
            {
                var numeroAssinatura = assinatura.ASN_NUM_ASSINATURA;
                var contrato = BuscarUltimoContratoValido(numeroAssinatura);

                if (contrato != null && contrato.CTR_DATA_FIM_VIGENCIA != null)
                {
                    var fimDeVigencia = (DateTime)contrato.CTR_DATA_FIM_VIGENCIA;
                    var proximaDataInicioVigencia = fimDeVigencia.AddDays(1);

                    return proximaDataInicioVigencia;
                }
            }
            return null;
        }
        public bool TentarObterProximaDataInicioVigencia(AssinaturaDTO assinatura, out DateTime? data)
        {
            data = null;
            var dataInicioVigencia = RetornarProximaDataInicioVigencia(assinatura);

            if (dataInicioVigencia != null)
            {
                data = dataInicioVigencia;
                return true;
            }
            else
            {
                data = DateUtil.RetornaDiaPrimeiro(DateTime.Now);
            }

            if (data != null)
            {
                data = DateUtil.RetornaDiaPrimeiro(data);
                return true;
            }

            return false;
        }
        public void PreencherParcelas(IEnumerable<ContratoDTO> lstContratos)
        {
            if (lstContratos != null)
            {
                foreach (var contrato in lstContratos)
                {
                    PreencherParcelas(contrato);
                }
            }
        }
        public void PreencherParcelas(ContratoDTO contrato)
        {
            if (contrato != null)
            {
                var numeroContrato = contrato.CTR_NUM_CONTRATO;
                var lstParcelas = _parcelasSRV.BuscarPorContrato(numeroContrato);
                contrato.PARCELAS = lstParcelas;
            }
        }
        public void PreencherContratos(AssinaturaDTO assinatura)
        {
            if (assinatura != null && !string.IsNullOrEmpty(assinatura.ASN_NUM_ASSINATURA))
            {
                var numeroAssinatura = assinatura.ASN_NUM_ASSINATURA;
                var contratos = BuscarPorAssinatura(numeroAssinatura);
                assinatura.CONTRATOS = contratos;
            }
        }
        public Pagina<ContratoDTO> ListarContratos(string numeroAssinatura, int pagina = 1, int registrosPorPagina = 7)
        {

            var listaContratoDTO = _dao.ListarContratos(numeroAssinatura, pagina, registrosPorPagina);

            foreach (var item in listaContratoDTO.lista)
            {
                if (DateTime.Now >= item.CTR_DATA_INI_VIGENCIA && DateTime.Now <= item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Vigente";
                else if (DateTime.Now > item.CTR_DATA_FIM_VIGENCIA)
                    item.SITUACAO = "Encerrado";
                else if (DateTime.Now < item.CTR_DATA_INI_VIGENCIA)
                    item.SITUACAO = "Futuro";
            }

            return listaContratoDTO;
        }
        public Pagina<ContratoDTO> ListarContratos(string numeroAssinatura, int? IPE_ID = null, int? IPE_ID_EXCLUIR = null, int pagina = 1, int registrosPorPagina = 7)
        {
            return _dao.ListarContratos(numeroAssinatura, IPE_ID, IPE_ID_EXCLUIR, pagina, registrosPorPagina);
        }
        public ContratoDTO BuscarUltimoObjetoContrato(string codAssinatura)
        {
            return _dao.BuscarUltimoObjetoContrato(codAssinatura);
        }
        public IList<ContratoDTO> ListarContratosDoItemPedido(int? ipeId, string codAssinatura = null)
        {
            return _dao.ListarContratosDoItemPedido(ipeId, codAssinatura);
        }

        public IList<RelContratosCanceladosDTO> BuscarContratosCancelados(int _mes, int _ano, int? _emp_id , int? _grupo_id, int _tipo, int? _rep_id)
        {
            return _dao.BuscarContratosCancelados(_mes, _ano, _emp_id, _grupo_id, _tipo, _rep_id);
        }
        public IList<RelFaturamentoProdutoDTO> BuscarFaturamentoProduto(DateTime _dtini
                                                                      , DateTime _dtfim
                                                                      , int? _emp_id
                                                                      , int? _grupo_id
                                                                      , bool _tipodata)
        {
            return _dao.BuscarFaturamentoProduto(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);
        }

        public IList<RelFaturamentoRepresentanteDTO> BuscarFaturamentoRepresentante(DateTime _dtini, DateTime _dtfim, int? _emp_id, int? _rep_id, int? _grupo_id, int _tipo)
        {
            return _dao.BuscarFaturamentoRepresentante(_dtini, _dtfim, _emp_id, _rep_id, _grupo_id, _tipo);
        }

        public IList<RelFaturamentoRepresentanteDTO> BuscarFaturamentoRepresentante(int? _mes, int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id, int _tipo)
        {
            return _dao.BuscarFaturamentoRepresentante(_mes, _ano, _emp_id, _rep_id, _grupo_id, _tipo);
        }
        public Pagina<RelFaturamentoContratoDTO> BuscarFaturamentoContrato(string _ctr, string _asn)
        {
            return _dao.BuscarFaturamentoContrato(_ctr, _asn);
        }

        public Pagina<RelFaturamentoContratoDTO> BuscarFaturamentoContrato(DateTime? _dtini
                                                                         , DateTime? _dtfim
                                                                         , int? _emp_id
                                                                         , int? _grupo_id
                                                                         , bool _tipodata
                                                                         , int numpagina = 1
                                                                         , int linhas = 10)
        {
            return _dao.BuscarFaturamentoContrato(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata, numpagina, linhas);
        }
        public Pagina<RelFaturamentoContratoDTO> BuscarFaturamentoContratoSint(DateTime? _dtini
                                                                     , DateTime? _dtfim
                                                                     , int? _grupo_id
                                                                     , bool _tipodata
                                                                     , int numpagina = 1
                                                                     , int linhas = 10)
        {
            return _dao.BuscarFaturamentoContratoSint(_dtini, _dtfim, _grupo_id, _tipodata, numpagina, linhas);
        }

        public List<RelFaturamentoContratoDTO> BuscarFaturamentoContrato(DateTime? _dtini, DateTime? _dtfim, int? _emp_id, int? _grupo_id, bool _tipodata)
        {
            return _dao.BuscarFaturamentoContrato(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);
        }
        public List<RelAReceberDTO> BuscarTitulosAReceberLista(DateTime _dtini, DateTime _dtfim, int _emp_id, int _tipodata = 0, int _tiporel = 0, int _tipobanco = 0, string _banid = null, int _grupoid = 0, int _tipobaixa = 0)
        {
            return _dao.BuscarTitulosAReceberLista(_dtini, _dtfim, _emp_id, _tipodata, _tiporel, _tipobanco, _banid, _grupoid, _tipobaixa);
        }
        public RelAReceberTotalisDTO BuscarTitulosAReceber(DateTime _dtini, DateTime _dtfim, int _emp_id, int _tipodata = 0, int _tiporel = 0, int _tipobanco = 0, string _banid = null, int _grupoid = 0, int _tipobaixa = 0, int _rem_id = 0, int pagina = 1, int numpaginas = 12)
        {
            return _dao.BuscarTitulosAReceber(_dtini, _dtfim, _emp_id, _tipodata, _tiporel, _tipobanco, _banid, _grupoid, _tipobaixa, _rem_id, pagina, numpaginas);
        }

        public IList<RelFaturamentoRepresentanteSintDTO> BuscarFaturamentoRepresentanteSint(DateTime _dtini, DateTime _dtfim, int? _emp_id, int? _rep_id, int? _grupo_id)
        {
            return _dao.BuscarFaturamentoRepresentanteSint(_dtini, _dtfim, _emp_id, _rep_id, _grupo_id);
        }
        public IList<RelFaturamentoRepresentanteSintDTO> BuscarFaturamentoRepresentanteSint(int? _mes, int? _ano, int? _emp_id, int? _rep_id, int? _grupo_id)
        {
            return _dao.BuscarFaturamentoRepresentanteSint(_mes, _ano, _emp_id, _rep_id, _grupo_id);
        }

        public IList<RelFaturamentoAreaDTO> BuscarFaturamentoProdutoUF( DateTime _dtini
                                                                      , DateTime _dtfim
                                                                      , int _emp_id
                                                                      , int _grupo_id
                                                                      , bool _tipodata)
        {
            return _dao.BuscarFaturamentoProdutoUF(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);
        }
        public IList<RPT_CONTRATOS_POR_REGIAO_Result> BuscarFaturamentoUF( DateTime _dtini
                                                                         , DateTime _dtfim
                                                                         , int _emp_id
                                                                         , int _grupo_id
                                                                         , bool _ordalfabetica
                                                                         , bool _tipodata)
        {
            return _dao.BuscarFaturamentoUF(_dtini, _dtfim, _emp_id, _grupo_id, _tipodata);
        }
        public IList<RelResumoCReceberDTO> BuscarResumoCReceber( DateTime _dtini
                                                               , DateTime _dtfim
                                                               , int _emp_id
                                                               , bool _tipodata)
        {
            return _dao.BuscarResumoCReceber(_dtini , _dtfim , _emp_id, _tipodata) ;
        }

        public DateTime? RetornarDataDoUltimoFaturamentoPorEmpresa(int? empId)
        {
            return _dao.RetornarDataDoUltimoFaturamentoPorEmpresa(empId);
        }

        public void CancelarContratoDaAssinaturaEPedido(int? ipeId, string assinatura)
        {
            var contratos = ListarContratosDoItemPedido(ipeId, assinatura);
            CancelarContratos(contratos);
        }

        public void CancelarTodosOsContratosDaAssinatura(string assinatura)
        {
            var contratos = ListarContratosValidosPorAssinatura(assinatura);
            CancelarContratos(contratos);
        }

        public void CancelarContrato(ContratoDTO contrato, bool cancelarSenhaAssinatura = false, AlteracaoStatusDTO alteracaoStatus = null)
        {
            if (contrato != null)
            {
                CancelarContratos(new HashSet<ContratoDTO>() { contrato }, cancelarSenhaAssinatura, alteracaoStatus);
            }
        }

        public void CancelarContratos(IEnumerable<ContratoDTO> lstContratos, bool cancelarSenhaAssinatura = false, AlteracaoStatusDTO alteracaoStatus = null)
        {
            ICollection<ContratoLegadoDTO> lstContratoLegado = new HashSet<ContratoLegadoDTO>();
            if (lstContratos != null && lstContratos.Count() > 0)
            {
                if (cancelarSenhaAssinatura)
                {
                    var assinaturaSenhaSRV = ServiceFactory
                       .RetornarServico<AssinaturaSenhaSRV>();

                    var lstAssinaturas = lstContratos
                        .Select(x => x.ASN_NUM_ASSINATURA)
                        .Distinct();

                    foreach (var ass in lstAssinaturas)
                    {
                        assinaturaSenhaSRV.DeletarAssinaturaSenha(ass);
                    }
                }

                foreach (var con in lstContratos)
                {
                    con.CTR_DATA_CANC = DateTime.Now;
                    _parcelasSRV.CancelarParcelasDoContrato(con.CTR_NUM_CONTRATO);

                    var contratoLegado = _contratoLegadoSRV.FindById(con.CTR_NUM_CONTRATO);
                    if (contratoLegado != null)
                        lstContratoLegado.Add(contratoLegado);


                    if (alteracaoStatus != null)
                    {
                        var repId = alteracaoStatus.REP_ID;
                        var login = alteracaoStatus.USU_LOGIN;
                        var observacoes = alteracaoStatus.MOTIVO_ALTERACAO;
                        var cliId = ServiceFactory.RetornarServico<AssinaturaSRV>()
                            .RetornarIdClienteDaAssinatura(con.ASN_NUM_ASSINATURA);

                        ServiceFactory.RetornarServico<HistoricoNotificacaoSRV>()
                            .RegistrarHistoricoCancelamentoContrato(login, repId, cliId, con.CTR_NUM_CONTRATO, observacoes);
                    }
                }
                SaveOrUpdateNonIdentityKeyEntity(lstContratos);
                _contratoLegadoSRV.DeleteAll(lstContratoLegado);
            }

        }

        public IEnumerable<ContratoDTO> ListarContratosValidosPorAssinatura(string assinatura)
        {
            return _dao.ListarContratosValidosPorAssinatura(assinatura);
        }

        public void TransferirUltimoContratoDaAssinatura(string assinaturaOrigem, string assinaturaDestino, ProcessoTransferenciaAssinaturaDTO transferencia)
        {
            var contrato = BuscarUltimoContratoValido(assinaturaOrigem);
            
            if(contrato != null)
            {
                contrato.ASN_NUM_ASSINATURA = assinaturaDestino;
                var contratoLegado = _contratoLegadoSRV.FindById(contrato.CTR_NUM_CONTRATO);
                SaveOrUpdateNonIdentityKeyEntity(contrato);

                if (contratoLegado != null)
                {
                    contratoLegado.ASSINATURA = assinaturaDestino;
                    _contratoLegadoSRV.SaveOrUpdateNonIdentityKeyEntity(contratoLegado);
                }

            }
           // var lstContratosLegados = new List<ContratoLegadoDTO>();
            //if (lstContratos != null && lstContratos.Count() > 0)
            //{
            //    foreach(var con in lstContratos)
            //    {
            //        con.ASN_NUM_ASSINATURA = assinaturaDestino;
            //        var contratoLegado = _contratoLegadoSRV.FindById(con.CTR_NUM_CONTRATO);
            //        contratoLegado.ASSINATURA = assinaturaDestino;
            //        lstContratosLegados.Add(contratoLegado);
            //    }
            
           
        }
        public IList<ContratoDTO> ListarContratosQGeraNota(IList<int?> lstPedCrmId, int? nctId)
        {
            return _dao.ListarContratosQGeraNota(lstPedCrmId, nctId);
        }

        public int CriarContratoComAssinatura(AssinaturaDTO assinatura, string estado, int id_composicao, bool cortesia)
        {
            try
            {
                var codigoDoContrato = _paramSRV.GetCodigoContrato();
                var composicao = _produtoComposicao.FindById(id_composicao);
                var area = 1;
                var usuLogin = assinatura.ASN_NUM_ASSINATURA;


                int? codigoTipoPeriodo = null;

                var produtoCurso = false;

                int? pedidoId = 0;
                int? repId = null;
                int? rgId = null;
                int? cmpId = null;
                string repOperId = null;

                DateTime? inicioVigencia = null;
                DateTime? fimDeVigencia = null;
                DateTime? dataAnoVicencia = null;

                string codigoContratoSTR = null;                

                codigoDoContrato++;
                _paramSRV.AtualizarCodigoContrato(codigoDoContrato);

                if (codigoDoContrato != null)
                {
                    codigoContratoSTR = codigoDoContrato.ToString();
                }

                
                if (composicao != null)
                {
                    cmpId = composicao.CMP_ID;
                    ServiceFactory.RetornarServico<ProdutoComposicaoSRV>().ChecaEMarcaProdutoCurso(composicao);
                    if (composicao.EhCurso)
                    {
                        area = 4;
                        produtoCurso = true;
                        inicioVigencia = DateUtil.RetornaDiaPrimeiro(DateTime.Now);
                    }
                }
                else
                {
                    throw new FaturamentoException("Erro ao gerar contrato. Não é possível localizar as informações do produto a ser faturado.");
                }

                if (!produtoCurso)
                {
                    if (!TentarObterProximaDataInicioVigencia(assinatura, out inicioVigencia))
                    {
                        inicioVigencia = DateUtil.RetornaDiaPrimeiro(DateTime.Now);
                    }
                }

                dataAnoVicencia = inicioVigencia;

                // Se não existe um uf dessa região
                string regiaoUf = estado;
                var uf = ServiceFactory.RetornarServico<UFSRV>().FindById(regiaoUf);

                if (uf == null)
                {
                    regiaoUf = null;
                }

                var contrato = new ContratoDTO()
                {
                    AREA_ID = area,
                    ASN_NUM_ASSINATURA = assinatura.ASN_NUM_ASSINATURA,
                    CTR_ANO_VIGENCIA = ((DateTime)dataAnoVicencia).Year.ToString(),
                    PED_CRM_ID = pedidoId,
                    CTR_NUM_CONTRATO = codigoContratoSTR,
                    CTR_VLR_CONTRATO = 0,
                    CTR_VLR_ENTRADA = 0,
                    CTR_QTE_PARCELAS = 0,
                    CTR_VLR_PARCELAS = 0,
                    CTR_DATA_INI_VIGENCIA = inicioVigencia,
                    REGIAO_UF = regiaoUf,
                    CTR_DATA_FIM_VIGENCIA = fimDeVigencia,
                    REP_ID = repId,
                    RG_ID = rgId,
                    CMP_ID = cmpId,
                    CTR_DATA_FATURAMENTO_EFETIVO = DateTime.Now,
                    CTR_CORTESIA = (cortesia == true) ? (int?)1 : null,
                    TTP_ID = codigoTipoPeriodo,
                    REP_OPER_ID = repOperId,
                    CTR_PRORROGADO = 1
                };

                
                this.SaveOrUpdateNonIdentityKeyEntity(contrato);
                return (int)contrato.CMP_ID;
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível gerar o contrato", e);
            }
        }

        public void TransferirContratosDaAssinatura(string assinaturaOrigem, string assinaturaDestino)
        {
            var lstContratos = BuscarPorAssinatura(assinaturaOrigem);
            if (lstContratos != null)
            {
                foreach(var contrato in lstContratos)
                {
                    contrato.ASN_NUM_ASSINATURA = assinaturaDestino;
                }
            }
            SaveOrUpdateNonIdentityKeyEntity(lstContratos);
        }

    }
}