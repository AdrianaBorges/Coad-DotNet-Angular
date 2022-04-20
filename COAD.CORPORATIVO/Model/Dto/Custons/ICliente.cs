using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public interface ICliente
    {   
        string CNPJ_CPF { get; }
        IEnumerable<string> TELEFONE { get; }
        IEnumerable<string> FAX { get; }
        IEnumerable<string> CELULAR { get; }
        IEnumerable<string> EMAIL { get; }

    }
}
