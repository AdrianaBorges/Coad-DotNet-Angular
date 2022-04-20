using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Util
{
    public static class MathUtil
    {
        public static string PreencherZeroEsquerda(int value, int lenth)
        {
            int tamanhoCodigo = value.ToString().Length;
            int diferenca = lenth - tamanhoCodigo;

            int nZeros = tamanhoCodigo + diferenca;
            return value.ToString("D" + nZeros.ToString());

        }

        public static int? CalcularDigitoVerificador32Digitos(string codigo32Digitos)
        {
            if (string.IsNullOrWhiteSpace(codigo32Digitos))
            {
                throw new ArgumentException("O código de 32 dígitos não pode ser nullo ou vazio.");
            }

            if (codigo32Digitos.Length != 32)
            {
                var msg = "O código passado deve conter 32 dígitos. Tamanho do código passado ({0}).";
                var tamanho = codigo32Digitos.Length;
                msg = string.Format(msg, tamanho);

                throw new ArgumentException(msg);
            }

            char[] arrayChar = codigo32Digitos.ToCharArray();
            int length = arrayChar.Length;

            int multiplicador = 2;
            int somatorio = 0;

            for (var index = (length - 1); index >= 0; index--)
            {
                int valorInteiro = int.Parse(arrayChar[index].ToString());
                int valorMultiplicado = valorInteiro * multiplicador;

                somatorio += valorMultiplicado;

                multiplicador++;
                if (multiplicador > 9)
                {
                    multiplicador = 2;
                }
            }

            var modulo = somatorio % 11;

            if (modulo == 0 || modulo == 1)
            {
                return 0;
            }

            var digitoVerificador = 11 - modulo;

            if (digitoVerificador > 9)
            {
                throw new Exception("Ocorreu um erro ao gerar o dígito verificador, o resultado obtido possui mais de 1 dígito");
            }

            return digitoVerificador;

        }

        /// <summary>
        /// Corta o número de casas decimals que excedem  quantidade de casas informadas no método.
        /// </summary>
        /// <param name="valorDecimal"></param>
        /// <param name="casasDecimais"></param>
        /// <returns></returns>
        public static decimal? TruncarCasasDecimais(decimal? valorDecimal, double casasDecimais)
        {
            double valorDouble = (double)valorDecimal;
            if (valorDouble != null)
            {
                double target = Math.Pow(10, casasDecimais);
                valorDouble = valorDouble * target;

                var valorFinal = Math.Truncate(valorDouble);
                valorFinal = valorFinal / target;

                return (decimal) valorFinal;
            }

            return null;
        }

    }
}
