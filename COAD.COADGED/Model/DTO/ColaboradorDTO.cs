using COAD.COADGED.Repositorios.Contexto;
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
    public class ColaboradorDTO
    {
        public ColaboradorDTO()
        {
            this.PUBLICACAO_CICLO_APROVACAO = new HashSet<PublicacaoCicloAprovacaoDTO>();
            this.PUBLICACAO = new HashSet<PublicacaoDTO>();
        }

        [DisplayName("ID")]
        public Nullable<int> COL_ID { get; set; }

        [DisplayName("Colaborador")]
        [Required(ErrorMessage = "Por favor, você precisa informar um nome de colaborador!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string COL_NOME { get; set; }

        [DisplayName("Ativo")]
        [Required(ErrorMessage = "Por favor, você precisa informar se o Colaborador está ou não ativo!")]
        public Nullable<int> COL_ATIVO { get; set; }

        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }

        [DisplayName("Cargo")]
        public Nullable<int> CRG_ID { get; set; }

        [DisplayName("Colecionador")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Cargo")]
        public virtual CargosDTO CARGOS { get; set; }
        public virtual ICollection<PublicacaoCicloAprovacaoDTO> PUBLICACAO_CICLO_APROVACAO { get; set; }
        public virtual ICollection<PublicacaoDTO> PUBLICACAO { get; set; }
        public virtual AreasDTO AREAS_CONSULTORIA { get; set; }
    }
}
