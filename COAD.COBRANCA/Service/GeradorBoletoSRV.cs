using COAD.COBRANCA.Config;
using COAD.COBRANCA.Exceptions;
using COAD.COBRANCA.Model.Constants;
using COAD.COBRANCA.Model.DTO;
using COAD.COBRANCA.Model.DTO.Interfaces;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Service
{
    public class GeradorBoletoSRV
    {
        public IBanco Banco { get; set; }
        public ContaDTO Conta { get; set; }

        private void Init()
        {
            ConfigBoleto.Configurar();
        }

        public GeradorBoletoSRV()
        {
            Init();
        }

        public GeradorBoletoSRV(IBanco Banco, ContaDTO Conta)
        {
            Init();
            this.Banco = Banco;
            this.Conta = Conta;
        }

        private string CalcularFatorVencimento(DateTime? data)
        {
            var dataBase = BoletoConstants.DATA_BASE;
            if(data != null && dataBase != null)
            {
                var timespam  = data.Value - dataBase;
                if(timespam != null)
                {
                    var dias = timespam.TotalDays;
                    var fatorVencimento = StringUtil.PreencherZeroEsquerda(dias.ToString(), 4);

                    return fatorVencimento;
                }
            }
            return "0000";
        }

        private IRegrasBoleto RetornarRegras()
        {
            if (Conta == null)
                throw new ConfigException("A Conta não foi informada.");

            if (Banco == null)
                throw new ConfigException("A Conta não foi informada.");

            var regra = ConfigBoletoSRV.RetornarRegra(Banco, Conta.CodigoCarteiraBoleto);
            return regra;
        }

        public string GerarCodigoBarras()
        {
            var regra = RetornarRegras();

            // Campos Padrão
            string identBanco = null;
            string codigoMoeda = null;
            string digitoVerificadorCodigoBarras = null;
            string fatorVencimento = null;
            string valorStr = null;
            double? valor = 2500.96;


            // Campos Livre
            string agenciaBeneficiario = null;
            string carteira = null;
            string nossoNumero = null;
            string conta = null;
            string modCobranca = null;


            identBanco = "000";
            codigoMoeda = "9";
            fatorVencimento = CalcularFatorVencimento(new DateTime(2010, 11, 17));
            valorStr = valor.ToString();
            valorStr = valorStr.Replace(".", "").Replace(",", "");
            valorStr = StringUtil.PreencherZeroEsquerda(valorStr, 10);

            modCobranca = StringUtil.PreencherZeroEsquerda("0", 2);

            agenciaBeneficiario = StringUtil.PreencherZeroEsquerda(Conta.CodigoAgencia, 4);
            carteira = StringUtil.PreencherZeroEsquerda(Conta.CodigoCarteiraBoleto, 2);

            var contaStr = Conta.NumeroConta;

            if (!string.IsNullOrWhiteSpace(contaStr))
            {
                contaStr = contaStr.Split('-').FirstOrDefault();
            }
            conta = StringUtil.PreencherZeroEsquerda(contaStr, 7);
            nossoNumero = "00000000002";
            //if(regra != null)
            //{
            //   
            //    var digitoVerificador = regra.CalcularDigitoNossoNumero(nossoNumero);
            //    var nossoNumeroComDigito =  nossoNumero + digitoVerificador;

            //    return digitoVerificador;
            //}

            if (!string.IsNullOrWhiteSpace(nossoNumero))
            {
                nossoNumero = StringUtil.PreencherZeroEsquerda(nossoNumero, 11);
            }

            var primeiraMetadeCodigoBarra = $@"{identBanco}{codigoMoeda}";
            var segundaMetadeCodigoBarra = $@"{fatorVencimento}{valorStr}{agenciaBeneficiario}{modCobranca}{nossoNumero}{conta}0";

            var codigoBarraProvisorio = primeiraMetadeCodigoBarra + segundaMetadeCodigoBarra;
            digitoVerificadorCodigoBarras = Banco.CalcularDVCodigoBarras(codigoBarraProvisorio);

            var codigoBarras = $@"{primeiraMetadeCodigoBarra}{digitoVerificadorCodigoBarras}{segundaMetadeCodigoBarra}";

            return codigoBarras;
        }
    }
}
