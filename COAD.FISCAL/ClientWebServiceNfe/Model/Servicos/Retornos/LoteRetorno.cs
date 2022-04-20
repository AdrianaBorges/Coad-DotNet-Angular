using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    [Serializable]
    [XmlRoot(ElementName = "retEnviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class LoteRetorno
    {
        [XmlAttribute]
        public string versao { get; set; }

        [XmlIgnore]
        public XmlDocument XmlLote { get; set; }

        public TipoAmbienteEnum tpAmb { get; set; }

        public string verAplic { get; set; }
        public int? cStat { get; set; }
        public string xMotivo { get; set; }
        public string cUF { get; set; }

        public DateTime? dhRecbto { get; set; }
        public ReciboLote infRec { get; set; }
        public ProtocoloRecebimento protNFe { get; set; }
    }
}
