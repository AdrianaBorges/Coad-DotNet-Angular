using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Trunca uma string baseado no tamanho máximo informado (maxLength).
        /// Se a string exceder esse limite o restante é removido.
        /// </summary>
        /// <param name="maxLength">Tamanho máximo permitido pela string para ela não ser truncada</param>
        /// <param name="truncateReverse">Indica se a string será truncada do começo para o fim ou do fim para o começo</param>
        /// <returns></returns>
        public static string Truncate(this String val, int maxLength, bool truncateReverse = false)
        {
            val = val.Trim();

            if (val.Count() > maxLength)
            {
                if (!truncateReverse)
                {
                    val = val.Substring(0, maxLength);
                }
                else
                {
                    var stringLength = val.Count();
                    int startIndex = stringLength - maxLength;

                    val = val.Substring(startIndex, maxLength);
                }
            }

            return val;
        }
    }
}
