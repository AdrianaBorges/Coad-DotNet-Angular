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
    
    public partial class FONTE_DADOS_TEMPLATE
    {
        public FONTE_DADOS_TEMPLATE()
        {
            this.FONTE_DADOS_DESCRICAO = new HashSet<FONTE_DADOS_DESCRICAO>();
            this.TEMPLATE_HTML = new HashSet<TEMPLATE_HTML>();
        }
    
        public int FDA_ID { get; set; }
        public string FDA_DESCRICAO { get; set; }
        public Nullable<int> TPL_ID { get; set; }
    
        public virtual ICollection<FONTE_DADOS_DESCRICAO> FONTE_DADOS_DESCRICAO { get; set; }
        public virtual ICollection<TEMPLATE_HTML> TEMPLATE_HTML { get; set; }
    }
}