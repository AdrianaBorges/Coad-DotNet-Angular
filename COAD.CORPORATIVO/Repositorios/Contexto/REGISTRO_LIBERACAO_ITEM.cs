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
    
    public partial class REGISTRO_LIBERACAO_ITEM
    {
        public REGISTRO_LIBERACAO_ITEM()
        {
            this.PROPOSTA_ITEM = new HashSet<PROPOSTA_ITEM>();
        }
    
        public int RIT_ID { get; set; }
        public Nullable<int> RLI_ID { get; set; }
        public Nullable<System.DateTime> RIT_DATA_ACAO { get; set; }
        public Nullable<bool> RIT_LIBERADO { get; set; }
        public string RIT_DESCRICAO { get; set; }
        public Nullable<System.DateTime> RIT_DATA_CRIACAO { get; set; }
    
        public virtual ICollection<PROPOSTA_ITEM> PROPOSTA_ITEM { get; set; }
        public virtual REGISTRO_LIBERACAO REGISTRO_LIBERACAO { get; set; }
    }
}
