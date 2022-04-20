using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    [Serializable]
    public enum SituacaoLoteRpsEnum
    {
        [XmlEnum("1")]
        NAO_RECEBIDO = 1,

        [XmlEnum("2")]
        NAO_PROCESSADO = 2,

        [XmlEnum("3")]
        PROCESSADO_COM_ERRO = 3,

        [XmlEnum("4")]
        PROCESSADO_COM_SUCESSO = 4
    }
}
