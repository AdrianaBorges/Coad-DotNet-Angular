using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.ModelBinding;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.EnvioEmail;
using COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate;
using COAD.CORPORATIVO.Model.Dto.Custons.Validacoes;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.CORPORATIVO.Service.Custons;
using COAD.CORPORATIVO.Util;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.SEGURANCA.Config.Email;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.SEGURANCA.Util;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Excel;
using GenericCrud.Exceptions;
using GenericCrud.Service;
using GenericCrud.Util;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PPI_ID")]
    public class PropostaItemSRV : GenericService<PROPOSTA_ITEM, PropostaItemDTO, int>
    {
        public PropostaItemDAO _dao; //= new PropostaDAO();
        public TipoPropostaSRV TipoPropostaSRV { get; set; }
        public PedidoStatusSRV PedidoStatusSRV { get; set; }
        public PedidoParticipanteSRV PedidoParticipanteSRV { get; set; }
        public PropostaItemComprovanteSRV PropostaItemComprovanteSRV { get; set; }
        public IEmailSRV EmailSRV { get; set; }
        public TipoPagamentoSRV TipoPagamentoSRV { get; set; }
        public ParcelasSRV ParcelaSRV { get; set; }
        public HistoricoNotificacaoSRV HistoricoNotificacao { get; set; }
        public AssinaturaSRV AssinaturaSRV { get; set; }
        public RegistroLiberacaoItemSRV _registroLiberacaoItemSRV { get; set; }
        public TipoPagamentoSRV _tipoPagamentoSRV { get; set; }
        public TemplateHTMLSRV _templateHTMLSRV { get; set; }
        public NotificacaoSistemaSRV _notificacaoSRV { get; set; }
        public JobAgendamentoSRV jobAgendamento { get; set; }

        public PropostaItemSRV()
        {
            _dao = new PropostaItemDAO();
            TipoPropostaSRV = new TipoPropostaSRV();
            PedidoStatusSRV = new PedidoStatusSRV();
            PedidoParticipanteSRV = new PedidoParticipanteSRV();
            PropostaItemComprovanteSRV = new PropostaItemComprovanteSRV();
            EmailSRV = new EmailSRV();
            TipoPagamentoSRV = new TipoPagamentoSRV();
            ParcelaSRV = new ParcelasSRV();
            HistoricoNotificacao = new HistoricoNotificacaoSRV();
            AssinaturaSRV = new SEGURANCA.Service.AssinaturaSRV();

            EmailActionContainer.AddActions("emailPropostaBoleto", parId =>
            {
                return RetornarBytesDoBoleto(parId);
            });
        }

        public PropostaItemSRV(PropostaItemDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;

            EmailActionContainer.AddActions("emailPropostaBoleto", parId =>
            {
                return RetornarBytesDoBoleto(parId);
            });
        }


        public ResumoPropostaDTO ObterResumoDaProposta(PropostaItemDTO propostaItem)
        {
            if (propostaItem != null)
            {
                var resumo = ObterResumoDaProposta(propostaItem.PPI_ID);
                return resumo;
              
            }
            return null;
        }

        public ResumoPropostaDTO ObterResumoDaProposta(int? ppiId)
        {
            var resumo =  _dao.ObterResumoDaProposta(ppiId);

            if(resumo != null)
            {
                if (resumo.TpgId != null)
                {
                    resumo.TipoPagamento = _tipoPagamentoSRV.FindByIdFullLoaded(resumo.TpgId);
                }

                if (resumo.ValorEntrada != null)
                {
                    if (resumo.TipoPagamento != null && resumo.TipoPagamento.ListaTipoPagamento != null)
                    {
                        var lstTipoPagamento = resumo.TipoPagamento.ListaTipoPagamento.ToList();
                        resumo.TipoPagamentoEntrada = lstTipoPagamento[0];
                        resumo.TipoPagamentoParcela = lstTipoPagamento[1];
                    }
                }
                else
                {
                    resumo.TipoPagamentoParcela = resumo.TipoPagamento;
                    resumo.DataDeVencimentoParcela = resumo.DataDeVencimentoEntrada;
                }
            }
            return resumo;

        }

        public PropostaItemDTO FindByIdFullLoaded(int? PPI_ID, bool preencherPropostaItemComprovante = false)
        {
            var propostaItem = FindById(PPI_ID);

            if (propostaItem != null)
            {
                if (preencherPropostaItemComprovante)
                {
                    PropostaItemComprovanteSRV.PreencherPropostaItemComprovante(propostaItem);
                }
            }

            return propostaItem;
        }

        public IList<PropostaItemDTO> ListarPropostaItemPorProposta(int? prtId)
        {
            var lstPropostaItem = _dao.ListarPropostaItemPorProposta(prtId);
            var cmpSRV = ServiceFactory.RetornarServico<ProdutoComposicaoSRV>();
            foreach (var proItm in lstPropostaItem)
            {
                proItm.TIPO_PAGAMENTO = TipoPagamentoSRV.FindByIdFullLoaded(proItm.TPG_ID);
                cmpSRV.ChecaEMarcaProdutoCurso(proItm.PRODUTO_COMPOSICAO);
            }

            return lstPropostaItem;
        }

        public void PreencherPropostaItem(PropostaDTO proposta, bool preencherPedidoParticipante = false, bool preencherPropostaItemComprovante = false, bool trazInfoFaturaImposto = false)
        {
            if (proposta != null && proposta.PRT_ID != null)
            {
                proposta.PROPOSTA_ITEM = ListarPropostaItemPorProposta(proposta.PRT_ID);

                foreach (var proItm in proposta.PROPOSTA_ITEM)
                {
                    if(proItm.PRODUTO_COMPOSICAO != null)
                    {
                        ServiceFactory.RetornarServico<ProdutoComposicaoSRV>()
                            .ChecaEMarcaProdutoCurso(proItm.PRODUTO_COMPOSICAO);
                    }

                    proItm.PROPOSTA = null;
                    if (preencherPedidoParticipante)
                    {
                        PedidoParticipanteSRV.PreencherPedidoParticipanteNaPropostaItem(proItm);
                    }

                    if (preencherPropostaItemComprovante)
                    {
                        PropostaItemComprovanteSRV.PreencherPropostaItemComprovante(proItm);
                    }
                }

                if (trazInfoFaturaImposto)
                {
                    ServiceFactory.RetornarServico<InfoFaturaSRV>().PreencherInfoFaturaImposto(proposta.PROPOSTA_ITEM);
                }
            }
        }

        public void ChecarExcluirPropostaItemAusentes(PropostaDTO proposta)
        {
            var propostaSRV = ServiceFactory.RetornarServico<PropostaSRV>();

            if (proposta != null)
            {
                var objetoDoBanco = propostaSRV.FindByIdFullLoaded(proposta.PRT_ID, true);
                var lstParaExcluir = GetMissinList(objetoDoBanco.PROPOSTA_ITEM, proposta.PROPOSTA_ITEM);

                MarcarPropostaItemComoExcluido(lstParaExcluir);
            }
        }

        public void SalvarPropostaItem(PropostaDTO proposta)
        {
            if (proposta != null && (proposta.PST_ID == null || proposta.PST_ID == 1))
            {
                if (proposta.PST_ID == null)
                    proposta.PST_ID = 1;

                var lstPropostaItem = proposta.PROPOSTA_ITEM;

                CheckAndAssignKeyFromParentToChildsList(proposta, lstPropostaItem, "PRT_ID");

                ChecarExcluirPropostaItemAusentes(proposta);

                var indexLoop = 0;
                foreach (var propostaItem in lstPropostaItem)
                {
                    ChecarValorParcelaETotal(propostaItem);
                    ChecarValoresDaProposta(propostaItem, indexLoop);
                    if (propostaItem.TPG_ID == null && propostaItem.TIPO_PAGAMENTO != null)
                    {
                        propostaItem.TPG_ID = propostaItem.TIPO_PAGAMENTO.TPG_ID;
                    }

                    if (propostaItem.PST_ID == null)
                        propostaItem.PST_ID = 1;

                    ChecarSePropostaItemFoiAlterada(propostaItem, proposta);
                    indexLoop++;
                }

                var lstPropostaItemSalvo = SaveOrUpdateAll(lstPropostaItem).ToList();

                var index = 0;

                foreach (var propostaItem in lstPropostaItem)
                {
                    if (propostaItem.PPI_ID == null && lstPropostaItemSalvo[index] != null)
                    {
                        propostaItem.PPI_ID = lstPropostaItemSalvo[index].PPI_ID;
                        propostaItem.Nova = true;
                        GerarCodLegado(propostaItem);
                    }

                    PedidoParticipanteSRV.SalvarPedidoParticipante(propostaItem);
                    ServiceFactory.RetornarServico<InfoFaturaSRV>().SalvarInfoFatura(propostaItem);
                    index++;
                }

                ProcessarParcelaDeEntradaPropostaItem(lstPropostaItem);
            }
        }


        private void GerarCodLegado(PropostaItemDTO propostaItem)
        {
            if (propostaItem != null && propostaItem.PPI_ID != null)
            {
                int? idCorrente = propostaItem.PPI_ID;

                string idString = MathUtil.PreencherZeroEsquerda((int)idCorrente, 5);
                idString += "P";
                propostaItem.COD_LEGADO = idString;

                SaveOrUpdate(propostaItem);
            }
        }

        public void MarcarPropostaItemComoExcluido(IEnumerable<PropostaItemDTO> lstPropostaItem)
        {
            if (lstPropostaItem != null)
            {
                foreach (var proItem in lstPropostaItem)
                {
                    proItem.DATA_EXCLUSAO = DateTime.Now;
                }

                MergeAll(lstPropostaItem);
            }
        }
        public int? RetornarEmpIdDaPropostaItem(int? ppiId)
        {
            return _dao.RetornarEmpIdDaPropostaItem(ppiId);
        }

        /// <summary>
        /// Verifica se os dados de pagamento da proposta item foi alterado e atribui o resultado 
        /// a proprieade Alterado da PropostaItem.
        /// </summary>
        /// <param name="propostaItem"></param>
        /// <returns></returns>
        private void ChecarSePropostaItemFoiAlterada(PropostaItemDTO propostaItem, PropostaDTO proposta)
        {
            if (propostaItem != null && propostaItem.PPI_ID != null && propostaItem.Alterada != true)
            {
                var objetoDoBanco = FindById(propostaItem.PPI_ID);

                bool resp =
                    (propostaItem.PPI_QTD_PARCELAS != objetoDoBanco.PPI_QTD_PARCELAS) ||
                    (propostaItem.PPI_TOTAL != objetoDoBanco.PPI_TOTAL) ||
                    (propostaItem.PPI_VALOR_ENTRADA != objetoDoBanco.PPI_VALOR_ENTRADA) ||
                    (propostaItem.PPI_VALOR_PARCELA != objetoDoBanco.PPI_VALOR_PARCELA) ||
                    (propostaItem.PPI_VALOR_UNITARIO != objetoDoBanco.PPI_VALOR_UNITARIO) ||
                    (propostaItem.TPG_ID != objetoDoBanco.TPG_ID) ||
                    (propostaItem.PPI_DATA_VENCIMENTO != objetoDoBanco.PPI_DATA_VENCIMENTO) ||
                    (propostaItem.PRT_IDENTIFICACAO_TURMA != objetoDoBanco.PRT_IDENTIFICACAO_TURMA) ||
                    (propostaItem.PPI_DATA_VENCIMENTO_SEG_PARCELA != objetoDoBanco.PPI_DATA_VENCIMENTO_SEG_PARCELA);

                propostaItem.Alterada = resp;

            }
        }

        /// <summary>
        /// Se o item de pedido possui o boleto como entrada ou a forma de pagamento única gera uma parcela.
        /// </summary>
        public void ProcessarParcelaDeEntradaPropostaItem(IEnumerable<PropostaItemDTO> lstPropostaItem)
        {
            if (lstPropostaItem != null)
            {
                foreach (var prItm in lstPropostaItem)
                {
                    if (prItm.TIPO_PAGAMENTO != null)
                    {

                        if (prItm.Alterada)
                            ParcelaSRV.ChecarEExcluirParcelaNaProposta(prItm.PPI_ID);

                        int? empId = ServiceFactory.RetornarServico<PropostaItemSRV>()
                            .RetornarEmpIdDaPropostaItem(prItm.PPI_ID);
                        //var tipoPagamentoEntradaAnterior = prItm
                        var tipoPagamentoEntrada = prItm.TIPO_PAGAMENTO.CodigoPagamento;

                        // verifica a proposta possui o tipo de pagamento boleto. Se tiver á crio.
                        if (tipoPagamentoEntrada == 7)
                        {
                            if (!ParcelaSRV.HasParcelaNaProposta(prItm.PPI_ID))
                            {
                                decimal? valorParcela = null;
                                bool entrada = false;
                                bool podeAlocar = false;
                                int? qtdParcelas = 1;
                                int? iffId = null;

                                if (prItm.TIPO_PAGAMENTO.TPG_TIPO == 1)
                                {
                                    valorParcela = prItm.PPI_VALOR_ENTRADA;
                                    entrada = true;
                                    iffId = prItm.IFF_ID_ENTRADA;
                                }
                                else
                                {
                                    valorParcela = prItm.PPI_VALOR_PARCELA;
                                    qtdParcelas = prItm.PPI_QTD_PARCELAS;
                                    iffId = prItm.IFF_ID;
                                }

                                var dataVencimento = prItm.PPI_DATA_VENCIMENTO;
                                var dataVencimentoSegParc = prItm.PPI_DATA_VENCIMENTO_SEG_PARCELA;

                                //if (dataVencimento < DateTime.Now)
                                //    dataVencimento = DateUtil.AdicionaDia(DateTime.Now, 2);

                                var dados = new List<DadosDeParcelaDTO>();
                                DateTime? novaDataVencimento = dataVencimento;
                                int? dia = novaDataVencimento.Value.Day;

                                for (int index = 0; index < qtdParcelas; index++)
                                {
                                    if (index > 0)
                                    {
                                        if (index == 1)
                                        {
                                            novaDataVencimento = dataVencimentoSegParc;

                                        }
                                        else
                                            novaDataVencimento = DateUtil.AdicionaMes(novaDataVencimento, 1, dia);
                                    }

                                    var dadosParcela = new DadosDeParcelaDTO()
                                    {
                                        dataVencimento = novaDataVencimento,
                                        empId = empId,
                                        entrada = entrada,
                                        numeroDaParcela = index,
                                        propostaItem = prItm,
                                        tipoPagamento = tipoPagamentoEntrada,
                                        valorParcela = valorParcela,
                                        PodeAlocar = podeAlocar,
                                        iffId = iffId
                                    };
                                    dados.Add(dadosParcela);
                                }

                                ParcelaSRV.GerarVariasParcelaDadosSemPedidoPagamento(dados);
                            }
                            else
                            {
                                if (prItm.PST_ID == 1)
                                    ParcelaSRV.AtualizarValorParcela(prItm);
                            }
                        }
                        else
                        {
                            // se não for boleto, verifico se existe parcela anterior, no caso da forma de pagamento ter sido alterada e excluo.
                            ParcelaSRV.ChecarEExcluirParcelaNaProposta(prItm.PPI_ID);
                        }
                    }
                }
            }
        }

        public void PagarPropostas(IEnumerable<ParcelasDTO> lstParcelas, int? repId, string usuario, bool baixarParcela = true)
        {
            foreach (var par in lstParcelas)
            {
                var ppiId = par.PPI_ID;
                PagarProposta(ppiId, repId, usuario, par);
            }

        }

        public bool PagarProposta(int? ppiId, int? repId, string usuario, ParcelasDTO parcela = null, bool baixarParcela = true, bool baixaManual = false)
        {
            var propostaItem = FindById(ppiId);
            var cliId = ServiceFactory.RetornarServico<PropostaSRV>().RetornarCliIdDaPropostaPorPropostaItem(ppiId);
            if (propostaItem.PST_ID == 1 ||
                propostaItem.PST_ID == 2)
            {
                AlterarStatusPropostaItem(ppiId, 7);
                if (parcela != null)
                {
                    if (baixarParcela && !ParcelaSRV.ExisteParcelasPagas(null, ppiId))
                    {
                        ParcelaSRV.PagarParcelaDaProposta(parcela, baixaManual);
                    }

                    var codigoPagamento = propostaItem.TIPO_PAGAMENTO.CodigoPagamento;
                    var tipoPagamento = TipoPagamentoSRV.FindById(codigoPagamento);
                    string tipoPagamentoDesc = null;

                    if (tipoPagamento != null)
                        tipoPagamentoDesc = tipoPagamento.TPG_DESCRICAO;

                    HistoricoNotificacao.histPedido.RegistrarHistoricoPagamentoProposta(usuario, repId, ppiId, 7, tipoPagamentoDesc);

                }
                else
                {
                    if (baixarParcela == true && !ParcelaSRV.ExisteParcelasPagas(null, ppiId))
                    {
                        ParcelaSRV.PagarParcelaDaProposta(ppiId, baixaManual);
                    }
                    HistoricoNotificacao.histPedido.RegistrarHistoricoConfirmarPagamento(usuario, repId, ppiId, 7);
                }

                AssinaturaSRV.ConcederAcessosDaProposta(ppiId, cliId, usuario);
                ServiceFactory.RetornarServico<RepresentanteSRV>().NotificarRepresentantePropostaPaga(ppiId, !baixaManual);
                return true;
            }
            return false;
        }

        public void AlterarStatusPropostaItem(int? ppiId, int? pstId)
        {
            var propostaItem = FindById(ppiId);
            propostaItem.PST_ID = pstId;
            Merge(propostaItem);
            AlterarStatusProposta(propostaItem);
        }

        /// <summary>
        /// Recusa a indicação de pagamento manual.
        /// </summary>
        /// <param name="status"></param>
        public void RecusarPagamentoDoPedido(AlteracaoStatusDTO status)
        {
            if (status != null)
            {
                using (var scope = new TransactionScope())
                {
                    var ppiId = status.PPI_ID;
                    var login = status.USU_LOGIN;
                    var cliId = status.CLI_ID;
                    var repId = status.REP_ID;
                    var propostaItem = FindById(ppiId);
                    var repIdRecebimento = propostaItem.PROPOSTA.REP_ID;

                    AlterarStatusPropostaItem(ppiId, 1);
                    HistoricoNotificacao.RegistrarHistoricoPropostaRecusaIndicacaoManualDePagamento(login, cliId, repId, repIdRecebimento, ppiId, status.OBSERVACOES);

                    scope.Complete();
                }
            }
        }
        public void InformarPedidoPagoComPendenciaDeConferencia(int? ppiId, string usuario, int? repId, string observacoes = null)
        {
            using (var scope = new TransactionScope())
            {
                var propostaItem = FindById(ppiId);
                propostaItem.PST_ID = 2;

                Merge(propostaItem);
                AlterarStatusProposta(propostaItem);

                var cliId = ServiceFactory.RetornarServico<PropostaSRV>().RetornarCliIdDaPropostaPorPropostaItem(ppiId);
                AssinaturaSRV.ConcederAcessosDaProposta(ppiId, cliId, usuario);
                HistoricoNotificacao.histPedido.RegistrarHistoricoPagamentoInformadoProposta(usuario, repId, ppiId, 2, observacoes);

                scope.Complete();
            }
        }

        public int? ChecaStatusItensIguais(int? PRT_ID)
        {
            return _dao.ChecaStatusItensIguais(PRT_ID);
        }

        public bool ChecaSeExisteStatus(int? prtId, int? statusId)
        {
            return _dao.ChecaSeExisteStatus(prtId, statusId);
        }

        public bool ChecaSeExisteStatusAguardandoAprovacaoPagamento(int? prtId)
        {
            return ChecaSeExisteStatus(prtId, 2);
        }

        public bool ChecaSeExisteStatusPendencia(int? prtId)
        {
            return ChecaSeExisteStatus(prtId, 9);
        }

        public void AlterarStatusProposta(PropostaItemDTO propostaItem)
        {
            if (propostaItem.PROPOSTA != null && propostaItem.PRT_ID != null)
            {
                var statusPredominante = ChecaStatusItensIguais(propostaItem.PRT_ID);
                if (statusPredominante != null) // se todos os filhos estão cancelados então altero o pai como cancelado
                {
                    propostaItem.PROPOSTA.PST_ID = statusPredominante;
                }
                else
                {
                    propostaItem.PROPOSTA.PST_ID = 1;

                    if (ChecaSeExisteStatusAguardandoAprovacaoPagamento(propostaItem.PRT_ID))
                    {
                        propostaItem.PROPOSTA.PST_ID = 2;
                    }

                    if (ChecaSeExisteStatusPendencia(propostaItem.PRT_ID))
                    {
                        propostaItem.PROPOSTA.PST_ID = 9;
                    }
                }

                ServiceFactory.RetornarServico<PropostaSRV>().Merge(propostaItem.PROPOSTA);
            }
        }

        public void MarcarManualmentePropostaComoPaga(int? ppiId, int? repId, string usuario)
        {
            using (var scope = new TransactionScope())
            {
                PagarProposta(ppiId, repId, usuario, null, true, true);
                scope.Complete();
            }
        }


        //public void EnviarLinkDoBoletoPorEmail(PropostaItemDTO propostaItem, int? cliId, string linkBoleto, string endEmail = null, string numParcela = null)
        //{
        //    var resumoPedido = ObterResumoDaProposta(propostaItem);
        //    linkBoleto = SysUtils.RetornarHostName() + "/cliente/GerarBoletoProposta?hashkey=" + linkBoleto;
        //    var nomeCliente = resumoPedido.NomeCliente;
        //    var nomeProduto = resumoPedido.NomeDoProduto;
        //    var qtd = resumoPedido.Quantidade;
        //    var codigoProposta = resumoPedido.CodigoProposta;
        //    var valorUnitario = resumoPedido.ValorParcela;
        //    var totalBruto = resumoPedido.TotalBruto;
        //    var total = resumoPedido.Total;

        //    var tabelaPedido = @"
        //    <div>
        //        <div><strong>Nome do Produto: </strong>{{nomeProduto}}</th>               
        //        <div><strong>Quantidade: </strong> {{qtd}}</div>                    
        //        <div><strong>Valor Unitário: </strong>R$ {{valorUnitario}}</div>                    
        //    </div>
        //    <div>
        //        <div style='float:right; max-width: 200px'>
        //            <label>Total Bruto:</label><strong>{{totalBruto}}</strong>
        //            <label>Total Liquido:</label><strong>R$ {{totalLiquido}}</strong>
        //        </div>
        //    </div>";


        //    tabelaPedido = tabelaPedido.Replace("{{nomeProduto}}", nomeProduto);
        //    tabelaPedido = tabelaPedido.Replace("{{qtd}}", "" + qtd);
        //    tabelaPedido = tabelaPedido.Replace("{{valorUnitario}}", "" + valorUnitario);
        //    tabelaPedido = tabelaPedido.Replace("{{totalBruto}}", "" + totalBruto);
        //    tabelaPedido = tabelaPedido.Replace("{{totalLiquido}}", "" + total);

        //    endEmail = SysUtils.DecidirEnderecoDeEmail(endEmail);
        //    if (endEmail != null)
        //    {
        //        var url = "https://ci4.googleusercontent.com/proxy/GgWnRPBud6_dbgT5a4AZGD1cXJaq7heSiSI6uRSLpqrbeRczzyf8rGzRft8ARSffAAjCKNryW9c1grWR6aZ4DfbBnsH6SAPgdbI5SsEUK5ISOjmLsiZKwAW0iJfwmKPQF_ufrNjh0VNiRRastLGv7F1SB7KA=s0-d-e1-ft#http://emkt.coad.com.br/emkt/dados/10268/10767/Image/Cursos_Novo/Header_Contabilidade_Geral.png";

        //        var templateEmail =
        //            @"<div style='padding:15px;'>
        //                <fieldset style='border:none;'>
        //                    <legend style='font-size:16px; color: #0970a3;'><strong>Pagamento do Boleto!!!</strong></legend>
        //                    <form>
        //                        <br />
        //                        <div style='font-size:14px'>
        //                            Prezado(a) {0},
        //                            Segue abaixo o link do boleto para o pedido {1}.
        //                            Verifique logo abaixo seus dados de acesso e comece agora mesmo!!                                    
        //                        </div>

        //                        <br />
        //                        <br />
        //                        {2}
        //                        <br /> 
        //                        <br />
        //                        <div>Link para o boleto: <a href='{3}' target='_new'>{4}</a></div>
        //                    </form>
        //                </fieldset>                    
        //            </div>";

        //        templateEmail = string.Format(templateEmail, nomeCliente, codigoProposta, tabelaPedido, linkBoleto, linkBoleto);
        //        EmailSRV.EnviarEmailParaCliente(endEmail, "Link para geração do boleto do pedido", templateEmail, url);
        //    }

        //}
        
        public byte[] RetornarBytesDoBoleto(string numeroParcela)
        {
            byte[] resposta = null;

            if (!string.IsNullOrWhiteSpace(numeroParcela))
            {
                var parametros = ParcelaSRV.RetornarDadosDoBoleto(numeroParcela);

                if (parametros.idConta == null)
                    throw new Exception("Não é possível achar os dados da conta.");
                if (parametros.idEmpresa == null)
                    throw new Exception("Não é possível achar os dados da empresa.");

                var boletoSRV = ServiceFactory
                        .RetornarServico<BoletoSRV>();
                resposta = boletoSRV.GerarVariosBoletosPDF(new List<ParametroDTO>() { parametros });
                
            }
            return resposta;
        }


        public void EnviarBoletoPorEmail(EnvioEmailDTO emailEnvioDTO)
        {
            if (emailEnvioDTO != null)
            {
                var PPI_ID = emailEnvioDTO.PPI_ID;
                var REP_ID = emailEnvioDTO.REP_ID;
                var USU_LOGIN = emailEnvioDTO.USU_LOGIN;

                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope())
                {
                    var propostaItem = FindById(PPI_ID);

                    if (propostaItem.DATA_EXCLUSAO != null)
                    {
                        throw new PedidoException("Esse Item da Proposta foi excluido.");
                    }

                    var proposta = propostaItem.PROPOSTA;
                    var parcela = ParcelaSRV.ObterProximaParcelaDaPropostaEmAberto(PPI_ID);
                    var statusDoPedido = propostaItem.PST_ID;

                    // ParcelaSRV.AdicionarContaEGerarNossoNumero(parcela, propostaItem);

                    var _email = emailEnvioDTO.LstEmail.FirstOrDefault();

                    if (String.IsNullOrWhiteSpace(parcela.PAR_NOSSO_NUMERO))
                    {
                        try
                        {

                            this.ParcelaSRV.RegistrarBoleto(parcela.PAR_NUM_PARCELA
                                                     , parcela.PAR_DATA_VENCTO
                                                     , (decimal)parcela.PAR_VLR_PARCELA
                                                     , _email.Email, REP_ID);
                        }
                        catch(Exception e)
                        {
                            throw new Exception("Não é possível registrar o boleto.", e);
                        }
                    }

                    if (proposta != null && parcela != null)
                    {
                        var cliId = proposta.CLI_ID;
                        
                        foreach(var email in emailEnvioDTO.LstEmail)
                        {
                            if (email.CadastrarEmail)
                                ServiceFactory.RetornarServico<AssinaturaEmailSRV>().AdicionarEmail(email.Email, cliId);
                        }

                        //EnviarEmailComBoleto(propostaItem, cliId, emailEnvioDTO.LstEmail, "emailPropostaBoleto", parcela.PAR_NUM_PARCELA);
                        EnviarEmailComBoletoComTemplate(propostaItem, cliId, emailEnvioDTO.LstEmail, "emailPropostaBoleto", parcela.PAR_NUM_PARCELA, REP_ID, USU_LOGIN);
                        HistoricoNotificacao.histPedido.RegistrarHistoricoEnvioLinkEmail(USU_LOGIN, REP_ID, emailEnvioDTO.EmailsConcat() , null, statusDoPedido, propostaItem.PPI_ID);
                    }

                    scope.Complete();
                }
            }
        }

        public void EnviarResumoDaProposta(EnvioEmailDTO envioEmail)
        {
            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            if (envioEmail == null)
                throw new Exception("Não á informações disponíveis para o envio de email.");

            if (envioEmail.LstEmail == null || envioEmail.LstEmail.Count <= 0)
                throw new Exception("Nenhum E-Mail foi informado.");

            var PPI_ID = envioEmail.PPI_ID;
            var USU_LOGIN = envioEmail.USU_LOGIN;
            var REP_ID = envioEmail.REP_ID;

            using (TransactionScope scope = new TransactionScope())
            {
                var propostaItem = FindById(PPI_ID);

                if (propostaItem.DATA_EXCLUSAO != null)
                {
                    throw new PedidoException("Esse Item da Proposta foi excluido.");
                }
                var proposta = propostaItem.PROPOSTA;
               
                if (proposta != null && envioEmail != null)
                {
                    var cliId = proposta.CLI_ID;

                    var resumoDaProposta = ObterResumoDaProposta(propostaItem);
                    var statusDoPedido = propostaItem.PST_ID;

                    if (resumoDaProposta.TipoPagamento.CodigoPagamento == 7)
                    {
                        throw new Exception(string.Format("Não é possível enviar o resumo. O tipo de pagamento não pode ser um boleto. Código Pagamento {0}", resumoDaProposta.TipoPagamento.CodigoPagamento));
                    }

                    var funcionadadeSistema = AuthUtil.RetornarFuncionalidadeSistemaDaRequisicao();
                    var corpo = ServiceFactory.RetornarServico<TemplateHTMLSRV>().ProcessarTemplate(funcionadadeSistema, resumoDaProposta);

                    foreach (var email in envioEmail.LstEmail)
                    {
                        if (email.CadastrarEmail)
                            ServiceFactory.RetornarServico<AssinaturaEmailSRV>().AdicionarEmail(email.Email, cliId);

                        email.Email = SysUtils.DecidirEnderecoDeEmail(email.Email);
                        //EmailSRV.EnviarEmail(email, "[COAD] - Pedido realizado com sucesso.", corpo, true, null, null, null, null);

                        var emailCC = ServiceFactory.RetornarServico<RepresentanteSRV>().RetornarEmailCCRepresentante(REP_ID);
                        EmailSRV.EnviarEmail(new EmailRequestDTO()
                        {
                            EmailDestino = email.Email,
                            Assunto = "[COAD] - Pedido realizado com sucesso.",
                            CorpoEmail = corpo,
                            codRepresentante = REP_ID,
                            usuario = USU_LOGIN,
                            CC_ERRO = emailCC
                        });
                    }

                    HistoricoNotificacao.histPedido.RegistrarHistoricoEnvioLinkEmail(USU_LOGIN, REP_ID, envioEmail.EmailsConcat(), null, statusDoPedido, propostaItem.PPI_ID);
                }

                scope.Complete();
            }
        }

        public bool ChecarPropostaEhDoRepresentante(int? repId, int? ppiId, out int? repIdDoPedido)
        {
            if (repId != null && ppiId != null)
            {
                var propostaItem = FindById(ppiId);

                if (propostaItem != null && propostaItem.PROPOSTA != null && propostaItem.PROPOSTA.REP_ID != null)
                {
                    var repIdDoPedidoR = propostaItem.PROPOSTA.REP_ID;
                    repIdDoPedido = repIdDoPedidoR;
                    return (repIdDoPedido == repId);
                }
            }
            repIdDoPedido = null;
            return false;
        }



        public void CancelarPropostaItem(AlteracaoStatusDTO cancelamento)
        {

            using (var scope = new TransactionScope())
            {
                CancelarPropostaItemSemTransacao(cancelamento);
                scope.Complete();
            }
        }


        public void CancelarPropostaItemSemTransacao(AlteracaoStatusDTO cancelamento)
        {
            if (cancelamento != null)
            {
                var ppiId = cancelamento.PPI_ID;
                var motivoCancelamento = cancelamento.OBSERVACOES;
                var login = cancelamento.USU_LOGIN;
                var cliId = cancelamento.CLI_ID;
                var repId = cancelamento.REP_ID;

                CancelarOuDevolverNotasDaProposta(ppiId);
                AlterarStatusPropostaItem(ppiId, 5);

                var propostaItem = FindById(ppiId);
                ParcelaSRV.ChecarEExcluirParcelaNaProposta(ppiId);
                if (propostaItem != null && propostaItem.PRT_ID != null)
                {
                    var pedidoCrm = propostaItem.PRT_ID;

                    HistoricoNotificacao.RegistrarHistoricoPropostaCancelada(login, cliId, repId, ppiId, motivoCancelamento);

                    int? idRepresentanteDoPedido = null;
                    if (!ChecarPropostaEhDoRepresentante(repId, ppiId, out idRepresentanteDoPedido))
                    {
                        ServiceFactory.RetornarServico<NotificacoesSRV>().InserirNotificacaoPropostaCancelada(repId, idRepresentanteDoPedido, cliId, pedidoCrm, ppiId, motivoCancelamento);
                    }
                }
            }
        }

        public string GerarHTMLResumoPropostaPraEnvioEmail(PropostaItemDTO propostaItem, int? cliId, string endEmail = null, string actionName = null, string numeroParcela = null)
        {
            var resumoPedido = ObterResumoDaProposta(propostaItem);
            var nomeCliente = resumoPedido.NomeCliente;
            var nomeProduto = resumoPedido.NomeDoProduto;
            var qtd = resumoPedido.Quantidade;
            var codigoProposta = resumoPedido.CodigoProposta;
            var valorUnitario = resumoPedido.ValorParcela;
            var total = resumoPedido.Total;

            if (resumoPedido.TipoPagamento.CodigoPagamento != 7)
            {
                throw new Exception(string.Format("Não é possível enviar o boleto. O tipo de pagamento está incorreto. Código Pagamento {0}", resumoPedido.TipoPagamento.CodigoPagamento));
            }
            //var funcionadadeSistema = AuthUtil.RetornarFuncionalidadeSistemaDaRequisicao();
            //var tabelaPedido1 = ServiceFactory.RetornarServico<TemplateHTMLSRV>().ProcessarTemplate(funcionadadeSistema, resumoPedido);
            var tabelaPedido = @" 
                <br><br>
                Quantidade: {{qtd}}<br>
                Valor unitário: R$ {{valorUnitario}}<br>
                Total Líquido: R$ {{totalLiquido}}<br><br>
            ";

            tabelaPedido = tabelaPedido.Replace("{{nomeProduto}}", nomeProduto);
            tabelaPedido = tabelaPedido.Replace("{{qtd}}", "" + qtd);
            tabelaPedido = tabelaPedido.Replace("{{valorUnitario}}", "" + valorUnitario);
            tabelaPedido = tabelaPedido.Replace("{{totalLiquido}}", "" + total);

            string conteudoTabela = "<tbody>";
            if (resumoPedido.ValorEntrada != null)
            {
                if (resumoPedido.TipoPagamento != null && resumoPedido.TipoPagamento.ListaTipoPagamento != null)
                {
                    var tipoPagamentoEntrada = resumoPedido.TipoPagamento.ListaTipoPagamento.ToList()[0];
                    var conteudo =
                    @"<tr>
                        <td><span class='label label-success'>Entrada</span>{0}</td>
                        <td><strong>{1}</strong> X <strong>{2}</strong></td>
                        <td>{2}</td>
                    </tr>";
                    conteudoTabela += string.Format(conteudo, tipoPagamentoEntrada.TPG_DESCRICAO, resumoPedido.QuantidadeParcela, resumoPedido.ValorEntrada,  resumoPedido.DataDeVencimentoEntrada);
                }

                if (resumoPedido.TipoPagamento != null && resumoPedido.TipoPagamento.ListaTipoPagamento != null)
                {
                    var lstTipoPagamento = resumoPedido.TipoPagamento.ListaTipoPagamento.ToList();

                    var tipoPagamentoEntrada = (resumoPedido.TipoPagamento.TPG_TIPO == 1 && lstTipoPagamento.Count() > 1)
                        ? lstTipoPagamento[1] :
                        lstTipoPagamento[0];
                    var dataVencimento = (resumoPedido.TipoPagamento.TPG_TIPO == 1) ? resumoPedido.DataDeVencimentoParcela : resumoPedido.DataDeVencimentoEntrada;


                    var conteudo =
                    @"<tr>
                        <td>{0}</td>
                        <td><strong>{1}</strong> X <strong>{2}</strong></td>
                        <td>{3}</td>
                    </tr>";
                    conteudoTabela += string.Format(conteudo, tipoPagamentoEntrada.TPG_DESCRICAO, resumoPedido.QuantidadeParcela, resumoPedido.ValorParcela, dataVencimento);
                }
            }

            conteudoTabela += @"</tbody>";

            tabelaPedido = tabelaPedido.Replace("{{corpoTabela}}", conteudoTabela);
            return tabelaPedido;
        }

        [Obsolete("Em breve esse método será substituído para o envio utilizando o template HTML")]
        public void EnviarEmailComBoleto(PropostaItemDTO propostaItem, int? cliId, string endEmail = null, string actionName = null, string numeroParcela = null, int? repId = null)
        {
            var nomeCliente = (propostaItem != null && propostaItem.PROPOSTA != null && propostaItem.PROPOSTA.CLIENTES != null)
                ? propostaItem.PROPOSTA.CLIENTES.CLI_NOME : "Não encontrado";

            string tabelaPedido = GerarHTMLResumoPropostaPraEnvioEmail(propostaItem, cliId, endEmail, actionName, numeroParcela);

            endEmail = SysUtils.DecidirEnderecoDeEmail(endEmail);
            if (endEmail != null)
            {
                var url = "http://www.coad.com.br/imagens/coadcorp/Header_FaltaPouco.png";

                var templateEmail = @"<div id=':lr' class='a3s aXjCH m15cf5173643ab5e2'>
                            Prezado(a) {0},<br><br>
                            Agora falta pouco para finalizar sua compra! Confira os dados abaixo:
                               {1}
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


                templateEmail = string.Format(templateEmail, nomeCliente, tabelaPedido);

                string emailCC = ServiceFactory.RetornarServico<RepresentanteSRV>().RetornarEmailCCRepresentante(repId);
                EmailSRV.EnviarEmailParaCliente(endEmail, "Finalize a compra do seu produto COAD", templateEmail, url, actionName, numeroParcela, emailCC, 3);
            }

        }

        public void EnviarEmailComBoletoComTemplate(PropostaItemDTO propostaItem, 
            int? cliId, 
            List<EmailDTO> lstEmail, 
            string actionName = null, 
            string numeroParcela = null,
            int? repId = null,
            string login = null)
        {
            //SysUtils.EnibirEmProducao();
            var resumoPedido = ObterResumoDaProposta(propostaItem);

            if(resumoPedido.TipoPagamento.CodigoPagamento != 7)
            {
                throw new Exception(string.Format("Não é possível enviar o boleto. O tipo de pagamento está incorreto. Código Pagamento {0}", resumoPedido.TipoPagamento.CodigoPagamento));
            }
            var funcionadadeSistema = AuthUtil.RetornarFuncionalidadeSistemaDaRequisicao();
            var corpo = ServiceFactory.RetornarServico<TemplateHTMLSRV>().ProcessarTemplate(funcionadadeSistema, resumoPedido);


            //EmailSRV.EnviarEmail(endEmail, "Finalize a compra do seu produto COAD", corpo, true, null, null, actionName, numeroParcela);

            foreach(var email in lstEmail)
            {
                email.Email = SysUtils.DecidirEnderecoDeEmail(email.Email);
                string emailCC = ServiceFactory.RetornarServico<RepresentanteSRV>().RetornarEmailCCRepresentante(repId);

                EmailSRV.EnviarEmail(new EmailRequestDTO() {

                    Assunto = "Finalize a compra do seu produto COAD",
                    EmailDestino = email.Email,
                    CorpoEmail = corpo,
                    ActionName = actionName,
                    ActionArg = numeroParcela,
                    codRepresentante = repId,
                    usuario = login,
                    CC_ERRO = emailCC,
                    codSMTP = 3                
                });
            }
        }

        /// <summary>
        /// Verifica se a proposta possui sua parcela paga mais não foi marcada como paga e baixa caso a parcela realmente seja paga.
        /// </summary>
        /// <param name="ppiId"></param>
        /// <param name="repId"></param>
        /// <param name="usuario"></param>
        public void ForcarBaixaAutomatica(int? ppiId, int? repId, string usuario)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (!ParcelaSRV.ExisteParcelasPagas(null, ppiId))
                    {
                        throw new PagamentoException("Não existem parcelas pagas.");
                    }

                    var propostaItem = FindById(ppiId);

                    if (propostaItem.PST_ID == 7)
                        throw new PagamentoException("A proposta já está paga pago");

                    if (propostaItem.PST_ID == 5)
                        throw new PagamentoException("A proposta está cancelada");

                    if (propostaItem.PST_ID == 8)
                        throw new PagamentoException("A proposta está paga e já exite pedido emitido");

                    PagarProposta(ppiId, repId, usuario, null, false, false);

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new PagamentoException("Não é possível marcar a proposta como paga", e);
            }
        }

        public PropostaItemDTO ListarPropostaItemDaAssinatura(string assinatura)
        {
            return _dao.ListarPropostaItemDaAssinatura(assinatura);
        }

        /// <summary>
        /// Compara o total do item de proposta com o somatório das parcelas mais a entrada
        /// </summary>
        /// <param name="propostaItem"></param>
        public void ChecarValorParcelaETotal(PropostaItemDTO propostaItem)
        {
            var valorParcela = (propostaItem.PPI_VALOR_PARCELA != null) ? propostaItem.PPI_VALOR_PARCELA : 0;
            var qtdParcela = (propostaItem.PPI_QTD_PARCELAS != null) ? propostaItem.PPI_QTD_PARCELAS : 0;
            var valorEntrada = (propostaItem.PPI_VALOR_ENTRADA != null) ? propostaItem.PPI_VALOR_ENTRADA : 0;
            var valorTotal = (propostaItem.PPI_TOTAL != null) ? propostaItem.PPI_TOTAL: 0;

            if(propostaItem.TPG_ID != 9)
            {
                var totalReal = (qtdParcela * valorParcela) + valorEntrada;
                if (valorTotal != totalReal)
                {
                    throw new InvalidDataException("Não é possível emitir a proposta. O total da proposta não corresponde ao 'Valor Total Parcelas'.");
                }
            }
        }

        public void ChecarValoresDaProposta(PropostaItemDTO propostaItem, int? index)
        {
            if(propostaItem.PPI_CORTESIA == null || propostaItem.PPI_CORTESIA == false)
            {
                var modelState = new ModelStateDictionary();
                var valorParcela = (propostaItem.PPI_VALOR_PARCELA != null) ? propostaItem.PPI_VALOR_PARCELA : 0;
                var qtdParcela = (propostaItem.PPI_QTD_PARCELAS != null) ? propostaItem.PPI_QTD_PARCELAS : 0;
                var valorEntrada = (propostaItem.PPI_VALOR_ENTRADA != null) ? propostaItem.PPI_VALOR_ENTRADA : 0;
                var valorTotal = (propostaItem.PPI_TOTAL != null) ? propostaItem.PPI_TOTAL : 0;
                var tipoPagamento = propostaItem.TIPO_PAGAMENTO;
            
                if(tipoPagamento != null && tipoPagamento.TPG_TIPO == 0 && qtdParcela > 1 && tipoPagamento.TPG_ID != 9)
                {
                    modelState.AddModelError(string.Format("PROPOSTA_ITEM[{0}]PPI_QTD_PARCELAS", index), "Quantidade de parcela inválida para pagamento à vista.");
                }

                if(!modelState.IsValid)
                    throw new ValidacaoException("Ocorreu um erro ao validação na proposta.", modelState);
            }

        }

        /// <summary>
        /// Retorna os itens de proposta cuja entrada ou forma de pagamento única é boleto.
        /// </summary>
        /// <param name="prtId"></param>
        /// <returns></returns>
        public IList<PropostaItemDTO> ListarPropostaItemDeBoleto(int? prtId)
        {
            return _dao.ListarPropostaItemDeBoleto(prtId);
        }

        public PropostaItemDTO FindByRegistroLiberacao(int? rliId)
        {
            return _dao.FindByRegistroLiberacao(rliId);
        }

        public ValidacaoRegraPropostaItemDTO RealizarValidacoesDeRegras(PropostaItemDTO propostaItem, PropostaDTO proposta)
        {
            var validacaoPropostaItem = new ValidacaoRegraPropostaItemDTO()
            {
                ppiId = propostaItem.PPI_ID,
                DescricaoProduto = propostaItem.PRODUTO_COMPOSICAO.CMP_DESCRICAO
            };  

            ValidarRegrasDeCampanhaPorItem(propostaItem, proposta, validacaoPropostaItem);

            if(proposta.TPP_ID == 2)
                ChecarInadimplenciaAssinaturaPorItem(propostaItem, proposta, validacaoPropostaItem);
            return validacaoPropostaItem;
        }

        public void ChecarInadimplenciaAssinaturaPorItem(PropostaItemDTO propostaItem, PropostaDTO proposta, ValidacaoRegraPropostaItemDTO validacaoPropostaItem)
        {
            if(proposta != null && propostaItem != null && proposta.TPP_ID == 2 && 
                !string.IsNullOrWhiteSpace(propostaItem.ASN_NUM_ASSINATURA))
            {
                var clienteInadimplente = ServiceFactory.RetornarServico<ClienteSRV>()
                    .ExecutarValidacaoDeInadimplencia(proposta.CLI_ID, proposta.TPP_ID, propostaItem.ASN_NUM_ASSINATURA);

                var mensagem = string.Format(@"A assinatura {0} possui {1} parcelas em aberto (Não pagas).", 
                    propostaItem.ASN_NUM_ASSINATURA, clienteInadimplente.ClienteInadimplente.Parcelas.Count);

                if (clienteInadimplente.ExisteInadimplencia)
                {
                    validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                    {
                        Nome = "Cliente com parcelas da assinatura não pagas",
                        Descricao = mensagem
                    });
                }
            }
        }        

        public void ValidarRegrasDeCampanhaPorItem(PropostaItemDTO propostaItem, PropostaDTO proposta, ValidacaoRegraPropostaItemDTO validacaoPropostaItem)
        {
            var tipoProposta = proposta.TPP_ID;
            var dataAtual = DateTime.Now;
            var numParcela = propostaItem.PPI_QTD_PARCELAS;
            var tpg = (propostaItem.TIPO_PAGAMENTO != null) ? propostaItem.TIPO_PAGAMENTO.CodigoPagamento : 0;

            if(propostaItem.PPI_VALOR_ENTRADA != null && propostaItem.PPI_VALOR_ENTRADA > 0)
                numParcela++;

            var campanhaSRV = ServiceFactory.RetornarServico<CampanhaVendaSRV>();
            ICollection<CampanhaVendaDTO> lstCampanhaVenda = null;

            lstCampanhaVenda = campanhaSRV.BuscarCampanhaVenda(dataAtual, tipoProposta, tpg, numParcela, propostaItem.CMP_ID);

            if(lstCampanhaVenda == null || lstCampanhaVenda.Count <= 0)
            {
                lstCampanhaVenda = campanhaSRV.BuscarCampanhaVenda(dataAtual, tipoProposta, tpg, numParcela);
            }
                       

            if (lstCampanhaVenda != null)
            {
                foreach (var campanhaVenda in lstCampanhaVenda)
                {
                    var valorUnitario = (propostaItem.PPI_VALOR_UNITARIO_PRODUTO != null) ? propostaItem.PPI_VALOR_UNITARIO_PRODUTO : 0;
                    var qtd = (propostaItem.PPI_QTD != null) ? propostaItem.PPI_QTD : 0;
                    var totalBruto = valorUnitario * qtd;
                    var total = (propostaItem.PPI_TOTAL != null) ? propostaItem.PPI_TOTAL : 0;

                    // Validação de acrescimo
                    var acrescimoMinimo = campanhaVenda.CVE_ACRESCIMO_MINIMO;
                    var diasMinDataAtual = campanhaVenda.CVE_DIAS_MIN_PRIMEIRA_PARCELA;
                    var diasEntreAPrimeiraESegundaParcela = campanhaVenda.CVE_DIAS_MIN_SEGUNDA_PARCELA;
                    var diasMaxDataAtual = campanhaVenda.CVE_DIAS_MAX_PRIMEIRA_PARCELA;
                    var diasFixoDataAtual = campanhaVenda.CVE_DIAS_FIXO_SEGUNDA_PARCELA;

                    // Descontos
                    var descontoMaximo = campanhaVenda.CVE_DESCONTO_MAX;

                    if (acrescimoMinimo != null)
                    {
                        if (descontoMaximo != null)
                            acrescimoMinimo = acrescimoMinimo - descontoMaximo;

                        var valorBrutoTeste = totalBruto + ((new decimal((int)acrescimoMinimo) / 100) * totalBruto);
                        if (total < valorBrutoTeste)
                        {
                            string mensagem = @"O valor total da proposta é R$ {0:N}.
                                        Porém, o acrescimo mínimo para esse tipo de venda está configurado para {1}%.
                                        Com base no valor mínimo do produto na proposta de R$ {2:N} o total desse item deveria ser no mínimo de R$ {3:N}.";
                            mensagem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), mensagem, total, acrescimoMinimo, totalBruto, valorBrutoTeste);
                            validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                            {

                                Nome = "Condição de Acréscimo Mínimo",
                                Descricao = mensagem
                            });
                        }
                    }

                    if (descontoMaximo != null && (acrescimoMinimo == null || acrescimoMinimo == 0))
                    {
                       var valorDesconto = ((new decimal((int)descontoMaximo) / 100) * totalBruto);
                       var valorComDescontoMaximo = totalBruto - valorDesconto;

                        if (total < valorComDescontoMaximo)
                        {
                            string mensagem = @"O valor total da proposta é R$ {0:N}.
                                        Porém, o desconto máximo para esse tipo de venda está configurado para {1}%.
                                        Com base no valor máximo de {2:N} de desconto o total do item deveria ser no mínimo de R$ {3:N}.";
                            mensagem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), mensagem, total, descontoMaximo, valorDesconto, valorComDescontoMaximo);
                            validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                            {

                                Nome = "Condição de Acréscimo Mínimo",
                                Descricao = mensagem
                            });
                        }
                    }

                    // Validação de Data

                    if ((diasEntreAPrimeiraESegundaParcela != null || diasMinDataAtual != null) &&
                        propostaItem.PPI_DATA_VENCIMENTO != null || diasMaxDataAtual != null)
                    {
                        var data = DateTime.Now;

                        var dataVencPrimeira = propostaItem.PPI_DATA_VENCIMENTO;


                        if (diasMinDataAtual != null)
                        {
                            var dataTeste = DateUtil.AdicionaDia(data, (int)diasMinDataAtual);

                            if (dataVencPrimeira != null && dataVencPrimeira.Value.Date < dataTeste.Value.Date)
                            {
                                string mensagem = @"A data da primeira parcela é de {0: dd/MM/yyyy} mas esse tipo de venda está configurado para ter no mínimo {1} dias da data atual.
                                               A data mínima da primeira parcela deveria ser {2: dd/MM/yyyy}";
                                mensagem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), mensagem, dataVencPrimeira, diasMinDataAtual, dataTeste);
                                validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                                {
                                    Nome = "Condição de Data Mínima da Primeira Parcela",
                                    Descricao = mensagem
                                });
                            }
                        }
                        else
                        {
                            diasMinDataAtual = (diasMinDataAtual != null) ? diasMinDataAtual : 0;
                        }
                        
                        if (diasMaxDataAtual != null)
                        {
                            var dataTeste = DateUtil.AdicionaDia(data, (int)diasMaxDataAtual);

                            if (dataVencPrimeira.Value.Date > dataTeste.Value.Date)
                            {
                                string mensagem = @"A data da primeira parcela é de {0: dd/MM/yyyy} mas esse tipo de venda está configurado para ter no máximo {1} dias da data atual.
                                               A data máxima da primeira parcela deveria ser {2: dd/MM/yyyy}";
                                mensagem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), mensagem, dataVencPrimeira, diasMaxDataAtual, dataTeste);
                                validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                                {
                                    Nome = "Condição de Data Máxima da Primeira Parcela",
                                    Descricao = mensagem
                                });
                            }
                        }


                        if (diasFixoDataAtual != null)
                        {
                            var dataTeste = DateUtil.AdicionaDia(data, (int)(diasMinDataAtual + diasFixoDataAtual));

                            var dataVencSegunda = propostaItem.PPI_DATA_VENCIMENTO_SEG_PARCELA;

                            if (dataVencSegunda != null && !dataVencSegunda.Value.Date.Equals(dataTeste.Value.Date))
                            {
                                string mensagem = @"A data da segunda parcela é de {0: dd/MM/yyyy} mas esse tipo de venda está configurado para ter {1} dias entre a primeira parcela e a segunda.
                                                    A data da segunda parcela deveria ser {2: dd/MM/yyyy}";
                                mensagem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), mensagem, dataVencSegunda, diasFixoDataAtual, dataTeste);
                                validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                                {
                                    Nome = "Condição de Data Fixa entre a Primeira e Segunda Parcela",
                                    Descricao = mensagem
                                });
                            }
                        }

                        if (diasEntreAPrimeiraESegundaParcela != null)
                        {
                            var dataVencSegunda = propostaItem.PPI_DATA_VENCIMENTO_SEG_PARCELA;
                            var dataTeste = DateUtil.AdicionaDia(data, (int)(diasMinDataAtual + diasEntreAPrimeiraESegundaParcela));

                            if (dataVencSegunda != null && (dataVencSegunda.Value.Date < dataTeste.Value.Date))
                            {
                                string mensagem = @"A data da segunda parcela é de {0: dd/MM/yyyy} mas esse tipo de venda está configurado para ter no mínimo {1} dias da data da primeira parcela.
                                                A data mínima da segunda parcela deveria ser {2: dd/MM/yyyy}";
                                mensagem = string.Format(CultureInfo.GetCultureInfo("pt-BR"), mensagem, dataVencSegunda, diasEntreAPrimeiraESegundaParcela, dataTeste);
                                validacaoPropostaItem.Validacoes.Add(new ValidacaoRegraDetalhesDTO()
                                {
                                    Nome = "Condição de Data da Entre a primeira e Segunda Parcela",
                                    Descricao = mensagem
                                });
                            }
                        }

                    }
                }
            }
        }

        public void RegistrarLiberacaoDosItens(PropostaItemDTO propostaItem, ICollection<ValidacaoRegraPropostaItemDTO> validacaoItem, int? REP_ID, string usuario, bool inserirHistorico)
        {
            if (propostaItem != null && validacaoItem != null && validacaoItem.Count > 0)
            {
                ValidacaoRegraPropostaItemDTO validacaoDaProposta = null;

                if(validacaoItem.Where(x => x.ppiId == propostaItem.PPI_ID).Count() <= 0)
                {
                    validacaoDaProposta = RealizarValidacoesDeRegras(propostaItem, propostaItem.PROPOSTA);
                }
                else
                {
                    validacaoDaProposta = validacaoItem.Where(x => x.ppiId == propostaItem.PPI_ID).FirstOrDefault();
                }

                if (validacaoDaProposta != null && !validacaoDaProposta.EhValido)
                {
                    var representanteQueExecutouAAcao = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);
                    string nomeRepresentanteQueExecutouAAcao =
                                        (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                            ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

                    int index = 0;
                    foreach (var val in validacaoDaProposta.Validacoes)
                    {

                        bool inserirHistoricoLocal = (index == 0 && inserirHistorico);
                        index++;
                        string mensagem = "O representante {{representante}} emitiu um item de proposta '{{PPI_ID}}'. Porém ouve pendência para liberação da mesma. ";
                        mensagem += string.Format("Detalhes: {0}. ", val);

                        var descriptor = new HistoricoFormatterSRV()
                        {
                            CLI_ID = propostaItem.PROPOSTA.CLI_ID,
                            Message = mensagem,
                            PPI_ID = propostaItem.PPI_ID,
                            REP_ID = REP_ID,
                            PST_ID = 9,
                            usuario = usuario
                        };

                        mensagem = descriptor.FormatMessage();
                        _registroLiberacaoItemSRV.CriarRegistroLiberacaoItem(propostaItem, mensagem, REP_ID, usuario, true, inserirHistoricoLocal);
                    }
                }
            }
        }


        public IList<ItemRelatorioPropostaEmAtrasoDTO> ListarCodPropostasItemEmAtraso24Horas()
        {
            var data24Horas = DateUtil.AdicionaDia(DateTime.Now, -1);
            return ListarCodPropostasItemEmAtrasoFixo(data24Horas);
        }

        public IList<ItemRelatorioPropostaEmAtrasoDTO> ListarCodPropostasItemEmAtraso72Horas()
        {
            var data72Horas = DateUtil.AdicionaDia(DateTime.Now, -3);
            return ListarCodPropostasItemEmAtrasoFixo(data72Horas);
        }

        public IList<ItemRelatorioPropostaEmAtrasoDTO> ListarCodPropostasItemEmAtrasoFixo(DateTime? dataVenc)
        {
            return _dao.ListarQTDPropostasItemEmAtrasoFixo(dataVenc);
        }

        private DetalhesPropostasEmAtrasoDTO GerarDetalhesPropostaEmAtraso(IList<ItemRelatorioPropostaEmAtrasoDTO> lstPropostasAtrasadas)
        {
            return new DetalhesPropostasEmAtrasoDTO()
            {
                RelatorioPropostaEmAtraso = lstPropostasAtrasadas,
                DataDeChecagem = DateTime.Now,
                HostName = SysUtils.RetornarHostName(),
                Ambiente = SysUtils.RetornarAmbienteNameTotal()
            };
        }

        public void ProcessarJobPropostasPagAtrasados(BatchContext batchContext = null)
        {

            int? ppiIdRef = null;
            if(batchContext == null)
                batchContext = new BatchContext();
            try
            {
                batchContext.IniciarPassoBatch("Gerando Relatorios", true, 2);

                var propostaAtrasos24Horas = ListarCodPropostasItemEmAtraso24Horas();
                var propostaAtrasos72Horas = ListarCodPropostasItemEmAtraso72Horas();

                if (propostaAtrasos24Horas != null && propostaAtrasos24Horas.Count > 0)
                {
                    var templateHTML = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(3);
                    if (templateHTML != null)
                    {
                        EnviarEmailPagamentoAtrasado(propostaAtrasos24Horas, templateHTML, 24, 1);                           
                    }
                    else
                    {
                        throw new Exception("Não foi possível encontrar o template para a funcionalidade de sistema 'Notificação proposta 24 horas de atraso'");
                    }
                }
                batchContext.IncrementarPassoBatch();
               
                if (propostaAtrasos72Horas != null && propostaAtrasos72Horas.Count > 0)
                {
                    var templateHTML = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(4);
                    if (templateHTML != null)
                    {
                        EnviarEmailPagamentoAtrasado(propostaAtrasos72Horas, templateHTML, 72, 1);
                    }
                    else
                    {
                        throw new Exception("Não foi possível encontrar o template para a funcionalidade de sistema 'Notificação proposta 72 horas de atraso'");
                    }
                }

                batchContext.IncrementarPassoBatch();
            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro ao notificar proposta em atraso.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Listagem de Propostas com Pagamentos Atrasados",
                    projeto = "CORPORATIVO",
                    servico = "PropostaSRV",
                    tipoJob = 6,
                    descricaoCodigoReferencia = "Código da Proposta Item",
                    codReferencia = ppiIdRef,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,
                    
                });
            }
        }

        private void InserirNotificacao(ItemRelatorioPropostaEmAtrasoDTO proAtr, int qtdDias)
        {
            var codReferencia = string.Format("{0}-{1}", qtdDias, proAtr.CodItemProposta);
            var notificacaoRepSRV = ServiceFactory.RetornarServico<NotificacoesSRV>();
            var jaNotificado = notificacaoRepSRV.ChecaRepreJaNotificado(proAtr.CodRepresentante, 7, codReferencia);
            if (!jaNotificado)
            {
                var mensagem = string.Format("O item de proposta {0} e proposta de código {1} está atrasada há {2} horas. Nome do Cliente '{3}'. Produto {4}", proAtr.CodItemProposta, proAtr.CodProposta, qtdDias, proAtr.NomeCliente, proAtr.Produto);
                notificacaoRepSRV.InserirNotificacao(7, "WARN", mensagem, proAtr.CodCliente, proAtr.CodRepresentante, codRefStr: codReferencia);
            }
        }

        /// <summary>
        /// Notifica todos representantes que possuem propostas atrasadas
        /// </summary>
        /// <param name="batchContext"></param>
        public void ProcessarJobNotiReprePropostasPagAtrasados(BatchContext batchContext = null)
        {

            int? ppiIdRef = null;
            if (batchContext == null)
                batchContext = new BatchContext();
            try
            {
                
                var propostaAtrasos24Horas = ListarCodPropostasItemEmAtraso24Horas();
                var propostaAtrasos72Horas = ListarCodPropostasItemEmAtraso72Horas();

                if (propostaAtrasos24Horas != null && propostaAtrasos24Horas.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Notificando atrasos de 24 horas.", true, propostaAtrasos24Horas.Count);

                    foreach (var proAtr in propostaAtrasos24Horas)
                    {
                        InserirNotificacao(proAtr, 24);
                        batchContext.IncrementarPassoBatch();
                    }
                }

                if (propostaAtrasos72Horas != null && propostaAtrasos72Horas.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Notificando atrasos de 72 horas.", true, propostaAtrasos24Horas.Count);

                    foreach (var proAtr in propostaAtrasos72Horas)
                    {
                        InserirNotificacao(proAtr, 72);
                        batchContext.IncrementarPassoBatch();
                    }
                }

            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro ao notificar proposta em atraso do Representante.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Listagem de Propostas com Pagamentos Atrasados Repre",
                    projeto = "CORPORATIVO",
                    servico = "PropostaSRV",
                    tipoJob = 6,
                    descricaoCodigoReferencia = "Código da Proposta Item",
                    codReferencia = ppiIdRef,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,
                });
            }
        }
        private void EnviarEmailPagamentoAtrasado(IList<ItemRelatorioPropostaEmAtrasoDTO> propostasEmAtraso, TemplateHTMLDTO templateHTML, int? tempoAtrasado, int? tnsId)
        {

            using(var scope = new TransactionScope())
            {
                var detalhesPropostasEmAtraso = GerarDetalhesPropostaEmAtraso(propostasEmAtraso);
                if(detalhesPropostasEmAtraso != null)
                {
                    var ambiente = SysUtils.RetornarAmbienteNameTotal();

                    if (!string.IsNullOrWhiteSpace(ambiente))
                        ambiente = StringUtil.LimparAcentuacao(ambiente.ToLower());

                    var fileName = string.Format(@"C:\planilha_temp\{0}\relatorio-{1:yyyy-MM-dd hh-mm-ss}.xlsx", ambiente, DateTime.Now);
                    var excelLoad = new ExcelLoad();
                    excelLoad.ToSheet(fileName, propostasEmAtraso);

                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(templateHTML, detalhesPropostasEmAtraso);
                    var email = jobAgendamento.EmailJobAgendamento(6);

                    _notificacaoSRV.Incluir(new RegistroNotificacaoSistemaDTO()
                    {
                        codTipoJob = 1,
                        codTipoNotificacaoSistema = tnsId,
                        data = DateTime.Now,
                        descricao = string.Format("Existem {0} propostas que estão atrasadas há mais de 24 horas. Um E-Mail de notificação será enviado para o representante.", propostasEmAtraso),
                        nomeDaExecucao = "Notificação de E-Mail para propostas atrasadas há 24 horas.",
                        nomeProjeto = "COAD.CORPORATIVO",
                        nomeServico = "PropostaItemSRV",
                        descricaoCodigoReferencia =  "Código do Item de Proposta"
                    });
                    var assunto = string.Format("Relatório de Propostas atrasadas há {0} - {1:dd/MM/yyyy - hh:mm:ss}. [{2}]", tempoAtrasado, detalhesPropostasEmAtraso.DataDeChecagem, detalhesPropostasEmAtraso.Ambiente);


                    EmailSRV.EnviarEmail(new EmailRequestDTO() {
                        EmailDestino = email,
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        pathAnexo = fileName
                    });

                }
                scope.Complete();
            }
        }

        public COAD.FISCAL.Model.Enumerados.TipoPagamentoEnum RetornarTipoPagamentoEntrada(int? ppiID)
        {

            var propostaItem = FindById(ppiID);
            int? tpg = null;
            if(propostaItem != null)
            {
                var tipoPg = propostaItem.TPG_ID;
                var tipoPagamento = ServiceFactory.RetornarServico<TipoPagamentoSRV>().FindByIdFullLoaded(ppiID);
                if (tipoPagamento != null)
                    tpg = tipoPagamento.CodigoPagamento;

                switch (tpg)
                {
                    case 7: return FISCAL.Model.Enumerados.TipoPagamentoEnum.BOLETO_BANCARIO;
                    case 8: return FISCAL.Model.Enumerados.TipoPagamentoEnum.CHEQUE;
                    case 9: return FISCAL.Model.Enumerados.TipoPagamentoEnum.CARTAO_CREDITO;
                    case 10: return FISCAL.Model.Enumerados.TipoPagamentoEnum.OUTROS;
                }
            }
            return FISCAL.Model.Enumerados.TipoPagamentoEnum.OUTROS;
        }

        public void RegistrarHistPedidoNotaFiscalAutorizada(PropostaItemDTO propostaItem, int? numeroNota, int? codRetorno, string motivo)
        {
            HistoricoNotificacao.RegistrarHistoricoNotaFiscalAutorizada("COADCORP", 1, null, numeroNota, codRetorno, motivo, propostaItem.PPI_ID);
        }

        /// <summary>
        /// Envia a nota fiscal para o SEFAZ
        /// </summary>
        /// <param name="notaFiscalBatch"></param>
        /// <returns></returns>
        public IList<INFeLote> AdicionarVariasNotasAntecipacaoAoLoteNFe(int? ppiID, int? repID)
        {
            IList<INFeLote> retorno = new List<INFeLote>();

            try
            {
                using (var scope = new TransactionScope())
                {
                    if (ppiID != null)
                    {
                        var itemProposta = FindById(ppiID);

                        if (itemProposta != null)
                        {
                            if(itemProposta.PPI_CORTESIA == true)
                            {
                                throw new Exception("A proposta é uma cortesia.");
                            }
                            if(itemProposta.PST_ID == 5)
                            {
                                throw new Exception("A proposta está cancelado.");
                            }

                            if(itemProposta.PST_ID != 1)
                            {
                                throw new Exception("A proposta não está pendente. Se ela já foi paga, gere a nota a partir do pedido dessa proposta.");
                            }

                            var requisicao = new RequisicaoNovoLote();
                            requisicao.EmpresaID = itemProposta.PROPOSTA.EMP_ID;

                            var lstLotesPendentes = ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>().ListarItensDoLotePendentePorPropostaItem(ppiID);
                            var lstNota = ServiceFactory.RetornarServico<NotaFiscalSRV>().ListarNotasDeEntradaEnviadaProposta(ppiID);

                            if (lstNota != null && lstNota.Count > 0)
                            {
                                throw new Exception("Essa proposta já possuí uma nota associada.");
                            }

                            if (lstLotesPendentes != null && lstLotesPendentes.Count > 0)
                            {
                                throw new Exception("Essa proposta já possuí uma nota pendente de envio. Aguarde o final do processamento.");
                            }

                            requisicao.LstRequisicoes.Add(new RequisicaoNovoLoteItem()
                            {
                                CodProposta = ppiID,
                                NotaAntecipada = true
                            });

                            var lote = ServiceFactory.RetornarServico<NotaFiscalSRV>().CriarNovoLote(requisicao);
                            if (lote != null)
                            {
                                retorno.Add(lote);
                                //ServiceFactory.RetornarServico<RepresentanteSRV>().NotificarDeNotaAntecipada(repID, ppiID, itemProposta.PRT_ID);
                            }
                        }

                    }
                    scope.Complete();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Não é possível emitir uma nota antecipada para a proposta.", e);
            }
            return retorno;
        }


        public void CancelarOuDevolverNotasDaProposta(int? CodItem)
        {
            if (CodItem != null)
            {
                var _notaSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
                var propostaItm = FindByIdFullLoaded(CodItem);
                var possuiPedido = ServiceFactory.RetornarServico<PedidoCRMSRV>().PropostaPossuiPedido(propostaItm.PRT_ID);

                if (propostaItm.PST_ID != 8 && !possuiPedido)
                {
                    var lstNotas = _notaSRV.ListarNotasDeEntradaEnviadaProposta(CodItem);
                    _notaSRV.CancelarOuDevolverNota(lstNotas);
                }
            }
        }


        public BatchContext MarcarManualmenteVariasPropostasComoPaga(ICollection<int> lstPpiId, int? repId, string usuario)
        {
            BatchContext batchContext = new BatchContext();

            if(lstPpiId != null && lstPpiId.Count > 0)
            {
                int? prtCorrente = null;

                foreach (var prtId in lstPpiId) {

                    try
                    {
                        prtCorrente = prtId;
                        var lstPropostaItem = ListarPropostaItemPorProposta(prtId);

                        if(lstPropostaItem != null && lstPropostaItem.Count > 0)
                        {
                            foreach(var proItm in lstPropostaItem)
                            {
                                using (var scope = new TransactionScope())
                                {
                                    PagarProposta(proItm.PPI_ID, repId, usuario, null, true, true);
                                    scope.Complete();

                                    batchContext.TotalExito++;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception($"A proposta {prtCorrente} não possui nenhum item.");
                        }
                    }
                    catch(Exception e)
                    {
                        var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);

                        batchContext.TotalFalha++;
                        batchContext.ListErros.Add(new ErroReportItemDTO() {
                            Contexto = $"Id da proposta: {prtCorrente}",
                            Mensagem = $"Mensagem de erro : '{mensagem}'."
                        });
                    }
                }
            }
            return batchContext;
        }
        

        public bool InformarAceiteVendaAPrazo(int? ppiId, int? repId, string usuario)
        {
            var propostaItem = FindById(ppiId);
            var cliId = ServiceFactory.RetornarServico<PropostaSRV>().RetornarCliIdDaPropostaPorPropostaItem(ppiId);
            if (propostaItem.PST_ID == 1 ||
                propostaItem.PST_ID == 2)
            {
                if (
                    (propostaItem != null ||
                    propostaItem.PROPOSTA != null) &&
                    propostaItem.PROPOSTA.TNE_ID != 2)
                {
                    throw new Exception("Não é possível registrar o aceite do cliente em uma proposta que não é à prazo.");
                }

                AlterarStatusPropostaItem(ppiId, 4);
                HistoricoNotificacao.histPedido.RegistrarHistoricoAceiteVendaAPrazo(usuario, repId, ppiId, 4);

                AssinaturaSRV.ConcederAcessosDaProposta(ppiId, cliId, usuario);
                ServiceFactory.RetornarServico<RepresentanteSRV>().NotificarPropostaAprovadaCliente(ppiId, true);
                return true;
            }
            return false;
        }


        public ICollection<PropostaItemDTO> ConverterParaPropostaItem(ICollection<EmissaoPedidoItemDTO> lstEmissaoPedidoItem)
        {
            ICollection<PropostaItemDTO> lstPropostaItem = new List<PropostaItemDTO>();

            if (lstEmissaoPedidoItem != null)
            {
                var index = 0;
                foreach (var item in lstEmissaoPedidoItem)
                {
                    index++;
                    var validadeResult = ValidatorProxy.RecursiveValidate(item);
                    if (!validadeResult.IsValid)
                    {
                        throw new ValidacaoException("Erro de validação", validadeResult);
                    }

                    var propostaItem = new PropostaItemDTO()
                    {
                        PRODUTO_COMPOSICAO = item.PRODUTO_COMPOSICAO,
                        PPI_QTD = item.QTD,
                        PRT_IDENTIFICACAO_TURMA = item.PRT_IDENTIFICACAO_TURMA,
                        PPI_QTD_PARCELAS = item.QTD_PARCELAS,
                        PPI_VALOR_UNITARIO = item.VALOR_UNITARIO,
                        PPI_TOTAL = item.VALOR_TOTAL,
                        PPI_VALOR_PARCELA = item.VALOR_PARCELAS,
                        PPI_VALOR_ENTRADA = item.VALOR_ENTRADA,
                        PPI_ID = item.PPI_ID,
                        PPI_DATA_VENCIMENTO = item.DataVencimento,
                        PPI_DATA_VENCIMENTO_SEG_PARCELA = item.DataVencimentoSegparcela,
                        ASN_NUM_ASSINATURA = item.ASN_NUM_ASSINATURA,
                        PPI_ACESSOS_CONCEDIDOS = item.ACESSOS_CONCEDIDOS,
                        PPI_QTD_CONSULTA = item.QuantidadeConsulta,
                        PPI_CORTESIA = item.Cortesia,
                        PPI_DATA_PARA_FATURAMENTO= item.DATA_PARA_FATURAMENTO,
                        PPI_PERIODO_FAT = item.PERIODO_FAT,
                        PPI_SEMANA_FAT = item.SEMANA_FAT,
                        PPI_PERIODO_MES_BONUS = item.PERIODO_MES_BONUS,
                        PPI_ASN_NUM_ASS_CANC = item.CodigoAssinaturaCanc,
                        TPG_ID = (item.TIPO_PAGAMENTO != null) ? item.TIPO_PAGAMENTO.TPG_ID : null,
                        PPI_GERA_NOTA = item.GeraNotaFiscal,
                        PEDIDO_PARTICIPANTE = item.PEDIDO_PARTICIPANTE,
                        IFF_ID = item.IFF_ID,
                        IFF_ID_ENTRADA = item.IFF_ID_ENTRADA,
                        LOC_ID = item.LOC_ID,
                    };
                    
                    lstPropostaItem.Add(propostaItem);
                }
            }

            return lstPropostaItem;
        }
    }
}