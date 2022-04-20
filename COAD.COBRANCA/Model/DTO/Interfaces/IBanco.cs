using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Model.DTO.Interfaces
{
    public interface IBanco
    {
        string CodigoBanco { get; set; }
        string NomeBanco { get; set; }

        string CalcularDVCodigoBarras(string codigoBarras);
        
    }
}
