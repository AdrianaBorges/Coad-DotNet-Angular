using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TIPO_VENDA))]
    public class TipoVendaDTO
    {
        public TipoVendaDTO()
        {
            this.PRODUTO_COMPOSICAO = new HashSet<ProdutoComposicaoDTO>();
        }

        public int TPV_ID { get; set; }
        public string TPV_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutoComposicaoDTO> PRODUTO_COMPOSICAO { get; set; }
        
    }
}
