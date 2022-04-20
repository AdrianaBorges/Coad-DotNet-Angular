using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(FONTE_DADOS_TEMPLATE))]
    public class FonteDadosTemplateDTO
    {
        public FonteDadosTemplateDTO()
        {
            this.TEMPLATE_HTML = new HashSet<TemplateHTMLDTO>();
            this.FONTE_DADOS_DESCRICAO = new HashSet<FonteDadosDescricaoDTO>();
        }

        public int? FDA_ID { get; set; }

        public string FDA_DESCRICAO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TemplateHTMLDTO> TEMPLATE_HTML { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<FonteDadosDescricaoDTO> FONTE_DADOS_DESCRICAO { get; set; }
    }
}
