using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PARCELAS_REMESSA))]
    public class ParcelasRemessaDTO
    {
        public ParcelasRemessaDTO()
        {
            this.PARCELA_ALOCADA = new HashSet<ParcelaAlocadaDTO>();
            this.PARCELAS = new HashSet<ParcelasDTO>();
        }

        public int REM_ID { get; set; }
        public string REM_REF { get; set; }
        public Nullable<System.DateTime> REM_DATA { get; set; }
        public string REM_TRANSMITIDO { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<int> CTA_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<DateTime> REM_DATA_TRANSMISSAO { get; set; }
        public Nullable<DateTime> REM_DATA_DESALOCACAO { get; set; }
        public Nullable<int> REM_QTDE { get; set; }
        public Nullable<decimal> REM_TOTAL_REMESSA { get; set; }
        public Nullable<DateTime> REM_DATA_REMESSA { get; set; }
        public Nullable<bool> REM_AVULSA { get; set; }
        public Nullable<int> TRE_ID { get; set; }
        public Nullable<bool> REM_SACADOR_AVALISTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual BancosDTO BANCOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ContaRefDTO CONTA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelaAlocadaDTO> PARCELA_ALOCADA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual TipoRemessaDTO TIPO_REMESSA { get; set; }
    }

}
