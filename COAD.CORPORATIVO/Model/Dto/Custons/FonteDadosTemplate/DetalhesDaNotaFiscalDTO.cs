using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate
{
    public class DetalhesDaNotaFiscalDTO
    {
        public string ClienteNome { get; set; }
        public int? EmpID { get; set; }
        public string EmpresaNome { get; set; }
        public string EmpresaCNPJ { get; set; }
        public int? NotaFiscalNumero { get; set; }
        public DateTime? NotaDataEmissao { get; set; }
        public string ProdutoNome { get; set; }
        public decimal? NotaValor { get; set; }
        public string PathArquivoNota { get; set; }
        public string Texto { get; set; }
        public decimal? ValorBruto { get; set; }
        public string Discriminacao { get; set; }
        public string LinkDetalhamento { get; set; }
        public string NotaChave { get; set; }
        public string CodVerificacao { get; set; }
        public string EmissorInscricaoMunicipal { get; set; }
        public string Serie { get; set; }
        public string NotaValorFormatado {
            get {
                if(NotaValor != null)
                {
                    var valor = StringUtil.FormatarDinheiro(NotaValor, false);
                    return valor;
                }
                return null;
            }
            private set { }
        }

        
        public string NotaDataEmissaoStr {
            get {

                if(NotaDataEmissao != null)
                {
                    return NotaDataEmissao.Value.ToString("dd/MM/yyyy");
                }
                return null;
            } private set { } }

    }
}
