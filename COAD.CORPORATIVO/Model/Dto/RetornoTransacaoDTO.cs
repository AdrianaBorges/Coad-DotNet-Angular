using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(RETORNO_TRANSACAO))]
    public class RetornoTransacaoDTO
    {

        public int RTT_ID { get; set; }
        public Nullable<decimal> RTT_VLR_TOTAL { get; set; }
        public Nullable<decimal> RTT_VLR_PAGO { get; set; }
        public string RTT_NOSSO_NUMERO { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public string RTT_IPE_REFERENCIA { get; set; }
        public string RTT_STATUS { get; set; }
        public string RTT_STATUS_ANT { get; set; }
        public Nullable<System.DateTime> RTT_DATA_INCLUSAO { get; set; }
        public string USU_LOGIN { get; set; }
        public int TPG_ID { get; set; }
        public string ORDER_KEY { get; set; }
        public string ORDER_KEY_REF { get; set; }
        public Nullable<System.DateTime> RTT_STATUS_CHANGE { get; set; }
        public string RTT_ADIQUIRENTE { get; set; }
        public Nullable<decimal> RTT_VLR_AUROTIZADO { get; set; }
        public string RTT_BANDEIRA { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
        public string RTT_MENSAGEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }


     
    }
}
