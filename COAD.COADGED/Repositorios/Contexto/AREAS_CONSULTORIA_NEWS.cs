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
    
    public partial class AREAS_CONSULTORIA_NEWS
    {
        public int NEWS_ID { get; set; }
        public int ARE_CONS_ID { get; set; }
        public string UF_ID { get; set; }
    
        public virtual AREAS_CONSULTORIA AREAS_CONSULTORIA { get; set; }
        public virtual AREAS_CONSULTORIA1 AREAS_CONSULTORIA1 { get; set; }
        public virtual NEWS NEWS { get; set; }
        public virtual UF_REF UF_REF { get; set; }
    }
}
