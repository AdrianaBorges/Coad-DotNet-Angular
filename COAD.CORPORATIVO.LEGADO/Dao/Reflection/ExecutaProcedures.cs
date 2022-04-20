using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.LEGADO.Dao.Reflection;

namespace COAD.CORPORATIVO.LEGADO.Dao.Reflection
{
    public static class ExecutaProcedures
    {
        /// <summary>
        /// Executa a procedure
        /// </summary>
        public static void ExecutarProcedure<T>(this DbContext context, IEnumerable<T> data, string procedureName, string paramName, string typeName)
        {
            //// convertendo a fonte de dados para procedure
            DataTable table = data.CriarTabela();

            //// criando os parâmetros
            SqlParameter parameter = new SqlParameter(paramName, table);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = typeName;

            //// preparando o sql
            string sql = String.Format("EXEC {0} {1};", procedureName, paramName);

            //// executando o sql via procedure
            context.Database.ExecuteSqlCommand(sql, parameter);
        }
    }
}
