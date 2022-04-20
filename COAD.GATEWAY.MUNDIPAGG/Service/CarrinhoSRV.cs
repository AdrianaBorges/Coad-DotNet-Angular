using COAD.GATEWAY.MUNDIPAGG.Model;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Service
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria o carrinho de compras
    /// </summary>
    public class CarrinhoSRV
    {
 
        /// <summary>
        /// Criar o carrinho de compras
        /// </summary>
        /// <returns></returns>
        public ShoppingCart Criar(CarrinhoDTO carrinho)
        {
            ShoppingCart shoppintCart = new ShoppingCart()
            {
                DeliveryAddress = new DeliveryAddress()
                {
                    City = carrinho.cidade,
                    Complement = carrinho.complemento,
                    Country = CountryEnum.Brazil.ToString(),
                    District = carrinho.bairro,
                    Number = carrinho.numero,
                    State = carrinho.UF,
                    Street = carrinho.endereco,
                    ZipCode = carrinho.CEP
                },
                DeliveryDeadline = DateTime.Now.AddDays(carrinho.prazoMaximoEntrega),
                EstimatedDeliveryDate = DateTime.Now.AddDays(carrinho.prazoEstimadoEntrega),
                FreightCostInCents = carrinho.valorFrete,
                ShippingCompany = carrinho.transportadora,
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
