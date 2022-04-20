using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoFaturas
    {

        [CustomFieldPDF("FATURA/DUPLICATAS", 0.42, 1.00, 0.25, 10.74, 6, false)]
        public string Fatura { get; set; }

        [CustomFieldPDF("FATURA", 0.85, 20.57, 0.25, 11.04, 6)]
        public string Faturas { get; set; }
    }
}
