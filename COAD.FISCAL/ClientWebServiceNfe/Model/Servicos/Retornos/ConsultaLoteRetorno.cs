using COAD.FISCAL.Model.Enumerados;
using COAD.FISCAL.Model.Servicos.Abstratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{

    [Serializable]
    [XmlRoot(ElementName = "retConsReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ConsultaLoteRetorno : AbstractRespostaServico
    {
        public ConsultaLoteRetorno()
        {
            this.protNFe = new List<ProtocoloRecebimento>();
        }

        public int? cMsg { get; set; }
        public string xMsg { get; set; }

        [XmlElement]
        public List<ProtocoloRecebimento> protNFe { get; set; }
    }
}
