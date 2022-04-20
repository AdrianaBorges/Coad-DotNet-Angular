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
    
    public partial class NOTA_FISCAL_LOTE_ITEM
    {
        public NOTA_FISCAL_LOTE_ITEM()
        {
            this.NOTA_FISCAL_LOTE_ITEM_MSG = new HashSet<NOTA_FISCAL_LOTE_ITEM_MSG>();
            this.NOTA_FISCAL_REFERENCIADA = new HashSet<NOTA_FISCAL_REFERENCIADA>();
        }
    
        public int NLI_ID { get; set; }
        public Nullable<int> NFL_ID { get; set; }
        public Nullable<int> NLS_ID { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<System.DateTime> NLI_DATA_FATURAMENTO { get; set; }
        public Nullable<System.DateTime> NLI_DATA_EMISAO { get; set; }
        public Nullable<int> NLI_NUMERO_NOTA { get; set; }
        public string NLI_CHAVE_NOTA { get; set; }
        public Nullable<int> NLI_COD_RETORNO { get; set; }
        public string NLI_MENSAGEM_RETORNO { get; set; }
        public string NLI_PATH_ARQUIVO_NFE_XML { get; set; }
        public byte[] NFL_BINARIO_NFE_XML { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string NLI_MSG_ERRO_SISTEMA { get; set; }
        public string NLI_NUMERO_PROTOCOLO { get; set; }
        public Nullable<System.DateTime> NLI_DATA_AUTORIZACAO_REJEICAO { get; set; }
        public Nullable<int> NF_ID { get; set; }
        public Nullable<int> NIT_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<int> FOR_ID { get; set; }
        public string NLI_CARTA_CORRECAO { get; set; }
        public Nullable<int> PPI_ID { get; set; }
        public Nullable<bool> NLI_NOTA_ANTECIPADA { get; set; }
        public string NLI_SERIE { get; set; }
        public Nullable<int> NLI_NUMERO_RPS { get; set; }
        public Nullable<int> NFC_ID { get; set; }
    
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual CONTRATOS CONTRATOS { get; set; }
        public virtual FORNECEDOR FORNECEDOR { get; set; }
        public virtual ITEM_PEDIDO ITEM_PEDIDO { get; set; }
        public virtual NOTA_FISCAL NOTA_FISCAL { get; set; }
        public virtual NOTA_FISCAL_CONFIG NOTA_FISCAL_CONFIG { get; set; }
        public virtual NOTA_FISCAL_LOTE NOTA_FISCAL_LOTE { get; set; }
        public virtual ICollection<NOTA_FISCAL_LOTE_ITEM_MSG> NOTA_FISCAL_LOTE_ITEM_MSG { get; set; }
        public virtual NOTA_FISCAL_LOTE_STATUS NOTA_FISCAL_LOTE_STATUS { get; set; }
        public virtual NOTA_FISCAL_LOTE_ITEM_TIPO NOTA_FISCAL_LOTE_ITEM_TIPO { get; set; }
        public virtual PROPOSTA_ITEM PROPOSTA_ITEM { get; set; }
        public virtual ICollection<NOTA_FISCAL_REFERENCIADA> NOTA_FISCAL_REFERENCIADA { get; set; }
    }
}
