using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(CNAB_ARQUIVOS_ITEM))]
    public partial class CnabArquivosItemDTO
    {
        public int CNI_ID { get; set; }
        public Nullable<System.DateTime> CNI_DATA_PAGTO { get; set; }
        public Nullable<decimal> CNI_VLR_JUROS { get; set; }
        public Nullable<decimal> CNI_VLR_PAGO { get; set; }
        public Nullable<decimal> CNI_VLR_PARCELA { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string PAR_NOSSO_NUMERO { get; set; }
        public int CNQ_ID { get; set; }
        public string BAN_ID { get; set; }
        public string OCT_CODIGO { get; set; }
        public int CTA_ID { get; set; }
        public string CNI_LINHA_ARQUIVO { get; set; }
        public int CNI_ACAO { get; set; }

        public virtual BancosDTO BANCOS { get; set; }
        public virtual CnabArquivosDTO CNAB_ARQUIVOS { get; set; }
        public virtual ParcelasDTO PARCELAS { get; set; }
    }
}
