using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    [Mapping(Source = typeof(CO_AREAS), confRef = "portal")]
    public class CoAreasDTO
    {
            public CoAreasDTO()
            {
                this.CO_OBRIGACOES = new HashSet<CoObrigacoesDTO>();
            }

            public string COD_AREA { get; set; }
            public string NOME_AREA { get; set; }
            public string ABRANGENCIA { get; set; }
            public Nullable<short> HABILITADO { get; set; }

            public virtual ICollection<CoObrigacoesDTO> CO_OBRIGACOES { get; set; }
        }
    
}
