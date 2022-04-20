using GenericCrud.Models;
using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.FiltersInfo
{
    [QueryFilter(typeof(PedidoCRMDTO), Name = "ProPendAut")]
    public class PesquisaProPendAutDTO
    {
        public PesquisaProPendAutDTO()
        {
            pagina = 1;
            registrosPorPagina = 15;
        }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterQuery]
        public string query { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterSelect(typeof(UENDTO), "UEN_ID", "UEN_DESCRICAO", "UEN_ID", "UEN")]
        [QueryFilterOrdem(0)]
        public int? UEN_ID { get; set; }
                
        public int pagina { get; set; } 
        public int registrosPorPagina { get; set; }
        public RequisicaoPaginacao requisicao { get; set; }
    }
}
