using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class DadosPrestadorRps
    {
        public IdentificacaoPrestadorRps IdentificacaoPrestador { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public EnderecoRps Endereco { get; set; }
        public ContatoRps Contato { get; set; }
    }
}
