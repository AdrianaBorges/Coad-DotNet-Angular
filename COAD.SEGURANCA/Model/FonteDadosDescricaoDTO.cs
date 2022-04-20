using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(FONTE_DADOS_DESCRICAO))]
    public class FonteDadosDescricaoDTO
    {
        public int? FDD_ID { get; set; }
        public string FDD_DESCRICAO { get; set; }
        public string DFD_TOKEN { get; set; }
        public string DFD_PATH { get; set; }
        public Nullable<int> FDA_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual FonteDadosTemplateDTO FONTE_DADOS_TEMPLATE { get; set; }
    }
}
