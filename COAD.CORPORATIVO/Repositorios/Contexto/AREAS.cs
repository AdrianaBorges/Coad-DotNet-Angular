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
    
    public partial class AREAS
    {
        public AREAS()
        {
            this.AREA_INFO_MARKETING = new HashSet<AREA_INFO_MARKETING>();
            this.CONTRATOS = new HashSet<CONTRATOS>();
            this.IMPORTACAO_SUSPECT = new HashSet<IMPORTACAO_SUSPECT>();
            this.PRODUTOS = new HashSet<PRODUTOS>();
        }
    
        public int AREA_ID { get; set; }
        public string AREA_NOME { get; set; }
    
        public virtual ICollection<AREA_INFO_MARKETING> AREA_INFO_MARKETING { get; set; }
        public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual ICollection<IMPORTACAO_SUSPECT> IMPORTACAO_SUSPECT { get; set; }
        public virtual ICollection<PRODUTOS> PRODUTOS { get; set; }
    }
}