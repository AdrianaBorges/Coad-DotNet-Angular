using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(BANDEIRA_CARTAO))]
    public class BandeiraCartaoDTO
    {
        public BandeiraCartaoDTO()
        {
            this.PROPOSTA_ITEM_COMPROVANTE = new HashSet<PropostaItemComprovanteDTO>();
        }

        public int BAC_ID { get; set; }
        public string BAC_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemComprovanteDTO> PROPOSTA_ITEM_COMPROVANTE { get; set; }
    }
}
