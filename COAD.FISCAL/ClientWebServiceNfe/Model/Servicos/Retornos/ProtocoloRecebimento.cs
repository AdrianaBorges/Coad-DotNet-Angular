using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    [XmlRoot(ElementName = "protNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ProtocoloRecebimento
    {
        [XmlAttribute]
        public string versao { get; set; }

        public ProtocoloResposta infProt { get; set; }
    }
}
