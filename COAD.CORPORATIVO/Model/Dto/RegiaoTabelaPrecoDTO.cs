using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(REGIAO_TABELA_PRECO))]
    public class RegiaoTabelaPrecoDTO
    {
        public int? TP_ID { get; set; }
        public int? RG_ID { get; set; }
        public Nullable<decimal> RTP_PRECO_VENDA { get; set; }
        public Nullable<System.DateTime> RTP_DATA_ASSOCIACAO { get; set; }
        public Nullable<System.DateTime> RTP_DATA_EXCLUSAO { get; set; }
        public bool Excluir { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TabelaPrecoDTO TABELA_PRECO { get; set; }

    }
}
