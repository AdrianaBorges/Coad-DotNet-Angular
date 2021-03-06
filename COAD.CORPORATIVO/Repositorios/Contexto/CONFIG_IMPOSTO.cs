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
    
    public partial class CONFIG_IMPOSTO
    {
        public CONFIG_IMPOSTO()
        {
            this.CONFIG_IMPOSTO_IMPOSTO = new HashSet<CONFIG_IMPOSTO_IMPOSTO>();
            this.CONFIG_IMPOSTO_REGIAO = new HashSet<CONFIG_IMPOSTO_REGIAO>();
        }
    
        public int CFI_ID { get; set; }
        public Nullable<decimal> CFI_VLR_MINIMO { get; set; }
        public Nullable<decimal> CFI_VLR_MAXIMO { get; set; }
        public Nullable<bool> CFI_QUALQUER_VALOR { get; set; }
        public Nullable<int> TIPO_CLI_ID { get; set; }
        public Nullable<bool> CFI_EMPRESA_DO_SIMPLES { get; set; }
        public Nullable<decimal> CFI_VLR_DESCONTO_MIM { get; set; }
        public string CFI_DESC_REGRA { get; set; }
        public Nullable<int> NFC_ID { get; set; }
        public Nullable<bool> CFI_CLIENTE_RETEM { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public string CFI_CODIGO_TRIBUTACAO_MUNICIPIO { get; set; }
    
        public virtual NOTA_FISCAL_CONFIG NOTA_FISCAL_CONFIG { get; set; }
        public virtual ICollection<CONFIG_IMPOSTO_IMPOSTO> CONFIG_IMPOSTO_IMPOSTO { get; set; }
        public virtual ICollection<CONFIG_IMPOSTO_REGIAO> CONFIG_IMPOSTO_REGIAO { get; set; }
        public virtual TIPO_CLIENTE TIPO_CLIENTE { get; set; }
    }
}
