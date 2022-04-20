using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class IdentificacaoIntermediarioServiceRps
    {
        [Required(ErrorMessage = "Informe a Razão Social do Intermediário do Serviço")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "Informe CPF/CNPJ do Intermediário do Serviço")]
        public string CpfCnpj { get; set; }

        public string InscricaoMunicipal { get; set; }
    }
}
