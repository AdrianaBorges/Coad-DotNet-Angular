using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoletoNet;

namespace COAD.CORPORATIVO.Model.Dto.Boleto
{
    /// <summary>
    /// ALT: 11/10/2016 - Informações do preenchimento do corpo do boleto.
    /// </summary>
    public class CorpoDTO : BoletoNet.Boleto
    {
        public CorpoDTO() { }
        public CorpoDTO(decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente) 
        {
            base.ValorBoleto = valorBoleto;
            base.Carteira = carteira;
            base.NossoNumero = nossoNumero;
            base.Cedente = cedente;
        }
        public CorpoDTO(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente) 
        {
            base.DataVencimento = dataVencimento;
            base.ValorBoleto = valorBoleto;
            base.Carteira = carteira;
            base.NossoNumero = nossoNumero;
            base.Cedente = cedente;
        }
        public CorpoDTO(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente, EspecieDocumento especieDocumento) 
        {
            base.DataVencimento = dataVencimento;
            base.ValorBoleto = valorBoleto;
            base.Carteira = carteira;
            base.NossoNumero = nossoNumero;
            base.Cedente = cedente;
            base.EspecieDocumento = especieDocumento;
        }
        public CorpoDTO(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, string digitoNossoNumero, Cedente cedente) 
        {
            base.DataVencimento = dataVencimento;
            base.ValorBoleto = valorBoleto;
            base.Carteira = carteira;
            base.NossoNumero = nossoNumero;
            base.DigitoNossoNumero = digitoNossoNumero;
            base.Cedente = cedente;
        }
        public CorpoDTO(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, string agencia, string conta) 
        {
            base.DataVencimento = dataVencimento;
            base.ValorBoleto = valorBoleto;
            base.Carteira = carteira;
            base.NossoNumero = nossoNumero;
            base.ContaBancaria = new ContaBancaria();
            base.ContaBancaria.Agencia = agencia;
            base.ContaBancaria.Conta = conta;
        }
    }
}
