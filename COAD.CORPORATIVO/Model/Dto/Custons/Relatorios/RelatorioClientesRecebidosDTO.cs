using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelatorioClientesRecebidosDTO
    {
        public int CLI_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string REP_NOME { get; set; }
        public string REP_NOME_DEMANDANTE { get; set; }
        public DateTime? DATA { get; set; }
        public int? RG_ID { get; set; }
        public string RG_DESCRICAO { get; set; }
        public int? RG_ID_REP_DEMANDANTE { get; set; }
        public string RG_DESCRICAO_DEMANDANTE { get; set; } 
        
    }
}
