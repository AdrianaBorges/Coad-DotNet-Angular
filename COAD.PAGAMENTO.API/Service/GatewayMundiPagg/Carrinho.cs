using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PAGAMENTO.API.Service.GatewayMundiPagg
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria o carrinho de compras
    /// </summary>
    public class Carrinho
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
        /// (DeliveryDeadline) Prazo máximo para entrega dos produtos. Este campo deve obedecer a máscara -> “yyyy-mm-ddThh:mm:ss”
        /// </summary>
        public int prazoMaximoEntrega { get; set; }

        /// <summary>
        /// (EstimatedDeliveryDate) Prazo estimado para entrega dos produtos. Este campo deve obedecer a máscara -> “yyyy-mm-ddThh:mm:ss”
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

        /// <summary>
        /// Criar o carrinho de compras
        /// </summary>
        /// <returns></returns>
        public ShoppingCart Criar()
        {
            ShoppingCart shoppintCart = new ShoppingCart()
            {
                DeliveryAddress = new DeliveryAddress()
                {
                    City = this.cidade,
                    Complement = this.complemento,
                    Country = CountryEnum.Brazil.ToString(),
                    District = this.bairro,
                    Number = this.numero,
                    State = this.UF,
                    Street = this.endereco,
                    ZipCode = this.CEP
                },
                DeliveryDeadline = DateTime.Now.AddDays(this.prazoMaximoEntrega),
                EstimatedDeliveryDate = DateTime.Now.AddDays(this.prazoEstimadoEntrega),
                FreightCostInCents = this.valorFrete,
                ShippingCompany = this.transportadora,
                ShoppingCartItemCollection = new Collection<ShoppingCartItem>()
            };

            return shoppintCart;
        }

        /// <summary>
        /// Adiciona um produto no carrinho de compras.
        /// <para>codigoProdutoNaLoja (ItemReference)</para>
        /// <para>nomeProduto (Name)</para>
        /// <para>descricaoProduto (Description)</para>
        /// <para>quantidade (Quantity)</para>
        /// <para>preco (UnitCostInCents)</para>
        /// <para>desconto (DiscountAmountInCents)</para>
        /// <para>total (TotalCostInCents)</para>
        /// </summary>
        /// <param name="shoppintCart"></param>
        /// <returns></returns>
        public ShoppingCart AdicionarProduto(ShoppingCart carrinho, string codigoProdutoNaLoja, string nomeProduto, string descricaoProduto, int quantidade, int preco, int desconto, int total)
        {
            carrinho.ShoppingCartItemCollection.Add(new ShoppingCartItem()
            {
                Description = descricaoProduto,
                DiscountAmountInCents = desconto,
                ItemReference = codigoProdutoNaLoja,
                Name = nomeProduto,
                Quantity = quantidade,
                TotalCostInCents = total,
                UnitCostInCents = preco
            });

            return carrinho;
        }
    }
}
