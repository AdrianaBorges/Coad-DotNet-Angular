using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TIPO_REMESSA))]
    public partial class TipoRemessaDTO
    {
        public TipoRemessaDTO()
        {
            this.PARCELAS_REMESSA = new HashSet<ParcelasRemessaDTO>();
        }
    
        public int TRE_ID { get; set; }
        public string TRE_DESCRICAO { get; set; }
        public Nullable<bool> TRE_AVULSA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasRemessaDTO> PARCELAS_REMESSA { get; set; }
    }
}