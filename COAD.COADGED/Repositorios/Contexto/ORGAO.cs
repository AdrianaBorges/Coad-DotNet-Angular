//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COAD.COADGED.Repositorios.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class ORGAO
    {
        public ORGAO()
        {
            this.MANUAL_DP_ITEM = new HashSet<MANUAL_DP_ITEM>();
            this.PUBLICACAO = new HashSet<PUBLICACAO>();
        }
    
        public int ORG_ID { get; set; }
        public string ORG_DESCRICAO { get; set; }
        public Nullable<int> ORG_ATIVO { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
    
        public virtual ICollection<MANUAL_DP_ITEM> MANUAL_DP_ITEM { get; set; }
        public virtual ICollection<PUBLICACAO> PUBLICACAO { get; set; }
    }
}
