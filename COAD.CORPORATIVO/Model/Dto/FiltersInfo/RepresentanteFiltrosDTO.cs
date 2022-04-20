using GenericCrud.Models;
using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.FiltersInfo
{
    [QueryFilter(typeof(RepresentanteDTO))]
    public class RepresentanteFiltrosDTO
    {
        public RepresentanteFiltrosDTO()
        {
            pagina = 1;
            registrosPorPagina = 5;
        }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterQuery]
        public string query { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterSelect(typeof(UENDTO), "UEN_ID", "UEN_DESCRICAO", "UEN_ID", "UEN")]
        [QueryFilterOrdem(0)]
        public int? UEN_ID { get; set; }

        [QueryFilterAgrupamento("Padrão")]
        [QueryFilterSelect(typeof(RegiaoDTO), "RG_ID", "RG_DESCRICAO", "RG_ID", "Região")]
        [QueryFilterOrdem(1)]
        public int? rgId { get; set; }

        //[QueryFilterAgrupamento("Padrão")]
        //[QueryFilterTexto("USU_LOGIN", "Login")]
        //[QueryFilterOrdem(5)]
        public string login { get; set; }

        //[QueryFilterAgrupamento("Padrão")]
        //[QueryFilterTexto("USU_NOME", "Nome do Usuário")]
        //[QueryFilterOrdem(6)]
        public string nome { get; set; }

        //[QueryFilterAgrupamento("Padrão")]
        //[QueryFilterTexto("USU_CPF", "CPF")]
        //[QueryFilterOrdem(2)]
        public string cpf { get; set; }

        //[QueryFilterAgrupamento("Padrão")]
        //[QueryFilterTexto("USU_EMAIL", "E-Mail")]
        //[QueryFilterOrdem(3)]
        public string email { get; set; }

        //[QueryFilterAgrupamento("Padrão")]
        //[QueryFilterTexto("REP_NOME", "Representante")]
        //[QueryFilterOrdem(4)]
        public string REP_NOME { get; set; }



        public bool cpfExato { get; set; }
        public int? REP_ID_SUP { get; set; }
        public int pagina { get; set; } 
        public int registrosPorPagina { get; set; }
        public bool naoAssociadosARepresentante { get; set; }
        public RequisicaoPaginacao requisicao { get; set; }
    }
}
