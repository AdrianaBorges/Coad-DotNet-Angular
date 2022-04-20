using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public partial class ManualDPItemDTO
    {
        public ManualDPItemDTO()
        {
            this.FUNDAMENTACAO = new HashSet<FundamentacaoDTO>();
            this.MANUAL_DP_ITEM1 = new HashSet<ManualDPItemDTO>();
            this.MANUAL_DP_LINK = new HashSet<ManualDPLinkDTO>();
        }
    
        public Nullable<int> MAI_ID { get; set; }
        public string MAI_DESCRICAO { get; set; }
        public string USU_LOGIN { get; set; }
        public string USU_LOGIN_ALT { get; set; }
        public System.DateTime DATA_INSERT { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public Nullable<int> MAI_ID_PAI { get; set; }
        public int MAN_ID { get; set; }
        public string MAI_TITULO { get; set; }
        public Nullable<System.DateTime> MAI_DATA_PUBLICACAO { get; set; }
        public Nullable<int> MAI_INDEX { get; set; }
        public Nullable<int> MAI_NIVEL { get; set; }
        public Nullable<int> TIP_ATO_ID { get; set; }
        public Nullable<int> MAI_NUMERO_ATO { get; set; }
        public Nullable<System.DateTime> MAI_DATA_ATO { get; set; }
        public Nullable<int> ORG_ID { get; set; }
        public string MAI_NUMERO_ARTIGO { get; set; }
        public string MOD_DESCRICAO { get; set; }
        public string MAN_ASSUNTO { get; set; }
        

        public virtual ICollection<FundamentacaoDTO> FUNDAMENTACAO { get; set; }
        public virtual ManualDPDTO MANUAL_DP { get; set; }
        public virtual ICollection<ManualDPItemDTO> MANUAL_DP_ITEM1 { get; set; }
        public virtual ManualDPItemDTO MANUAL_DP_ITEM2 { get; set; }
        public virtual ICollection<ManualDPLinkDTO> MANUAL_DP_LINK { get; set; }
        public virtual OrgaoDTO ORGAO { get; set; }
        public virtual TipoAtoDTO TIPO_ATO { get; set; }


    }
}
