using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class InformacoesPagamento
    {
        public InformacoesPagamento()
        {
            Detalhamentos = new List<InfPagamentoDetalhamento>();
        }

        [XmlElement("detPag")]
        public List<InfPagamentoDetalhamento> Detalhamentos { get; set; }
    }
}
