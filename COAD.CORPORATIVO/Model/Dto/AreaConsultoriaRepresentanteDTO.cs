using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(AREA_CONSULTORIA_REPRESENTANTE))]
    public class AreaConsultoriaRepresentanteDTO
    {
        public Nullable<int> REP_ID { get; set; }
        
        [Required(ErrorMessage = "Selecione o Colecionador")]
        public Nullable<int> ARE_CONS_ID { get; set; }
        public Nullable<int> TIT_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ColecionadorRefDTO COLECIONADOR_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TitulacaoRefDTO TITULACAO_REF { get; set; }
    }
}
