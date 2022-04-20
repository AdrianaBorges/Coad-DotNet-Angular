

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Linq;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace COAD.CORPORATIVO.Model.Dto
{    
	[Mapping(typeof(NOTA_FISCAL_LOTE_ITEM))]
	public class NotaFiscalLoteItemDTO : INFeLoteItem
    {
        public NotaFiscalLoteItemDTO()
        {
            NOTA_FISCAL_REFERENCIADA = new HashSet<NotaFiscalReferenciadaDTO>();
            this.NOTA_FISCAL_LOTE_ITEM_MSG = new HashSet<NotaFiscalLoteItemMsgDTO>();
        }

        // Normal Properties
        public Int32? NLI_ID { get; set; }
		public Nullable<Int32> NFL_ID { get; set; }
		public Nullable<Int32> NLS_ID { get; set; }
		public Nullable<Int32> IPE_ID { get; set; } 
		public Nullable<DateTime> NLI_DATA_FATURAMENTO { get; set; }
		public Nullable<DateTime> NLI_DATA_EMISAO { get; set; }
		public Nullable<Int32> NLI_NUMERO_NOTA { get; set; }
		public String NLI_CHAVE_NOTA { get; set; }
        public Nullable<int> NLI_COD_RETORNO { get; set; }
        public string NLI_MENSAGEM_RETORNO { get; set; }
        public string NLI_PATH_ARQUIVO_NFE_XML { get; set; }
        public string NLI_MSG_ERRO_SISTEMA { get; set; }

        [ScriptIgnore]
        public byte[] NFL_BINARIO_NFE_XML { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string NLI_NUMERO_PROTOCOLO { get; set; }
        public Nullable<System.DateTime> NLI_DATA_AUTORIZACAO_REJEICAO { get; set; }
        public Nullable<int> NF_ID { get; set; }
        public Nullable<int> NIT_ID { get; set; }
        public Nullable<int> NLI_ID_PAI { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> FOR_ID { get; set; }
        public string NLI_CARTA_CORRECAO { get; set; }
        public Nullable<int> PPI_ID { get; set; }
        public Nullable<bool> NLI_NOTA_ANTECIPADA { get; set; }
        public string NLI_SERIE { get; set; }
        public Nullable<int> NLI_NUMERO_RPS { get; set; }
        public Nullable<int> NFC_ID { get; set; }

        // Object Properties

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
		public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }
		
		[IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalLoteDTO NOTA_FISCAL_LOTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalDTO NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
		public virtual NotaFiscalLoteStatusDTO NOTA_FISCAL_LOTE_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ContratoDTO CONTRATOS { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NotaFiscalLoteItemTipoDTO NOTA_FISCAL_LOTE_ITEM_TIPO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalReferenciadaDTO> NOTA_FISCAL_REFERENCIADA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual FornecedorDTO FORNECEDOR { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PropostaItemDTO PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemMsgDTO> NOTA_FISCAL_LOTE_ITEM_MSG { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]

        public virtual NotaFiscalConfigDTO NOTA_FISCAL_CONFIG { get; set; }

        public int? LoteID { get => NFL_ID; set => NFL_ID = value; }
        public int? ItemLoteID { get => NLI_ID; set => NLI_ID = value; }
        public StatusLoteItemEnum Status {
            get
            {
                switch (NLS_ID)
                {
                    case 1: return StatusLoteItemEnum.ENVIO_PENDENTE;
                    case 4: return StatusLoteItemEnum.AGUARDANDO_RETORNO;
                    case 5: return StatusLoteItemEnum.REJEITADA;
                    case 6: return StatusLoteItemEnum.AUTORIZADA;
                    case 7: return StatusLoteItemEnum.CANCELADA;
                    case 9: return StatusLoteItemEnum.LOTE_EM_PROCESSAMENTO;
                    case 10: return StatusLoteItemEnum.REJEITADA_E_INUTILIZADA;
                    case 11: return StatusLoteItemEnum.AUTORIZADA_E_ENVIADA;
                    default: return StatusLoteItemEnum.ENVIO_PENDENTE;
                }
            }
            set
            {
                switch (value)
                {
                    case StatusLoteItemEnum.ENVIO_PENDENTE: NLS_ID = 1; break;
                    case StatusLoteItemEnum.AGUARDANDO_RETORNO: NLS_ID = 4; break;
                    case StatusLoteItemEnum.REJEITADA: NLS_ID = 5; break;
                    case StatusLoteItemEnum.AUTORIZADA: NLS_ID = 6; break;
                    case StatusLoteItemEnum.CANCELADA: NLS_ID = 7; break;
                    case StatusLoteItemEnum.LOTE_EM_PROCESSAMENTO: NLS_ID = 9; break;
                    case StatusLoteItemEnum.REJEITADA_E_INUTILIZADA: NLS_ID = 10; break;
                    case StatusLoteItemEnum.AUTORIZADA_E_ENVIADA: NLS_ID = 11; break;
                }
            }
        }
        public int? CodPedido { get => IPE_ID; set => IPE_ID = value; }
        public DateTime? DataFaturamento { get => NLI_DATA_FATURAMENTO; set => NLI_DATA_FATURAMENTO = value; }
        public DateTime? DataEmissao { get => NLI_DATA_EMISAO; set => NLI_DATA_EMISAO = value; }
        public int? NumeroNota { get => NLI_NUMERO_NOTA; set => NLI_NUMERO_NOTA = value; }
        public string ChaveNota { get => NLI_CHAVE_NOTA; set => NLI_CHAVE_NOTA = value; }
        public int? CodRetorno { get => NLI_COD_RETORNO; set => NLI_COD_RETORNO = value; }
        public string MensagemRetorno { get => NLI_MENSAGEM_RETORNO; set => NLI_MENSAGEM_RETORNO = value; }
        public string PathArquivoNFeXml { get => NLI_PATH_ARQUIVO_NFE_XML; set => NLI_PATH_ARQUIVO_NFE_XML = value; }

        [ScriptIgnore]
        public byte[] BinarioNFeXml { get => NFL_BINARIO_NFE_XML; set => NFL_BINARIO_NFE_XML = value; }
        public string CodContrato { get => CTR_NUM_CONTRATO; set => CTR_NUM_CONTRATO = value; }
        public string MsgErroSistema { get => NLI_MSG_ERRO_SISTEMA; set => NLI_MSG_ERRO_SISTEMA = value; }
        public INFeLote Lote { get => NOTA_FISCAL_LOTE; set => NOTA_FISCAL_LOTE = (NotaFiscalLoteDTO) value; }
        public string NumeroProtocolo { get => NLI_NUMERO_PROTOCOLO; set => NLI_NUMERO_PROTOCOLO = value; }
        public DateTime? DataAutorizacaoRejeicao { get => NLI_DATA_AUTORIZACAO_REJEICAO; set => NLI_DATA_AUTORIZACAO_REJEICAO = value; }
        public int? CodNotaFiscal { get => NF_ID; set => NF_ID = value; }
        public ICollection<INotaFiscalReferenciada> NotaFiscalReferenciados
        {
            get
            {
                if(NOTA_FISCAL_REFERENCIADA != null)
                {
                    var collect = new ObservableCollection<INotaFiscalReferenciada>(NOTA_FISCAL_REFERENCIADA);
                    //collect.CollectionChanged += NotaRefChanged;
                    return collect;
                }
                return new ObservableCollection<INotaFiscalReferenciada>();

            }
            set
            {
                if (value != null) {

                    NOTA_FISCAL_REFERENCIADA = value.Cast<NotaFiscalReferenciadaDTO>().ToList();
                }
                else
                {
                    NOTA_FISCAL_REFERENCIADA = null;
                }
                
            }
        }
        public TipoLoteItemEnum Tipo
        {
            get
            {
                switch (NIT_ID)
                {
                    case 1: return TipoLoteItemEnum.ENVIO;
                    case 2: return TipoLoteItemEnum.CANCELAMENTO;
                    case 3: return TipoLoteItemEnum.DEVOLUCAO;
                    case 4: return TipoLoteItemEnum.CARTA_CORRECAO;
                    default: return TipoLoteItemEnum.ENVIO;
                }
            }
            set
            {
                switch (value)
                {
                    case TipoLoteItemEnum.ENVIO: NIT_ID = 1; break;
                    case TipoLoteItemEnum.CANCELAMENTO: NIT_ID = 2; break;
                    case TipoLoteItemEnum.DEVOLUCAO: NIT_ID = 3; break;
                    case TipoLoteItemEnum.CARTA_CORRECAO: NIT_ID = 4; break;
                }
            }
        }

        public int? ClienteID { get => CLI_ID; set => CLI_ID = value; }
        public int? EmpresaID { get => EMP_ID; set => EMP_ID = value; }
        public int? FornecedorID { get => FOR_ID; set => FOR_ID = value; }
        public string CartaCorrecao { get => NLI_CARTA_CORRECAO; set => NLI_CARTA_CORRECAO = value; }
        public int? CodProposta { get => PPI_ID; set => PPI_ID = value; }
        public bool? NotaAntecipada { get => NLI_NOTA_ANTECIPADA; set => NLI_NOTA_ANTECIPADA = value; }
        public string Serie { get => NLI_SERIE; set => NLI_SERIE = value; }
        
        public ICollection<INotaFiscalItemMSG> NotaFiscalLoteItemMSG
        {
            get
            {
                if (NOTA_FISCAL_LOTE_ITEM_MSG != null)
                {
                    var collect = new ObservableCollection<INotaFiscalItemMSG>(NOTA_FISCAL_LOTE_ITEM_MSG);
                    //collect.CollectionChanged += NotaRefChanged;
                    return collect;
                }
                return new ObservableCollection<INotaFiscalItemMSG>();

            }
            set
            {
                if (value != null)
                {
                    NOTA_FISCAL_LOTE_ITEM_MSG = value.Cast<NotaFiscalLoteItemMsgDTO>().ToList();
                }
                else
                {
                    NOTA_FISCAL_LOTE_ITEM_MSG = null;
                }

            }
        }

        public int? NumeroRps { get => NLI_NUMERO_RPS; set => NLI_NUMERO_RPS = value; }
        public int? NfConfigID { get => NFC_ID; set => NFC_ID = value; }
    }
}
