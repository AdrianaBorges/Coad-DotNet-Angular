
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Indicador da IE do Destinatário
    /// </summary>
    public enum TipoIEDestinatarioEnum
    {
        [XmlEnum("1")]
        ContribuinteICMS = 1,
        
        [XmlEnum("2")]
        ContribuinteIsentoDoICMS = 2
    }
}
