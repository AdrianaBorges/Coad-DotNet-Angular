using Coad.GenericCrud.Dao.Base.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelResumoCReceberDTO
    {
        public int? MES_FAT { get; set; }
        public int? ANO_FAT { get; set; }
        public decimal VALOR_FATURADO { get; set; }
        public decimal VALOR_PAGO { get; set; }
        public decimal VALOR_CANCELADO { get; set; }
        public decimal VALOR_RECEBER { get; set; }
    }
    public class RelAReceberTotalisDTO
    {

        public RelAReceberTotalisDTO()
        {
            this.Lista = new Pagina<RelAReceberDTO>();
        }
        public Nullable<decimal> VALOR_FATURADO { get; set; }
        public Nullable<decimal> VALOR_PREVISTO { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public virtual Pagina<RelAReceberDTO> Lista { get; set; }


    }
    public class RelAReceberDTO
    {
        public string TIPO_VENDA { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> REM_ID { get; set; }
        public string CLI_NOME { get; set; }
        public string BAN_ID { get; set; }
        public string BAN_NOME { get; set; }
        public Nullable<DateTime> ALOCACAO { get; set; }
        public Nullable<DateTime> FATURAMENTO { get; set; }
        public Nullable<DateTime> VENCIMENTO { get; set; }
        public Nullable<DateTime> PAR_DATA_PAGTO { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public Nullable<decimal> VALOR_FATURADO { get; set; }
        public Nullable<decimal> VALOR_PAGO { get; set; }
        public Nullable<int> TPG_ID { get; set; }
        public string TPG_DESCRICAO { get; set; }

    }

} 
