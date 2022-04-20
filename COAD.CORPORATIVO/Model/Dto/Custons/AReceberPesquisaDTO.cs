using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class AReceberPesquisaDTO
    {
        public Nullable<int> emp_id { get; set; }
        public Nullable<int> data  { get; set; }
        public Nullable<int> tipo  { get; set; }
        public Nullable<int> banco  { get; set; }
        public Nullable<DateTime> dtini { get; set; }
        public Nullable<DateTime> dtfim { get; set; }

    }
    public class AReceberRelatorioDTO
    {
        public string GRUPO { get; set; }
        public int? EMP_ID { get; set; }
        public string EMP_RAZAO_SOCIAL { get; set; }
        public string BAN_NOME { get; set; }
        public string FORMA_PGTO { get; set; }
        public Nullable<DateTime> CTR_DATA_FAT { get; set; }
        public string CLI_CPF_CNPJ { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public Nullable<DateTime> PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public Nullable<DateTime> PAR_DATA_PAGTO { get; set; }
        public Nullable<decimal> PAR_VLR_PAGO { get; set; }

    }
}
