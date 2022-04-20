using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
namespace COAD.COADGED.Model.DTO
{
    [Mapping(typeof(PUBLICACAO_AREAS_CONSULTORIA))]
    public class PublicacaoAreaConsultoriaDTO
    {
        public PublicacaoAreaConsultoriaDTO()
        {
            this.PUBLICACAO_CONFIG = new HashSet<PublicacaoConfigDTO>();
            this.PUBLICACAO_PALAVRA_CHAVE = new HashSet<PublicacaoPalavraChaveDTO>();
            this.PUBLICACAO_UF = new HashSet<PublicacaoUfDTO>();
            this.PUBLICACAO_REMISSAO = new HashSet<PublicacaoRemissaoDTO>();
            this.PUBLICACAO_REMISSIVO = new HashSet<PublicacaoRemissivoDTO>();
            this.PUBLICACAO_TITULACAO = new HashSet<PublicacaoTitulacaoDTO>();
            //this.PUBLICACAO_BUSCA = new HashSet<PublicacaoBuscaDTO>();
            this.PUBLICACAO_REVISAO_COLABORADOR = new HashSet<PublicacaoRevisaoColaboradorDTO>();
            this.PUBLICACAO_REVISAO = new HashSet<PublicacaoRevisaoDTO>();
        }

        // para o Painel Estatístico-----------------------------\\
        // qtd. matérias...
        public Nullable<int> RDC { get; set; }
        public Nullable<int> RVT { get; set; }
        public Nullable<int> DGT { get; set; }
        public Nullable<int> RVO { get; set; }
        public Nullable<int> DIA { get; set; }

        // matérias reprovadas...
        public Nullable<int> RP_RVT { get; set; }
        public Nullable<int> RP_DGT { get; set; }
        public Nullable<int> RP_RVO { get; set; }
        public Nullable<int> RP_COL { get; set; }

        // editadas fora da redação...
        public Nullable<int> ED_RVT { get; set; }
        public Nullable<int> ED_DGT { get; set; }
        public Nullable<int> ED_RVO { get; set; }
        //-------------------------------------------------------\\

        // define a operação ao salvar...
        public Boolean lIncluir { get; set; }

        // (L)iberada (A)provada (R)eprovada *** Editada pela área: (0)Redator (T)ecnica (D)igitação (O)rtográfica
        public string revisao { get; set; }

        // TRUE informa que ao salvar deverá publicar informações da matéria...
        public Boolean lPublicar { get; set; }

        // ID da matéria no Portal...
        public Nullable<int> PUB_ID_PORTAL { get; set; }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe um Colecionador!")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Manchete do Impresso")]
        public string PUB_MANCHETE { get; set; }

        [DisplayName("Ementa do Impresso")]
        public string PUB_EMENTA { get; set; }

        [DisplayName("Publicar no Portal")]
        public string PUBLICAR_PORTAL { get; set; }

        [DisplayName("Matéria Impressa")]
        public string MATERIA_IMPRESSA { get; set; }

        [DisplayName("Página do sumário")]
        public Nullable<int> PUB_PAGINA_SUMARIO { get; set; }

        [DisplayName("Página do índice")]
        public Nullable<int> PUB_PAGINA_INDICE { get; set; }

        [DisplayName("Municipio")]
        public Nullable<int> CAP_ID { get; set; }

        [DisplayName("4ª Expressão")]
        public string PUB_EXPRESSAO { get; set; }

        [DisplayName("Manchete do Portal")]
        public string PUB_MANCHETE_PORTAL { get; set; }

        [DisplayName("Ementa do Portal")]
        public string PUB_EMENTA_PORTAL { get; set; }

        public virtual AreasDTO AREAS_CONSULTORIA { get; set; }
        public virtual AreasDTO AREAS_CONSULTORIA1 { get; set; }
        public virtual PublicacaoDTO PUBLICACAO { get; set; }
        public virtual CapitalDTO CAPITAL { get; set; }

        [DisplayName("Publicações")]
        public virtual ICollection<PublicacaoConfigDTO> PUBLICACAO_CONFIG { get; set; }

        [DisplayName("Palavra chave [será usada futuramente para buscar esta publicação]")]
        public virtual ICollection<PublicacaoPalavraChaveDTO> PUBLICACAO_PALAVRA_CHAVE { get; set; }

        [DisplayName("Estados")]
        public virtual ICollection<PublicacaoUfDTO> PUBLICACAO_UF { get; set; }

        public virtual ICollection<PublicacaoRemissaoDTO> PUBLICACAO_REMISSAO { get; set; }
        public virtual ICollection<PublicacaoRemissivoDTO> PUBLICACAO_REMISSIVO { get; set; }
        public virtual ICollection<PublicacaoTitulacaoDTO> PUBLICACAO_TITULACAO { get; set; }
        //public virtual ICollection<PublicacaoBuscaDTO> PUBLICACAO_BUSCA { get; set; }
        public virtual ICollection<PublicacaoRevisaoColaboradorDTO> PUBLICACAO_REVISAO_COLABORADOR { get; set; }
        public virtual ICollection<PublicacaoRevisaoDTO> PUBLICACAO_REVISAO { get; set; }
    }
}
