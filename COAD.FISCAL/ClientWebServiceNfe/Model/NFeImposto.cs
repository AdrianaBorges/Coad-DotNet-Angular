using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    [Serializable]
    public class NFeImposto
    {
        [Required(ErrorMessage = "O objeto ICMS é obrigatório")]
        public NFeImpostoICMS ICMS { get; set; }

        public NfeImpostoIPI IPI { get; set; }

        [Required(ErrorMessage = "O objeto PIS é obrigatório")]
        public NFeImpostoPIS PIS { get; set; }

        [Required(ErrorMessage = "O objeto COFINS é obrigatório")]
        public NFeImpostoCOFINS COFINS { get; set; }

    }
}
