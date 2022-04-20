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
    /// Este objeto cria o registro do Comprador ou Cliente
    /// </summary>
    public class Comprador
    {
        /// <summary>
        /// (BuyerCategory) Comprador da categoria 0=Normal ou 1=Plus
        /// </summary>
        public int categoria { get; set; }

        /// <summary>
        /// (Birthdate) Data de nascimento do cliente
        /// </summary>
        public DateTime dataNascimento { get; set; }

        /// <summary>
        /// (CreateDateInMerchant) Data do cadastro do cliente na plataforma da loja
        /// </summary>
        public DateTime dataCadastroNaLoja { get; set; }

        /// <summary>
        /// (DocumentNumber) Número do documento (CPF/CNPJ)
        /// </summary>
        public string numeroDocumento { get; set; }

        /// <summary>
        /// (DocumentType) Tipo do documento informado (0=CPF ou 1=CNPJ)
        /// </summary>
        public int tipoDocumento { get; set; }

        /// <summary>
        /// (Email) Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// (EmailType) Email tipo 0=Pessoal ou 1=Comercial
        /// </summary>
        public int tipoEmail { get; set; }

        /// <summary>
        /// (FacebookId) Identificação do cliente no Facebook
        /// </summary>
        public string contaFacebook { get; set; }

        /// <summary>
        /// (Gender) Sexo 0=Masculino ou 1=Feminino
        /// </summary>
        public int sexo { get; set; }

        /// <summary>
        /// (HomePhone) Telefone residencial do cliente. Padrão obrigatório: (xx)123456789
        /// </summary>
        public string foneResidencial { get; set; }

        /// <summary>
        /// (MobilePhone) Telefone celular do cliente. Padrão obrigatório: (xx)123456789
        /// </summary>
        public string foneMovel { get; set; }

        /// <summary>
        /// (Name) Nome do comprador ou cliente
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// (PersonType) Tipo de pessoa (0=Física ou 1=Jurídica)
        /// </summary>
        public int tipoPessoa { get; set; }

        /// <summary>
        /// (TwitterId) Identificação do cliente no Twitter
        /// </summary>
        public string contaTwitter { get; set; }

        /// <summary>
        /// (WorkPhone) Telefone comercial do cliente. Padrão obrigatório: (xx)123456789
        /// </summary>
        public string foneComercial { get; set; }

        /// <summary>
        /// (AddressType) Tipo de endereço: 0=Residencial 1=Comercial
        /// </summary>
        public int tipoEndereco { get; set; }

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
        /// Criar o registro do Comprador ou Cliente
        /// </summary>
        /// <returns></returns>
        public Buyer Criar()
        {
            BuyerCategoryEnum[] Categoria = { BuyerCategoryEnum.Normal, BuyerCategoryEnum.Plus };
            DocumentTypeEnum[] Documento = { DocumentTypeEnum.CPF, DocumentTypeEnum.CNPJ };
            EmailTypeEnum[] Email = { EmailTypeEnum.Personal, EmailTypeEnum.Comercial };
            GenderEnum[] Sexo = { GenderEnum.M, GenderEnum.F };
            PersonTypeEnum[] Pessoa = { PersonTypeEnum.Person, PersonTypeEnum.Company };
            AddressTypeEnum[] Endereco = { AddressTypeEnum.Residential, AddressTypeEnum.Comercial };

            // Cria o comprador.
            Buyer buyer = new Buyer()
            {
                AddressCollection = new Collection<BuyerAddress>(),
                Birthdate = this.dataNascimento, //new DateTime(1990, 8, 20),
                BuyerCategory = Categoria[this.categoria],
                BuyerReference = "C3PO",
                CreateDateInMerchant = this.dataCadastroNaLoja, //DateTime.Now,
                DocumentNumber = this.numeroDocumento,
                DocumentType = Documento[this.tipoDocumento],
                Email = this.email,
                EmailType = Email[this.tipoEmail],
                FacebookId = this.contaFacebook,
                Gender = Sexo[this.sexo],
                HomePhone = this.foneResidencial,
                MobilePhone = this.foneMovel,
                Name = this.nome,
                PersonType = Pessoa[this.tipoPessoa],
                TwitterId = this.contaTwitter,
                WorkPhone = this.foneComercial
            };

            // Adiciona um endereço para o comprador.
            buyer.AddressCollection.Add(new BuyerAddress()
            {
                AddressType = Endereco[this.tipoEndereco],
                City = this.cidade,
                Complement = this.complemento,
                Country = CountryEnum.Brazil.ToString(),
                District = this.bairro,
                Number = this.numero,
                State = this.UF,
                Street = this.endereco,
                ZipCode = this.CEP
            });

            return buyer;
        }
    }
}
