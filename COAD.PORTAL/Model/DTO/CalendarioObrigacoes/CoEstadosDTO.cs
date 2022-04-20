using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    [Mapping(Source = typeof(CO_ESTADOS), confRef = "portal")]
    public class CoEstadosDTO
    {
        public CoEstadosDTO()
        {
            this.CO_MUNICIPIOS = new HashSet<CoMunicipiosDTO>();
            this.CO_OBRIGACOES = new HashSet<CoObrigacoesDTO>();
        }
    
        public long NUM_UF { get; set; }
        public string COD_UF { get; set; }
        public string NOME_UF { get; set; }
        public Nullable<short> HABILITADO { get; set; }

        public virtual ICollection<CoMunicipiosDTO> CO_MUNICIPIOS { get; set; }
        public virtual ICollection<CoObrigacoesDTO> CO_OBRIGACOES { get; set; }
    }
}
