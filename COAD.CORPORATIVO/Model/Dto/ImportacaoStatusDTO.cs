using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(IMPORTACAO_STATUS))]
    public class ImportacaoStatusDTO
    {
        public ImportacaoStatusDTO()
        {
            this.IMPORTACAO = new HashSet<ImportacaoDTO>();
            this.IMPORTACAO_SUSPECT = new HashSet<ImportacaoSuspectDTO>();
            this.IMPORTACAO_HISTORICO = new HashSet<ImportacaoHistoricoDTO>();
        }

        public int? IMS_ID { get; set; }
        public string IMS_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoDTO> IMPORTACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoSuspectDTO> IMPORTACAO_SUSPECT { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoHistoricoDTO> IMPORTACAO_HISTORICO { get; set; }
    }
}
