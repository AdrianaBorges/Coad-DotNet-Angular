using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(AREA_CONSULTORIA_CURSO))]
    public class AreaConsultoriaCursoDTO
    {
        public int? CMP_ID { get; set; }
        public int ARE_CONS_ID { get; set; }
        public int TIT_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ColecionadorRefDTO COLECIONADOR_REF { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ProdutoComposicaoDTO PRODUTO_COMPOSICAO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TitulacaoRefDTO TITULACAO_REF { get; set; }
    }
}
