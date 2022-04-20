using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(AREAS))]
    public class AreasCorpDTO
    {
        public AreasCorpDTO()
        {
            //this.CONTRATOS = new HashSet<CONTRATOS>();
            this.PRODUTOS = new HashSet<ProdutosDTO>();
            this.AREA_INFO_MARKETING = new HashSet<AreaInfoMarketingDTO>();
            this.IMPORTACAO_SUSPECT = new HashSet<ImportacaoSuspectDTO>();
        }
    
        public int AREA_ID { get; set; }
        public string AREA_NOME { get; set; }
    
        //public virtual ICollection<CONTRATOS> CONTRATOS { get; set; }
        public virtual ICollection<ProdutosDTO> PRODUTOS { get; set; }
        public virtual ICollection<AreaInfoMarketingDTO> AREA_INFO_MARKETING { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoSuspectDTO> IMPORTACAO_SUSPECT { get; set; }
    }
}
