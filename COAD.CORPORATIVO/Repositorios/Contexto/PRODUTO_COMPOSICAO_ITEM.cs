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
    
    public partial class PRODUTO_COMPOSICAO_ITEM
    {
        public PRODUTO_COMPOSICAO_ITEM()
        {
            this.PROPOSTA_ITEM = new HashSet<PROPOSTA_ITEM>();
        }
    
        public int CMP_ID { get; set; }
        public int PRO_ID { get; set; }
        public Nullable<int> CMI_QTDE { get; set; }
        public Nullable<decimal> CMI_PRECO_UNIT { get; set; }
        public string CMI_PRECO_TOTAL { get; set; }
        public Nullable<int> CMI_QTDE_PERIODO { get; set; }
        public Nullable<int> TTP_ID { get; set; }
        public bool CMI_GERA_ASSINATURA_LEGADO { get; set; }
    
        public virtual PRODUTO_COMPOSICAO PRODUTO_COMPOSICAO { get; set; }
        public virtual PRODUTOS PRODUTOS { get; set; }
        public virtual TIPO_PERIODO TIPO_PERIODO { get; set; }
        public virtual ICollection<PROPOSTA_ITEM> PROPOSTA_ITEM { get; set; }
    }
}
