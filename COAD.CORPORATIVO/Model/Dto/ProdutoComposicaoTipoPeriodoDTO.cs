using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(PRODUTO_COMPOSICAO_TIPO_PERIODO))]
    public class ProdutoComposicaoTipoPeriodoDTO
    {
        public int? CMP_ID { get; set; }
        public int? TTP_ID { get; set; }
        public Nullable<System.DateTime> DATA_ASSOCIACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPeriodoDTO TIPO_PERIODO { get; set; }
        
    }
}
