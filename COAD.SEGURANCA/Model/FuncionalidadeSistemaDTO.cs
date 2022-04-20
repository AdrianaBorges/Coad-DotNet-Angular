using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(FUNCIONALIDADE_SISTEMA))]
    public class FuncionalidadeSistemaDTO
    {
        public int? FSI_ID { get; set; }
        public string FSI_DESCRICAO { get; set; }
        public Nullable<bool> FSI_USA_TEMPLATE { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> TPL_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TemplateHTMLDTO TEMPLATE_HTML { get; set; }
    }
}
