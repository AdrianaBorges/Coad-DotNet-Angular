using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_OPERADOR_LOGICO))]
    public class RelatorioOperadorLogicoDTO
    {
        public RelatorioOperadorLogicoDTO()
        {
            this.RELATORIO_CONDICAO = new HashSet<RelatorioCondicaoDTO>();
        }
    
        public int? ROL_ID { get; set; }
        public string ROL_DESCRICAO { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioCondicaoDTO> RELATORIO_CONDICAO { get; set; }
    
    }
}
