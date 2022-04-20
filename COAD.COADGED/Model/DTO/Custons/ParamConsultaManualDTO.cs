using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO.Custons
{
    public class ParamConsultaManualDTO
    {

        public Nullable<int> MOD_ID { get; set; }
        public Nullable<int> MAI_ID { get; set; }
        public Nullable<int> MAN_ID { get; set; }
        public string MAN_ASSUNTO { get; set; }
        public string MAI_TITULO {get;set;}
        public Nullable<int>  TIP_ATO_ID {get;set;}
        public Nullable<int>  MAI_NUMERO_ATO {get;set;}
        public string MAI_DATA_ATO {get;set;}
        public Nullable<int> ORG_ID {get;set;}
        public string MAI_NUMERO_ARTIGO { get; set; }
        public bool PUBLICADO { get; set; }
        public string MAI_DESCRICAO { get; set; }
        public Nullable<int> FUN_NUM_PARAGRAFO { get; set; }
        public string FUN_INCISO { get; set; }

    }
}
