using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TIPO_JOIN))]
    public class TipoJoinDTO
    {
        public TipoJoinDTO()
        {
            this.RELATORIO_JOIN = new HashSet<RelatorioJoinDTO>();
        }
    
        public int TPJ_ID { get; set; }
        public string TPJ_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RelatorioJoinDTO> RELATORIO_JOIN { get; set; }
    }
}
