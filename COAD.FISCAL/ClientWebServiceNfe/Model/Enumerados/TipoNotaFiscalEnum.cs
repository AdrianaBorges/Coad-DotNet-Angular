using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Indicador da forma de pagamento
    /// </summary>
    public enum TipoNotaFiscalEnum
    {
        [XmlEnum("0")]
        Entrada = 0,


        [XmlEnum("1")]
        Saida = 1,
    }
}
