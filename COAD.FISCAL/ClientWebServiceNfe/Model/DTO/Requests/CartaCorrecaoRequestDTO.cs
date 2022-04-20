using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO.Requests
{
    public class CartaCorrecaoRequestDTO
    {
        [Required(ErrorMessage = "Informe o Código da Nota Fiscal")]
        public int? NotaFiscalID { get; set; }

        [Required(ErrorMessage = "Informe o texto da Carta de Correção")]
        [StringLength(1000, MinimumLength = 15, ErrorMessage = "A carta deve conter no mínimo 15 e no máximo 1000")]
        public string CartaCorrecao { get; set; }
    }
}
