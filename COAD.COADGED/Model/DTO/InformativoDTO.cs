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
    public class InformativoDTO
    {
        public InformativoDTO()
        {
            this.PUBLICACAO_UF = new HashSet<PublicacaoUfDTO>();
        }
    
        [DisplayName("Ano")]
        [Required(ErrorMessage = "Por favor, você precisa informar o ano deste Informativo!")]
        [MaxLength(4, ErrorMessage = "Por favor, informe um ano com 4 caracteres!")]
        [MinLength(4, ErrorMessage = "Por favor, informe um ano com 4 caracteres!")]
        public string INF_ANO { get; set; }

        [DisplayName("Informativo")]
        [Required(ErrorMessage = "Por favor, você precisa informar o número deste Informativo!")]
        public Nullable<int> INF_NUMERO { get; set; }

        [DisplayName("Início da produção")]
        public Nullable<System.DateTime> INF_DATA_INICIO { get; set; }

        [DisplayName("Final da produção")]
        public Nullable<System.DateTime> INF_DATA_FINAL { get; set; }

        [DisplayName("Previsão Postagem")]
        public Nullable<System.DateTime> INF_DATA_PREV_POSTAGEM { get; set; }

        [DisplayName("Postagem realizada")]
        public Nullable<System.DateTime> INF_DATA_POSTAGEM { get; set; }

        [DisplayName("Ativo")]
        public Nullable<int> INF_ATIVO { get; set; }

        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
    
        [DisplayName("Publicação")]
        public virtual ICollection<PublicacaoUfDTO> PUBLICACAO_UF { get; set; }
    }
}
