using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoCanhoto
    {

        [CustomFieldPDF("", 0.85, 16.10, 0.25, 0.42, 6)]
        public string Recebemos { get; set; }
     
        [CustomFieldPDF("DATA DE RECEBIMENTO", 0.85, 4.10, 0.25, 1.27, 6)]
        public string DataRecebimento { get; set; }

        [CustomFieldPDF("IDENTIFICAÇÃO E ASSINATURA DO RECEBEDOR", 0.85, 12, 4.35, 1.27, 6)]
        public string IdenficacaoAssinatura { get; set; }
             
        [CustomFieldPDF("                                 NF-e", 1.70, 4.50, 16.35, 0.42, 6)]
        public string NumeroSerieNfe { get; set; }

        [CustomFieldPDF("", 1.70, 4.50, 17.65, 0.42, 12, false)]
        public string NumeroNfe { get; set; }

        [CustomFieldPDF("", 1.70, 4.50, 17.65, 1.00, 12, false)]
        public string SerieNfe { get; set; }

    }
}
