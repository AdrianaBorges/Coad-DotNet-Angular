using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class Cartao
    {
        [XmlElement("tpIntegra")]
        public TipoIntegracaoPagamentoEnum TipoIntegracaoPag { get; set; }
        public string CNPJ { get; set; }

        [XmlElement("tBand")]
        public BandeiraCartaoEnum BandeiraCartao { get; set; }
    }
}
