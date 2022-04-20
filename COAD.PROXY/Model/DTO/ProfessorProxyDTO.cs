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
    [Mapping(Source = typeof(RepresentanteDTO))]
    public class ProfessorProxyDTO : RepresentanteDTO
    {
        public ProfessorProxyDTO()
        {
            AREA_CONSULTORIA_REPRESENTANTE_PROXY = new HashSet<AreaConsultoriaRepresentanteProxyDTO>();
        }

        [ListHasDuplication("REP_ID","ARE_CONS_ID","TIT_ID", ErrorMessage = "Não é permitido adicionar um Colecionador e Grande Grupo repetido.")]
        public ICollection<AreaConsultoriaRepresentanteProxyDTO> AREA_CONSULTORIA_REPRESENTANTE_PROXY { get; set; }
    }
}
