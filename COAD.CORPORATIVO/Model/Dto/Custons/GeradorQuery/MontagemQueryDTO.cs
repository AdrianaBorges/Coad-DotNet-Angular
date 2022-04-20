using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class MontagemQueryDTO
    {
        public string NomeRelatorio { get; set; }
        private IList<MontagemQueryTabelasDTO> _tabelasUtilizadas { get; set; }
        public IList<IQueryFromExpression> From { get; set; }
        public MontagemQuerySelectDTO Select { get; set; }
        public MontagemQueryWhereDTO Where { get; set; }
        public IList<string> OrderBy { get; set; }
        public int? Top { get; set; }
        public MontagemQueryDTO SubConjunto { get; set; }
        public string AliasSubConjunto { get; set; }
        public bool RelatorioDerivado { get; set; }

        public MontagemQueryDTO()
        {
            _tabelasUtilizadas = new List<MontagemQueryTabelasDTO>();
            From = new List<IQueryFromExpression>();
            Select = new MontagemQuerySelectDTO();
            Where = new MontagemQueryWhereDTO();
            OrderBy = new List<string>();
        }

        public void AdicionarTabela(MontagemQueryTabelasDTO tabela)
        {
            if (tabela != null)
            {
                _tabelasUtilizadas.Add(tabela);
                From.Add(tabela);
            }
        }

        public void AdicionarJoin(MontagemQueryTabelasDTO tabela1, 
            MontagemQueryTabelasDTO tabela2, 
            string tipo, 
            string campo1, 
            string campo2)
        {
            bool tab1 = false;
            bool tab2 = false;

            if (!_tabelasUtilizadas.Contains(tabela1))
            {
                tab1 = true;
                _tabelasUtilizadas.Add(tabela1);
            }

            if (!_tabelasUtilizadas.Contains(tabela2))
            {
                tab2 = true;
                _tabelasUtilizadas.Add(tabela2);
            }

            MontagemQueryJoinDTO expressaoJoin = new MontagemQueryJoinDTO()
            {
                GenerateTab1 = tab1,
                GenerateTab2 = tab2,
                Tabela1 = tabela1,
                Tabela2 = tabela2,
                Tipo = tipo,
                Campo1 = campo1,
                Campo2 = campo2
            };

            From.Add(expressaoJoin);
        }

        public void MarcarMontagemComoSubConjunto()
        {
            if (!RelatorioDerivado && 
                Where != null && 
                !Where.SubConjunto)
            {
                Where.SubConjunto = true;
            }
        }

        public override string ToString()
        {
            MarcarMontagemComoSubConjunto();
            StringBuilder sb = new StringBuilder();
               
            sb.Append("\r\nSELECT \r\n ");

            if (Top != null)
            {
                sb.Append("\tTOP ");
                sb.Append(Top);
            }

            sb.Append(Select.GerarExpressao());
            sb.Append("\r\n FROM \r\n");

            int index = 0;
            int count = From.Count();

            if (RelatorioDerivado && SubConjunto != null)
            {
                sb.Append("(");

                var subQuery = SubConjunto.ToString();
                subQuery = new Regex("^(\n*)$")
                    .Replace(subQuery, "    $1");
                
                sb.Append(subQuery);
                sb.Append(") AS [");

                sb.Append(SubConjunto.AliasSubConjunto);
                sb.Append("]");
            }
            else
            {
                foreach (var exp in From)
                {
                    sb.Append(exp.GerarExpressao());
                    index++;
                }
            }

            if (Where != null)
            {
                sb.Append("\r\n WHERE \r\n");
                sb.Append(Where.GerarExpressao());
            }

            return sb.ToString();
            
        }
    }
}
