using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Modalidade do frete
    /// </summary>
    public enum TipoModalidadeTransporteEnum
    {
        [XmlEnum("0")]
        CONTRATACAO_POR_CONTA_REMETENTE = 0,

        [XmlEnum("1")]
        CONTRATACAO_POR_CONTA_DESTINATARIO = 1,

        [XmlEnum("2")]
        CONTRATACAO_POR_CONTA_TERCEIROS = 2,

        [XmlEnum("3")]
        TRANSPORTE_POR_CONTA_REMETENTE = 3,

        [XmlEnum("4")]
        TRANSPORTE_POR_CONTA_DESTINATARIO = 4,

        [XmlEnum("9")]
        SEM_CORRENCIA_FRETE = 9
    }
}
