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
    public class Areas2DTO
    {
        public Areas2DTO()
        {
            //this.AREAS_CONSULTORIA_NEWS = new HashSet<AREAS_CONSULTORIA_NEWS>();
            this.PUBLICACAO_AREAS_CONSULTORIA = new HashSet<PublicacaoAreaConsultoriaDTO>();
            this.TITULACAO = new HashSet<TitulacaoDTO>();
            this.COLABORADOR = new HashSet<ColaboradorDTO>();
        }
    
        [DisplayName("ID")]
        public Nullable<int> ARE_CONS_ID { get; set; }

        [DisplayName("Area")]
        [Required(ErrorMessage = "Por favor, você precisa informar o nome da area!")]
        [MaxLength(50, ErrorMessage = "Por favor, informe no máximo 50 caracteres!")]
        public string ARE_CONS_DESCRICAO { get; set; }

        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERACAO { get; set; }
        public string USU_LOGIN { get; set; }

        [DisplayName("Cabeça da Matéria")]
        public string ARE_CABECA_MATERIA { get; set; }

        //public virtual ICollection<AREAS_CONSULTORIA_NEWS> AREAS_CONSULTORIA_NEWS { get; set; }
        public virtual ICollection<PublicacaoAreaConsultoriaDTO> PUBLICACAO_AREAS_CONSULTORIA { get; set; }
        public virtual ICollection<TitulacaoDTO> TITULACAO { get; set; }
        public virtual ICollection<ColaboradorDTO> COLABORADOR { get; set; }
    }
}
