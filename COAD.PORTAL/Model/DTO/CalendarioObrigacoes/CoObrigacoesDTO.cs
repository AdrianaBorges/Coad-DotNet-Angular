using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    [Mapping(Source = typeof(CO_OBRIGACOES), confRef = "portal")]
    public class CoObrigacoesDTO
    {
        public CoObrigacoesDTO()
        {
            this.CO_CALENDARIO = new HashSet<CoCalendarioDTO>();
        }
    
        public string TXT_TITULO { get; set; }
        public long NUM_OBRIGACAO { get; set; }
        public Nullable<short> HABILITADO { get; set; }
        public string TXT_PESSOAS_OBRIGADAS { get; set; }
        public string TXT_FATO_GERADOR { get; set; }
        public string TXT_PENALIDADE { get; set; }
        public string TXT_ABRANGENCIA { get; set; }
        public string COD_TIPO { get; set; }
        public string TXT_OBSERVACAO { get; set; }
        public string TXT_FORMA_ENTREGA { get; set; }
        public string LNK_ORIENTACAO { get; set; }
        public string TXT_FUNDLEGAL { get; set; }
        public string LNK_GUIARECOL { get; set; }
        public string TXT_RECOLHIMENTO { get; set; }
        public string LNK_IRSITE { get; set; }
        public Nullable<long> NUM_UF { get; set; }
        public Nullable<long> NUMPK_MUNICIPIO { get; set; }
        public string COD_AREA { get; set; }
        public Nullable<System.DateTime> CriadoEmDT { get; set; }
        public Nullable<System.DateTime> AtualizadoEmDT { get; set; }
        public string CriadoPor { get; set; }
        public string AtualizadoPor { get; set; }
        public string Sigla { get; set; }
    
        public virtual CoAreasDTO CO_AREAS { get; set; }
        public virtual ICollection<CoCalendarioDTO> CO_CALENDARIO { get; set; }
        public virtual CoEstadosDTO CO_ESTADOS { get; set; }
        public virtual CoMunicipiosDTO CO_MUNICIPIOS { get; set; }
        public virtual CoTiposDTO CO_TIPOS { get; set; }
    }
}
