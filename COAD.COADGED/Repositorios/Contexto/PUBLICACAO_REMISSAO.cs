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
    
    public partial class PUBLICACAO_REMISSAO
    {
        public int PRE_ID { get; set; }
        public int PUB_ID { get; set; }
        public int ARE_CONS_ID { get; set; }
        public int PRE_NUMERO { get; set; }
        public string PRE_REMISSAO { get; set; }
    
        public virtual PUBLICACAO_AREAS_CONSULTORIA PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
