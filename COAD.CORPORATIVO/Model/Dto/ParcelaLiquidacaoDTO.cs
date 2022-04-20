using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class ParcelaLiquidacaoDTO
    {
        public string PAR_NUM_PARCELA { get; set; }
        public string PLI_TIPO_DOC { get; set; }
        public string PLI_NUMERO { get; set; }
        public string BAN_ID { get; set; }
        public string PLI_NAUT { get; set; }
        public Nullable<System.DateTime> PLI_DATA { get; set; }
        public Nullable<System.DateTime> PLI_DATA_VALIDADE { get; set; }
        public string PLI_PRACA { get; set; }
        public Nullable<decimal> PLI_VALOR { get; set; }
        public Nullable<System.DateTime> PLI_DATA_BAIXA { get; set; }
        public string PLI_NUM_ARQ { get; set; }
        public string PLI_ORIGEM_PGTO { get; set; }
        public string PLI_IDENT_DOCTO { get; set; }
        public string PLI_SEQ_BX_CART { get; set; }
        public Nullable<System.DateTime> PLI_DATA_BORDERO { get; set; }
        public Nullable<System.DateTime> PLI_DATA_EXCLUSAO { get; set; }

        public virtual BancosDTO BANCOS { get; set; }
        public virtual ParcelasDTO PARCELAS { get; set; }

        // Só existe na aplicação
        public bool atualizaCodigo { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual DocLiquidacaoDTO DOC_LIQUIDACAO { get; set; }
    }
}
