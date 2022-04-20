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
    /// Este objeto cria o registro do Comprador ou Cliente
    /// </summary>
    public class CompradorSRV
    {

        /// <summary>
        /// Criar o registro do Comprador ou Cliente
        /// </summary>
        /// <returns></returns>
        public Buyer Criar(CompradorDTO comprador)
        {
            // Cria o comprador.
            Buyer buyer = new Buyer()
            {
                AddressCollection = new Collection<BuyerAddress>(),
                Birthdate = comprador.dataNascimento, //new DateTime(1990, 8, 20),
                BuyerCategory = comprador.categoria,
                BuyerReference = "C3PO",
                CreateDateInMerchant = comprador.dataCadastroNaLoja, //DateTime.Now,
                DocumentNumber = comprador.numeroDocumento,
                DocumentType = comprador.tipoDocumento,
                Email = comprador.email,
                EmailType = comprador.tipoEmail,
                FacebookId = comprador.contaFacebook,
                Gender = comprador.sexo,
                HomePhone = comprador.foneResidencial,
                MobilePhone = comprador.celular,
                Name = comprador.nome,
                PersonType = comprador.tipoPessoa,
                TwitterId = comprador.contaTwitter,
                WorkPhone = comprador.foneComercial
            };

            // Adiciona um endereço para o comprador.
            buyer.AddressCollection.Add(new BuyerAddress()
            {
                AddressType = comprador.tipoEndereco,
                City = comprador.cidade,
                Complement = comprador.complemento,
                Country = CountryEnum.Brazil.ToString(),
                District = comprador.bairro,
                Number = comprador.numero,
                State = comprador.UF,
                Street = comprador.endereco,
                ZipCode = comprador.CEP
            });

            return buyer;
        }
    }
}
