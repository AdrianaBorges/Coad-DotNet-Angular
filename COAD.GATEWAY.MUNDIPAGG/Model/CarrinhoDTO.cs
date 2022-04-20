using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Model
{
    public class CarrinhoDTO
    {
        /// <summary>
        /// (City) Cidade
        /// </summary>
        public string cidade { get; set; }

        /// <summary>
        /// (Complement) Complemento do endereço
        /// </summary>
        public string complemento { get; set; }

        /// <summary>
        /// (District) Bairro
        /// </summary>
        public string bairro { get; set; }

        /// <summary>
        /// (Number) Número
        /// </summary>
        public string numero { get; set; }

        /// <summary>
        /// (State) Estado - Unidade Federativa
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// (Street) Logradouro/Endereço
        /// </summary>
        public string endereco { get; set; }

        /// <summary>
        /// (ZipCode) CEP
        /// </summary>
        public string CEP { get; set; }

        /// <summary>
        /// (DeliveryDeadline) Prazo máximo em dias para entrega dos produtos a partir da data atual.
        /// </summary>
        public int prazoMaximoEntrega { get; set; }

        /// <summary>
        /// (EstimatedDeliveryDate) Prazo estimado em dias para entrega dos produtos a partir da data atual.
        /// </summary>
        public int prazoEstimadoEntrega { get; set; }

        /// <summary>
        /// (FreightCostInCents) Valor do frete em centavos
        /// </summary>
        public int valorFrete { get; set; }

        /// <summary>
        /// (ShippingCompany) Nome da empresa responsável pela entrega do(s) produto(s)
        /// </summary>
        public string transportadora { get; set; }

    }
}
