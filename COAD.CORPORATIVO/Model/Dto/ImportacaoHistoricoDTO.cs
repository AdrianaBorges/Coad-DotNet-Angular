using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{

    [Mapping(typeof(IMPORTACAO_HISTORICO))]
    public class ImportacaoHistoricoDTO
    {
        public int? IMH_ID { get; set; }
        public string IMH_DESCRICAO { get; set; }
        public Nullable<int> IPS_ID { get; set; }
        public Nullable<int> IMP_ID { get; set; }
        public Nullable<int> IMS_ID { get; set; }
        public Nullable<bool> IMH_ERRO { get; set; }
        public Nullable<bool> IMH_HISTORICO_DA_IMPORTACAO { get; set; }
        public Nullable<int> IMH_TOTAL_SUSPECTS { get; set; }
        public Nullable<int> IMH_TOTAL_DUPLICADO { get; set; }
        public Nullable<int> IMH_TOTAL_PROCESSADO { get; set; }
        public Nullable<int> IMH_TOTAL_SUCESSO { get; set; }
        public Nullable<int> IMH_TOTAL_FALHA { get; set; }
        public Nullable<System.DateTime> IMH_DATA { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoDTO IMPORTACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoStatusDTO IMPORTACAO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoSuspectDTO IMPORTACAO_SUSPECT { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
    }
}
