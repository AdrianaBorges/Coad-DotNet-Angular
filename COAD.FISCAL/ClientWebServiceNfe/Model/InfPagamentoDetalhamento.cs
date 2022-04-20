using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class InfPagamentoDetalhamento
    {
        public InfPagamentoDetalhamento()
        {
            Cartoes = new List<Cartao>();
        }

        [XmlElement("tPag")]
        public TipoPagamentoEnum FormaPagamento { get; set; }

        [Required(ErrorMessage = "O valor de pagamento do dto Pagamento é obrigatório")]
        [XmlElement("vPag")]
        public decimal ValorPagamento { get; set; }

        [XmlElement("card")]
        public List<Cartao> Cartoes { get; set; }

        //[XmlElement("vTroco")]
        //public decimal ValorTroco { get; set; }

    }
}
