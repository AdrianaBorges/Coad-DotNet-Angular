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
    
    public partial class FUNCIONALIDADE_SISTEMA
    {
        public int FSI_ID { get; set; }
        public string FSI_DESCRICAO { get; set; }
        public Nullable<bool> FSI_USA_TEMPLATE { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> TPL_ID { get; set; }
    
        public virtual TEMPLATE_HTML TEMPLATE_HTML { get; set; }
    }
}
