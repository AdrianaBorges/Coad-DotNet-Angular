using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(TITULACAO_REF))]
    public class TitulacaoRefDTO
    {
        public TitulacaoRefDTO()
        {
            this.AREA_CONSULTORIA_REPRESENTANTE = new HashSet<AreaConsultoriaRepresentanteDTO>();
        }
    
        public int TIT_ID { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AreaConsultoriaRepresentanteDTO> AREA_CONSULTORIA_REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<AreaConsultoriaCursoDTO> AREA_CONSULTORIA_CURSO { get; set; }
    }
}
