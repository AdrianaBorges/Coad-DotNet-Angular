using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service.Utils
{
    /// <summary>
    /// Utilidades para Assinatura
    /// </summary>
    public static class AssinaturaUtil
    {
        private static Dictionary<int, char> letras = new Dictionary<int, char>()
        {
           {1, 'A' },
           {2, 'B' },
           {3, 'C' },
           {4, 'D' },
           {5, 'E' },
           {6, 'F' },
           {7, 'G' },
           {8, 'H' },
           {9, 'I' },
           {10, 'J' },
           {11, 'K' },
           {12, 'L' },
        };

        /// <summary>
        /// Pega a letra do mês a partir do ano
        /// </summary>
        /// <param name="ano"></param>
        /// <returns></returns>
        public static char GetLetraFromMes(int mes)
        {
            return letras[mes];
        }
    }
}
