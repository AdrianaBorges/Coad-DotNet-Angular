using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe.Enumerados
{
    [Serializable]
    public enum IndicacaoCpfCnpjEnum
    {
        [XmlEnum("1")]
        CPF = 1,

        [XmlEnum("2")]
        CNPJ = 2,

        [XmlEnum("3")]
        NAO_INFORMADO = 3
    }
}
