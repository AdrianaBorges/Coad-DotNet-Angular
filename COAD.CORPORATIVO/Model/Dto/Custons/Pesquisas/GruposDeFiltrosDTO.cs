using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class GruposDeFiltrosDTO
    {
        public Dictionary<string, IQueryable<GrupoDeFiltroDTO>> Grupos { get; set; }

        public GruposDeFiltrosDTO()
        {
            Grupos = new Dictionary<string, IQueryable<GrupoDeFiltroDTO>>();
        }

        public void AdicionarGrupoDeFiltros(string chave, IQueryable<GrupoDeFiltroDTO> grupo)
        {
            if (Grupos.Keys.Contains(chave))
                throw new Exception(string.Format("Não é possível adicionar o grupo de filtro. A chave {0} já foi adicionada.", chave));

            Grupos.Add(chave, grupo);
        }
    }
}
