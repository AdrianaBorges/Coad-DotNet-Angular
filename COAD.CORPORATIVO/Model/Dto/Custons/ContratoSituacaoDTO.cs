using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ContratoSituacaoDTO
    {
        public int CLI_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string LINHA_PRODUTO { get; set; }
        public int SENHAS_CADASTRAS { get; set; }
        public int TITULOS_VENCIDOS { get; set; }
        public int TELEFONES_URA { get; set; }
        public string CONTRATO_VIGENTE { get; set; }
        public string CTR_VIGENCIA { get; set; }
        public int QTDE_CONS_CONTRATO { get; set; }
        public int QTDE_CONS_UTILIZADA { get; set; }
        public Nullable<int> CTR_PRORROGADO { get; set; }
        public Boolean ACESSA_URA { get; set; }
        public Boolean ACESSA_PORTAL { get; set; }
        public Boolean ACESSA_PORTAL_ST { get; set; }

    }
}
