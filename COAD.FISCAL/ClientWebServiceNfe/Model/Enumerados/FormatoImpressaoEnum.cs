using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Formato de Impressão do DANFE
    /// </summary>
    public enum FormatoImpressaoEnum
    {
        [XmlEnum("1")]
        Retrato = 1,

        [XmlEnum("3")]
        Paisagem = 2
    }
}
