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
    
    public partial class PUBLICACAO_AREAS_CONSULTORIA
    {
        public PUBLICACAO_AREAS_CONSULTORIA()
        {
            this.PUBLICACAO_BUSCA = new HashSet<PUBLICACAO_BUSCA>();
            this.PUBLICACAO_CONFIG = new HashSet<PUBLICACAO_CONFIG>();
            this.PUBLICACAO_PALAVRA_CHAVE = new HashSet<PUBLICACAO_PALAVRA_CHAVE>();
            this.PUBLICACAO_REMISSAO = new HashSet<PUBLICACAO_REMISSAO>();
            this.PUBLICACAO_REMISSIVO = new HashSet<PUBLICACAO_REMISSIVO>();
            this.PUBLICACAO_REVISAO_COLABORADOR = new HashSet<PUBLICACAO_REVISAO_COLABORADOR>();
            this.PUBLICACAO_REVISAO = new HashSet<PUBLICACAO_REVISAO>();
            this.PUBLICACAO_TITULACAO = new HashSet<PUBLICACAO_TITULACAO>();
            this.PUBLICACAO_UF = new HashSet<PUBLICACAO_UF>();
        }
    
        public int PUB_ID { get; set; }
        public int ARE_CONS_ID { get; set; }
        public Nullable<int> PUB_PAGINA_SUMARIO { get; set; }
        public Nullable<int> PUB_PAGINA_INDICE { get; set; }
        public Nullable<int> CAP_ID { get; set; }
        public string PUB_MANCHETE { get; set; }
        public string PUB_EMENTA { get; set; }
        public string PUB_EXPRESSAO { get; set; }
        public string PUB_MANCHETE_PORTAL { get; set; }
        public string PUB_EMENTA_PORTAL { get; set; }
    
        public virtual AREAS_CONSULTORIA1 AREAS_CONSULTORIA1 { get; set; }
        public virtual CAPITAL CAPITAL { get; set; }
        public virtual ICollection<PUBLICACAO_BUSCA> PUBLICACAO_BUSCA { get; set; }
        public virtual ICollection<PUBLICACAO_CONFIG> PUBLICACAO_CONFIG { get; set; }
        public virtual ICollection<PUBLICACAO_PALAVRA_CHAVE> PUBLICACAO_PALAVRA_CHAVE { get; set; }
        public virtual ICollection<PUBLICACAO_REMISSAO> PUBLICACAO_REMISSAO { get; set; }
        public virtual ICollection<PUBLICACAO_REMISSIVO> PUBLICACAO_REMISSIVO { get; set; }
        public virtual ICollection<PUBLICACAO_REVISAO_COLABORADOR> PUBLICACAO_REVISAO_COLABORADOR { get; set; }
        public virtual ICollection<PUBLICACAO_REVISAO> PUBLICACAO_REVISAO { get; set; }
        public virtual ICollection<PUBLICACAO_TITULACAO> PUBLICACAO_TITULACAO { get; set; }
        public virtual ICollection<PUBLICACAO_UF> PUBLICACAO_UF { get; set; }
    }
}
