
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(INFO_MARKETING))]
    public class InfoMarketingDTO
    {
        public InfoMarketingDTO()
        {
            this.AREA_INFO_MARKETING = new HashSet<AreaInfoMarketingDTO>();
            this.PRODUTO_COMPOSICAO_INFO_MARKETING = new HashSet<ProdutoComposicaoInfoMarketingDTO>();
        }
    
        public int? MKT_CLI_ID { get; set; }
        public Nullable<int> O_CAD_ID { get; set; }

        /// <summary>
        /// Esse campo não existe no banco
        /// </summary>
        public bool PodeEditarOrigem { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<AreaInfoMarketingDTO> AREA_INFO_MARKETING { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ClienteDto CLIENTES { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutoComposicaoInfoMarketingDTO> PRODUTO_COMPOSICAO_INFO_MARKETING { get; set; }

        public virtual OrigemCadastroDTO ORIGEM_CADASTRO { get; set; }
    }
}
