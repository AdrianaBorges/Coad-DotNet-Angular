using GenericCrud.Models;
using GenericCrud.Models.Filtros;


namespace COAD.SEGURANCA.Model.FiltersInfo
{
 
    [QueryFilter(typeof(EmpresaFiltrosDTO))]
    public class EmpresaFiltrosDTO
    {
        public EmpresaFiltrosDTO()
        {
            pagina = 1;
            registrosPorPagina = 10;
        }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterQuery]
        public string query { get; set; }

        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
        public RequisicaoPaginacao requisicao { get; set; }
    }
}
