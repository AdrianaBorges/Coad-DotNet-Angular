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
    
    public partial class SEMANA_PREMIACAO_REPR
    {
        public int SPR_SEMANA { get; set; }
        public System.DateTime SPR_DATA_INI { get; set; }
        public System.DateTime SPR_DATA_FIM { get; set; }
        public int REP_ID { get; set; }
        public Nullable<decimal> SER_VLR_META { get; set; }
        public Nullable<decimal> SER_VLR_PREMIO { get; set; }
    
        public virtual REPRESENTANTE REPRESENTANTE { get; set; }
        public virtual SEMANA_PREMIACAO SEMANA_PREMIACAO { get; set; }
    }
}
