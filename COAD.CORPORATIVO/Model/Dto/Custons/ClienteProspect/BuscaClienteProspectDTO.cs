using Coad.GenericCrud.Dao.Base.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect
{
    public class BuscaClienteProspectDTO
    {
        public Pagina<BuscarClienteDTO> ListaClientes { get; set; }
        public Pagina<BuscarClienteDTO> ListaProspect { get; set; }
    }
}
