using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(REGISTRO_LIBERACAO_TIPO))]
    public class RegistroLiberacaoTipoDTO
    {
        public RegistroLiberacaoTipoDTO()
        {
            this.REGISTRO_LIBERACAO = new HashSet<RegistroLiberacaoDTO>();
        }

        public int? RLT_ID { get; set; }
        public string RLT_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegistroLiberacaoDTO> REGISTRO_LIBERACAO { get; set; }
    }
}
