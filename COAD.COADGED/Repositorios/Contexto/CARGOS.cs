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
    
    public partial class CARGOS
    {
        public CARGOS()
        {
            this.COLABORADOR = new HashSet<COLABORADOR>();
            this.PUBLICACAO_FLUXO_ETAPA = new HashSet<PUBLICACAO_FLUXO_ETAPA>();
        }
    
        public int CRG_ID { get; set; }
        public string CRG_DESCRICAO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public string USU_LOGIN { get; set; }
        public string CRG_SIGLA { get; set; }
    
        public virtual ICollection<COLABORADOR> COLABORADOR { get; set; }
        public virtual ICollection<PUBLICACAO_FLUXO_ETAPA> PUBLICACAO_FLUXO_ETAPA { get; set; }
    }
}