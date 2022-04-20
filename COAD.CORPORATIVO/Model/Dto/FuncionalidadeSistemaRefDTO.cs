using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Dto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(FUNCIONALIDADE_SISTEMA_REF))]
    public class FuncionalidadeSistemaRefDTO
    {
        public int? FSI_ID { get; set; }
        public Nullable<int> TPL_ID { get; set; }

        public FuncionalidadeSistemaDTO FuncionalidadeSistema { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TemplateHTMLDTO TEMPLATE_HTML { get; set; }
    }
}
