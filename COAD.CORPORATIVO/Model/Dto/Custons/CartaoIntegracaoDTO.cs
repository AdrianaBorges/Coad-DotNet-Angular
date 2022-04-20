using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CartaoIntegracaoDTO
    {

        public Nullable<int> ipeId { get; set; }
        public string ipeHash { get; set; } 

        /// <summary>
        /// (AmountInCents) Valor da transação em centavos. R$ 1,00 = 100
        /// </summary>
        public long valor { get; set; }

        /// <summary>
        /// (CreditCardBrand) Bandeira do cartão do cliente [0=Visa, 1=MasterCard, 2=Hipercard, 3=Amex, 4=Diners, 5=Elo, 6=Aura, 7=Discover, 8=CasaShow, 9=HugCard, 10=AndarAki, 11=Havan, 12=LeaderCard]
        /// </summary>
        public Nullable<CreditCardBrandEnum> bandeira { get; set; }

        /// <summary>
        /// (CreditCardNumber) Número do cartão do cliente. Informar apenas números.
        /// </summary>
        public string numeroCartao { get; set; }

        /// <summary>
        /// (ExpMonth) Mês de expiração do cartão
        /// </summary>
        public int mesExpiracao { get; set; }

        /// <summary>
        /// (ExpYear) Ano de expiração do cartão
        /// </summary>
        public int anoExpiracao { get; set; }

        /// <summary>
        /// (HolderName) Nome do portador do cartão
        /// </summary>
        public string portador { get; set; }

        /// <summary>
        /// (SecurityCode) Código de segurança do cartão
        /// </summary>
        public string codigoSeguranca { get; set; }


        /// <summary>
        /// (PaymentMethodCode) Meio de pagamento que deve ser utilizado para a transação: 3=RedecardKomerci 5=Cielo 18=GetNetSitef 20=Stone 23=Elavon 32=eRede
        /// </summary>
        public int meioDePagamento { get; set; }

        public int erro { get; set; }
        public string mensagem { get; set; }

    }
}
