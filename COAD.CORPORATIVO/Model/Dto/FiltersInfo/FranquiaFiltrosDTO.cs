using GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Models.Filtros;

namespace COAD.CORPORATIVO.Model.Dto.FiltersInfo
{
    [QueryFilter(typeof(FranquiaDTO))]
    public class FranquiaFiltrosDTO
    {

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterTexto("FRA_ID", "Código")]
        [QueryFilterOrdem(0)]
        public string fraID { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterTexto("FRA_NOME", "Nome")]
        [QueryFilterOrdem(1)]
        public string Nome { get; set; }

        [QueryFilterAgrupamento("Região")]
        [QueryFilterSelect(typeof(RegiaoDTO), "RG_ID", "RG_DESCRICAO", "RG_ID", "Região")]
        [QueryFilterOrdem(2)]
        public int? rgID { get; set; }
    }
}
