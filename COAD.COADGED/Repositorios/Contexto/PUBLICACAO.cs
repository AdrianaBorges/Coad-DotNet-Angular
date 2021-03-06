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
    
    public partial class PUBLICACAO
    {
        public PUBLICACAO()
        {
            this.LOG_ACESSO_PORTAL = new HashSet<LOG_ACESSO_PORTAL>();
            this.NOTICIA = new HashSet<NOTICIA>();
            this.PUBLICACAO_ALTERACAO_REVOGACAO = new HashSet<PUBLICACAO_ALTERACAO_REVOGACAO>();
            this.PUBLICACAO_EDITADA = new HashSet<PUBLICACAO_EDITADA>();
            this.PUBLICACAO_REVISAO_COLABORADOR = new HashSet<PUBLICACAO_REVISAO_COLABORADOR>();
            this.PUBLICACAO_REVISAO = new HashSet<PUBLICACAO_REVISAO>();
        }
    
        public int PUB_ID { get; set; }
        public Nullable<int> TIP_MAT_ID { get; set; }
        public Nullable<int> TIP_ATO_ID { get; set; }
        public string PUB_NUMERO_ATO { get; set; }
        public Nullable<int> TVI_ID { get; set; }
        public Nullable<int> ORG_ID { get; set; }
        public string PUB_CONTEUDO_RESENHA { get; set; }
        public string PUB_CONTEUDO { get; set; }
        public Nullable<int> SEC_ID { get; set; }
        public Nullable<int> LBL_ID { get; set; }
        public string PUB_OBS { get; set; }
        public Nullable<System.DateTime> PUB_DATA_ATO { get; set; }
        public Nullable<System.DateTime> PUB_DATA_PUB_ATO { get; set; }
        public string PUB_DESCRICAO_NEWS { get; set; }
        public string PUB_COMPL_VEICULO { get; set; }
        public Nullable<int> PUB_ATIVO { get; set; }
        public Nullable<int> FLU_ID { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public string CABECALHO { get; set; }
        public string PUB_CONTEUDO_RVT { get; set; }
        public string PUB_CONTEUDO_RESENHA_DGT { get; set; }
        public string PUB_CONTEUDO_RESENHA_RVO { get; set; }
        public string PUB_CONTEUDO_RESENHA_RVT { get; set; }
        public string PUB_CONTEUDO_RESENHA_RDC { get; set; }
        public string PUB_CONTEUDO_RDC { get; set; }
        public string PUB_CONTEUDO_RVO { get; set; }
    
        public virtual LABELS LABELS { get; set; }
        public virtual ICollection<LOG_ACESSO_PORTAL> LOG_ACESSO_PORTAL { get; set; }
        public virtual ICollection<NOTICIA> NOTICIA { get; set; }
        public virtual ORGAO ORGAO { get; set; }
        public virtual PLUBLICACAO_FLUXO PLUBLICACAO_FLUXO { get; set; }
        public virtual ICollection<PUBLICACAO_ALTERACAO_REVOGACAO> PUBLICACAO_ALTERACAO_REVOGACAO { get; set; }
        public virtual ICollection<PUBLICACAO_EDITADA> PUBLICACAO_EDITADA { get; set; }
        public virtual ICollection<PUBLICACAO_REVISAO_COLABORADOR> PUBLICACAO_REVISAO_COLABORADOR { get; set; }
        public virtual ICollection<PUBLICACAO_REVISAO> PUBLICACAO_REVISAO { get; set; }
        public virtual SECOES SECOES { get; set; }
        public virtual TIPO_ATO TIPO_ATO { get; set; }
        public virtual TIPO_MATERIA TIPO_MATERIA { get; set; }
        public virtual VEICULO VEICULO { get; set; }
    }
}
