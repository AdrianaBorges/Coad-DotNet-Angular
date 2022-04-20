using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class MontagemQueryWhereItemDTO
    {
        public string OperadorLogico { get; set; }
        public string AliasTabela { get; set; }
        public string NomeColuna { get; set; }
        public IList<string> OperadoresCondicionais { get; set; }
        public object Valor { get; set; }
        public string Label { get; set; }
        public bool? Filtro { get; set; }
        public string NomeTipoDado { get; set; }
    }
}
