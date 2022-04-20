using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class BoletosAlocadosDTO
    {
        public string BAN_ID { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public string BAN_DESCRICAO { get; set; }
        public int QTDE_TITULOS { get; set; }
        public Nullable<decimal> VALOR_TITULOS { get; set; }
        public Nullable<decimal> TOTAL_ALOCADO { get; set; }
        public Nullable<decimal> TOTAL_VENCIDO { get; set; }
        public Nullable<decimal> TOTAL_PAGO { get; set; }
        public Nullable<decimal> TOTAL_EMABERTO { get; set; }
        public Nullable<decimal> TOTAL_ORIGINAL { get; set; }
        public Nullable<decimal> TOTAL_JUROS { get; set; }
        public Nullable<decimal> TOTAL_DISPONIVEL { get; set; }
        public Nullable<decimal> TOTAL_NECESSARIO { get; set; }

    }
}
