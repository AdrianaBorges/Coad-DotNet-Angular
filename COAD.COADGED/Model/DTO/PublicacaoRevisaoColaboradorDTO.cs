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
    public class PublicacaoRevisaoColaboradorDTO
    {
        [DisplayName("ID")]
        public int? REV_ID { get; set; }

        [DisplayName("Nº Matéria")]
        public int PUB_ID { get; set; }

        [DisplayName("Colecionador")]
        [Required(ErrorMessage = "Por favor, informe o Colecionador!")]
        public int ARE_CONS_ID { get; set; }

        [DisplayName("Colaborador")]
        [Required(ErrorMessage = "Por favor, informe o Colaborador!")]
        public int COL_ID { get; set; }

        [DisplayName("Data da ocorrência")]
        [Required(ErrorMessage = "Por favor, informe a Data da ocorrência!")]
        public System.DateTime DATA { get; set; }

        [DisplayName("Revisão aprovada?")]
        [Required(ErrorMessage = "Por favor, informe se a Revisão foi ou não aprovada!")]
        public string REVISAO { get; set; }

        [DisplayName("Editou a matéria?")]
        [Required(ErrorMessage = "Por favor, informe se houve ou não Edição da Matéria!")]
        public string EDITOU { get; set; }

        [DisplayName("Motivo da reprovação")]
        [Required(ErrorMessage = "Por favor, informe o Motivo da ocorrência!")]
        public string MOTIVO { get; set; }

        public virtual ColaboradorDTO COLABORADOR { get; set; }
        public virtual PublicacaoDTO PUBLICACAO { get; set; }
        public virtual PublicacaoAreaConsultoriaDTO PUBLICACAO_AREAS_CONSULTORIA { get; set; }
    }
}
