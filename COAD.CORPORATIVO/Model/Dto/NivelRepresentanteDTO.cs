using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(NIVEL_REPRESENTANTE))]
    public class NivelRepresentanteDTO
    {
        public NivelRepresentanteDTO()
        {
            this.REPRESENTANTE = new HashSet<RepresentanteDTO>();
        }
    
        public int NRP_ID { get; set; }
        public string NRP_DESCRICAO { get; set; }
        public Nullable<int> NRP_NIVEL { get; set; }
        public string PER_ID { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<RepresentanteDTO> REPRESENTANTE { get; set; }
    }
}
