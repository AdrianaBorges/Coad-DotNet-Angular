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
    
    public partial class RELATORIO_TABELA_COLUNAS
    {
        public int COR_ID { get; set; }
        public string COR_DESCRICAO { get; set; }
        public Nullable<short> COR_ORDEM { get; set; }
        public Nullable<bool> COR_ORDENACAO { get; set; }
        public Nullable<bool> COR_ORDEM_ASC { get; set; }
        public Nullable<int> REL_ID { get; set; }
        public Nullable<int> RET_ID { get; set; }
        public string COR_ALIAS { get; set; }
        public string COR_TYPE_NAME { get; set; }
        public Nullable<bool> COR_IS_NULLABLE { get; set; }
        public Nullable<int> REL_ID_GRUPO { get; set; }
        public Nullable<bool> COR_AGRUPAR { get; set; }
        public Nullable<short> COR_AGRUPAMENTO_ORDEM { get; set; }
    
        public virtual RELATORIO_PERSONALIZADO RELATORIO_PERSONALIZADO { get; set; }
        public virtual RELATORIO_PERSONALIZADO RELATORIO_PERSONALIZADO1 { get; set; }
        public virtual RELATORIO_TABELAS RELATORIO_TABELAS { get; set; }
    }
}
