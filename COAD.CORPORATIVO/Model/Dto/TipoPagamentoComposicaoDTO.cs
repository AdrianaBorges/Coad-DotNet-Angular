using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TIPO_PAGAMENTO_COMPOSICAO))]
    public class TipoPagamentoComposicaoDTO
    {

        public int? TPG_ID_PAI { get; set; }
        public int? TPG_ID_FILHO { get; set; }
        public int? TPC_ORDEM { get; set; }

        // pai
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        //filho
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO1 { get; set; }
        
    }
}
