using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(REGISTRO_LIBERACAO_ITEM))]
    public class RegistroLiberacaoItemDTO
    {

        public int? RIT_ID { get; set; }
        public Nullable<int> RLI_ID { get; set; }
        public Nullable<System.DateTime> RIT_DATA_ACAO { get; set; }
        public Nullable<bool> RIT_LIBERADO { get; set; }
        public string RIT_DESCRICAO { get; set; }
        public Nullable<System.DateTime> RIT_DATA_CRIACAO { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegistroLiberacaoDTO REGISTRO_LIBERACAO { get; set; }

    }
}
