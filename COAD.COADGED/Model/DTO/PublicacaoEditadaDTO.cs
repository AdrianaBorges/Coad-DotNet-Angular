using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    [Mapping(Source = typeof(PUBLICACAO_EDITADA))]

    public class PublicacaoEditadaDTO
    {
        public Nullable<int> EDT_ID { get; set; }
        public Nullable<int> PUB_ID { get; set; }
        public System.DateTime EDT_HORARIO { get; set; }
        public Nullable<System.DateTime> EDT_LIBERADA { get; set; }
        public string USU_LOGIN { get; set; }
        public bool EDT_EDITANDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PublicacaoDTO PUBLICACAO { get; set; }
    }
}
