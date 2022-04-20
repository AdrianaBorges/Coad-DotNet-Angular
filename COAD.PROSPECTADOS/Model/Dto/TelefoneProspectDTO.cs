using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Model.Dto
{
    [Mapping(Source = typeof(TELEFONES_PROSP))]
    public class TelefoneProspectDTO
    {
        public string CODIGO { get; set; }
        public string DDD_TEL { get; set; }
        public string TELEFONE { get; set; }
        public string TIPO { get; set; }

        [IgnoreMemberMapping(MappingDirection.DestinyToSource)]
        public virtual CartCoadDTO cart_coad { get; set; }
    }
}
