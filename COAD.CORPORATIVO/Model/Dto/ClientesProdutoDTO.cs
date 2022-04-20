using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
  
    public class ClientesProdutoDTO
    {
        public Nullable<int> PRO_ID { get; set; }
        public string PRO_SIGLA { get; set; }
        public Nullable<int> QTDE { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        
    }
}
