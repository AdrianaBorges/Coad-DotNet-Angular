using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PRODUTO_PERFIL))]
    public partial class ProdutoPerfilDTO
    {
        public int PRO_ID { get; set; }
        public string PER_ID { get; set; }
        public Nullable<int> PPR_QTDE_LOGIN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutosDTO PRODUTOS { get; set; }
    }
}
