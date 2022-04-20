using COAD.FISCAL.Model.Servicos.Retornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos
{

    [Serializable]
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NFeProcessada
    {
        [XmlAttribute]
        public string versao { get; set; }

        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public List<NFeDistribuicaoDTO> NFe { get; set; }
        public ProtocoloRecebimento protNFe { get; set; }
    }
}
