

using COAD.COBRANCA.Bancos.Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;

namespace COAD.CORPORATIVO.Model.Dto.Boleto
{
    public class BoletoDTO
    {
       
        public string Beneficiario { get; set; }
        public string AgenciaCodBeneficiario { get; set; }
        public string CodBeneficiario { get; set; }
        public string Especie { get; set; }
        public string EspecieDoc { get; set; }
        public string Bancologo { get; set; }
        public string Quantidade { get; set; }
        public string Carteira { get; set; }
        public string NossoNumero { get; set; }
        public string NumeroDocumento { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime DtVencimento { get; set; }
        public DateTime DtProcessamento { get; set; }
        public Nullable<decimal> ValorDocumento { get; set; }
        public string Aceite { get; set; }

        public string NomeCpfCnpj { get; set; }
        public string Endereco { get; set; }
        public string BairroCidadeCep { get; set; }

        public string SacadorAvalista { get; set; }
        public string SacadorEndereco { get; set; }

        public Nullable<decimal> VlrDescAbatimetos { get; set; }
        public Nullable<decimal> VlrOutrasDeducoes { get; set; }
        public Nullable<decimal> VlrMoraMulta { get; set; }
        public Nullable<decimal> VlrOutraAcrecimos { get; set; }
        public Nullable<decimal> ValorCobrado { get; set; }

        public string Instrucoes { get; set; }
        public string Instrucoes01 { get; set; }
        public string Instrucoes02 { get; set; }
        public string Instrucoes03 { get; set; }
        public string Instrucoes04 { get; set; }
        public string Instrucoes05 { get; set; }

        public string logo { get; set; }

        public string BarcodeImage { get; set; }
        public CodigoBarrasDTO CodigoBarras { get; set; }
        public LinhaDigitavelDTO LinhaDigivel { get; set; }

    }
}
