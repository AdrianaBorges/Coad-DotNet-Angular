using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Model
{
    public class CompradorDTO
    {
        /// <summary>
        /// (BuyerCategory) Comprador da categoria 0=Normal ou 1=Plus
        /// </summary>
        public Nullable<BuyerCategoryEnum> categoria { get; set; }

        /// <summary>
        /// (Birthdate) Data de nascimento do cliente
        /// </summary>
        public Nullable<DateTime> dataNascimento { get; set; }

        /// <summary>
        /// (CreateDateInMerchant) Data do cadastro do cliente na plataforma da loja
        /// </summary>
        public Nullable<DateTime> dataCadastroNaLoja { get; set; }

        /// <summary>
        /// (DocumentNumber) Número do documento (CPF/CNPJ)
        /// </summary>
        public string numeroDocumento { get; set; }

        /// <summary>
        /// (DocumentType) Tipo do documento informado (0=CPF ou 1=CNPJ)
        /// </summary>
        public DocumentTypeEnum tipoDocumento { get; set; }

        /// <summary>
        /// (Email) Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// (EmailType) Email tipo 0=Pessoal ou 1=Comercial
        /// </summary>
        public EmailTypeEnum tipoEmail { get; set; }

        /// <summary>
        /// (FacebookId) Identificação do cliente no Facebook
        /// </summary>
        public string contaFacebook { get; set; }

        /// <summary>
        /// (Gender) Sexo 0=Masculino ou 1=Feminino
        /// </summary>
        public Nullable<GenderEnum> sexo { get; set; }

        /// <summary>
        /// (HomePhone) Telefone residencial do cliente. Padrão obrigatório: (xx)123456789
        /// </summary>
        public string foneResidencial { get; set; }

        /// <summary>
        /// (MobilePhone) Telefone celular do cliente. Padrão obrigatório: (xx)123456789
        /// </summary>
        public string celular { get; set; }

        /// <summary>
        /// (Name) Nome do comprador ou cliente
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// (PersonType) Tipo de pessoa (0=Física ou 1=Jurídica)
        /// </summary>
        public PersonTypeEnum tipoPessoa { get; set; }

        /// <summary>
        /// (TwitterId) Identificação do cliente no Twitter
        /// </summary>
        public string contaTwitter { get; set; }

        /// <summary>
        /// (WorkPhone) Telefone comercial do cliente. Padrão obrigatório: (xx)123456789
        /// </summary>
        public string dddComercial { get; set; }
        public string foneComercial { get; set; }
        public string dddCelular { get; set; }
        public string foneCelular { get; set; }


        /// <summary>
        /// (AddressType) Tipo de endereço: 0=Residencial 1=Comercial
        /// </summary>
        public AddressTypeEnum tipoEndereco { get; set; }

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
        /// AOS CUIDADOS
        /// </summary>
        public string ac { get; set; }


        public Nullable<int> cli_id { get; set; }
        public Nullable<int> cmp_id { get; set; }
        public Nullable<int> munid { get; set; }
        public long valor { get; set; }
        public string senha { get; set; }

    }
}
