using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    public enum TipoRPSEnum
    {
        [XmlEnum("1")]
        RPS = 1,

        [XmlEnum("2")]
        NOTA_FISCAL_CONJUNTA = 2,

        [XmlEnum("3")]
        CUPOM = 3
    }
}
