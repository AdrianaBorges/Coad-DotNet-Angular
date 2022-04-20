using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    [XmlRoot("CancelarNfseEnvio", Namespace = "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd")]
    [Serializable]
    public class CancelarNfseEnvio
    {
        public PedidoCancelamentoNfse Pedido { get; set; }
    }
}
