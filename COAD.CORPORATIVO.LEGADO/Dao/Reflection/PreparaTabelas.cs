using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao.Reflection
{
    public static class PreparaTabelas
    {
        /// <summary>
        /// Criando a tabela baseada na fonte de dados.
        /// </summary>
        public static DataTable CriarTabela<T>(this IEnumerable<T> source)
        {
            DataTable table = new DataTable();

            //// ler propriedades
            var binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            var options = DefinirOpcoesDePropriedades.IgnoreEnumerable | DefinirOpcoesDePropriedades.IgnoreIndexer;

            var properties = PreparaPropriedades.LerPropriedades<T>(binding, options).ToList();

            //// criar esquema/estrutura baseado nas propriedades
            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType); //property.PropertyType);
            }

            //// criar a tabela
            object[] values = new object[properties.Count];

            foreach (T item in source)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
}
