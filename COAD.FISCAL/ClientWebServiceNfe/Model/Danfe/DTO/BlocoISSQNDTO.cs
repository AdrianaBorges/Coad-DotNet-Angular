using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoISSQN
    {
       
        [CustomFieldPDF("CÁLCULO DO ISSQN",0.42, 2.29, 0.25, 24.76 , 10, false)]
        public string CalculoISSQN { get; set; }

        [CustomFieldPDF("INSCRIÇÃO MUNICIPAL", 0.85, 5.08, 0.25, 25.06 , 10)]
        public string InscricaoMunicipal { get; set; }

        [CustomFieldPDF("VALOR TOTAL DOS SERVIÇOS",  0.85, 5.08, 5.33, 25.06 , 10)]
        public string ValorServicos { get; set; }

        [CustomFieldPDF("BASE DE CÁLCULO DO ISSQN", 0.85, 5.08, 10.41, 25.06, 10)]
        public string BaseCalcISSQN { get; set; }

        [CustomFieldPDF("VALOR DO ISSQN",  0.85, 5.33, 15.49, 25.06, 10)]
        public string ValorISSQN { get; set; }
    }
}
