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
    
    public partial class IMPORTACAO_STATUS
    {
        public IMPORTACAO_STATUS()
        {
            this.IMPORTACAO = new HashSet<IMPORTACAO>();
        }
    
        public int IMS_ID { get; set; }
        public string IMS_DESCRICAO { get; set; }
    
        public virtual ICollection<IMPORTACAO> IMPORTACAO { get; set; }
    }
}