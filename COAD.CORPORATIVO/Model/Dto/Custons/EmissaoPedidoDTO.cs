using Coad.GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    
    public class EmissaoPedidoDTO
    {
        public EmissaoPedidoDTO()
        {
            EMISSAO_PEDIDO_ITEM = new HashSet<EmissaoPedidoItemDTO>();
            ValidarCliente = true;
        }

        public TipoDePedidoEnum? TipoDePedido { get; set; }

        public string ASN_NUM_ASSINATURA { get; set; }
        public int? RG_ID { get; set; }
        public int? UEN_ID { get; set; }
        public int? REP_ID { get; set; }
        public int? REP_ID_EMITENTE { get; set; }
        public int? PRT_ID { get; set; }
        public bool ValidarCliente { get; set; }
        public bool Pago { get; set; }
        public int? EMP_ID { get; set; }
        public string CarId { get; set; }
        public string EmailContato { get; set; }
        public string EmailNotaFiscal { get; set; }
        public Nullable<bool> EmpresaDoSimples { get; set; }
        public Nullable<bool> CemPorCentoFaturado { get; set; }
        public string Observacoes { get; set; }
        public string ObservacoesNotaFiscal { get; set; }
        public int? TneId { get; set; }

        [Required(ErrorMessage = "Não há cliente relacionado ao pedido.")]
        public virtual ClienteDto CLIENTE { get; set; }
        
        [RequiredList(1, ErrorMessage = "Adicione ao menos um produto ao pedido.")]
        public ICollection<EmissaoPedidoItemDTO> EMISSAO_PEDIDO_ITEM { get; set; }
    }
}
