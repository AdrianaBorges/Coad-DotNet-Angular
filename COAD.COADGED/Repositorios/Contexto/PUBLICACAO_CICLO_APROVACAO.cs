//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.COADGED.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class PUBLICACAO_CICLO_APROVACAO
    {
        public int FLU_ID { get; set; }
        public int FLU_ETAPA_ID { get; set; }
        public int CIC_ID { get; set; }
        public Nullable<int> COL_ID { get; set; }
        public Nullable<System.DateTime> CIC_DATA { get; set; }
        public string CIC_OBS { get; set; }
        public Nullable<bool> CIC_STATUS { get; set; }
    
        public virtual COLABORADOR COLABORADOR { get; set; }
        public virtual PUBLICACAO_FLUXO_ETAPA PUBLICACAO_FLUXO_ETAPA { get; set; }
    }
}
