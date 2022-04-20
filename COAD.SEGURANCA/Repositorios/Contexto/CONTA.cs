//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.SEGURANCA.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class CONTA
    {
        public int CTA_ID { get; set; }
        public int EMP_ID { get; set; }
        public string CTA_AGENCIA { get; set; }
        public string CTA_CONTA { get; set; }
        public string CTA_TIPO { get; set; }
        public string CTA_CONVENIO { get; set; }
        public string CTA_CODIGO_240 { get; set; }
        public string CTA_CODIGO_400 { get; set; }
        public string CTA_COMPL_CTA_COB { get; set; }
        public string CTA_CARTEIRA_REMESSA { get; set; }
        public string CTA_CARTEIRA_BOLETO { get; set; }
        public string CTA_CEDENTE_REMESSA { get; set; }
        public string CTA_CEDENTE_BOLETO { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERACAO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<decimal> CTA_PERC_MULTA { get; set; }
        public Nullable<decimal> CTA_PERC_MORA_MES { get; set; }
        public bool CTA_CEDENTE_EMITE_BOLETO { get; set; }
        public int CTA_NR_ARQ_ENVIADO { get; set; }
        public Nullable<int> CTA_ALOCAR_TITULO_DA_EMP_ID { get; set; }
        public Nullable<bool> CTA_ENVIA_BOLETO { get; set; }
        public string CTA_ESPECIE_DOC { get; set; }
        public string CTA_ESPECIE { get; set; }
        public string CTA_ACEITE { get; set; }
        public string CTA_INSTRUCOES_BOLETO { get; set; }
        public Nullable<bool> CTA_GERA_NOSSO_NUMERO { get; set; }
        public Nullable<int> EMP_ID_S_AVS { get; set; }
        public Nullable<long> CTA_ULTIMO_NOSSO_NUMERO { get; set; }
    
        public virtual BANCOS_REF BANCOS_REF { get; set; }
        public virtual EMPRESA EMPRESA { get; set; }
        public virtual EMPRESA EMPRESA1 { get; set; }
    }
}