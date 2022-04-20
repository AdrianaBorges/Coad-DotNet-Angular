using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    [Mapping(Source = typeof(CO_CALENDARIO), confRef = "portal")]
    public class CoCalendarioDTO
    {
        public long NUM_CALENDARIO { get; set; }
        public long NUM_OBRIGACAO { get; set; }
        public System.DateTime DTReferencia { get; set; }
        public string TXT_REFERENCIA { get; set; }
        public Nullable<short> HABILITADO { get; set; }
        public Nullable<System.DateTime> CriadoEmDT { get; set; }
        public Nullable<System.DateTime> AtualizadoEmDT { get; set; }
        public string CriadoPor { get; set; }
        public string AtualizadoPor { get; set; }

        public virtual CoObrigacoesDTO CO_OBRIGACOES { get; set; }
    }
}
