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
    
    public partial class FONTE_DADOS_DESCRICAO
    {
        public int FDD_ID { get; set; }
        public string FDD_DESCRICAO { get; set; }
        public string DFD_TOKEN { get; set; }
        public string DFD_PATH { get; set; }
        public Nullable<int> FDA_ID { get; set; }
    
        public virtual FONTE_DADOS_TEMPLATE FONTE_DADOS_TEMPLATE { get; set; }
    }
}
