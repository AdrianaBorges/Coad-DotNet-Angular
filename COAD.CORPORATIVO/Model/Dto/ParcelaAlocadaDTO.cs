using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PARCELA_ALOCADA))]
    public partial class ParcelaAlocadaDTO
    {
        [DisplayName("Nosso Número (cedente)")]
        public Nullable<int> ALO_NOSSO_NUMERO { get; set; }

        [DisplayName("NºTítulo")]
        public string PAR_NUM_PARCELA { get; set; }

        [DisplayName("NºTítulo")]
        public string PAR_REMESSA { get; set; }

        [DisplayName("Conta")]
        public int CTA_ID { get; set; }

        [DisplayName("Transmitida em")]
        public Nullable<System.DateTime> ALO_DATA_TRANSMISSAO { get; set; }

        [DisplayName("Cód.Remessa")]
        public string OCM_CODIGO { get; set; }

        [DisplayName("Dia Ocor.Remessa")]
        public Nullable<System.DateTime> ALO_REM_DATA_OCORRENCIA { get; set; }

        [DisplayName("Cód.Retorno")]
        public string OCT_CODIGO { get; set; }

        [DisplayName("Dia Ocor.Retorno")]
        public Nullable<System.DateTime> ALO_RET_DATA_OCORRENCIA { get; set; }

        [DisplayName("Cód.Erro")]
        public string OCE_CODIGO { get; set; }

        [DisplayName("Banco")]
        public string BAN_ID { get; set; }


        [DisplayName("Alocada em")]
        public Nullable<System.DateTime> ALO_DATA_ALOCACAO { get; set; }

        [DisplayName("Desalocada em")]
        public Nullable<System.DateTime> ALO_DATA_DESALOCACAO { get; set; }


        public Nullable<int> REM_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ContaRefDTO CONTA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual OcorrenciaErroDTO OCORRENCIA_ERRO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual OcorrenciaRemessaDTO OCORRENCIA_REMESSA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual OcorrenciaRetornoDTO OCORRENCIA_RETORNO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ParcelasDTO PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ParcelasRemessaDTO PARCELAS_REMESSA { get; set; }
    }
}

