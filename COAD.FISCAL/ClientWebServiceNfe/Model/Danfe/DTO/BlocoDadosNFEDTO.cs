using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.Danfe.Anotacao;

namespace COAD.FISCAL.Model.Danfe.DTO
{
    public class BlocoDadosNFE
    {

      [CustomFieldPDF("", 3.43, 9.10, 0.25, 2.54, 7)]
      public string IdentificacaoEmitente { get; set; }

      [CustomFieldPDF("", 3.40, 9.10, 0.25, 3.30, 12, false,7)]
      public string emiRazaoSocial { get; set; }

      [CustomFieldPDF("", 3.40, 9.10, 0.25, 3.55, 7, false,7)]
      public string emiEndereco { get; set; }

      [CustomFieldPDF("", 3.40, 9.10, 0.25, 3.80, 7, false,7)]
      public string emiMunicipio { get; set; }

      [CustomFieldPDF("", 3.40, 9.10, 0.25, 4.05, 7, false,7)]
      public string emiFoneFax { get; set; }

      //-------------------------------------------------------

      [CustomFieldPDF("", 3.43, 3.49, 9.34, 2.54, 10)]
      public string DescricaoDanfe { get; set; }

      [CustomFieldPDF("    DANFE", 3.20, 20.10, 10, 2.70, 12, false)]
      public string emiDanfe01{ get; set; }

      [CustomFieldPDF("Documento Auxiliar da Nota", 3.96, 20.10, 9.39, 3.10, 8, false)]
      public string emiDanfe02 { get; set; }

      [CustomFieldPDF("        Fiscal Eletrônica", 3.96, 20.10, 9.39, 3.39, 8, false)]
      public string emiDanfe03 { get; set; }
      
      [CustomFieldPDF("0 - Entrada", 3.96, 20.10, 9.39, 3.80, 8, false)]
      public string emiEntrada { get; set; }

      [CustomFieldPDF("1 - Saída ", 3.96, 20.10, 9.39, 4.15, 8, false)]
      public string emiSaida { get; set; }

      [CustomFieldPDF(" ", 0.59, 0.51, 12, 3.80, 8)]
      public string emiOpcao { get; set; }
        
      [CustomFieldPDF("", 3.92, 20.10, 9.42, 4.20, 10, false)]
      public string emiNumNF{ get; set; }

      [CustomFieldPDF("", 3.92, 20.10, 9.42, 4.56, 10, false)]
      public string emiSerieNF{ get; set; }

      [CustomFieldPDF("", 3.70, 20.10, 9.42, 5.12, 10, false)]
      public string emiPagina { get; set; }


        //-------------------------------------------------------

      
      [CustomFieldPDF("PROTOCOLO DE AUTORIZAÇÃO DE USO", 0.85, 8.00, 12.83, 5.99, 10)]
      public string ProtocoloAutorizacao { get; set; }

      [CustomFieldPDF("", 1.33, 8.00, 12.83, 4.64, 10, true, 4)]
      public string InformacaoNF { get; set; }
    
      [CustomFieldPDF("CONTROLE DO FISCO", 1.25, 8.00, 12.83, 2.54, 10, true, 2)]
      public string CodigoBarras { get; set; }

      [CustomFieldPDF("CHAVE DE ACESSO", 0.85, 8.00, 12.83, 3.80, 8, true, 3)]
      public string ChaveAcesso { get; set; }

      [CustomFieldPDF("NATUREZA DA OPERAÇÃO", 0.85, 12.59, 0.25, 5.99, 10)]
      public string NaturezaOperacao { get; set; }

      [CustomFieldPDF("INSCRIÇÃO ESTADUAL", 0.85, 6.86, 0.25, 6.84, 10)]
      public string InscricaoEstadual { get; set; }

      [CustomFieldPDF("INSCRIÇÃO ESTADUAL DE SUBST.", 0.85, 6.86, 7.11, 6.84, 10)]
      public string InscricaoEstadualST { get; set; }

      [CustomFieldPDF("CNPJ/CPF", 0.85, 6.86, 13.97, 6.84, 10)]
      public string CNPJEmitente { get; set; }
    }
}
