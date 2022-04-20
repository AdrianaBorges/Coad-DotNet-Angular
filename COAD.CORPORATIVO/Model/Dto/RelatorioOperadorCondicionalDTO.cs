using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_OPERADOR_CONDICIONAL))]
    public class RelatorioOperadorCondicionalDTO
    {
        public RelatorioOperadorCondicionalDTO()
        {
            this.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL = new HashSet<RelatorioCondicaoRelatorioOperadorCondicionalDTO>();
        }
    
        public int ROC_ID { get; set; }
        public string ROC_DESCRICAO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioCondicaoRelatorioOperadorCondicionalDTO> RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL { get; set; }
  
    }
}
