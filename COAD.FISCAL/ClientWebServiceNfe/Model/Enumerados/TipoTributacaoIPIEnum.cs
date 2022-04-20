using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    public enum TipoTributacaoIPIEnum
    {
        [XmlEnum("01")]
        EntradaTributadaComAliquotaZero = 1,

        [XmlEnum("02")]
        EntradaIsenta = 2,

        [XmlEnum("03")]
        EntradaNaoTributada = 3,

        [XmlEnum("04")]
        EntradaImune = 4,

        [XmlEnum("05")]
        EntradaComSuspensao = 5,

        [XmlEnum("51")]
        SaidaTributadaComAliquotaZero = 51,

        [XmlEnum("52")]
        SaidaIsenta = 52,

        [XmlEnum("53")]
        SaidaNaoTributada = 53,

        [XmlEnum("54")]
        SaidaImune = 54,

        [XmlEnum("55")]
        SaidaComSuspensao = 55
    }
}
