using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class EmpresaDTO
    {
        public int? SequencialNFe { get; set; }
        public int? SequencialNFSe { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string IE { get; set; }
        public string IM { get; set; }
        public string CNPJ { get; set; }

        public EnderecoDTO Endereco { get; set; }
    }
}
