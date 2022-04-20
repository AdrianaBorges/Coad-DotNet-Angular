using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class EncaminhamentoDTO
    {
        [Required(ErrorMessage = "Não há representante selecionado.")]
        public int? REP_ID { get; set; }

        [Required(ErrorMessage = "Não há cliente selecionado.")]
        public int? CLI_ID { get; set; }

        [Required(ErrorMessage = "Digite uma observação")]
        public string observacao { get; set; }
    }
}
