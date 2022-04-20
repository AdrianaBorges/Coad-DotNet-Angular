using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(ORIGEM_ACESSO))]
    public class OrigemAcessoDTO
    {
        public OrigemAcessoDTO()
        {
            this.PRODUTOS = new HashSet<ProdutosDTO>();
        }
    
        public int OAC_ID { get; set; }
        public string OAC_DESCRICAO { get; set; }
        public string OAC_OBSERVACAO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }

        public virtual LinhaProdutoDTO LINHA_PRODUTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }

    }
}
