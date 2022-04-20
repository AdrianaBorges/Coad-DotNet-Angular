using COAD.CORPORATIVO.Model.Dto.Custons.Mundipagg;

using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace COAD.CORPORATIVO.Service.Mundipagg
{
    public class CarrinhoSRV
    {

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
