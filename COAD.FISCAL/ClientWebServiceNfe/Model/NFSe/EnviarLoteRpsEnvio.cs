using COAD.FISCAL.Model.DTOCriptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    [XmlRoot("EnviarLoteRpsEnvio", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    [Serializable]
    public class EnviarLoteRpsEnvio
    {
        public LoteRps LoteRps { get; set; }
        public SignatureDTO Signature { get; set; }
    }
}
