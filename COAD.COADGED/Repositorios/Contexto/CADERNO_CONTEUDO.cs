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
    
    public partial class CADERNO_CONTEUDO
    {
        public int CON_ID { get; set; }
        public int CAD_ID { get; set; }
        public int CONT_OFFLINE { get; set; }
        public int PUB_ID { get; set; }
    
        public virtual CADERNO CADERNO { get; set; }
    }
}
