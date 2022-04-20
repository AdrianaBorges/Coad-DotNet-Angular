using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoDestinatarioRemetente
    {

        [CustomFieldPDF("DESTINATÁRIO/REMETENTE", 0.42, 3.30, 0.25, 7.81, 10, false)]
        public string DestinatarioRemetente { get; set; }

        [CustomFieldPDF("RAZÃO SOCIAL", 0.85, 12.32, 0.25, 8.11, 10)]
        public string RazaoSocial { get; set; }

        [CustomFieldPDF("CNPJ/CPF", 0.85, 5.33, 12.57, 8.11, 10)]
        public string CNPJ { get; set; }

        [CustomFieldPDF("DATA DA EMISSÃO", 0.85, 2.92, 17.91, 8.11, 10)]
        public string DataEmissao { get; set; }

        [CustomFieldPDF("ENDEREÇO", 0.85, 10.16, 0.25, 8.96, 10)]
        public string Endereco { get; set; }

        [CustomFieldPDF("BAIRRO/DISTRITO", 0.85, 4.83, 10.41, 8.96, 10)]
        public string BairroDistrito { get; set; }

        [CustomFieldPDF("CEP", 0.85, 2.67, 15.24, 8.96, 10)]
        public string CEP { get; set; }

        [CustomFieldPDF("DATA DA ENTRADA/SAÍDA", 0.85, 2.92, 17.91, 8.96, 10)]
        public string DataEntradaSaida { get; set; }

        [CustomFieldPDF("MUNICÍPIO", 0.85, 7.11, 0.25, 9.81, 10)]
        public string Municipio { get; set; }

        [CustomFieldPDF("FONE/FAX", 0.85, 4.06, 7.36, 9.81, 10)]
        public string FoneFax { get; set; }

        [CustomFieldPDF("UF", 0.85, 1.14, 11.42, 9.81, 10)]
        public string UF { get; set; }

        [CustomFieldPDF("INSCRIÇÃO ESTADUAL", 0.85, 5.33, 12.56, 9.81, 10)]
        public string InscricaoEstadual { get; set; }

        [CustomFieldPDF("HORA DA ENTRADA/SAÍDA", 0.85, 2.92, 17.91, 9.81, 10)]
        public string HoraEntradaSaida { get; set; }

    }
}
