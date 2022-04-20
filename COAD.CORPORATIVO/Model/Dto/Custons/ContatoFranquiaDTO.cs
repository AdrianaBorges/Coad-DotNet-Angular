using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ContatoFranquiaDTO
    {
        //[Required(ErrorMessage = "A data de contato é obrigatória")]
        //[PresentDate(ErrorMessage = "A data do contato deve ser a partir de hoje.")]
        public DateTime? Data { get; set; }

        public int CLI_ID { get; set; }

        [Required(ErrorMessage = "Digite as observações")]
        public string Observacoes { get; set; }
    }
}
