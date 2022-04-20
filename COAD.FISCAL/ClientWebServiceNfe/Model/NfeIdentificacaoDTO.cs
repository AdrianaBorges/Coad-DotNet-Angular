using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    /// <summary>
    /// Grupo das informações de identificação da NF-e
    /// </summary>
    [Serializable]
    public class NfeIdentificacaoDTO 
    {
        /// <summary>
        /// Código da UF do emitente do Documento Fiscal
        /// </summary>
        [Required(ErrorMessage = "O campo CodigoUFEmitente é obrigatório")]
        [Range(1,99, ErrorMessage = "O campo CodigoUFEmitente deve ter no mínimo 1 e no máximo 2 dígitos")]
        [XmlElement("cUF")]
        public int? CodigoUFEmitente { get; set; }

        /// <summary>
        /// Código Numérico que compõe a Chave de Acesso
        /// </summary>
        [Required(ErrorMessage = "O campo CodigoNumerico (Número da Nota Fiscal) é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O campo CodigoNumerico deve possuir 8 caracteres")]
        [XmlElement("cNF")]
        public string CodigoNumerico { get; set; }

        /// <summary>
        /// Descrição da Natureza da Operação
        /// </summary>
        [Required(ErrorMessage = "O campo NaturezaOperacao (Natureza Operação) é obrigatório")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo NaturezaOperacao deve possuir no máximo 60 caracteres")]
        [XmlElement("natOp")]
        public string NaturezaOperacao { get; set; }

        /// <summary>
        /// Indicador da forma de pagamento
        /// </summary>
        //[Required(ErrorMessage = "O objeto IndicacaoPagamento é obrigatório")]
        //[XmlElement("indPag")]
        //public IndPagEnum IndicacaoPagamento { get; set; }

        /// <summary>
        /// Código do Modelo do Documento Fiscal
        /// </summary>
        [XmlElement("mod")]
        public string ModeloDocumentoFiscal { get { return "55"; } 
            set {
                //throw new InvalidOperationException("O valor deste campo é fixo. Não deve ser alterado.");
            } 
        }

        /// <summary>
        /// Série do Documento Fiscal
        /// </summary> /// Verificar os tipos possíveis
        [Required(ErrorMessage = "O campo Serie é obrigatório")]
        [Range(1, 999, ErrorMessage = "O campo Serie deve possuir no máximo 3 dígitos")]
        [XmlElement("serie")]
        public int? Serie { get; set; }


        /// <summary>
        /// Número do Documento Fiscal
        /// </summary>
        [Required(ErrorMessage = "O campo NumeroNotaFiscal é obrigatório")]
        [Range(1, 999999999999999, ErrorMessage = "O campo NumeroNotaFiscal deve não pode exceder 999999999999999")]
        [XmlElement("nNF")]
        public int? NumeroNotaFiscal { get; set; }

        /// <summary>
        /// Data de emissão do Documento Fiscal (Formato “AAAA-MM-DD”)
        /// </summary>
        [XmlIgnore]
        [Required(ErrorMessage = "O campo DataDeEmissao é obrigatório")]
        public DateTime DataDeEmissao { get; set; }

        /// <summary>
        /// Data de Saída ou da Entrada da Mercadoria/Produto (Formato “AAAA-MM-DD”)
        /// </summary>
        [XmlIgnore]
        public DateTime DataEntradaSaida { get; set; }

        
        public string dhEmi { 
            
            get {

                if (DataDeEmissao != null)
                {
                    return DataDeEmissao.ToString("yyyy-MM-ddTHH:mm:sszzz");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DataDeEmissao = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", provider);
                }
            }
        }

        public string dhSaiEnt
        {
            get
            {
                if (DataEntradaSaida != null)
                {
                    return DataEntradaSaida.ToString("yyyy-MM-ddTHH:mm:sszzz");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DataEntradaSaida = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", provider);
                }
            }
        }

        /// <summary>
        /// Tipo de Operação - 0-entrada / 1-saída
        /// </summary>
        [Required(ErrorMessage = "O objeto TipoOperacao é obrigatório")]
        [XmlElement("tpNF")]
        public TipoNotaFiscalEnum TipoOperacao { get; set; }

        /// <summary>
        /// Identificador de local de destino da operação
        /// </summary>
        [Required(ErrorMessage = "O objeto IndicadorLocalDestino é obrigatório")]
        [XmlElement("idDest")]
        public TipoLocalDestinoOperacaoEnum IndicadorLocalDestino { get; set; }
       
        /// <summary>
        /// Código do Município de Ocorrência do Fato Gerador
        /// </summary>
        [Required(ErrorMessage = "O campo CodigoDoMunicipio é obrigatório")]
        [Range(1, 9999999, ErrorMessage = "O campo CodigoDoMunicipio deve possuir no máximo 7 dígitos")]
        [XmlElement("cMunFG")]
        public int? CodigoDoMunicipio { get; set; }

        /// <summary>
        /// Formato de Impressão do DANFE - 1-Retrato/ 2-Paisagem
        /// </summary>
        [Required(ErrorMessage = "O objeto FormatoImpressao é obrigatório")]
        [XmlElement("tpImp")]
        public FormatoImpressaoEnum FormatoImpressao { get; set; }

        /// <summary>
        /// Tipo de Emissão da NF-e
        /// </summary>
        [Required(ErrorMessage = "O objeto TipoEmissao é obrigatório")]
        [XmlElement("tpEmis")]
        public TipoEmissaoEnum TipoEmissao { get; set; }

        /// <summary>
        /// Dígito Verificador da Chave de Acesso da NF-e
        /// </summary>
        [Required(ErrorMessage = "O campo DigitoVerificador é obrigatório")]
        [Range(0, 9, ErrorMessage = "O campo DigitoVerificador dever ser um número inteiro entre 0 e 9.")]
        [XmlElement("cDV")]
        public int? DigitoVerificador { get; set; }

        /// <summary>
        /// Identificação do Ambiente
        /// </summary>
        [Required(ErrorMessage = "O objeto TipoAmbiente é obrigatório")]
        [XmlElement("tpAmb")]
        public TipoAmbienteEnum TipoAmbiente { get; set; }

        /// <summary>
        /// Finalidade de emissão da NF-e
        /// </summary>
        [Required(ErrorMessage = "O objeto finNFe é obrigatório")]
        public FinalidadeNotaFiscalEnum finNFe { get; set; }

        public int indFinal { get { return 0; } set { } }

        [XmlElement("indPres")]
        public IndicacaoPresencaEnum IndicacaoPrensenca { get; set; }
        /// <summary>
        /// Valor Padrão Zero. Não precisa setar
        /// </summary>
        ///
        [XmlElement("procEmi")]
        public int? procEmi { get { return procEmitente; } set { procEmitente = value; } }

        private int? procEmitente = 0;
        /// <summary>
        /// Versão do Processo de emissão da NF-e
        /// </summary>
        [Required(ErrorMessage = "O campo verProc é obrigatório")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "O campo verProc deve possuir no máximo 20 caracteres")]
        [XmlElement("verProc")]
        public string VersaoProcesso { get; set; }

        [XmlElement("NFref")]
        public List<NFeReferenciadaDTO> NotasRefenciadas { get; set; }
        
    }
}
