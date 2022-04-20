using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class MontagemQueryTabelasDTO : IQueryFromExpression, IEquatable<MontagemQueryTabelasDTO>
    {
        public int? RET_ID { get; set; }
        public string Schema { get; set; }
        public string NomeTabela { get; set; }
        public string Alias { get; set; }

        public string GerarExpressao()
        {
            string expressao =  "\t[{0}].[{1}] [{2}]";
            expressao = string.Format(expressao, Schema, NomeTabela, Alias);

            return expressao;
        }

        public bool Equals(MontagemQueryTabelasDTO other)
        {
            if (other == null)
                return false;
            bool result = true;

            result = result && ((this.Schema == null && other.Schema == null) || (this.Schema != null && this.Schema.Equals(other.Schema)));
            result = result && ((this.Alias == null && other.Alias == null) || (this.Alias != null && this.Alias.Equals(other.Alias)));
            result = result && ((this.NomeTabela == null && other.NomeTabela == null) || (this.NomeTabela != null && this.NomeTabela.Equals(other.NomeTabela)));

            return result;
        }
    }
}
