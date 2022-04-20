using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.AcaoSalvamento;
using COAD.CORPORATIVO.Model.Dto.Custons.Listagens;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Model.Dto.Custons.Validacoes;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service.Custons;
using COAD.SEGURANCA.Service;
using COAD.UTIL.Grafico;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Models.Filtros;
using GenericCrud.Service;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PRT_ID", profileName = "default")]
    public class PropostaSRV : GenericService<PROPOSTA, PropostaDTO, int>
    {
        private PropostaDAO _dao; //= new PropostaDAO();
        public TipoPropostaSRV tipoPropostaSRV { get; set; }
        public PedidoStatusSRV PedidoStatus { get; set; }
        public PropostaItemSRV _propostaItemSRV { get; set; }
        public HistoricoNotificacaoSRV HistoricoNotificacao { get; set; }
        public RegistroLiberacaoSRV _registroLiberacaoSRV { get; set; }
        public RegistroLiberacaoItemSRV _registroLiberacaoItemSRV { get; set; }
        public ParcelasSRV ParcelaSRV { get; set; }
        public EmpresaSRV EmpresaSRV { get; set; }

        public PropostaSRV(PropostaDAO _dao) : base(_dao)
        {
            this._dao = _dao;
        }

        public PropostaSRV()
        {
            _dao = new PropostaDAO();
            tipoPropostaSRV = new TipoPropostaSRV();
            PedidoStatus = new PedidoStatusSRV();
            _propostaItemSRV = new PropostaItemSRV();
        }

        public PropostaDTO FindByIdFullLoaded(int? prtId, bool preencherItem = false, bool preencherPedidoParticipante = false, bool preencherPropostaItemComprovante = false, bool trazInfoFaturaImposto = false)
        {
            var proposta = FindById(prtId);

            if (proposta != null)
            {
                _propostaItemSRV.PreencherPropostaItem(proposta, preencherPedidoParticipante, preencherPropostaItemComprovante, trazInfoFaturaImposto);
            }
            return proposta;
        }

        public SalvarPropostaResultDTO SalvarProposta(PropostaDTO proposta, int? repId = null, string usuario = null, int? rgId = null, int? uenId = null)
        {
            SalvarPropostaResultDTO resultSalvamento = new SalvarPropostaResultDTO();
            
            if (proposta != null)
            {
                if (proposta.DATA_CADASTRO == null)
                    proposta.DATA_CADASTRO = DateTime.Now;

                if (proposta.PRT_ID == null || proposta.PRT_ID == 10)
                    proposta.PST_ID = 1;

                if (proposta.TIPO_PROPOSTA != null && proposta.TPP_ID == null)
                    proposta.TPP_ID = proposta.TIPO_PROPOSTA.TPP_ID;

                if (proposta.CLIENTES != null && proposta.CLI_ID == null)
                    proposta.CLI_ID = proposta.CLIENTES.CLI_ID;

                if (repId != null && proposta.REP_ID_EMITENTE == null)
                    proposta.REP_ID_EMITENTE = repId;

                if (rgId != null && proposta.RG_ID == null)
                    proposta.RG_ID = rgId;

                if (uenId != null && proposta.UEN_ID == null)
                    proposta.UEN_ID = uenId;


                if (!string.IsNullOrWhiteSpace(usuario) && string.IsNullOrWhiteSpace(proposta.USU_LOGIN))
                    proposta.USU_LOGIN = usuario;

                var clienteSRV = ServiceFactory
                    .RetornarServico<ClienteSRV>();
                proposta.CLIENTES = clienteSRV.FindByIdFullLoaded(proposta.CLI_ID, false, true, true);

                clienteSRV.ValidacaoTotalCliente(proposta.CLIENTES, "emitir/alterar proposta");

                // Checa se a proposta possui pelo menos 1 item
                if (proposta.PROPOSTA_ITEM == null || proposta.PROPOSTA_ITEM.Count() <= 0)
                {
                    var pedidoException = new PedidoException("A proposta deve haver pelo menos 1 produto");

                    var validationEx = new ValidacaoException("Não é possível gerar a proposta.");
                    validationEx.ModelState.AddModelError("PROPOSTA_ITEM", "A proposta deve haver pelo menos 1 produto");

                    throw validationEx;
                }
                
                var indexLoop = 0;
                foreach (var propostaItem in proposta.PROPOSTA_ITEM)
                {
                    _propostaItemSRV.ChecarValorParcelaETotal(propostaItem);
                    _propostaItemSRV.ChecarValoresDaProposta(propostaItem, indexLoop);
                    indexLoop++;
                }

                if (proposta.TPP_ID == 1 && proposta.UEN_ID == 2)
                {
                    resultSalvamento.ValidacaoInadimplencia = ChecarClienteDevedor(proposta);
                }

                if(proposta.UEN_ID == 2)
                    resultSalvamento.ValidacoesItens = ChecarRegrasDeCampanhaVenda(proposta);

                if (resultSalvamento.EhValido || proposta.Forcar == true)
                {
                    ChecarSePropostaFoiAlterada(proposta);

                    using (var scope = new TransactionScope())
                    {
                        resultSalvamento.PropostaSalva = SaveOrUpdate(proposta);
                        proposta.PRT_ID = resultSalvamento.PropostaSalva.PRT_ID;

                        _propostaItemSRV.SalvarPropostaItem(proposta);
                        GerarHistoricoNasPropostasItens(proposta);

                        if (!resultSalvamento.EhValido && proposta.Forcar == true)
                        {
                            var lstPropostaItem = _propostaItemSRV.ListarPropostaItemPorProposta(resultSalvamento.PropostaSalva.PRT_ID);
                            foreach(var proItm in lstPropostaItem)
                            {
                                bool inserirHistorico = true;
                                if(resultSalvamento.ValidacaoInadimplencia != null && resultSalvamento.ValidacaoInadimplencia.ExisteInadimplencia)
                                {
                                    inserirHistorico = false;
                                    RegistrarLiberacao(proItm, resultSalvamento.ValidacaoInadimplencia, repId, usuario);
                                }
                                _propostaItemSRV.RegistrarLiberacaoDosItens(proItm, resultSalvamento.ValidacoesItens, repId, usuario, inserirHistorico);
                            }
                        }
                        ServiceFactory.RetornarServico<CarteiraClienteSRV>().ChecarECriarCarteiraCliente(proposta.CLI_ID, proposta.CAR_ID, proposta.REP_ID_EMITENTE);

                        scope.Complete();
                    }

                }
            }
            return resultSalvamento;
        }
        public JsonGraficoResumo ListarPropostaPeriodo(int _mes, int _ano, int? _emp_id, int? _grupo)
        {
            return _dao.ListarPropostaPeriodo(_mes, _ano, _emp_id, _grupo);
        }
       
        public void GerarHistoricoNasPropostasItens(PropostaDTO proposta)
        {
            if (proposta != null && proposta.PROPOSTA_ITEM != null)
            {
                var lstPropostaItens = proposta.PROPOSTA_ITEM;
                var usuario = proposta.USU_LOGIN;
                var repId = proposta.REP_ID;
                var cliId = proposta.CLI_ID;

                foreach (var proItm in lstPropostaItens)
                {
                    var cmpId = proItm.CMP_ID;
                    var ppiId = proItm.PPI_ID;

                    this.HistoricoNotificacao.RegistrarHistoricoPropostaEmitida(usuario, repId, cliId, ppiId, cmpId, null, proItm.Nova);
                }
            }
        }

        public Pagina<PropostaDTO> ListarPropostas(
            PesquisaPropostaDTO pesquisaPropostaDTO)
        {
            var pagina = _dao.ListarPropostas(
                pesquisaPropostaDTO);

            if(pagina != null && 
                pagina.lista != null)
            {
                PreencherEmpresa(pagina.lista);
            }

            return pagina;
        }

        /// <summary>
        /// Retorna o Id do Cliente que pertence ao Item da Proposta
        /// </summary>
        /// <param name="ppiId"></param>
        /// <returns></returns>
        public int? RetornarCliIdDaPropostaPorPropostaItem(int? ppiId)
        {
            return _dao.RetornarCliIdDaPropostaPorPropostaItem(ppiId);
        }

        /// <summary>
        /// Pega uma proposta já paga e emiti um pedido a partir dela.
        /// </summary>
        /// <param name="prtId"></param>
        /// <returns></returns>
        public PedidoCRMDTO EmitirPedidoDaProposta(int? prtId, string usuario = null, int? repId = null)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var proposta = FindByIdFullLoaded(prtId, true, true, true, true);
                    var pedido = EmitirPedidoDaProposta(proposta, usuario, repId);

                    scope.Complete();

                    return pedido;
                }
            }
            catch (Exception e)
            {
                throw new PedidoException("Não é possível gerar o pedido a partir da proposta", e);
            }
        }

        /// <summary>
        /// Pega uma proposta já paga e emiti um pedido a partir dela. 
        /// </summary>
        /// <param name="proposta"></param>
        /// <returns></returns>
        public PedidoCRMDTO EmitirPedidoDaProposta(PropostaDTO proposta, string usuario = null, int? repId = null)
        {
            if (proposta == null)
            {
                throw new PedidoException("Não é possível gerar o pedido. A proposta não foi encontrada.");
            }

            if (proposta.PST_ID != 7 && proposta.PST_ID != 3 && proposta.PST_ID != 4)
            {
                throw new PedidoException("Não é possível gerar o pedido. A proposta não está paga.");
            }

            var cliSRV = ServiceFactory.RetornarServico<ClienteSRV>();
            var cliente = cliSRV.FindByIdFullLoaded(proposta.CLI_ID, false, false, false, true);

            cliSRV.ValidacaoTotalCliente(cliente, "Emitir o Pedido");
            EmissaoPedidoDTO emissaoPedido = new EmissaoPedidoDTO()
            {
                CLIENTE = cliente,
                REP_ID = proposta.REP_ID,
                REP_ID_EMITENTE = proposta.REP_ID_EMITENTE,
                RG_ID = proposta.RG_ID,
                PRT_ID = proposta.PRT_ID,
                UEN_ID = proposta.UEN_ID,
                ValidarCliente = false,
                Pago = true,
                ASN_NUM_ASSINATURA = proposta.ASN_NUM_ASSINATURA,
                TipoDePedido = (TipoDePedidoEnum)proposta.TPP_ID,
                EMP_ID = proposta.EMP_ID,
                CarId = proposta.CAR_ID,
                EmailContato = proposta.PRT_EMAIL_CONTATO,
                EmailNotaFiscal = proposta.PRT_EMAIL_NOTA_FISCAL,
                EmpresaDoSimples = proposta.PRT_EMPRESA_DO_SIMPLES,
                CemPorCentoFaturado = proposta.PRT_POR_CENTO_FATURADO,
                ObservacoesNotaFiscal = proposta.PRT_OBSERVACOES_NOTA_FISCAL,
                TneId = proposta.TNE_ID
            };

            foreach (var proItm in proposta.PROPOSTA_ITEM)
            {
                if (proItm.PRT_ID == 5)
                    continue;

                _propostaItemSRV.AlterarStatusPropostaItem(proItm.PPI_ID, 8);

                emissaoPedido.EMISSAO_PEDIDO_ITEM.Add(new EmissaoPedidoItemDTO()
                {

                    PRODUTO_COMPOSICAO = proItm.PRODUTO_COMPOSICAO,
                    QTD = proItm.PPI_QTD,
                    PRT_IDENTIFICACAO_TURMA = proItm.PRT_IDENTIFICACAO_TURMA,
                    QTD_PARCELAS = proItm.PPI_QTD_PARCELAS,
                    RG_ID = proposta.RG_ID,
                    TIPO_PAGAMENTO = proItm.TIPO_PAGAMENTO,
                    TTP_ID = 6,
                    VALOR_PARCELAS = proItm.PPI_VALOR_PARCELA,
                    VALOR_TOTAL = proItm.PPI_TOTAL,
                    VALOR_UNITARIO = proItm.PPI_VALOR_UNITARIO,
                    VALOR_ENTRADA = proItm.PPI_VALOR_ENTRADA,
                    PPI_ID = proItm.PPI_ID,
                    DataVencimento = proItm.PPI_DATA_VENCIMENTO,
                    DataVencimentoSegparcela = proItm.PPI_DATA_VENCIMENTO_SEG_PARCELA,
                    ASN_NUM_ASSINATURA = proItm.ASN_NUM_ASSINATURA,
                    ACESSOS_CONCEDIDOS = proItm.PPI_ACESSOS_CONCEDIDOS,
                    QuantidadeConsulta = proItm.PPI_QTD_CONSULTA,
                    DATA_PARA_FATURAMENTO = proposta.PRT_DATA_FATURAMENTO_AGENDADA,
                    Cortesia = proItm.PPI_CORTESIA,
                    PERIODO_FAT = proItm.PPI_PERIODO_FAT,
                    SEMANA_FAT = proItm.PPI_SEMANA_FAT,
                    PERIODO_MES_BONUS = proItm.PPI_PERIODO_MES_BONUS,
                    CodigoAssinaturaCanc = proItm.PPI_ASN_NUM_ASS_CANC,
                    GeraNotaFiscal = proItm.PPI_GERA_NOTA,
                    PEDIDO_PARTICIPANTE = proItm.PEDIDO_PARTICIPANTE,
                    IFF_ID = proItm.IFF_ID,
                    IFF_ID_ENTRADA = proItm.IFF_ID_ENTRADA,
                    LOC_ID = proItm.LOC_ID
                });
            }

            var pedidoSRV = ServiceFactory.RetornarServico<PedidoCRMSRV>();
            var itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();
            var pedido =  pedidoSRV.EmitirPedidoOnline(emissaoPedido);

            if (pedido != null) // Processo pós salvamento de pedido
            {
                foreach (var itemPedido in pedido.ITEM_PEDIDO)
                {
                    if (pedido.TNE_ID == null || pedido.TNE_ID == 1)
                        itemPedidoSRV.MudarItemPedidoDaPropostaParaPago(itemPedido, usuario);
                    else if (pedido.TNE_ID == 2)
                        itemPedidoSRV.MudarItemPedidoDaPropostaAprovadoCliente(itemPedido, usuario);

                    ServiceFactory.RetornarServico<PropostaItemComprovanteSRV>()
                        .AssociarComprovantesDaPropostaNoItemPedido(itemPedido.PPI_ID, itemPedido.IPE_ID);                    
                    ServiceFactory.RetornarServico<NotaFiscalLoteItemSRV>()
                        .AssociarLoteProPedido(itemPedido.PPI_ID, itemPedido.IPE_ID);


                    HistoricoNotificacao.RegistrarHistoricoPedidoEmitidoAPartirDaProposta
                        (usuario,
                        repId,
                        pedido.CLI_ID,
                        itemPedido.IPE_ID,
                        itemPedido.PPI_ID);
                }
            }

            return pedido;
        }

        public IList<AutoCompleteDTO> ListarAssinaturaDaPropostaAutoComplete(string assinatura)
        {
            return _dao.ListarAssinaturaDaPropostaAutoComplete(assinatura);
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorStatus()
        {
            return _dao.ObterGrupoFiltroPorStatus();
        }

        public IQueryable<GrupoDeFiltroDTO> ObterGrupoFiltroPorTipoProposta()
        {
            return _dao.ObterGrupoFiltroPorTipoProposta();
        }
        public GruposDeFiltrosDTO ObterGruposDeFiltroDoPedido()
        {
            GruposDeFiltrosDTO grupos = new GruposDeFiltrosDTO();
            var grupoStatus = ObterGrupoFiltroPorStatus();
            var grupoTipoProposta = ObterGrupoFiltroPorTipoProposta();
            
            grupos.AdicionarGrupoDeFiltros("status", grupoStatus);
            grupos.AdicionarGrupoDeFiltros("tipoProposta", grupoTipoProposta);
            return grupos;
        }       

        public ValidacaoClienteInadimplenteDTO ChecarClienteDevedor(PropostaDTO proposta)
        {
            //SysUtils.EnibirEmProducao();
            if (proposta != null && proposta.CLI_ID != null && proposta.TPP_ID == 1)
            {
                var tipoProposta = proposta.TPP_ID;

                var validacaoInadimplente = ServiceFactory.RetornarServico<ClienteSRV>().ExecutarValidacaoDeInadimplencia(proposta.CLI_ID, tipoProposta, null, proposta.PRT_ID);
                return validacaoInadimplente;
            }

            return null;
        }       

        public ICollection<ValidacaoRegraPropostaItemDTO> ChecarRegrasDeCampanhaVenda(PropostaDTO proposta)
        {
            //SysUtils.EnibirEmProducao();
            ICollection<ValidacaoRegraPropostaItemDTO> validacoes = new HashSet<ValidacaoRegraPropostaItemDTO>();
            if (proposta != null)
            {
                var lstPropostaItem = proposta.PROPOSTA_ITEM;

                foreach(var proItm in lstPropostaItem)
                {
                    var result = _propostaItemSRV.RealizarValidacoesDeRegras(proItm, proposta);
                    validacoes.Add(result);
                }
            }

            return validacoes;
        }

        public void RegistrarLiberacao(PropostaItemDTO propostaItem, ValidacaoClienteInadimplenteDTO validacao, int? REP_ID, string usuario)
        {
            if (validacao != null && validacao.ExisteInadimplencia)
            {
                var representanteQueExecutouAAcao = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);

                string nomeRepresentanteQueExecutouAAcao =
                                    (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                        ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

                string mensagem = "O representante {{representante}} emitiu um item de proposta '{{PPI_ID}}'. Porém ouve pendência para liberação da mesma. ";
                mensagem += string.Format("Detalhes: {0}. ", validacao);

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
                _registroLiberacaoItemSRV.CriarRegistroLiberacaoItem(propostaItem, mensagem, REP_ID, usuario, true);
            }
        }

        public int? RetornarCodPedidoDaProposta(int? prtId)
        {
            return _dao.RetornarCodPedidoDaProposta(prtId);
        }

        private void ChecarSePropostaFoiAlterada(PropostaDTO proposta)
        {
            if (proposta != null && proposta.PRT_ID != null)
            {
                var objetoDoBanco = FindById(proposta.PRT_ID);
                
                if (proposta != null &&
                    objetoDoBanco != null &&
                    proposta.EMP_ID != objetoDoBanco.EMP_ID)
                {
                    foreach(var itm in proposta.PROPOSTA_ITEM)
                    {
                        itm.Alterada = true;
                    }
                }

            }
        }

        public Pagina<ListagemPropostaDTO> PesquisarPropostaPendConf(string queryStr, DateTime? dataInicial = null, DateTime? dataFinal = null, RequisicaoPaginacao requisicao = null)
        {
            return _dao.PesquisarPropostaPendConf(queryStr, dataInicial, dataFinal, requisicao);
        }


        public void PreencherEmpresa(IEnumerable<PropostaDTO> lstProposta)
        {
            if (lstProposta != null)
            {
                foreach (var pro in lstProposta)
                {
                    if (pro.EMP_ID != null)
                    {
                        pro.EMPRESAS = EmpresaSRV.FindById(pro.EMP_ID);
                    }
                }
            }
        }
        public void PreencherEmpresa(PropostaDTO proposta)
        {
            if (proposta != null && proposta.EMP_ID != null)
            {
                proposta.EMPRESAS = EmpresaSRV.FindById(proposta.EMP_ID);
            }
        }

        /// <summary>
        /// Emite pedidos on-line. Pode ser usado como método de integração com outros sistemas.
        /// </summary>
        /// <param name="emissaoPedido"></param>
        public PropostaDTO EmitirPropostaOnline(EmissaoPedidoDTO emissaoPedido)
        {
            try
            {
                var proposta = ConverterProposta(emissaoPedido);
                var result = SalvarProposta(proposta);

                if (result.EhValido)
                {
                    var propostaSalva = result.PropostaSalva;
                    var propostaResultado = FindByIdFullLoaded(proposta.PRT_ID, true, true, true);
                    return propostaResultado;
                }
                return null;

            }
            catch (Exception e)
            {
                throw new PedidoException("Não é possível emitir o pedido", e);
            }

        }


        public PropostaDTO ConverterProposta(EmissaoPedidoDTO emissaoDTO)
        {
            if (emissaoDTO != null)
            {
                var result = ValidatorProxy.RecursiveValidate<EmissaoPedidoDTO>(emissaoDTO);

                if (result.IsValid)
                {
                    //if (emissaoDTO.CLIENTE != null && emissaoDTO.CLIENTE.CLI_ID != null)
                    if (emissaoDTO.CLIENTE == null)
                    {
                        throw new NullReferenceException("O objeto do cliente possui uma referência nula.");
                    }

                    PropostaDTO pedido = new PropostaDTO()
                    {
                        CLI_ID = emissaoDTO.CLIENTE.CLI_ID,
                        CLIENTES = emissaoDTO.CLIENTE,
                        DATA_CADASTRO = DateTime.Now,
                        RG_ID = emissaoDTO.RG_ID,
                        UEN_ID = emissaoDTO.UEN_ID,
                        REP_ID = emissaoDTO.REP_ID,
                        REP_ID_EMITENTE = emissaoDTO.REP_ID_EMITENTE,
                        PRT_ID = emissaoDTO.PRT_ID,
                        EMP_ID = emissaoDTO.EMP_ID,
                        PRT_OBSERVACOES = emissaoDTO.Observacoes,
                        ASN_NUM_ASSINATURA = emissaoDTO.ASN_NUM_ASSINATURA,
                        TPP_ID = (emissaoDTO.TipoDePedido != null) ? (int)emissaoDTO.TipoDePedido : 1,
                        CAR_ID = emissaoDTO.CarId,
                        PRT_EMAIL_CONTATO = emissaoDTO.EmailContato,
                        PRT_EMAIL_NOTA_FISCAL = emissaoDTO.EmailNotaFiscal,
                        PRT_POR_CENTO_FATURADO = emissaoDTO.CemPorCentoFaturado,
                        PRT_OBSERVACOES_NOTA_FISCAL = emissaoDTO.ObservacoesNotaFiscal,
                        PRT_EMPRESA_DO_SIMPLES = emissaoDTO.EmpresaDoSimples,
                        TNE_ID = emissaoDTO.TneId
                    };

                    var lstPropostaItem = _propostaItemSRV.ConverterParaPropostaItem(emissaoDTO.EMISSAO_PEDIDO_ITEM);
                    pedido.PROPOSTA_ITEM = lstPropostaItem;

                    return pedido;
                }
                else
                {
                    throw new ValidacaoException("Foram encotrados problemas ao validar as informações do pedido.", result);
                }
            }

            throw new NullReferenceException("Não é possível gerar o pedido. Objeto do pedido possui refencia nulla.");
        }
    }
}