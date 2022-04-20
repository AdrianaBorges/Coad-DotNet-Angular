using COAD.FISCAL.Model.NFSe.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    [Serializable]
    public class IdentificacaoRps
    {
        [Required(ErrorMessage = "Informe o Número da Rps")]
        public int? Numero { get; set; }

        [Required(ErrorMessage = "Informe a Série da Rps")]
        public string Serie { get; set; }

        [Required(ErrorMessage = "Informe o Tipo da Rps")]
        public TipoRPSEnum Tipo { get; set; }
    }
}
