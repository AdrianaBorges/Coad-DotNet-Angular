using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    [Mapping(Source = typeof(CO_MUNICIPIOS), confRef = "portal")]
    public class CoMunicipiosDTO
    {
        public CoMunicipiosDTO()
        {
            this.CO_OBRIGACOES = new HashSet<CoObrigacoesDTO>();
        }
    
        public long PK_MUNIC { get; set; }
        public long NUM_UF { get; set; }
        public long NUM_MUNICIPIO { get; set; }
        public string NOME_MUNICIPIO { get; set; }
        public Nullable<short> HABILITADO { get; set; }
    
        public virtual CoEstadosDTO CO_ESTADOS { get; set; }
        public virtual ICollection<CoObrigacoesDTO> CO_OBRIGACOES { get; set; }
    }
}
