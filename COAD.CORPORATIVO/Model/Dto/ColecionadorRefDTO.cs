using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(COLECIONADOR_REF))]
    public class ColecionadorRefDTO
    {
        public ColecionadorRefDTO()
        {
            this.AREA_CONSULTORIA_REPRESENTANTE = new HashSet<AreaConsultoriaRepresentanteDTO>();
        }
    
        public int ARE_CONS_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AreaConsultoriaRepresentanteDTO> AREA_CONSULTORIA_REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AreaConsultoriaCursoDTO> AREA_CONSULTORIA_CURSO { get; set; }
    }
}
