using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Model.DTO
{
    [Mapping(Source = typeof(ProdutoComposicaoDTO))]
    public class CursoProxyDTO : ProdutoComposicaoDTO
    {
        public CursoProxyDTO()
        {
            AREA_CONSULTORIA_CURSO_PROXY = new HashSet<AreaConsultoriaCursoProxyDTO>();
            AREA_CONSULTORIA_CURSO = new HashSet<AreaConsultoriaCursoDTO>();
            EhCurso = true;
        }

        [ListHasDuplication("CMP_ID", "ARE_CONS_ID", "TIT_ID", ErrorMessage = "Não é permitido adicionar um Colecionador e Grande Grupo repetido.")]
        public virtual ICollection<AreaConsultoriaCursoProxyDTO> AREA_CONSULTORIA_CURSO_PROXY { get; set; }
    }
}
