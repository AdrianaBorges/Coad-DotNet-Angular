using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    public enum BandeiraCartaoEnum
    {
        [XmlEnum("01")]
        VISA = 1,

        [XmlEnum("02")]
        MASTERCARD = 2,

        [XmlEnum("03")]
        AMERICAN_EXPRESS = 3,

        [XmlEnum("04")]
        SOROCRED = 4,

        [XmlEnum("05")]
        DISNERS_CLUB = 05,

        [XmlEnum("06")]
        ELO = 6,

        [XmlEnum("07")]
        HIPERCARD = 7,

        [XmlEnum("08")]
        AURA = 8,

        [XmlEnum("09")]
        CABAL = 9,

        [XmlEnum("99")]
        OUTROS = 99,
    }
}
