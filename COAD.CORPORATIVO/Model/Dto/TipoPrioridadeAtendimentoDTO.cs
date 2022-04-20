using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TIPO_PRIORIDADE_ATENDIMENTO))]
    public class TipoPrioridadeAtendimentoDTO
    {
        public TipoPrioridadeAtendimentoDTO()
        {
            this.PRIORIDADE_ATENDIMENTO = new HashSet<PrioridadeAtendimentoDTO>();
        }
    
        public int TP_PRI_ID { get; set; }
        public string TP_PRI_DESCRICAO { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PrioridadeAtendimentoDTO> PRIORIDADE_ATENDIMENTO { get; set; }
    }
}
