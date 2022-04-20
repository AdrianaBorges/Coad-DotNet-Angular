using COAD.FISCAL.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes.Interfaces
{
    public interface IEmpresa
    {
        int? EmpresaID { get; set; }
        string RazaoSocial { get; set; }
        string NomeFantasia { get; set; }
        string CNPJ { get; set; }
        string IE { get; set; }
        string IM { get; set; }
        string Logradouro { get; set; }
        string Numero { get; set; }
        string Complemento { get; set; }
        string Bairro { get; set; }
        string CEP { get; set; }
        string Telefone { get; set; }
        string Email { get; set; }
        Nullable<int> SeqNFe { get; set; }
        Nullable<int> SeqNFSe { get; set; }
        string CodIBGE { get; set; }
        string Municipio { get; set; }
        UFDTO UFDTO { get; set; }
    }
}
