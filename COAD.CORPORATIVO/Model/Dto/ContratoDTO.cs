using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CONTRATOS))]
    public class ContratoDTO : IPrototype<ContratoDTO>
    {
        public ContratoDTO()
        {
            this.PARCELAS = new HashSet<ParcelasDTO>();
            this.REGISTRO_FATURAMENTO = new HashSet<RegistroFaturamentoDTO>();
            this.ASSINATURA_TRANSFERENCIA = new HashSet<AssinaturaTransferenciaDTO>();
            this.ASSINATURA_TRANSFERENCIA1 = new HashSet<AssinaturaTransferenciaDTO>();
            this.NOTA_FISCAL_LOTE_ITEM = new HashSet<NotaFiscalLoteItemDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            
        }
  
        public string CTR_NUM_CONTRATO { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_ANO_VIGENCIA { get; set; }
        public string PED_NUM_PEDIDO { get; set; }
        public string CTR_ANO_FAT { get; set; }
        public string CTR_PERIODO_FAT { get; set; }
        public string CTR_SEMANA_FAT { get; set; }
        public string CTR_ANO_PROD { get; set; }
        public Nullable<decimal> CTR_VLR_CONTRATO { get; set; }
        public Nullable<decimal> CTR_VLR_ENTRADA { get; set; }
        public Nullable<decimal> CTR_VLR_PARCELAS { get; set; }
        public Nullable<int> CTR_QTE_PARCELAS { get; set; }
        public Nullable<System.DateTime> CTR_DATA_CANC { get; set; }
        public Nullable<System.DateTime> CTR_DATA_REATIVACAO { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public int AREA_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<System.DateTime> CTR_DATA_INI_VIGENCIA { get; set; }
        public Nullable<System.DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }
        public string REGIAO_UF { get; set; }
        public Nullable<int> CTR_PRORROGADO { get; set; }
        public Nullable<int> CTR_CORTESIA { get; set; }
        public Nullable<System.DateTime> CTR_DATA_INI_PROMOCIONAL { get; set; }
        public Nullable<System.DateTime> CTR_DATA_FAT { get; set; }
        public Nullable<int> PED_CRM_ID { get; set; }
        public string SITUACAO { get; set; }
        public bool CTR_VENDA_RECORRENTE { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public Nullable<short> CTR_DIA_VENCIMENTO_VENDA_RECORRENTE { get; set; }
        public string CTR_COD_ULTIMA_PARCELA_GERADA { get; set; }
        public bool CTR_PAGAMENTO_GATEWAY { get; set; }
        public Nullable<System.DateTime> CTR_DATA_FATURAMENTO_EFETIVO { get; set; }
        public string REP_OPER_ID { get; set; }
        public string CAR_ID { get; set; }
        public Nullable<int> CTR_PERIODO_MES_BONUS { get; set; }
        public Nullable<int> TTP_ID { get; set; }
        public Nullable<bool> CTR_GERA_NOTA_FISCAL { get; set; }
        public Nullable<int> CTR_NUMERO_NOTA { get; set; }
        public Nullable<int> NFX_ID { get; set; }
        public Nullable<decimal> CTR_VLR_SERVICO { get; set; }
        public Nullable<decimal> CTR_VLR_PRODUTO { get; set; }
        public Nullable<decimal> CTR_VLR_BRUTO { get; set; }

        public virtual AreasCorpDTO AREAS { get; set; }
        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
        public EmpresaModel EMPRESA { get; set; }
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        public virtual UFDTO UF { get; set; }
        public virtual string HISTORICO_CANCELAMENTO { get; set; }

        public Nullable<bool> CTR_SERVICO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PedidoCRMDTO PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroFaturamentoDTO> REGISTRO_FATURAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPeriodoDTO TIPO_PERIODO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual NfeXmlDTO NFE_XML { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AssinaturaTransferenciaDTO> ASSINATURA_TRANSFERENCIA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AssinaturaTransferenciaDTO> ASSINATURA_TRANSFERENCIA1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalLoteItemDTO> NOTA_FISCAL_LOTE_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

        public ContratoDTO Clone()
        {
            ContratoDTO contratoNovo = new ContratoDTO()
            {
                AREA_ID = this.AREA_ID,
                ASN_NUM_ASSINATURA = this.ASN_NUM_ASSINATURA,
                CAR_ID = this.CAR_ID,
                CMP_ID = this.CMP_ID,
                CTR_ANO_FAT = this.CTR_ANO_FAT,
                CTR_ANO_PROD = this.CTR_ANO_PROD,
                CTR_ANO_VIGENCIA = this.CTR_ANO_VIGENCIA,
                CTR_COD_ULTIMA_PARCELA_GERADA = this.CTR_COD_ULTIMA_PARCELA_GERADA,
                CTR_CORTESIA = this.CTR_CORTESIA,
                CTR_DATA_CANC = this.CTR_DATA_CANC,
                CTR_DATA_FAT = this.CTR_DATA_FAT,
                CTR_DATA_FATURAMENTO_EFETIVO = this.CTR_DATA_FATURAMENTO_EFETIVO,
                CTR_DATA_FIM_VIGENCIA = this.CTR_DATA_FIM_VIGENCIA,
                CTR_DATA_INI_PROMOCIONAL = this.CTR_DATA_INI_PROMOCIONAL,
                CTR_DATA_INI_VIGENCIA = this.CTR_DATA_INI_VIGENCIA,
                CTR_DATA_REATIVACAO = this.CTR_DATA_REATIVACAO,
                CTR_DIA_VENCIMENTO_VENDA_RECORRENTE = this.CTR_DIA_VENCIMENTO_VENDA_RECORRENTE,
                CTR_GERA_NOTA_FISCAL = this.CTR_GERA_NOTA_FISCAL,
                CTR_NUMERO_NOTA = this.CTR_NUMERO_NOTA,
                CTR_NUM_CONTRATO = this.CTR_NUM_CONTRATO,
                CTR_PAGAMENTO_GATEWAY = this.CTR_PAGAMENTO_GATEWAY,
                CTR_PERIODO_FAT = this.CTR_PERIODO_FAT,
                CTR_PERIODO_MES_BONUS = this.CTR_PERIODO_MES_BONUS,
                CTR_PRORROGADO = this.CTR_PRORROGADO,
                CTR_QTE_PARCELAS = this.CTR_QTE_PARCELAS,
                CTR_SEMANA_FAT = this.CTR_SEMANA_FAT,
                CTR_VENDA_RECORRENTE = this.CTR_VENDA_RECORRENTE,
                CTR_VLR_CONTRATO = this.CTR_VLR_CONTRATO,
                CTR_VLR_ENTRADA = this.CTR_VLR_ENTRADA,
                CTR_VLR_PARCELAS = this.CTR_VLR_PARCELAS,
                EMP_ID = this.EMP_ID,
                IPE_ID = this.IPE_ID,
                NFX_ID = this.NFX_ID,
                PED_CRM_ID = this.PED_CRM_ID,
                PED_NUM_PEDIDO = this.PED_NUM_PEDIDO,
                CTR_SERVICO = this.CTR_SERVICO,
                REGIAO_UF = this.REGIAO_UF,
                REP_ID = this.REP_ID,
                REP_OPER_ID = this.REP_OPER_ID,
                RG_ID = this.RG_ID,
                SITUACAO = this.SITUACAO,
                TTP_ID = this.TTP_ID,
            };

            return contratoNovo;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
