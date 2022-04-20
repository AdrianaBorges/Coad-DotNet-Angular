using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Model.Dto
{
    [Mapping(Source = typeof(EMAILS_PROSP))]
    public class EmailsProspDTO
    {
        public string CODIGO { get; set; }
        public string E_MAIL { get; set; }
        public string TIPO { get; set; }

        [IgnoreMemberMapping(MappingDirection.DestinyToSource)]
        public virtual CartCoadDTO cart_coad { get; set; }        
    }
}
