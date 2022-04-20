
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Identificador de local de destino da operação
    /// </summary>
    public enum TipoLocalDestinoOperacaoEnum
    {
        [XmlEnum("1")]
        OperacaoInterna = 1,
        
        [XmlEnum("2")]
        OperacaoInterestadual = 2,
        
        [XmlEnum("3")]
        OperacaoComExterior = 3
    }
}
