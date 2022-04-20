using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoCalculoImposto
    {
        [CustomFieldAlinPDF(37)] 
        [CustomFieldPDF("CÁLCULO DO IMPOSTO", 0.42, 5.60, 0.25, 12.01, 7, false)]
        public string CalculoImposto {get; set;}

        [CustomFieldAlinPDF(37)]
        [CustomFieldPDF("BASE DE CÁLCULO DO ICMS", 0.85, 4.06, 0.25, 12.31, 10)]
        public string BaseCalcIcms { get; set; }

        [CustomFieldAlinPDF(37)]
        [CustomFieldPDF("VALOR DO ICMS", 0.85, 4.06, 4.31, 12.31 , 10)]
        public string ValorIcms { get; set; }

        [CustomFieldAlinPDF(39)]
        [CustomFieldPDF("BASE DE CÁLCULO DO ICMS ST", 0.85, 4.06, 8.37 ,12.31 , 10)]
        public string BaseCalcIcmsST { get; set; }

        [CustomFieldAlinPDF(38)]
        [CustomFieldPDF("VALOR DO ICMS ST",0.85, 4.06, 12.43, 12.31, 10)]
        public string ValorIcmsST { get; set; }

        [CustomFieldAlinPDF(40)]
        [CustomFieldPDF("VALOR TOTAL DOS PRODUTOS",0.85, 4.32, 16.49, 12.31 , 10)]
        public string ValorProdutos { get; set; }

        [CustomFieldAlinPDF(31)]
        [CustomFieldPDF("VALOR DO FRETE", 0.85, 3.30, 0.25, 13.16 , 10)]
        public string ValorFrete { get; set; }

        [CustomFieldAlinPDF(31)]
        [CustomFieldPDF("VALOR DO SEGURO",0.85, 3.30 ,3.55, 13.16 , 10)]
        public string ValorSeguro { get; set; }

        [CustomFieldAlinPDF(32)]
        [CustomFieldPDF("DESCONTO",0.85, 3.30, 6.85, 13.16, 10)]
        public string ValorDesconto { get; set; }

        [CustomFieldAlinPDF(36)]
        [CustomFieldPDF("OUTRAS DESPESAS ACESSÓRIAS", 0.85, 3.84 ,10.15, 13.16 , 10)]
        public string ValorOutrasDesp { get; set; }

        [CustomFieldAlinPDF(31)]
        [CustomFieldPDF("VALOR DO IPI",0.85, 3.30 ,14.00, 13.16, 10)]
        public string ValorIPI { get; set; }

        [CustomFieldAlinPDF(31)]
        [CustomFieldPDF("VALOR TOTAL DA NOTA ",0.85, 3.50, 17.30, 13.16, 10)]
        public string ValorTotalNota { get; set; }
    }
}
