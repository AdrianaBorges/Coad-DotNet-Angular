using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoTransportador
    {
        [CustomFieldPDF("TRANSPORTADOR/VOLUMES TRANSPORTADOS", 0.42, 5.20, 0.25, 14.13, 10, false)]
        public string Transportador { get; set; }

        [CustomFieldPDF("RAZÃO SOCIAL", 0.85, 9.02, 0.25, 14.43, 10)]
        public string tranRazaoSocial { get; set; }

        [CustomFieldPDF("FRETE POR CONTA", 0.85, 2.79, 9.27, 14.43, 10)]
        public string FretePorContaDe { get; set; }

        [CustomFieldPDF("CÓDIGO ANTT", 0.85, 1.78, 12.06, 14.43, 10)]
        public string CodigoAntt { get; set; }

        [CustomFieldPDF("PLACA DO VEÍCULO", 0.85, 2.29, 13.84, 14.43, 10)]
        public string Placa { get; set; }

        [CustomFieldPDF("UF", 0.85, 0.76, 16.13, 14.43, 10)]
        public string PlacaUF { get; set; }

        [CustomFieldPDF("CNPJ/CPF", 0.85, 3.94, 16.89, 14.43, 10)]
        public string tranCNPJ { get; set; }

        [CustomFieldPDF("ENDEREÇO", 0.85, 9.02, 0.25, 15.28, 10)]
        public string tranEndereco { get; set; }

        [CustomFieldPDF("MUNICÍPIO", 0.85, 6.86, 9.27, 15.28, 10)]
        public string tranMunicipio { get; set; }

        [CustomFieldPDF("UF", 0.85, 0.76, 16.13, 15.28, 10)]
        public string tranUF { get; set; }

        [CustomFieldPDF("INSCRIÇÃO ESTADUAL", 0.85, 3.94, 16.89, 15.28, 10)]
        public string tranInscEstadual { get; set; }

        [CustomFieldPDF("QUANTIDADE", 0.85, 2.92, 0.25, 16.13, 10)]
        public string QtdeVolumes { get; set; }

        [CustomFieldPDF("ESPÉCIE", 0.85, 3.05, 3.17, 16.13, 10)]
        public string Especie { get; set; }

        [CustomFieldPDF("MARCA", 0.85, 3.05, 6.22, 16.13, 10)]
        public string Marca { get; set; }

        [CustomFieldPDF("NUMERAÇÃO", 0.85, 4.83, 9.27, 16.13, 10)]
        public string Numeracao { get; set; }

        [CustomFieldPDF("PESO BRUTO", 0.85, 3.43, 14.10, 16.13, 10)]
        public string PesoBruto { get; set; }

        [CustomFieldPDF("PESO LÍQUIDO", 0.85, 3.30, 17.53, 16.13, 10)]
        public string PesoLiquido { get; set; }
    }
}
