using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class IdentificacaoPrestadorRps
    {
        public IdentificacaoPrestadorRps()
        {

        }

        [Required(ErrorMessage = "Informe o CNPJ do Prestador")]
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
    }
}
