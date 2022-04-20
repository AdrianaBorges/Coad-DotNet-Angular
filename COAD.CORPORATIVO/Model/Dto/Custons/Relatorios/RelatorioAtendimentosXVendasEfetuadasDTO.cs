using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelatorioAtendimentosXVendasEfetuadasDTO
    {
        public string RG_DESCRICAO { get; set; }
        public int QTD_ATENDIMENTOS { get; set; } 
        public int QTD_VENDAS_REALIZADAS { get; set; }
    }
}
