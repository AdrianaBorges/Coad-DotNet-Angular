using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    public enum IndicacaoPresencaEnum
    {
        [XmlEnum("1")]
        OPERACAO_PRESENCIAL = 1,

        [XmlEnum("2")]
        OPERACAO_PRESENCIAL_PELA_INTERNET = 2,

        [XmlEnum("3")]
        OPERACAO_NAO_PRESENCIAL_TELEATENDIMENTO = 3,

        [XmlEnum("4")]
        NFE_EM_OPERACAO_COM_ENTREGA_A_DOMICILIO = 4,

        [XmlEnum("5")]
        OPERACAO_PRESENCIAL_FORA_DO_ESTABELECIMENTO = 5,

        [XmlEnum("9")]
        OPERACAO_NAO_PRESENCIAL_OUTROS = 9
    }
}
