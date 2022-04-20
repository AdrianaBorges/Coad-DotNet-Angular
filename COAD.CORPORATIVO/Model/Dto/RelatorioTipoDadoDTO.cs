using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_TIPO_DADO))]
    public class RelatorioTipoDadoDTO
    {
        public RelatorioTipoDadoDTO()
        {
            this.RELATORIO_CONDICAO = new HashSet<RelatorioCondicaoDTO>();
        }
    
        public int? RTV_ID { get; set; }
        public string RTV_DESCRICAO { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioCondicaoDTO> RELATORIO_CONDICAO { get; set; }
    }
}
