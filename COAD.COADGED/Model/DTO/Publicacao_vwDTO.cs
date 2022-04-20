using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace COAD.COADGED.Model.DTO
{
    public class Publicacao_vwDTO
    {
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
        [Required(ErrorMessage = "Por favor, informe o Tipo do Ato!")]
        public Nullable<int> TIP_ATO_ID { get; set; }

        [DisplayName("Nº do Ato")]
        [MaxLength(50, ErrorMessage = "Por favor, informe um numero de ato com 50 caracteres no máximo!")]
        public string PUB_NUMERO_ATO { get; set; }

        [DisplayName("Veículo publicador deste Ato")]
        public Nullable<int> TVI_ID { get; set; }

        [DisplayName("Órgão emissor deste Ato")]
        public Nullable<int> ORG_ID { get; set; }

        [DisplayName("Resenha ou resumo desta matéria para a revista semanal (periódico/informativo)")]
        public string PUB_CONTEUDO_RESENHA { get; set; }

        [DisplayName("Conteúdo desta matéria na íntegra para o portal e base de conhecimento da consultoria")]
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
        public Nullable<System.DateTime> PUB_DATA_ATO { get; set; }

        [DisplayName("Ato publicado em")]
        public Nullable<System.DateTime> PUB_DATA_PUB_ATO { get; set; }

        [DisplayName("Ativa")]
        public Nullable<int> PUB_ATIVO { get; set; }

        [DisplayName("Fluxo")]
        public Nullable<int> FLU_ID { get; set; }

        [DisplayName("Descrição da NEWS")]
        [MaxLength(100, ErrorMessage = "Por favor, informe uma descrição de news com 100 caracteres no máximo!")]
        public string PUB_DESCRICAO_NEWS { get; set; }

        [DisplayName("Complemento do veículo")]
        [MaxLength(100, ErrorMessage = "Por favor, informe um complemento com 100 caracteres no máximo!")]
        public string PUB_COMPL_VEICULO { get; set; }
    }
}
