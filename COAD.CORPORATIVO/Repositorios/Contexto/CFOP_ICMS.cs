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
    
    public partial class CFOP_ICMS
    {
        public string CFOP { get; set; }
        public string UF_EMPRESA { get; set; }
        public string UF_NOTA { get; set; }
        public Nullable<decimal> ICMS_ENTRADA { get; set; }
        public Nullable<decimal> ICMS_SAIDA { get; set; }
    
        public virtual CFOP_TABLE CFOP_TABLE { get; set; }
        public virtual UF UF { get; set; }
        public virtual UF UF1 { get; set; }
    }
}
