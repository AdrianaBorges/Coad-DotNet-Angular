using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Identificação do Ambiente
    /// </summary>
    public enum TipoAmbienteEnum
    {

        [XmlEnum("1")]
        Producao = 1,
        
        [XmlEnum("2")]
        Homologacao = 2,
    }
}
