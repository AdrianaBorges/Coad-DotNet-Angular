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
    /// ALT: 11/10/2016 - É a própria empresa, em nosso caso, a COAD.
    /// </summary>
    public class CedenteDTO : Cedente
    {
        public CedenteDTO() { }
        public CedenteDTO(ContaBancaria contaBancaria) { base.ContaBancaria = contaBancaria; }
        public CedenteDTO(string cpfcnpj, string nome) { base.CPFCNPJ = cpfcnpj; base.Nome = nome; }
        public CedenteDTO(string cpfcnpj, string nome, string agencia, string conta) { base.CPFCNPJ = cpfcnpj; base.Nome = nome; base.ContaBancaria.Agencia = agencia; base.ContaBancaria.Conta = conta; }
        public CedenteDTO(string cpfcnpj, string nome, string agencia, string conta, string digitoConta) 
        { 
            base.CPFCNPJ = cpfcnpj; 
            base.Nome = nome;
            base.ContaBancaria = new ContaBancaria();
            base.ContaBancaria.Agencia = agencia; 
            base.ContaBancaria.Conta = conta; 
            base.ContaBancaria.DigitoConta = digitoConta; 
        }
        public CedenteDTO(string cpfcnpj, string nome, string agencia, string digitoAgencia, string conta, string digitoConta) 
        { 
            base.CPFCNPJ = cpfcnpj; 
            base.Nome = nome;
            base.ContaBancaria = new ContaBancaria();
            base.ContaBancaria.Agencia = agencia; 
            base.ContaBancaria.DigitoAgencia = digitoAgencia; 
            base.ContaBancaria.Conta = conta; 
            base.ContaBancaria.DigitoConta = digitoConta; 
        }
        public CedenteDTO(string cpfcnpj, string nome, string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta)
        {
            base.CPFCNPJ = cpfcnpj;
            base.Nome = nome;
            base.ContaBancaria = new ContaBancaria();
            base.ContaBancaria.Agencia = agencia;
            base.ContaBancaria.DigitoAgencia = digitoAgencia;
            base.ContaBancaria.Conta = conta;
            base.ContaBancaria.DigitoConta = digitoConta;
            base.ContaBancaria.OperacaConta = operacaoConta;
        }
    }
}
