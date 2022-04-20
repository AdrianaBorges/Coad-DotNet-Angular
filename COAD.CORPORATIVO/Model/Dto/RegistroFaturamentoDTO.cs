using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(REGISTRO_FATURAMENTO))]
    public class RegistroFaturamentoDTO
    {
        public int? RFT_ID { get; set; }
        public Nullable<int> IPE_ID { get; set; }
        public Nullable<System.DateTime> RFT_DATA_FATURAMENTO { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<bool> RFT_GERAR_NOTA_FISCAL { get; set; }
        public Nullable<System.DateTime> RFT_FATURAMENTO_EFETIVO { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual AssinaturaDTO ASSINATURA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ContratoDTO CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ItemPedidoDTO ITEM_PEDIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }
        
        public virtual EmpresaModel EMPRESA { get; set; }

    }
}
