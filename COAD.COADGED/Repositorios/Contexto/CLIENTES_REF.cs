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
    
    public partial class CLIENTES_REF
    {
        public CLIENTES_REF()
        {
            this.CADERNO = new HashSet<CADERNO>();
            this.CADERNO_COMPARTILHADO = new HashSet<CADERNO_COMPARTILHADO>();
        }
    
        public int CLI_ID { get; set; }
    
        public virtual ICollection<CADERNO> CADERNO { get; set; }
        public virtual ICollection<CADERNO_COMPARTILHADO> CADERNO_COMPARTILHADO { get; set; }
    }
}
