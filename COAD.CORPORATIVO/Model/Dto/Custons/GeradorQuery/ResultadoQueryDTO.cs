using GenericCrud.Models.SqlDinamico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.GeradorQuery
{
    public class ResultadoQueryDTO
    {
        public ResultadoQueryDTO()
        {
            Colunas = new List<string>();
        }

        public ResultadoQueryDTO(IList<ColunaSqlDinamicoDTO> lstColuna, IEnumerable<object> dados)
        {
            if (lstColuna != null)
            {
                this.Colunas = lstColuna.Select(x => 
                    (!string.IsNullOrEmpty(x.Alias))? x.Alias : x.Nome)
                    .ToList();
            }
            this.Dados = dados;            
        }

        public IList<string> Colunas { get; set; }
        public IEnumerable<object> Dados { get; set; }
    }
}
