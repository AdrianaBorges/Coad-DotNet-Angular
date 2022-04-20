using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    [Serializable]
    public enum StatusRpsEnum
    {

        [XmlEnum("1")]
        NORMAL = 1,

        [XmlEnum("2")]
        CANCELADA = 2,

    }
}
