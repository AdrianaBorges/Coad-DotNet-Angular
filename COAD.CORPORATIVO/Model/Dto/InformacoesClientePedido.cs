using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class InformacoesClientePedido
    {
        public InformacoesClientePedido()
        {
            emails = new List<EmailClienteDTO>();
            telefones = new List<PrePedidoTelefoneDTO>();
        }

        public int? clienteId { get; set; }
        public string nomeRazaoSocial { get; set; }
        public string aosCuidados { get; set; }
        public string tipoDeCliente { get; set; }
        public string cpfCnpj { get; set; }
        public string inscricaoEstadual { get; set; }
        public string enderecoEntrega { get; set; }
        public string numeroEntrega { get; set; }
        public string complementoEntrega { get; set; }
        public string cepEntrega { get; set; }
        public string bairroEntrega { get; set; }
        public string municipioEntrega { get; set; }
        public string ufEntrega { get; set; }
        public string enderecoFaturamento { get; set; }
        public string numeroFaturamento { get; set; }
        public string complementoFaturamento { get; set; }
        public string cepFaturamento { get; set; }
        public string bairroFaturamento { get; set; }
        public string municipioFaturamento { get; set; }
        public string ufFaturamento { get; set; }
        public List<EmailClienteDTO> emails { get; set; }
        public List<PrePedidoTelefoneDTO> telefones { get; set; }
    }
}