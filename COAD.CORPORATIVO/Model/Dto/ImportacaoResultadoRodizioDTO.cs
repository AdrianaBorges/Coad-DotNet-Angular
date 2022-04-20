using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(IMPORTACAO_RESULTADO_RODIZIO))]
    public class ImportacaoResultadoRodizioDTO
    {
        public int? IRR_ID { get; set; }
        public Nullable<int> IRR_QTD { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public Nullable<int> IMP_ID { get; set; }
        public Nullable<int> IRR_QTD_PRIORIDADES { get; set; }

        public int Total {
            get
            {
                int qtd = (IRR_QTD != null) ? (int) IRR_QTD : 0;
                int qtdPri = (IRR_QTD_PRIORIDADES != null) ? (int)IRR_QTD_PRIORIDADES : 0;
                int total = (qtd + qtdPri);
                return total;
            }
        }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ImportacaoDTO IMPORTACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
    }
}
