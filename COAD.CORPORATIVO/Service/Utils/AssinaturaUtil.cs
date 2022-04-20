using Coad.GenericCrud.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Utils
{
    /// <summary>
    /// Utilidades para Assinatura
    /// </summary>
    public static class AssinaturaUtil
    {
        private static Dictionary<int, char> letras = new Dictionary<int, char>()
        {
           {0, 'A' },
           {1, 'B' },
           {2, 'C' },
           {3, 'D' },
           {4, 'E' },
           {5, 'F' },
           {6, 'G' },
           {7, 'H' },
           {8, 'I' },
           {9, 'J' },
           {10, 'K' },
           {11, 'L' },
           {12, 'M' },
           {13, 'N' },
           {14, 'O' },
           {15, 'P' },
           {16, 'Q' },
           {17, 'R' },
           {18, 'S' },
           {29, 'T' },
           {20, 'U' },
           {21, 'V' },
           {22, 'W' },
           {23, 'X' },
           {24, 'Y' },
           {25, 'Z' },
        };

        /// <summary>
        /// Pega a letra do mês a partir do ano
        /// </summary>
        /// <param name="ano"></param>
        /// <returns></returns>
        public static char GetLetraFromMes(int mes)
        {
            mes--;
            if (mes > 12)
            {
                throw new ValidacaoException("Não é possível obter uma letra. O mês não pode ser maior que 12");
            }
            return letras[mes];
        }

        /// <summary>
        /// Pega a letra do mês a partir do ano
        /// </summary>
        /// <param name="ano"></param>
        /// <returns></returns>
        public static char GetLetraDoAlfabeto(int numero)
        {
            if (numero > 25)
            {
                throw new ValidacaoException("Não é possível obter uma letra. O número não pode ser maior que 25");
            }
            return letras[numero];
        }

        public static string CriaDigito(string codigoAssinatura)
        {
            
            if (codigoAssinatura != null)
            {
               // Random rd = new Random();
               //int digito = rd.Next(0, 9);

               var codigoAssi = codigoAssinatura.ToString() + 9.ToString();
               return codigoAssi; 
            }
            return null;
        }

        public static char GetProximaLetra(char letra)
        {
            var key = letras.Where(x => x.Value == letra).Select(sel => sel.Key).FirstOrDefault();

            if (key != null)
            {
                key++;
                return letras[key];
            }
            return ' ';
        }

        public static int GetNumeroDaLetra(char letra)
        {
            var key = letras.Where(x => x.Value == letra).Select(sel => sel.Key).FirstOrDefault();
            return key;
        }

        public static int GetNumeroDaLetraIndice1(char letra)
        {
            var key = letras.Where(x => x.Value == letra).Select(sel => sel.Key).FirstOrDefault();
            return ++key;
        }

        public static int? ExtrairMesDaAssinatura(string numAssinatura)
        {
            if (!string.IsNullOrWhiteSpace(numAssinatura) && numAssinatura.Count() >= 3)
            {
                var letra = numAssinatura.Substring(2, 1).ToCharArray().FirstOrDefault();
                var mes = GetNumeroDaLetraIndice1(letra);
                return mes;
            }
            return null;
            
        }
    }
}
