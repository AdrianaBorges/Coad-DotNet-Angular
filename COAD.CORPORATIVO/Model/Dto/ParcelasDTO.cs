using System;
using System.Collections.Generic;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(PARCELAS))]
    public partial class ParcelasDTO
    {
        public ParcelasDTO()
        {
            this.AGENDA_COBRANCA = new HashSet<AgendaCobrancaDTO>();
            this.CNAB_ARQUIVOS_ITEM = new HashSet<CnabArquivosItemDTO>();
            this.PARCELA_ALOCADA = new HashSet<ParcelaAlocadaDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
        }
    
        // ALT: 19/09/2016 - atributos para alocação
        public decimal vAlocacao { get; set; }
        public int qAlocacao { get; set; }

        // ALT: 16/09/2016 - atributos para resultado sintético
        public string banco { get; set; }

        public int qDisponiveis { get; set; }
        public decimal vDisponiveis { get; set; }

        public int qTransmitidos { get; set; }
        public decimal vTransmitidos { get; set; }

        public int qRejeitados { get; set; }
        public decimal vRejeitados { get; set; }

        public int qAlocados { get; set; }
        public decimal vAlocados { get; set; }

        public int qVencidosReceber { get; set; }
        public decimal vVencidosReceber { get; set; }

        public int qVincendosReceber { get; set; }
        public decimal vVincendosReceber { get; set; }

        public int qRecebidos { get; set; }
        public decimal vRecebidos { get; set; }

        public int qExpirando { get; set; }
        public decimal vExpirando { get; set; }

        public int qExpirados { get; set; }
        public decimal vExpirados { get; set; }

        // Analitico \\
        public string empresa { get; set; }
        public string CLI_NOME { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public Nullable<System.DateTime> CTR_DATA_INI_VIGENCIA { get; set; }
        public Nullable<System.DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }

        // Fim --------------------------------------------\\


        public string PAR_NUM_PARCELA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public int?  EMP_ID { get; set; }
        public System.DateTime PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> PAR_VLR_BOLETO { get; set; }
        public Nullable<System.DateTime> PAR_VENC_BOLETO { get; set; }
        public Nullable<int> CNQ_ID { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public Nullable<decimal> PAR_MORA_MES { get; set; }
        public string PAR_CART_ALOC { get; set; }
        public Nullable<System.DateTime> PAR_DATA_ALOC { get; set; }
        public Nullable<System.DateTime> PAR_DATA_REIMPRESSAO { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }
        public Nullable<System.DateTime> PAR_DATA_PAGTO { get; set; }
        public string PAR_NOSSO_NUMERO { get; set; }
        public string PAR_CEDENTE { get; set; }
        public string PAR_SITUACAO { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public string USU_LOGIN_PRORROGACAO { get; set; }
        public Nullable<System.DateTime> DATA_PRORROGACAO  { get; set; }
        public int PAR_DIAS_ATRASO { get; set; }
        public string PAR_TIPO_DOC { get; set; }

        public Nullable<decimal> PAR_VLR_JUROS { get; set; }
        public Nullable<decimal> PAR_MORA_DIA { get; set; }
        public Nullable<decimal> PAR_VLR_DESP_ADM { get; set; }
        public Nullable<System.DateTime> PAR_DATA_BAIXA { get; set; }

        public string PAR_CHAVE_TRANSACAO { get; set; }
        public string PAR_URL_BOLETO { get; set; }
        public string PAR_CODIGO_DE_BARRAS { get; set; }
        public string PAR_STATUS_TRANSACAO { get; set; }
        public Nullable<int> PAR_SEQ_PARCELA { get; set; }
        public Nullable<int> AGC_ID { get; set; }

        public Nullable<int> TPG_ID { get; set; }
        public Nullable<int> PAR_DIAS_VENCIMENTO { get; set; }

        public Nullable<int> CTA_ID { get; set; }
        public string PAR_REMESSA { get; set; }
        public Nullable<int> PGT_ID { get; set; }
        public string PAR_TRANSMITIDO { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<bool> PAR_PARCELA_DO_PEDIDO { get; set; }
        public string PAR_COD_LEGADO { get; set; }
        public string ORDER_KEY { get; set; }
        public string ORDER_KEY_REF { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
        public bool PAR_PODE_ALOCAR { get; set; }
        public Nullable<int> REM_ID { get; set; }
        public Nullable<int> PAR_TIPO_ALOC { get; set; }
        public Nullable<int> PPI_ID { get; set; }
        public Nullable<bool> PAR_ALOC_AUTOMATICA { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<bool> PAR_BAIXA_MANUAL { get; set; }
        public Nullable<bool> PAR_SEG_VIA { get; set; }
        public Nullable<bool> CTA_ENVIA_BOLETO { get; set; }
        public bool? PAR_BOLETO_AVULSO { get; set; }

        public Nullable<int> IFF_ID { get; set; }
        public Nullable<System.DateTime> PAR_DATA_AGEN_ENVIO { get; set; }
        public Nullable<System.DateTime> PAR_DATA_ENVIO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual BancosDTO BANCOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ContratoDTO CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PedidoPagamentoDTO PEDIDO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PARCELAS_REMESSA PARCELAS_REMESSA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelaAlocadaDTO> PARCELA_ALOCADA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PropostaItemDTO PROPOSTA_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabArquivosItemDTO> CNAB_ARQUIVOS_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AgendaCobrancaDTO> AGENDA_COBRANCA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual InfoFaturaDTO INFO_FATURA { get; set; }
    }
}
