using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{

    public class RelApuracaoRecebimentoTotalCustomDTO
    {
        public RelApuracaoRecebimentoTotalCustomDTO()
        {
            this.LISTA = new HashSet<RelApuracaoRecebimentoCustomDTO>();
        }
        
        public Nullable<decimal> PAR_VLR_PAGO_PRIMEIRA { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO_VENCIMENTO { get; set; }
        public Nullable<decimal> PAR_VLR_BAIXA_MANUAL { get; set; }
        public Nullable<decimal> PAR_VLR_BAIXA { get; set; }
        public Nullable<decimal> PAR_VLR_DIFERENCA { get; set; }
        public Nullable<decimal> PAR_VLR_PREV_PRIMEIRA { get; set; }
        public Nullable<decimal> PAR_VLR_PREVISTO { get; set; }
        public virtual ICollection<RelApuracaoRecebimentoCustomDTO> LISTA { get; set; }

    }

    public class  RelApuracaoRecebimentoCustomDTO
    {
        public RelApuracaoRecebimentoCustomDTO()
        {
            this.APURARCAO = new HashSet<APURAR_RECEBIMENTO_Result>();
        }

        public Nullable<System.DateTime> PAR_DATA_PAGTO { get; set; }
        public virtual ICollection<APURAR_RECEBIMENTO_Result> APURARCAO { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO_PRIMEIRA { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO_VENCIMENTO { get; set; }
        public Nullable<decimal> PAR_VLR_BAIXA_MANUAL { get; set; }
        public Nullable<decimal> PAR_VLR_BAIXA { get; set; }
        public Nullable<decimal> PAR_VLR_DIFERENCA { get; set; }
        public Nullable<decimal> PAR_VLR_PREV_PRIMEIRA { get; set; }
        public Nullable<decimal> PAR_VLR_PREVISTO { get; set; }

    }
}
