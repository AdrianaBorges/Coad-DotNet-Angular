using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Enumerados
{
    public enum TipoEventoEnum
    {
        [XmlEnum("210200")]
        CONFIRMACAO_OPERACAO = 210200,

        [XmlEnum("210210")]
        CIENCIA_OPERACAO = 210210,

        [XmlEnum("210220")]
        DESCONHECIMENTO_OPERACAO = 210220,

        [XmlEnum("210240")]
        OPERACAO_NAO_REALIZADA = 210240,

        [XmlEnum("110111")]
        CANCELAMENTO = 110111,

        [XmlEnum("110110")]
        CARTA_CORRECAO = 110110
    }
}
