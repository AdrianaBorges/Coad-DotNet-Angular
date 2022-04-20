using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class PublicacaoAlteracaoRevogacaoDTO
    {
        [DisplayName("ID")]
        public Nullable<int> PAR_ID { get; set; }

        [DisplayName("Nº Matéria")]
        public Nullable<int> PUB_ID { get; set; }

        [DisplayName("Altera ou Revoga?")]
        [Required(ErrorMessage = "Por favor, informe se esta Publicação será (R)evogada ou (A)lterada!")]
        public string PAR_TIPO { get; set; }

        [DisplayName("Tipo do Ato")]
        [Required(ErrorMessage = "Por favor, informe o Tipo do Ato Revogado/Alterado!")]
        public int TIP_ATO_ID { get; set; }

        [DisplayName("Nº do Ato")]
        [Required(ErrorMessage = "Por favor, informe o Nº do Ato Revogado/Alterado!")]
        public string PUB_NUMERO_ATO { get; set; }

        [DisplayName("Órgão Emissor")]
        //[Required(ErrorMessage = "Por favor, informe o Órgão Emissor do Ato Revogado/Alterado!")]
        public Nullable<int> ORG_ID { get; set; }

        [DisplayName("Data do Ato")]
        [Required(ErrorMessage = "Por favor, informe a Data do Ato Revogado/Alterado!")]
        public Nullable<System.DateTime> PUB_DATA_ATO { get; set; }

        [DisplayName("Alteração/Revogação ocorrida")]
        [Required(ErrorMessage = "Por favor, informe a Alteração/Revogação Ato!")]
        public string PUB_ALTERACAO_ATO { get; set; }

        public virtual PublicacaoDTO PUBLICACAO { get; set; }
    }
}
