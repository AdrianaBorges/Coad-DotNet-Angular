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
    
    public partial class CLASSIFICACAO
    {
        public CLASSIFICACAO()
        {
            this.CLIENTES = new HashSet<CLIENTES>();
        }
    
        public int CLA_ID { get; set; }
        public string CLA_DESCRICAO { get; set; }
    
        public virtual ICollection<CLIENTES> CLIENTES { get; set; }
    }
}
