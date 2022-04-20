using COAD.FISCAL.Model.DTOCriptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    
    [Serializable]
    [XmlRoot(ElementName = "NFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NotaFiscal
    {
        public NotaFiscal()
        {
            lstInfNFe = new List<InfoNfeDTO>();
        }

        [XmlElement(Namespace = "http://www.portalfiscal.inf.br/nfe", ElementName = "infNFe")]
        public List<InfoNfeDTO> lstInfNFe { get; set; }


        [XmlElement(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public SignatureDTO Signature { get; set; }
    }
}
