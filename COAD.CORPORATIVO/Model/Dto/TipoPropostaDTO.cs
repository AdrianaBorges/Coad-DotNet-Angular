using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(TIPO_PROPOSTA))]
    public class TipoPropostaDTO
    {
        
        public TipoPropostaDTO()
        {
            this.PROPOSTA = new HashSet<PropostaDTO>();
            this.CAMPANHA_VENDA_TIPO_PROPOSTA = new HashSet<CampanhaVendaTipoPropostaDTO>();
        }
    
        public int? TPP_ID { get; set; }
        public string TPP_DESCRICAO { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CampanhaVendaTipoPropostaDTO> CAMPANHA_VENDA_TIPO_PROPOSTA { get; set; }
    }
}
