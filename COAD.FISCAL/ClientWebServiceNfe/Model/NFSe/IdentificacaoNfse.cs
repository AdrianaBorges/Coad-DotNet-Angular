using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class IdentificacaoNfse
    {
        [Required(ErrorMessage = "Informe o Número da Nfse")]
        public int? Numero { get; set; }

        [Required(ErrorMessage = "Informe o Cnpj na Nfse")]
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string CodigoMunicipio { get; set; }
    }
}
