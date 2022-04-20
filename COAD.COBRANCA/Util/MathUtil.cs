using COAD.COBRANCA.Bancos.Model.Constants;
using COAD.COBRANCA.Bancos.Model.DTO;
using GenericCrud.Util;
using System;
using System.Linq;

namespace COAD.COBRANCA.Util
{
    public static class MathUtil
    {

        /// <summary>
        /// Calcula o módulo de um valor baseado nos parametros passados.
        /// A multiplicação dos caracteres são feitas em ordem crescente a partir de 2.
        /// Ex: Base 9. O multiplicador inicia em 2 depois vai incrementando até 9 que é o limite. Depois o múltiplicador volta para 2 e assim por diante.
        /// </summary>
        /// <param name="valorBase"></param>
        /// <param name="numeroCaracteres"></param>
        /// <param name="modulo"></param>
        /// <param name="base"></param>
        /// <returns></returns>
        public static int? CalcularModuloCrescente(string valorBase, int numeroCaracteres, int modulo, int @base, int multiplicadorInicial = 2, int? valorMultiplicavel = null)
        {
            
            if (string.IsNullOrWhiteSpace(valorBase))
            {
                throw new ArgumentException($"O código de {numeroCaracteres} dígitos não pode ser nullo ou vazio.");
            }

            if (valorBase.Length != numeroCaracteres)
            {
                var tamanho = valorBase.Length;
                var msg = $"O código passado deve conter {numeroCaracteres} dígitos.  Tamanho do código passado ({tamanho}).";

                throw new ArgumentException(msg);
            }

            char[] arrayChar = valorBase.ToCharArray();
            int length = arrayChar.Length;

            int multiplicador = multiplicadorInicial;
            int somatorio = 0;

            for (var index = (length - 1); index >= 0; index--)
            {
                int valorInteiro = int.Parse(arrayChar[index].ToString());
                int valorMultiplicado = valorInteiro * multiplicador;

                somatorio += valorMultiplicado;

                multiplicador++;
                if (multiplicador > @base)
                {
                    multiplicador = 2;
                }
            }

            if(valorMultiplicavel != null &&
                valorMultiplicavel > 0)
            {
                somatorio *= (int)valorMultiplicavel;
            }

            var moduloResult = somatorio % modulo;
            return moduloResult;
        }


        /// <summary>
        /// Calcula o módulo de um valor baseado nos parametros passados.
        /// A multiplicação dos caracteres são feitas em ordem decrecente a partir de 2.
        /// Ex: Base 9. O multiplicador inicia em 2 depois vai para 9 e vai decrementando a cada iteração até voltar e ser 2 novamente.
        /// </summary>
        /// <param name="valorBase"></param>
        /// <param name="numeroCaracteres"></param>
        /// <param name="modulo"></param>
        /// <param name="base"></param>
        /// <returns></returns>
        public static int? CalcularModuloDecrescente(string valorBase, int numeroCaracteres, int modulo, int @base, int multiplicadorInicial = 2)
        {

            if (string.IsNullOrWhiteSpace(valorBase))
            {
                throw new ArgumentException($"O código de {numeroCaracteres} dígitos não pode ser nullo ou vazio.");
            }

            if (valorBase.Length != numeroCaracteres)
            {
                var tamanho = valorBase.Length;
                var msg = $"O código passado deve conter {numeroCaracteres} dígitos.  Tamanho do código passado ({tamanho}).";

                throw new ArgumentException(msg);
            }

            char[] arrayChar = valorBase.ToCharArray();
            int length = arrayChar.Length;

            int multiplicador = multiplicadorInicial;
            int somatorio = 0;

            for (var index = 0; index < length; index++)
            {
                int valorInteiro = int.Parse(arrayChar[index].ToString());
                int valorMultiplicado = valorInteiro * multiplicador;

                somatorio += valorMultiplicado;

                multiplicador--;
                if (multiplicador == 1)
                {
                    multiplicador = @base;
                }
            }

            var moduloResult = somatorio % modulo;
            return moduloResult;
        }

        public static int? CalcularModulo10(string valorBase/*, int numeroCaracteres*/)
        {
            //if (string.IsNullOrWhiteSpace(valorBase))
            //{
            //    throw new ArgumentException($"O código de {numeroCaracteres} dígitos não pode ser nullo ou vazio.");
            //}

            //if (valorBase.Length != numeroCaracteres)
            //{
            //    var tamanho = valorBase.Length;
            //    var msg = $"O código passado deve conter {numeroCaracteres} dígitos.  Tamanho do código passado ({tamanho}).";

            //    throw new ArgumentException(msg);
            //}

            char[] arrayChar = valorBase.ToCharArray();
            int length = arrayChar.Length;

            int multiplicador = 2;
            int somatorio = 0;

            for (var index = (length - 1); index >= 0; index--)
            {
                int valorInteiro = int.Parse(arrayChar[index].ToString());
                int valorMultiplicado = valorInteiro * multiplicador;

                if(valorMultiplicado > 9)
                {
                    var valorMultiCharArray = valorMultiplicado.ToString().ToCharArray();
                    var soma = 0;
                    foreach (var numero in valorMultiCharArray)
                    {
                        soma += int.Parse(numero.ToString());
                    }
                    valorMultiplicado = soma;
                }

                somatorio += valorMultiplicado;

                multiplicador--;
                if (multiplicador < 1)
                {
                    multiplicador = 2;
                }
            }

            var valor = 10;
            while(valor < somatorio)
            {
                valor += 10;
            }

            var moduloResult = valor - somatorio;
            return moduloResult;
        }


        public static void CalcularFatorVencimento(CodigoBarrasDTO linhaDigitavel)
        {
            if (linhaDigitavel != null && linhaDigitavel.DataVencimento != null)
            {
                var dataBase = BoletoConstants.DATA_BASE;
                var data = linhaDigitavel.DataVencimento;

                if (data != null && dataBase != null)
                {
                    var timespam = data.Value - dataBase;
                    if (timespam != null)
                    {
                        var dias = timespam.TotalDays;
                        var diasStr = dias.ToString().Split('.').First();
                        var fatorVencimento = StringUtil.PreencherZeroEsquerda(diasStr, 4);

                        linhaDigitavel.FatorVencimento = fatorVencimento;
                    }
                }
            }
        }
    }
}
