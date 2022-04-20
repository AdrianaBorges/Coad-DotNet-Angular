using System;
using System.Collections.Generic;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{

    public class ParcelasAtrasoCustomDTO
    {
        public ParcelasAtrasoCustomDTO()
        {
            this.PARCELAS = new HashSet<ParcelasDTO>();
        }

        public string CLI_NOME { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public Nullable<decimal> VLR_TOTAL_DEBITO { get; set; }
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        public string PAR_NUM_PARCELA { get; set; }
        public DateTime? PAR_DATA_VENCTO { get; set; }
        public int DIAS_ATRASO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }

        public object ListarDebitoDetalhadamente(string assinatura, int cliente)
        {
            throw new NotImplementedException();
        }
    }

    public class ParcelasNegociacaoCustomDTO
    {
        public int QTDE_PARCELAS { get; set; }
        public Nullable<decimal> VALOR_PARCELAS { get; set; }
        public Nullable<decimal> VALOR_TOTAL { get; set; }

    }

    public class ParcelasRetornoCustomDTO
    {
        public string BAN_ID { get; set; }
        public Nullable<int> CTA_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }
        public DateTime? PAR_DATA_PAGTO { get; set; }
        public string PAR_NOSSO_NUMERO { get; set; }
        public Nullable<int> CNQ_ID { get; set; }
        public Nullable<int> REM_ID { get; set; }
        public Nullable<bool> PAR_BAIXA_MANUAL { get; set; }

        public Nullable<System.DateTime> CNI_DATA_PAGTO { get; set; }
        public string OCT_CODIGO { get; set; }
        public Nullable<int> CNI_ACAO { get; set; }
        public Nullable<decimal> CNI_VLR_JUROS { get; set; }
        public Nullable<decimal> CNI_VLR_PAGO { get; set; }

    }
}