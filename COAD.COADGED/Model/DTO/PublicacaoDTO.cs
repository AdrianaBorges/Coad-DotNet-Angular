using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COAD.COADGED.Model.DTO
{
    public class PublicacaoDTO
    {
        public PublicacaoDTO()
        {
            this.NOTICIA = new HashSet<NoticiaDTO>();
            this.PUBLICACAO_ALTERACAO_REVOGACAO = new HashSet<PublicacaoAlteracaoRevogacaoDTO>();
            this.PUBLICACAO_AREAS_CONSULTORIA = new HashSet<PublicacaoAreaConsultoriaDTO>();
            this.PUBLICACAO_ALTERACAO_REVOGACAO = new HashSet<PublicacaoAlteracaoRevogacaoDTO>();
            //this.PUBLICACAO_REVISAO_COLABORADOR = new HashSet<PUBLICACAO_REVISAO_COLABORADOR>();
            //this.PUBLICACAO_REVISAO = new HashSet<PUBLICACAO_REVISAO>();
        }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Seção")]
        public Nullable<int> SEC_ID { get; set; }

        [DisplayName("Label")]
        public Nullable<int> LBL_ID { get; set; }

        [DisplayName("Tipo de Matéria")]
        [Required(ErrorMessage = "Por favor, informe o Tipo desta Matéria!")]
        public Nullable<int> TIP_MAT_ID { get; set; }

        [DisplayName("Tipo do Ato")]
        public Nullable<int> TIP_ATO_ID { get; set; }

        [DisplayName("Nº do Ato")]
        [MaxLength(50, ErrorMessage = "Por favor, informe um numero de ato com 50 caracteres no máximo!")]
        public string PUB_NUMERO_ATO { get; set; }

        [DisplayName("Veículo publicador deste Ato")]
        public Nullable<int> TVI_ID { get; set; }

        [DisplayName("Órgão emissor deste Ato")]
        public Nullable<int> ORG_ID { get; set; }

        [DisplayName("Resenha ou resumo desta matéria para a revista semanal (periódico/informativo)")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RESENHA { get; set; }

        [DisplayName("Conteúdo desta matéria na íntegra para o portal e base de conhecimento da consultoria")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO { get; set; }

        [DisplayName("Observações desta matéria")]
        [MaxLength(400, ErrorMessage = "Por favor, informe uma Observação com no máximo 400 caracteres!")]
        public string PUB_OBS { get; set; }

        [DisplayName("Cadastrada em")]
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }

        [DisplayName("Última alteração")]
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }

        [DisplayName("Redator")]
        public string USU_LOGIN { get; set; }

        [DisplayName("Data do Ato")]
        //[PresentDate(ErrorMessage="Por favor, informe uma data anterior ao dia de hoje!")]
        public Nullable<System.DateTime> PUB_DATA_ATO { get; set; }

        [DisplayName("Ato publicado em")]
        //[PresentDate(ErrorMessage = "Por favor, informe uma data anterior ao dia de hoje!")]
        public Nullable<System.DateTime> PUB_DATA_PUB_ATO { get; set; }

        [DisplayName("Ativa")]
        [Required(ErrorMessage="Por favor, informe [Sim] se esta matéria será usada neste informativo, e [Não] para arquivá-la para uso posterior!")]
        public Nullable<int> PUB_ATIVO { get; set; }

        [DisplayName("Fluxo")]
        public Nullable<int> FLU_ID { get; set; }

        [DisplayName("Descrição da NEWS")]
        [MaxLength(100, ErrorMessage = "Por favor, informe uma descrição de news com 100 caracteres no máximo!")]
        public string PUB_DESCRICAO_NEWS { get; set; }

        [DisplayName("Complemento do veículo")]
        [MaxLength(100, ErrorMessage = "Por favor, informe um complemento com 100 caracteres no máximo!")]
        public string PUB_COMPL_VEICULO { get; set; }

        [DisplayName("Cabeça da Matéria")]
        [Column(TypeName = "varchar(MAX)")]
        public string CABECALHO { get; set; }

        [DisplayName("REVISÃO TÉCNICA - Matéria do Portal")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RVT { get; set; }

        [DisplayName("DIGITAÇÃO - Matéria Impressa")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RESENHA_DGT { get; set; }

        [DisplayName("REVISÃO ORTOGRÁFICA - Matéria Impressa")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RESENHA_RVO { get; set; }

        [DisplayName("REVISÃO TÉCNICA - Matéria Impressa")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RESENHA_RVT { get; set; }

        [DisplayName("REDAÇÃO - Matéria Impressa")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RESENHA_RDC { get; set; }

        [DisplayName("REDAÇÃO - Matéria do Portal")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RDC { get; set; }

        [DisplayName("REVISÃO ORTOGRÁFICA - Matéria do Portal")]
        [Column(TypeName = "varchar(MAX)")]
        public string PUB_CONTEUDO_RVO { get; set; }

        public virtual LabelsDTO LABELS { get; set; }
        public virtual ICollection<NoticiaDTO> NOTICIA { get; set; }
        public virtual OrgaoDTO ORGAO { get; set; }
        public virtual PublicacaoFluxoDTO PLUBLICACAO_FLUXO { get; set; }
        public virtual ICollection<PublicacaoAlteracaoRevogacaoDTO> PUBLICACAO_ALTERACAO_REVOGACAO { get; set; }
        public virtual ICollection<PublicacaoAreaConsultoriaDTO> PUBLICACAO_AREAS_CONSULTORIA { get; set; }
        //public virtual ICollection<PUBLICACAO_REVISAO_COLABORADOR> PUBLICACAO_REVISAO_COLABORADOR { get; set; }
        //public virtual ICollection<PUBLICACAO_REVISAO> PUBLICACAO_REVISAO { get; set; }
        public virtual SecoesDTO SECOES { get; set; }
        public virtual TipoAtoDTO TIPO_ATO { get; set; }
        public virtual TipoMateriaDTO TIPO_MATERIA { get; set; }
        public virtual VeiculoDTO VEICULO { get; set; }


    }
}
