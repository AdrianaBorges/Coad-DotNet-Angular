using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoProdutoServico
    {
        [CustomFieldPDF("DADOS DO PRODUTO/SERVIÇO", 0.52, 5.60, 0.25, 17.02, 7, false, 8)]
        public string ProdutoServico { get; set; }

        [CustomFieldPDF("      CÓDIGO", 0.52, 1.80, 0.25, 17.33, 6, true, 8)] 
        public string Codigo { get; set; }

        [CustomFieldPDF("        DESCRIÇÃO DOS PRODUTOS/SERVIÇOS", 0.52, 5.48, 2.05, 17.33, 6, true, 8)] 
        public string Descricao { get; set; }

        [CustomFieldAlinPDF(10)] 
        [CustomFieldPDF("    NCM/SH", 0.52, 1.30, 7.53, 17.33, 6, true, 8)] 
        public string NcmSh { get; set; }

        [CustomFieldAlinPDF(05)]
        [CustomFieldPDF("  CST", 0.52, 0.85, 8.83, 17.33, 6, true, 8)] 
        public string Cst { get; set; }

        [CustomFieldAlinPDF(04)]
        [CustomFieldPDF(" CFOP", 0.52, 0.85, 9.68, 17.33, 6, true, 8)] 
        public string Cfop { get; set; }

        [CustomFieldPDF(" UNID.", 0.52, 0.85, 10.53, 17.33, 6, true, 8)] 
        public string Unid { get; set; }

        [CustomFieldAlinPDF(07)] 
        [CustomFieldPDF("  QTD.", 0.52, 0.85, 11.38, 17.33, 6, true, 8)] 
        public string Qtd { get; set; }

        [CustomFieldAlinPDF(13)] 
        [CustomFieldPDF("  VLR UNIT.", 0.52, 1.40, 12.23, 17.33, 6, true, 8)] 
        public string VlrUnit { get; set; }
        
        [CustomFieldAlinPDF(13)] 
        [CustomFieldPDF("VLR TOTAL", 0.52, 1.40, 13.63, 17.33, 6, true, 8)]
        public string VlrTotal { get; set; }

        [CustomFieldAlinPDF(15)] 
        [CustomFieldPDF("   BC.ICMS", 0.52, 1.40, 15.03, 17.33, 6, true, 8)]
        public string BcIcms { get; set; }

        [CustomFieldAlinPDF(15)] 
        [CustomFieldPDF(" VLR. ICMS", 0.52, 1.40, 16.43, 17.33, 6, true, 8)]
        public string VlrIcms { get; set; }

        [CustomFieldAlinPDF(16)] 
        [CustomFieldPDF("    VLR. IPI", 0.52, 1.40, 17.83, 17.33, 6, true, 8)]
        public string VlrIpi { get; set; }

        [CustomFieldPDF("ALÍQ.\n ICMS", 0.52, 0.80, 19.23, 17.33, 6, true, 8)]
        public string AliqIcms { get; set; }

        [CustomFieldPDF("ALÍQ.\n IPI", 0.52, 0.80, 20.03, 17.33, 6, true, 8)]
        public string AliqIpi { get; set; }
    }
}
