using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(TIPO_JOB))]
    public class TipoJobDTO
    {
        public TipoJobDTO()
        {
            this.HISTORICO_EXECUCAO = new HashSet<HistoricoExecucaoDTO>();
        }

        public int TPJ_ID { get; set; }
        public string TPJ_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<HistoricoExecucaoDTO> HISTORICO_EXECUCAO { get; set; }
    }
}
