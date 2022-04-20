using GenericCrud.Models;
using GenericCrud.Models.Filtros;


namespace COAD.SEGURANCA.Model.FiltersInfo
{

    [QueryFilter(typeof(ContaFiltrosDTO))]
    public class ContaFiltrosDTO
    {
        public ContaFiltrosDTO()
        {
            pagina = 1;
            registrosPorPagina = 10;
        }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterQuery]
        public string query { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterSelect(typeof(EmpresaModel), "EMP_ID", "EMP_RAZAO_SOCIAL", "EMP_CNPJ", "EMP_CNPJ")]
        [QueryFilterOrdem(0)]
        public int? EMP_ID { get; set; }

        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
        public RequisicaoPaginacao requisicao { get; set; }
    }
}
