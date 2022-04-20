using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TABELA_PRECO_TIPO_PAGAMENTO))]
    public class TabelaPrecoTipoPagamentoDTO
    {
        public int TP_ID { get; set; }
        public int TPG_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TabelaPrecoDTO TABELA_PRECO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }
        
    }
}
