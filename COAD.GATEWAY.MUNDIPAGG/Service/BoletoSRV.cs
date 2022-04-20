using COAD.GATEWAY.MUNDIPAGG.Model;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.GATEWAY.MUNDIPAGG.Service
{
    /// <summary>
    /// ALT: 20/06/2016
    /// Este objeto cria boletos para os bancos [Itau, Santander, Bradesco, Banco do Brasil e HSBC]
    /// <para>ATENÇÃO! Boletos demoram até 2 dias da data do pagamento para serem conciliados.</para>
    /// 
    /// </summary>
    public class BoletoSRV
    {
        public BoletoTransaction Criar(BoletoDTO boleto)
        {
            // Cria o boleto.
            var boletoTransaction = new BoletoTransaction()
            {
                AmountInCents = boleto.valor,
                BankNumber = boleto.banco,
                BillingAddress = new BillingAddress()
                {
                    City = boleto.cidade,
                    Complement = boleto.complemento,
                    Country = CountryEnum.Brazil.ToString(),
                    Number = boleto.numero,
                    District = boleto.bairro,
                    State = boleto.UF,
                    Street = boleto.endereco,
                    ZipCode = boleto.CEP
                },
                DocumentNumber = boleto.numeroDocumento,
                Instructions = boleto.instrucoes,
                Options = new BoletoTransactionOptions()
                {
                    CurrencyIso = CurrencyIsoEnum.BRL,
                    DaysToAddInBoletoExpirationDate = boleto.dias
                },
                TransactionReference = boleto.transacaoInterna
            };

            return boletoTransaction;
        }
    }
}
