using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class DadosTomadorRps
    {
        public IdentificacaoTomadorRps IdentificacaoTomador { get; set; }
        public string RazaoSocial { get; set; }
        public EnderecoRps Endereco { get; set; }
        public ContatoRps Contato { get; set; }
    }
}
