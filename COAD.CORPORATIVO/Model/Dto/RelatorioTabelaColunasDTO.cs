using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RELATORIO_TABELA_COLUNAS))]
    public class RelatorioTabelaColunasDTO
    {
        public int? COR_ID { get; set; }
        public string COR_DESCRICAO { get; set; }
        public Nullable<short> COR_ORDEM { get; set; }
        public Nullable<bool> COR_ORDENACAO { get; set; }
        public Nullable<bool> COR_ORDEM_ASC { get; set; }
        public Nullable<int> REL_ID { get; set; }
        public Nullable<int> RET_ID { get; set; }
        public string COR_ALIAS { get; set; }
        public string COR_TYPE_NAME { get; set; }
        public Nullable<bool> COR_IS_NULLABLE { get; set; }
        public Nullable<bool> COR_AGRUPAR { get; set; }
        public Nullable<short> COR_AGRUPAMENTO_ORDEM { get; set; }

        public string TableAlias { get; set; }
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual RelatorioPersonalizadoDTO RELATORIO_PERSONALIZADO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RelatorioTabelasDTO RELATORIO_TABELAS { get; set; }
    }
}
