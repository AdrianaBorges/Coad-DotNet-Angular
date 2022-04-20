using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class MontagemQueryJoinDTO : IQueryFromExpression
    {
        public MontagemQueryTabelasDTO Tabela1 { get; set; }
        public MontagemQueryTabelasDTO Tabela2 { get; set; }

        public bool GenerateTab1 { get; set; }
        public bool GenerateTab2 { get; set; }
        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Tipo { get; set; }

        public string GerarExpressao()
        {
            if(Tabela1 != null && Tabela2 != null){

                if(string.IsNullOrEmpty(Tipo))
                    Tipo = "INNER"; 

                StringBuilder sb = new StringBuilder();

                if(GenerateTab1)
                    sb.Append(Tabela1.GerarExpressao());
                sb.Append("  ");
                sb.Append(Tipo);
                sb.Append(" JOIN ");
                sb.Append(" \r\n  ");
                
                if(GenerateTab2)
                    sb.Append(Tabela2.GerarExpressao());
                sb.Append(" ON [");

                sb.Append(Tabela1.Alias);
                sb.Append("].[");
                sb.Append(Campo1);
                sb.Append("] = [");
                sb.Append(Tabela2.Alias);
                sb.Append("].[");
                sb.Append(Campo2);
                sb.Append("] ");
                //sb.Append("\r\n");

                return sb.ToString();
            }

            return null;
        }
    }
}
