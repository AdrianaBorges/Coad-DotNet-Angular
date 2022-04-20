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
    public enum IndPagEnum
    {
        [XmlEnum("0")]
        PagamentoAVista = 0,

        [XmlEnum("1")]
        PagamentoAPrazo = 1,

        [XmlEnum("2")]
        Outros = 2
    }
}
