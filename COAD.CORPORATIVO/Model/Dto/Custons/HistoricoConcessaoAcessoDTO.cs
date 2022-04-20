using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class HistoricoConcessaoAcessoDTO
    {
        public string SemanticaAcao { get; set; }
        public int? QtdConsulta { get; set; }
        public int? CodProduto { get; set; }
        public int? PpiId { get; set; }
        public int? IpeId { get; set; }
        public int? CliId { get; set; }
        public string Usuario { get; set; }
        public int? AcaId { get; set; }
        public int? PstId { get; set; }
    }
}
