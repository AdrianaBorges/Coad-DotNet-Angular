using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    public enum TipoNaturezaOperacaoEnum
    {
        [XmlEnum("1")]
        TRIBUTACAO_NO_MUNICIPIO = 1,

        [XmlEnum("2")]
        TRIBUTACAO_FORA_MUNICIPIO = 2,

        [XmlEnum("3")]
        ISENCAO = 3,

        [XmlEnum("4")]
        IMUNE = 4,

        [XmlEnum("5")]
        EXIBILIDADE_SUSPENSA_DECISAO_JUDICIAL = 5,

        [XmlEnum("6")]
        EXIBILIDADE_SUSPENSA_PROCEDIMENTO_ADMIN = 6
    }
}
