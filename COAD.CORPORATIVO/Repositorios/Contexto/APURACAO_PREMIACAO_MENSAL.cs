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
    
    public partial class APURACAO_PREMIACAO_MENSAL
    {
        public int REP_ID { get; set; }
        public int APU_MES { get; set; }
        public int APU_ANO { get; set; }
        public Nullable<decimal> APU_VLR_RENOVACAO { get; set; }
        public Nullable<decimal> APU_VLR_VENDA_NOVA { get; set; }
        public Nullable<decimal> APU_VLR_PRODUTOS { get; set; }
        public Nullable<decimal> APU_VLR_AVISTA { get; set; }
        public Nullable<decimal> APU_VLR_4PARCELAS { get; set; }
        public Nullable<decimal> APU_PERC_4PARCELAS { get; set; }
        public Nullable<decimal> APU_VLR_TOTAL { get; set; }
    
        public virtual REPRESENTANTE REPRESENTANTE { get; set; }
    }
}
