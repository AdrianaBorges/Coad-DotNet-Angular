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
    
    public partial class PRODUTO_COMPOSICAO_TIPO_PERIODO
    {
        public int CMP_ID { get; set; }
        public int TTP_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }
    
        public virtual PRODUTO_COMPOSICAO PRODUTO_COMPOSICAO { get; set; }
        public virtual TIPO_PERIODO TIPO_PERIODO { get; set; }
    }
}