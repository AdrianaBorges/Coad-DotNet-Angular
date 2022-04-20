using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Util
{
    public static class DataUtil
    {
        /// <summary>
        /// Verifica se o primeiro parâmetro (param1) é nulo. Se for nulo retorna o segundo parametro (otherwise).
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="param1"></param>
        /// <param name="otherwise"></param>
        /// <returns></returns>
        public static TSource ReturnNotNull<TSource>(TSource param1, TSource otherwise)
        {
            if (param1 is String)
            {
                return (!string.IsNullOrWhiteSpace(param1 as string)) ? param1 : otherwise;
            }
            
            return (param1 != null) ? param1 : otherwise;
        }
    }
}
