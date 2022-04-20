using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GenericCrud.Util
{

    public class SubstituiRegex
    {
        public SubstituiRegex() { }
        public SubstituiRegex(string _regex, string _subst) {
            this.regex = _regex;
            this.subst = _subst;
        }

        public string regex { get; set; }
        public string subst { get; set; }
    }

    public static class StringUtil
    {
        /// <summary>
        /// Trunca uma string baseado no tamanho máximo informado (maxLength).
        /// Se a string exceder esse limite o restante é removido.
        /// </summary>
        /// <param name="val">Valor string</param>
        /// <param name="maxLength">Tamanho máximo permitido pela string para ela não ser truncada</param>
        /// <param name="truncateReverse">Indica se a string será truncada do começo para o fim ou do fim para o começo</param>
        /// <returns></returns>
        public static string Truncate(String val, int maxLength, bool truncateReverse = false)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                val = val.Truncate(maxLength, truncateReverse);
            }
            return val;
        }
        
        public static string PreencherZeroEsquerda(int value, int length)
        {
            int tamanhoCodigo = value.ToString().Length;
            int diferenca = length - tamanhoCodigo;

            int nZeros = tamanhoCodigo + diferenca;
            return value.ToString("D" + nZeros.ToString());

        }

        public static string PreencherZeroEsquerda(string strValue, int length)
        {
            if (!string.IsNullOrWhiteSpace(strValue))
            {   
                long value = 0;
                long.TryParse(strValue, out value);
                int tamanhoCodigo = value.ToString().Length;
                int diferenca = length - tamanhoCodigo;

                int nZeros = tamanhoCodigo + diferenca;
                return value.ToString("D" + nZeros.ToString());
            }
            return strValue;

        }

        public static string LimparAcentuacao(string strValue)
        {
            if (!string.IsNullOrWhiteSpace(strValue))
            {
                byte[] tempBytes;
                tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(strValue);
                string strSemAcento = System.Text.Encoding.UTF8.GetString(tempBytes);
                return strSemAcento;
            }
            return null;
        }

        public static string FormatarDinheiro(decimal? valor, bool mostrarSimboloMoeda = true, bool removerPonto = true)
        {
            if (valor != null)
            {
                CultureInfo culturaBrasileira = new CultureInfo("pt-BR");

                string valorFormatado = ((decimal) valor).ToString("C", culturaBrasileira);

                if (removerPonto)
                {
                    valorFormatado = valorFormatado.Replace(".", "");
                }
                
                if(valorFormatado != null && mostrarSimboloMoeda == false)
                {
                    valorFormatado = valorFormatado.Replace("R$", "");
                }
                return valorFormatado;
            }
            return null;

        }

        public static void SepararTelefoneDoDDD(string strTelefone, out string ddd, out string telefone)
        {
            ddd = null;
            telefone = null;

            if (!string.IsNullOrWhiteSpace(strTelefone))
            {
                Regex rgx = new Regex(@"\D"); // mantém apenas os números
                Regex rgx2 = new Regex(@"^0+"); // retira os zeros a esquerda
                
                strTelefone = rgx.Replace(strTelefone, "");
                strTelefone = rgx2.Replace(strTelefone, "");

                ddd = strTelefone.Substring(0, 2);
                telefone = strTelefone.Substring(2, strTelefone.Length - 2);

            }
        }
        public static string RetirarCaractereEspecial(string texto, bool retirarespaco = false, bool upperCase = true)
        {
            if (!string.IsNullOrWhiteSpace(texto))            {

                List<SubstituiRegex> lista = new List<SubstituiRegex>();
                lista.Add(new SubstituiRegex("[ÓÒÕÔ]","O"));
                lista.Add(new SubstituiRegex("(&)","e"));
                lista.Add(new SubstituiRegex("[°º]", " "));
                lista.Add(new SubstituiRegex("ª","a"));
                lista.Add(new SubstituiRegex("[áàãâ]","a"));
                lista.Add(new SubstituiRegex("[ÁÀÃÂ]","A"));
                lista.Add(new SubstituiRegex("[éèêë]","e"));
                lista.Add(new SubstituiRegex("[ÉÈÊË]","E"));
                lista.Add(new SubstituiRegex("[íìïî]","i"));
                lista.Add(new SubstituiRegex("[ÍÌÎÏ]","I"));
                lista.Add(new SubstituiRegex("[óòõô]","o"));
                lista.Add(new SubstituiRegex("[ÓÒÕÔ]","O"));
                lista.Add(new SubstituiRegex("[úùûü]","u"));
                lista.Add(new SubstituiRegex("[ÚÙÛÜ]", "U"));
                lista.Add(new SubstituiRegex("[çÇ]", "C"));
                lista.Add(new SubstituiRegex("[Ññ]", "C"));
                if (retirarespaco)
                    lista.Add(new SubstituiRegex("[-_]", ""));
                else
                    lista.Add(new SubstituiRegex("[-_]", " "));

                lista.Add(new SubstituiRegex(@"([^\w\d\s,\/\-\\\.])"," "));
                lista.Add(new SubstituiRegex(@"[\/\\\^=#$%¨&*+§!@\{\}\[\]]"," "));
            
                foreach (var item in lista)
                {
                    if(upperCase)
                    {
                        texto = texto.ToUpper();
                    }
                    texto = Regex.Replace(texto, item.regex, item.subst);
                }

                return texto;
            }

            return texto;
        }

        public static string RetirarCaractereEspecialComTrim(string texto, bool retirarespaco = false, bool upperCase = true)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                texto = RetirarCaractereEspecial(texto, retirarespaco, upperCase);
                texto = texto.Trim();

                if (!string.IsNullOrWhiteSpace(texto))
                    return texto;
            }

            return null;
        }

        public static bool ValidarCPF(string cpf)
        {
            if(!string.IsNullOrEmpty(cpf) && cpf.Length == 11)
            {
                var primeirosDigitos = StringUtil.Truncate(cpf, 9);
                char[] digitos = primeirosDigitos.ToCharArray();
                int primeiroVerificador = int.Parse(cpf.Substring(9,1));
                int segundoVerificador = int.Parse(cpf.Substring(10,1));
                int multiplicador = 10;
                int resultado = 0;
                int ResultadoPrimeiroVerificador = 0;
                int ResultadoSegundoVerificador = 0;

                foreach(var digito in digitos)
                {
                    int digitoInt = int.Parse(digito.ToString());
                    resultado += digitoInt * multiplicador--;                    
                }

                ResultadoPrimeiroVerificador = (resultado * 10) % 11;
                if (ResultadoPrimeiroVerificador == 10)
                    ResultadoPrimeiroVerificador = 0;

                if (primeiroVerificador != ResultadoPrimeiroVerificador)
                    return false;

                primeirosDigitos += primeiroVerificador;
                digitos = primeirosDigitos.ToCharArray();
                multiplicador = 11;
                resultado = 0;

                foreach(var digito in digitos)
                {
                    int digitoInt = int.Parse(digito.ToString());
                    resultado += digitoInt * multiplicador--;
                }

                ResultadoSegundoVerificador = (resultado * 10) % 11;
                if(ResultadoSegundoVerificador == 10)
                    ResultadoSegundoVerificador = 0;

                if (segundoVerificador != ResultadoSegundoVerificador)
                    return false;

                return true;
            }

            return false;
        }

        public static bool ValidarCNPJ(string cnpj)
        {
            if (!string.IsNullOrEmpty(cnpj) && cnpj.Length == 14)
            {
                var primeirosDigitos = StringUtil.Truncate(cnpj, 12);
                char[] digitos = primeirosDigitos.ToCharArray();
                int primeiroVerificador = int.Parse(cnpj.Substring(12, 1));
                int segundoVerificador = int.Parse(cnpj.Substring(13, 1));
                int multiplicador = 5;
                int resultado = 0;
                int ResultadoPrimeiroVerificador = 0;
                int ResultadoSegundoVerificador = 0;

                foreach (var digito in digitos)
                {
                    int digitoInt = int.Parse(digito.ToString());
                    resultado += digitoInt * multiplicador--;
                    if (multiplicador < 2)
                        multiplicador = 9;
                }

                ResultadoPrimeiroVerificador = resultado % 11;
                if (ResultadoPrimeiroVerificador < 2)
                    ResultadoPrimeiroVerificador = 0;
                else
                    ResultadoPrimeiroVerificador = 11 - ResultadoPrimeiroVerificador;

                if (primeiroVerificador != ResultadoPrimeiroVerificador)
                    return false;

                primeirosDigitos += primeiroVerificador;
                digitos = primeirosDigitos.ToCharArray();
                multiplicador = 6;
                resultado = 0;

                foreach (var digito in digitos)
                {
                    int digitoInt = int.Parse(digito.ToString());
                    resultado += digitoInt * multiplicador--;
                    if (multiplicador < 2)
                        multiplicador = 9;
                }

                ResultadoSegundoVerificador = resultado % 11;
                if (ResultadoSegundoVerificador < 2)
                    ResultadoSegundoVerificador = 0;
                else
                    ResultadoSegundoVerificador = 11 - ResultadoSegundoVerificador;

                if (segundoVerificador != ResultadoSegundoVerificador)
                    return false;

                return true;
            }

            return false;
        }
    }
}
