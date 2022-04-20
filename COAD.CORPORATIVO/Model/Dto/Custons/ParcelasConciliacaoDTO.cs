using Coad.GenericCrud.Dao.Base.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{

    public class ParcelasBoletoDTO
    {
        public string PAR_NOSSO_NUMERO { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public DateTime? CTR_DATA_FAT { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string CLI_NOME { get; set; }
        public DateTime? PAR_DATA_VENCTO { get; set; }
        public DateTime? PAR_DATA_PAGTO { get; set; }
        public decimal? PAR_VLR_PARCELA { get; set; }
        public int? DIAS_ATRASO { get; set; }
        public string ASN_A_C { get; set; }
        public string BAN_ID { get; set; }
        public int? CLI_ID { get; set; }
        public bool? CTA_ENVIA_BOLETO { get; set; }
        public int? CTA_ID { get; set; }

    }

    public class ParcelasAtrasoCobrancaDTO
    {
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string CLI_NOME { get; set; }
        public Nullable<DateTime> PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public Nullable<int> DIAS_ATRASO { get; set; }
        public string ASN_A_C { get; set; }
        public string BAN_ID { get; set; }
        public int? CLI_ID { get; set; }
    }

    public class ParcelasConciliacaoRemTotalDTO
    {
        public ParcelasConciliacaoRemTotalDTO()
        {
            this.LISTA_CONCILIACAO = new HashSet<ParcelasConciliacaoRemDTO>();
            this.PAGINA_CONCILIACAO = new Pagina<ParcelasConciliacaoRemDTO>();
        }
        public int? REM_ID { get; set; }
        public int? CNQ_ID { get; set; }
        public Nullable<decimal> PAR_VLR_TOTAL { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }
        public virtual ICollection<ParcelasConciliacaoRemDTO> LISTA_CONCILIACAO { get; set; }
        public virtual Pagina<ParcelasConciliacaoRemDTO> PAGINA_CONCILIACAO { get; set; }

    }


    public class ParcelasConciliacaoRemDTO
    {
        public int? REM_ID { get; set; }
        public int? CNQ_ID { get; set; }
        public int? IPE_ID { get; set; }
        public int? PPI_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CLI_NOME { get; set; }
        public string BAN_NOME { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public DateTime PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public Nullable<DateTime> PAR_DATA_PAGTO { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }
        public string BAN_ID { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
        public string ORDER_KEY { get; set; }
        public string PAR_NOSSO_NUMERO { get; set; }
        public Nullable<DateTime> CNQ_DATA_LIDO { get; set; }
        public Nullable<DateTime> CNQ_DATA_PROCESSADO { get; set; }
        public Nullable<DateTime> PAR_DATA_ALOC { get; set; }
        public string OCT_CODIGO { get; set; }
        public string OCT_DESCRICAO { get; set; }
        public Nullable<bool> OCT_BAIXAR_TITULO { get; set; }
        public Nullable<bool> OCT_DESALOCAR_TITULO { get; set; }
        public Nullable<DateTime> DATA_EXCLUSAO { get; set; }

    }

    public class ParcelasConciliacaoDTO
    {
        public Nullable<int> CLI_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CLI_NOME { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public DateTime  PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public Nullable<DateTime> PLI_DATA { get; set; }
        public Nullable<DateTime> PLI_DATA_BAIXA { get; set; }
        public Nullable<decimal> PLI_VALOR { get; set; }
        public string BAN_ID { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
        public string PLI_TIPO_DOC { get; set; }
        public string ORDER_KEY { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public string REGIAO_UF { get; set; }

        
    }

    public class ParcelasConciliacaoTotalDTO
    {
        public Nullable<int> EMP_ID { get; set; }
        public DateTime DATA_INICIAL { get; set; }
        public DateTime DATA_FINAL { get; set; }
        public Nullable<decimal> TOTAL_PERIODO { get; set; }
        public Nullable<decimal> TOTAL_PAGO { get; set; }
    }


}
