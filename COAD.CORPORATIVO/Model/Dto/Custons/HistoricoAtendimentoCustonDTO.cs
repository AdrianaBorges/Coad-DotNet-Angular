using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class  HistoricoAtendimentoCustonDTO
    {
        public string CLA_ATEND_DESCRICAO { get; set; }
        public string TIP_ATEND_ID {get; set;}
        public string TIP_ATEND_DESCRICAO {get; set;}
        public string TIP_ATEND_GRUPO {get; set;}
        public string USU_LOGIN {get; set;}
        public double QTDETEL { get; set; }
        public double QTDEEMA { get; set; }
        public double QTDE { get; set; }
    }
}
