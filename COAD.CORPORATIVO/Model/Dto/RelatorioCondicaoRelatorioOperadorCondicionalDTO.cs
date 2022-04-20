using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL))]
    public class RelatorioCondicaoRelatorioOperadorCondicionalDTO
    {
        public int? REO_ID { get; set; }
        public int? REC_ID { get; set; }
        public int? ROC_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RelatorioCondicaoDTO RELATORIO_CONDICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RelatorioOperadorCondicionalDTO RELATORIO_OPERADOR_CONDICIONAL { get; set; }
    }
}
