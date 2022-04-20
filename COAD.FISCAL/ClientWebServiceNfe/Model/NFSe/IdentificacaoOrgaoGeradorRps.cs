using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class IdentificacaoOrgaoGeradorRps
    {
        [Required(ErrorMessage = "Informe o Código Municipal do Orgão Gerador")]
        public string CodigoMunicipio { get; set; }

        [Required(ErrorMessage = "Informe a UF do Orgão Gerador")]
        public string Uf { get; set; }
    }
}
