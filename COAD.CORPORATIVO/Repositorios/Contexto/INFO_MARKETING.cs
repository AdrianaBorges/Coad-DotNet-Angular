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
    
    public partial class INFO_MARKETING
    {
        public INFO_MARKETING()
        {
            this.AREA_INFO_MARKETING = new HashSet<AREA_INFO_MARKETING>();
            this.PRODUTO_COMPOSICAO_INFO_MARKETING = new HashSet<PRODUTO_COMPOSICAO_INFO_MARKETING>();
        }
    
        public int MKT_CLI_ID { get; set; }
        public Nullable<int> O_CAD_ID { get; set; }
    
        public virtual ICollection<AREA_INFO_MARKETING> AREA_INFO_MARKETING { get; set; }
        public virtual CLIENTES CLIENTES { get; set; }
        public virtual ORIGEM_CADASTRO ORIGEM_CADASTRO { get; set; }
        public virtual ICollection<PRODUTO_COMPOSICAO_INFO_MARKETING> PRODUTO_COMPOSICAO_INFO_MARKETING { get; set; }
    }
}
