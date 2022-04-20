using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Bancos.Model.DTO
{
    /// <summary>
    /// Armazena o resultado da decomposição do código de barra aos 3 primeiros campos.
    /// </summary>
    public class CamposLinhaDigitalDTO
    {
        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
    }
}
