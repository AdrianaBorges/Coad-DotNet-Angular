using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.URAS.Model.DTO
{
    public class QtdeConsultasPeridoDTO
    {
        public string codigo { get; set; }
        public int contratadas { get; set; }
        public int qtdemail { get; set; }
        public int qtdurarj { get; set; }
        public int qtduramg { get; set; }
        public int qtdurapr { get; set; }
        public string periodo { get; set; }
    }
}
