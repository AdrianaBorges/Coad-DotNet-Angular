using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    
    public class EnderecoRps
    {
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int? CodigoMunicipio { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }

        public bool ShouldSerializeComplemento()
        {
            return (!string.IsNullOrWhiteSpace(Complemento));
        }
    }
}
