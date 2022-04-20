using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    [Mapping(Source = typeof(CO_TIPOS), confRef = "portal")]
    public class CoTiposDTO
    {
        public CoTiposDTO()
        {
            this.CO_OBRIGACOES = new HashSet<CoObrigacoesDTO>();
        }
    
        public string COD_TIPO { get; set; }
        public string NOME_TIPO { get; set; }
        public Nullable<short> HABILITADO { get; set; }

        public virtual ICollection<CoObrigacoesDTO> CO_OBRIGACOES { get; set; }
    }
}
