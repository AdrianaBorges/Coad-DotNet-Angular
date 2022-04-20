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
    
    public partial class TIPO_PERIODO
    {
        public TIPO_PERIODO()
        {
            this.CONTRATOS = new HashSet<CONTRATOS>();
            this.ITEM_PEDIDO = new HashSet<ITEM_PEDIDO>();
            this.PRODUTO_COMPOSICAO_ITEM = new HashSet<PRODUTO_COMPOSICAO_ITEM>();
            this.PRODUTO_COMPOSICAO_TIPO_PERIODO = new HashSet<PRODUTO_COMPOSICAO_TIPO_PERIODO>();
            this.TABELA_PRECO = new HashSet<TABELA_PRECO>();
        }
    
        public int TTP_ID { get; set; }
        public string TTP_DESCRICAO { get; set; }
        public Nullable<int> TTP_QTDE_DIAS { get; set; }
        public Nullable<int> TTP_QTD_MESES { get; set; }
        public bool TTP_RECORRENTE { get; set; }
    
        public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual ICollection<ITEM_PEDIDO> ITEM_PEDIDO { get; set; }
        public virtual ICollection<PRODUTO_COMPOSICAO_ITEM> PRODUTO_COMPOSICAO_ITEM { get; set; }
        public virtual ICollection<PRODUTO_COMPOSICAO_TIPO_PERIODO> PRODUTO_COMPOSICAO_TIPO_PERIODO { get; set; }
        public virtual ICollection<TABELA_PRECO> TABELA_PRECO { get; set; }
    }
}