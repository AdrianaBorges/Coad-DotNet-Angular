using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(TEMPLATE_GRUPO))]
    public class TemplateGrupoDTO
    {
        public TemplateGrupoDTO()
        {
            this.TEMPLATE_HTML = new HashSet<TemplateHTMLDTO>();
        }

        public int? TGR_ID { get; set; }
        public string TGR_DESCRICAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TemplateHTMLDTO> TEMPLATE_HTML { get; set; }
    }
}
