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
    
    public partial class TIPO_FORNECEDOR
    {
        public TIPO_FORNECEDOR()
        {
            this.FORNECEDOR = new HashSet<FORNECEDOR>();
        }
    
        public int TIPO_FOR_ID { get; set; }
        public string TIPO_FOR_DESCRICAO { get; set; }
    
        public virtual ICollection<FORNECEDOR> FORNECEDOR { get; set; }
    }
}