using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Model.Dto;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PEDIDO_PAGAMENTO))]
    public class PedidoPagamentoDTO
    {

        public PedidoPagamentoDTO()
        {
            this.ITEM_PEDIDO_PEDIDO_PAGAMENTO = new HashSet<ItemPedidoPedidoPagamentoDTO>();
            this.PARCELAS = new HashSet<ParcelasDTO>();
        }

        public int? PGT_ID { get; set; }
        public int? EMP_ID { get; set; }
        public int? PED_CRM_ID { get; set; }
        public int? TPG_ID { get; set; }
        public Nullable<decimal> PGT_VLR_TOTAL { get; set; }
        public Nullable<int> PGT_QTDE_PARCELAS { get; set; }
        public Nullable<decimal> PGT_VLR_ENTRADA { get; set; }
        public Nullable<decimal> PGT_VLR_PARCELA { get; set; }

        //[RequiredIf("PGT_ENTRADA", false, ErrorMessage = "Informe a data de vencimento")]
        public Nullable<System.DateTime> PGT_SEGUNDO_VENCTO { get; set; }

        public Nullable<int> PGT_TIPO_DOCUMENTO { get; set; }
        public string PGT_NUMERO_DOCUMENTO { get; set; }
        public string PGT_BANCO { get; set; }
        public Nullable<System.DateTime> PGT_CHEQUE_BOM_PARA { get; set; }
        public string PGT_OBS { get; set; }
        public string PGT_CODIGO_CARTAO { get; set; }
        public Nullable<System.DateTime> PGT_VENCIMENTO_CARTAO { get; set; }
        public Nullable<bool> PGT_ENTRADA { get; set; }
        public Nullable<bool> PGT_PAGO { get; set; }
        public Nullable<System.DateTime> PGT_DATA_PAGAMENTO { get; set; }
        public string PGT_CHAVE_TRANSACAO { get; set; }
        public string PGT_URL_BOLETO { get; set; }
        public string PGT_CODIGO_DE_BARRAS { get; set; }
        public string PGT_STATUS_TRANSACAO { get; set; }
        public string ORDER_KEY { get; set; }
        public string ORDER_KEY_REF { get; set; }


        [PresentDate(ErrorMessage = "A data de vencimento não pode ser anterior ao dia atual.")]
        public Nullable<System.DateTime> PGT_DATA_VENCIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual PedidoCRMDTO PEDIDO_CRM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoPagamentoDTO TIPO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemPedidoPedidoPagamentoDTO> ITEM_PEDIDO_PEDIDO_PAGAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }
        
    }
}