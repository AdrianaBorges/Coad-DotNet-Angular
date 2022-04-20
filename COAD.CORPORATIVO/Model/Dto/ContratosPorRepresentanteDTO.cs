using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ContratosPorRepresentanteDTO
    {
        public Nullable<int> MES { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public string CAR_ID { get; set; }
        public Nullable<int> QTDE { get; set; }
     
    }
}
