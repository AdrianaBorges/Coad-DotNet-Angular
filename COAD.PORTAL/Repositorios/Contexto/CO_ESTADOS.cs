//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.PORTAL.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class CO_ESTADOS
    {
        public CO_ESTADOS()
        {
            this.CO_MUNICIPIOS = new HashSet<CO_MUNICIPIOS>();
            this.CO_OBRIGACOES = new HashSet<CO_OBRIGACOES>();
        }
    
        public long NUM_UF { get; set; }
        public string COD_UF { get; set; }
        public string NOME_UF { get; set; }
        public Nullable<short> HABILITADO { get; set; }
    
        public virtual ICollection<CO_MUNICIPIOS> CO_MUNICIPIOS { get; set; }
        public virtual ICollection<CO_OBRIGACOES> CO_OBRIGACOES { get; set; }
    }
}
