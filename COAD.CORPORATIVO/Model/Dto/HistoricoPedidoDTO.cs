using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(HISTORICO_PEDIDO))]
    public class HistoricoPedidoDTO
    {
        public Nullable<int> HIP_ID { get; set; }
        public string HIP_DESCRICAO { get; set; }
        public Nullable<System.DateTime> HIP_DATA { get; set; }
        public Nullable<int> PST_ID { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public int? PPI_ID { get; set; }
        public string ASN_NUM_ASSINATURA_ANT { get; set; }
        public string ASN_NUM_ASSINATURA_ATU { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual PedidoStatusDTO PEDIDO_STATUS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaTransferenciaDTO ASSINATURA_TRANSFERENCIA { get; set; }

    }
}
