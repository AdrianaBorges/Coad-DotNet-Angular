using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class EnderecoDTO
    {
        public string CEP { get; set; }
        public string Numero { get; set; }
        public UFDTO UF { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public string Municipio { get; set; }
        public int? CodMunicipio { get; set; }
        public string Telefone { get; set; }
        public string Pais { get; set; }
    }
}
