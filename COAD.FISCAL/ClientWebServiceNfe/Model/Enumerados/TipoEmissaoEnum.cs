using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Tipo de Emissão da NF-e
    /// </summary>
    public enum TipoEmissaoEnum
    {
        [XmlEnum("1")]
        Normal = 1,

        [XmlEnum("2")]
        ContigenciaFS = 2,

        [XmlEnum("3")]
        ContigenciaSCAN = 3,

        [XmlEnum("4")]
        ContigenciaDPEC = 4,

        [XmlEnum("5")]
        ContigenciaFS_DA = 5,

        [XmlEnum("6")]
        ContigenciaSVC_AN = 6,

        [XmlEnum("7")]
        ContigenciaSVC_RS = 7
    }
}
