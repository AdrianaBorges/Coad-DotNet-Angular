using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    public class InfSubstituicaoNfse
    {
        [XmlAttribute]
        public string Id { get; set; }
        public string NfseSubstituidora { get; set; }

    }
}
