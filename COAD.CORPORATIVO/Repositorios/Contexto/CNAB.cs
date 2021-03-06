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
    
    public partial class CNAB
    {
        public int CNB_ID { get; set; }
        public string CNB_CNAB { get; set; }
        public string CNB_ARQUIVO { get; set; }
        public string CNB_REGISTRO { get; set; }
        public string CNB_CAMPO { get; set; }
        public string CNB_TIPO { get; set; }
        public int CNB_INICIO { get; set; }
        public int CNB_FINAL { get; set; }
        public int CNB_TAMANHO { get; set; }
        public string CNB_CONTEUDO { get; set; }
        public System.DateTime DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERACAO { get; set; }
        public int EMP_ID { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> CNB_DECIMAL { get; set; }
        public bool EMP_GRP_COAD { get; set; }
        public Nullable<int> CNC_ID { get; set; }
        public Nullable<int> CCA_ID { get; set; }
    
        public virtual BANCOS BANCOS { get; set; }
        public virtual EMPRESA_REF EMPRESA_REF { get; set; }
        public virtual CNAB_CONFIG CNAB_CONFIG { get; set; }
        public virtual CNAB_CONFIG_ARQUIVO CNAB_CONFIG_ARQUIVO { get; set; }
        public virtual CNAB_TIPO_DADOS CNAB_TIPO_DADOS { get; set; }
        public virtual CNAB_TIPO_REGISTRO CNAB_TIPO_REGISTRO { get; set; }
    }
}
