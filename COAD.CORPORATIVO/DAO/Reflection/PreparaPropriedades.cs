using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO.Reflection
{
    public static class PreparaPropriedades
    {
        /// <summary>
        /// Lendo as propriedades de T
        /// </summary>
        public static IEnumerable<PropertyInfo> LerPropriedades<T>(BindingFlags binding, DefinirOpcoesDePropriedades options = DefinirOpcoesDePropriedades.All)
        {
            var properties = typeof(T).GetProperties(binding);

            bool all = (options & DefinirOpcoesDePropriedades.All) != 0;
            bool ignoreIndexer = (options & DefinirOpcoesDePropriedades.IgnoreIndexer) != 0;
            bool ignoreEnumerable = (options & DefinirOpcoesDePropriedades.IgnoreEnumerable) != 0;

            foreach (var property in properties)
            {
                if (!all)
                {
                    if (ignoreIndexer && ChecarIndexacao(property))
                    {
                        continue;
                    }

                    if (ignoreIndexer && !property.PropertyType.Equals(typeof(string)) && ChecarLista(property))
                    {
                        continue;
                    }
                }

                yield return property;
            }
        }

        /// <summary>
        /// Checa se a propriedade possui um índice
        /// </summary>
        private static bool ChecarIndexacao(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            if (parameters != null && parameters.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checa se a propriedade possui uma lista
        /// </summary>
        private static bool ChecarLista(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Any(x => x.Equals(typeof(System.Collections.IEnumerable)));
        }
    }

    [Flags]
    public enum DefinirOpcoesDePropriedades : int
    {
        /// <summary>
        /// Todos
        /// </summary>
        All = 0,

        /// <summary>
        /// Ignorar os índices da propriedade
        /// </summary>
        IgnoreIndexer = 1,

        /// <summary>
        /// Ignorar todas as propriedades, exceto strings
        /// </summary>
        IgnoreEnumerable = 2
    }
}
