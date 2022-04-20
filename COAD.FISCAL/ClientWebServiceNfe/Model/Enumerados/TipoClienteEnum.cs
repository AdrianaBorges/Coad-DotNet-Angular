using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Enumerados
{
    public enum TipoClienteEnum
    {
        /// <summary>
        /// Utiliza CPF
        /// </summary>
        Fisica = 0,

        /// <summary>
        /// Utiliza CPNJ
        /// </summary>
        Juridica = 1,

        /// <summary>
        /// Utiliza CNPJ
        /// </summary>
        OrgaoPublico = 2
    }
}
