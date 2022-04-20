using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(LINHA_PRODUTO))]
    public partial class LinhaProdutoDTO
    {
        public LinhaProdutoDTO()
        {
            this.LINHA_PRODUTO_INFORMATIVO = new HashSet<LinhaProdutoInformativoDTO>();
            this.ORIGEM_ACESSO = new HashSet<OrigemAcessoDTO>();
            this.PRODUTO_COMPOSICAO = new HashSet<ProdutoComposicaoDTO>();
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }
    
        public int LIN_PRO_ID { get; set; }
        public string LIN_PRO_DESCRICAO { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<LinhaProdutoInformativoDTO> LINHA_PRODUTO_INFORMATIVO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<OrigemAcessoDTO> ORIGEM_ACESSO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutoComposicaoDTO> PRODUTO_COMPOSICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }

    }
}
