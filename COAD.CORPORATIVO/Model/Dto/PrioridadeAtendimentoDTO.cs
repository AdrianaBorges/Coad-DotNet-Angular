using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PRIORIDADE_ATENDIMENTO))]
    public class PrioridadeAtendimentoDTO
    {
        public int PRI_ID { get; set; }
        public Nullable<System.DateTime> PRI_DATA { get; set; }
        public string PRI_NOTA { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> TP_PRI_ID { get; set; }
        public Nullable<System.DateTime> PRI_DATA_CONFIRMACAO { get; set; }
        public Nullable<int> REP_ID_DEMANDANTE { get; set; }
        public Nullable<int> RG_ID { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPrioridadeAtendimentoDTO TIPO_PRIORIDADE_ATENDIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }
    }
}
