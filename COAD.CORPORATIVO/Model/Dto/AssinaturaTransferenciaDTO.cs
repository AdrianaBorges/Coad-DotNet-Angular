using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(ASSINATURA_TRANSFERENCIA))]
    public partial class AssinaturaTransferenciaDTO
    {
        public AssinaturaTransferenciaDTO()
        {
            HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            HISTORICO_PEDIDO = new HashSet<HistoricoPedidoDTO>();
        }

        public string ASN_NUM_ASSINATURA_ANT { get; set; }
        public string ASN_NUM_ASSINATURA_ATU { get; set; }
        public int CLI_ID { get; set; }
        public string ASN_TRANSF_SOLICITANTE { get; set; }
        public Nullable<System.DateTime> ASN_DATA_TRANSF { get; set; }
        public string USU_LOGIN { get; set; }
        public string ASN_TRANSF_MOTIVO { get; set; }

        public string ASN_TRANSF_CONTRATO_ORIGEM { get; set; }
        public string ASN_TRANSF_CONTRATO_GERADO { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ContratoDTO CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ContratoDTO CONTRATOS1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual ICollection<HistoricoPedidoDTO> HISTORICO_PEDIDO { get; set; }
    }
}
