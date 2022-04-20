using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class PublicacaoCicloAprovacaoDTO
    {
        public int FLU_ID { get; set; }
        public int FLU_ETAPA_ID { get; set; }
        public int CIC_ID { get; set; }
        public Nullable<int> COL_ID { get; set; }
        public Nullable<System.DateTime> CIC_DATA { get; set; }
        public string CIC_OBS { get; set; }
        public Nullable<bool> CIC_STATUS { get; set; }

        public virtual ColaboradorDTO COLABORADOR { get; set; }
        public virtual PublicacaoFluxoEtapaDTO PUBLICACAO_FLUXO_ETAPA { get; set; }
    }
}
