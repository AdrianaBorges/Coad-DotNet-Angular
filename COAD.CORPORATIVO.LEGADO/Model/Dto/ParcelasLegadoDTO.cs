using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    [Mapping(typeof(Parcelas))]
    public class ParcelasLegadoDTO
    {
        public string CONTRATO { get; set; }
        public string LETRA { get; set; }
        public string CD { get; set; }
        public string DATA_VENCTO { get; set; }
        public string SITUACAO { get; set; }
        public string VLR_PARCELA { get; set; }
        public string MORA_MES { get; set; }
        public string PG_PART_BTC { get; set; }
        public string BCO_ALOC { get; set; }
        public string CART_ALOC { get; set; }
        public string DT_ALOC { get; set; }
        public string DT_EMISSAO_BLQ { get; set; }
        public string VLR_PAGO { get; set; }
        public string DT_PAGTO { get; set; }
        public string DATA_DIARIO { get; set; }
        public string DATA_RECIBO { get; set; }
        public string DT_IMP_AUT_CRA { get; set; }
        public string ALOC_BANCO { get; set; }
        public string DATA_DIARIO_CANC { get; set; }
        public string DATA_SITUACAO_9 { get; set; }
        public string VENCTO_PRORROG { get; set; }
        public string CART_ALOC_2 { get; set; }
        public string nosso_numero { get; set; }
        public string DH_SUBIR { get; set; }
        public string DH_SUBIU { get; set; }
        public string cedente { get; set; }
        public Nullable<System.DateTime> DATA_INSERT { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string TAXA_ADM { get; set; }
        public Nullable<int> REM_ID { get; set; }
        public string PAR_NUM_PARCELA { get; set; }

    }
}
