using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Enumerados
{
    public enum IndicadorConstribuinteCTeEnum
    {
        [XmlEnum("0")]
        NAO_CREDENCIADO_EMISSAO_CTE = 0,

        [XmlEnum("1")]
        CREDENCIADO = 1,

        [XmlEnum("2")]
        CREDENCIADO_OBRIGATORIEDADE_TODAS_OPERACOES = 2,

        [XmlEnum("3")]
        CREDENCIADO_OBRIGATORIEDADE_PARCIAL = 3,
        
        [XmlEnum("4")]
        A_SEFAZ_NAO_FORNECE_INFORMACAO = 4,
    }
}
