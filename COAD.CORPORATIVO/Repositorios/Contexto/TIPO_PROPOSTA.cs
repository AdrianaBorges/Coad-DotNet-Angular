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
    
    public partial class TIPO_PROPOSTA
    {
        public TIPO_PROPOSTA()
        {
            this.CAMPANHA_VENDA_TIPO_PROPOSTA = new HashSet<CAMPANHA_VENDA_TIPO_PROPOSTA>();
            this.PROPOSTA = new HashSet<PROPOSTA>();
        }
    
        public int TPP_ID { get; set; }
        public string TPP_DESCRICAO { get; set; }
    
        public virtual ICollection<CAMPANHA_VENDA_TIPO_PROPOSTA> CAMPANHA_VENDA_TIPO_PROPOSTA { get; set; }
        public virtual ICollection<PROPOSTA> PROPOSTA { get; set; }
    }
}