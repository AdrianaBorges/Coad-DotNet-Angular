using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    public enum RegimeEspecialTributacaoEnum
    {
        [XmlEnum("1")]
        MICROEMPRESA_MUNICIPAL = 1,

        [XmlEnum("2")]
        ESTIMATIVA = 2,

        [XmlEnum("3")]
        SOCIEDADE_DE_PROFISSIONAIS = 3,

        [XmlEnum("4")]
        COOPERATIVA = 4,

        [XmlEnum("5")]
        MICROEMPRESARIO_INDIVIDUAL_MEI = 5,

        [XmlEnum("6")]
        MICROEMPRESARIO_PEQUENO_PORTE_ME_EPP = 6
    }
}
