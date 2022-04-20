using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.Filtros
{
    public class FiltroGrupoDTO
    {
        public FiltroGrupoDTO()
        {
            this.filtros = new HashSet<FiltroDTO>();
        }
        public string idGrupo { get; set; }
        public string nomeGrupo { get; set; }
        public FiltroDTO queryFilter { get; set; }

        public ICollection<FiltroDTO> filtros { get; set; }
    }
}
