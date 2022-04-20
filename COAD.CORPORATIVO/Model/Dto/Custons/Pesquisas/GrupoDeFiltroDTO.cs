using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class GrupoDeFiltroDTO
    {
        public string label { get; set; }
        public int? Count { get; set; }
        public string chave { get; set; }
        public object valor { get; set; }
    }
}
