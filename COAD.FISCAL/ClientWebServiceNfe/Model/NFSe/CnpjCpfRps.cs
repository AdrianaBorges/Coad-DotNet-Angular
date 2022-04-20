using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class CnpjCpfRps
    {
        public string Cnpj { get; set; }
        public string Cpf { get; set; }

        public bool ShouldSerializeCnpj()
        {
            return (!string.IsNullOrWhiteSpace(Cnpj));
        }

        public bool ShouldSerializeCpf()
        {
            return (!string.IsNullOrWhiteSpace(Cpf));
        }
    }
}
