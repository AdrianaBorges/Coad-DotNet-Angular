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
    
    public partial class CONFIG_SPED_FISCAL
    {
        public int EMP_ID { get; set; }
        public string REG1010_IND_EXP { get; set; }
        public string REG1010_IND_CCRF { get; set; }
        public string REG1010_IND_COMB { get; set; }
        public string REG1010_IND_USINA { get; set; }
        public string REG1010_IND_VA { get; set; }
        public string REG1010_IND_EE { get; set; }
        public string REG1010_IND_CART { get; set; }
        public string REG1010_IND_FORM { get; set; }
        public string REG1010_IND_AER { get; set; }
        public Nullable<int> CRE_DEB_ICMS { get; set; }
    
        public virtual EMPRESA_REF EMPRESA_REF { get; set; }
    }
}
