using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    [Serializable]
    public enum IdenSimNaoEnum
    {
        [XmlEnum("1")]
        SIM = 1,

        [XmlEnum("2")]
        NAO = 2,
    }
}
