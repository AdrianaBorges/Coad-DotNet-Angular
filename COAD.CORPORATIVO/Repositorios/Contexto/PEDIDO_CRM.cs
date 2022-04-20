//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.CORPORATIVO.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class PEDIDO_CRM
    {
        public PEDIDO_CRM()
        {
            this.CONTRATOS = new HashSet<CONTRATOS>();
            this.HISTORICO_ATENDIMENTO = new HashSet<HISTORICO_ATENDIMENTO>();
            this.ITEM_PEDIDO = new HashSet<ITEM_PEDIDO>();
            this.PEDIDO_PARTICIPANTE = new HashSet<PEDIDO_PARTICIPANTE>();
            this.PEDIDO_PAGAMENTO = new HashSet<PEDIDO_PAGAMENTO>();
        }
    
        public int PED_CRM_ID { get; set; }
        public Nullable<System.DateTime> PED_CRM_DATA { get; set; }
        public string PED_CRM_DESCRICAO { get; set; }
        public Nullable<int> PROSP_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public Nullable<decimal> PED_CRM_VALOR { get; set; }
        public Nullable<int> PST_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> CO_PG_ID { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<bool> PED_CRM_VENDA_INFORMADA { get; set; }
        public Nullable<bool> PED_EMPRESA_DO_SIMPLES { get; set; }
        public Nullable<bool> PED_CEM_POR_CENTO_FATURADO { get; set; }
        public string PED_CRM_COD_LEGADO { get; set; }
        public Nullable<int> PRT_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public Nullable<int> TPD_ID { get; set; }
        public Nullable<int> PED_CRM_QTD_CONSULTAS { get; set; }
        public Nullable<System.DateTime> PED_CRM_DATA_FATURAMENTO { get; set; }
        public string CAR_ID { get; set; }
        public Nullable<int> REP_ID_EMITENTE { get; set; }
        public string PED_CRM_EMAIL_CONTATO { get; set; }
        public string PED_CRM_EMAIL_NOTA_FISCAL { get; set; }
        public string PED_OBSERVACOES_NOTA_FISCAL { get; set; }
        public Nullable<int> TNE_ID { get; set; }
        public Nullable<int> ENV_ID { get; set; }
    
        public virtual ASSINATURA ASSINATURA { get; set; }
        public virtual CARTEIRA CARTEIRA { get; set; }
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual EMPRESA_REF EMPRESA_REF { get; set; }
        public virtual ENDERECO_VENDA ENDERECO_VENDA { get; set; }
        public virtual ICollection<HISTORICO_ATENDIMENTO> HISTORICO_ATENDIMENTO { get; set; }
        public virtual ICollection<ITEM_PEDIDO> ITEM_PEDIDO { get; set; }
        public virtual ICollection<PEDIDO_PARTICIPANTE> PEDIDO_PARTICIPANTE { get; set; }
        public virtual PEDIDO_STATUS PEDIDO_STATUS { get; set; }
        public virtual PRODUTO_COMPOSICAO PRODUTO_COMPOSICAO { get; set; }
        public virtual PROPOSTA PROPOSTA { get; set; }
        public virtual REGIAO REGIAO { get; set; }
        public virtual REPRESENTANTE REPRESENTANTE { get; set; }
        public virtual REPRESENTANTE REPRESENTANTE1 { get; set; }
        public virtual TIPO_PEDIDO_CRM TIPO_PEDIDO_CRM { get; set; }
        public virtual UEN UEN { get; set; }
        public virtual TIPO_NEGOCIACAO TIPO_NEGOCIACAO { get; set; }
        public virtual ICollection<PEDIDO_PAGAMENTO> PEDIDO_PAGAMENTO { get; set; }
    }
}
