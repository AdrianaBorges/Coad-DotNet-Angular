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
    
    public partial class UNIDADE_MEDIDA
    {
        public UNIDADE_MEDIDA()
        {
            this.NOTA_FISCAL_ITEM = new HashSet<NOTA_FISCAL_ITEM>();
            this.PRODUTOS = new HashSet<PRODUTOS>();
            this.PRODUTOS1 = new HashSet<PRODUTOS>();
        }
    
        public string UND_ID { get; set; }
        public string UND_DESCRICAO { get; set; }
    
        public virtual FATOR_CONVERSAO FATOR_CONVERSAO { get; set; }
        public virtual ICollection<NOTA_FISCAL_ITEM> NOTA_FISCAL_ITEM { get; set; }
        public virtual ICollection<PRODUTOS> PRODUTOS { get; set; }
        public virtual ICollection<PRODUTOS> PRODUTOS1 { get; set; }
    }
}
