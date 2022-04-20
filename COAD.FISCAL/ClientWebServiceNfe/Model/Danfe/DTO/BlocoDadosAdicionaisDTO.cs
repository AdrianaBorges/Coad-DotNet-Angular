using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoDadosAdicionais
    {
      [CustomFieldPDF("DADOS ADICIONAIS", 0.42, 2.29, 0.25 ,26.01, 8, false)]
      public string DadosAdicionais { get; set; }

      [CustomFieldPDF("INFORMAÇÕES COMPLEMENTARES",  3.07, 12.93, 0.25, 26.33, 8, true, 1)]
      public string InformacoesComplementares { get; set; }

      [CustomFieldPDF("RESERVADO AO FISCO", 3.07, 7.62, 13.17, 26.33, 8)]
      public string ReservadoaoFisco { get; set; }

    }
}
