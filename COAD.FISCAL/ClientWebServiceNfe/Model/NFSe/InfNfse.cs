using COAD.FISCAL.Model.NFSe.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    public class InfNfse
    {
        [XmlAttribute]
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe o Número da informação da Nfse")]
        public int? Numero { get; set; }

        [Required(ErrorMessage = "Informe o Código de Verificação")]
        public string CodigoVerificacao { get; set; }


        [Required(ErrorMessage = "Informe a Data de Emissão")]
        [XmlIgnore]
        public DateTime DataEmissaoDateTime { get; set; }

        public string DataEmissao
        {

            get
            {

                if (DataEmissaoDateTime != null)
                {
                    return DataEmissaoDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DataEmissaoDateTime = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", provider);
                }
            }
        }

        public IdentificacaoRps IdentificacaoRps { get; set; }


        [Required(ErrorMessage = "Informe a Data de DataEmissaoRps")]
        [XmlIgnore]
        public DateTime DataEmissaoRpsDateTime { get; set; }

        public string DataEmissaoRps
        {

            get
            {

                if (DataEmissaoRpsDateTime != null)
                {
                    return DataEmissaoRpsDateTime.ToString("yyyy-MM-dd");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DataEmissaoRpsDateTime = DateTime.ParseExact(value, "yyyy-MM-dd", provider);
                }
            }
        }

        [Required(ErrorMessage = "Informe a Natureza da Operação")]
        public TipoNaturezaOperacaoEnum NaturezaOperacao { get; set; }
        public RegimeEspecialTributacaoEnum? RegimeEspecialTributacao { get; set; }

        [Required(ErrorMessage = "Informe se é optante simples nacional ou não")]
        public IdenSimNaoEnum OptanteSimplesNacional { get; set; }

        [Required(ErrorMessage = "Informe se é incentivador a cultura ou não")]
        public IdenSimNaoEnum IncentivadorCultural { get; set; }

        [Required(ErrorMessage = "Informe a Data de Competência")]
        [XmlIgnore]
        public DateTime CompetenciaDateTime { get; set; }

        public string Competencia
        {
            get
            {
                if (CompetenciaDateTime != null)
                {
                    return CompetenciaDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    CompetenciaDateTime = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", provider);
                }
            }
        }

        public string NfseSubstituida { get; set; }
        public string OutrasInformacoes { get; set; }

        [Required(ErrorMessage = "Informe os Dados de Serviço")]
        public DadosServicoRps Servico { get; set; }
        public decimal? ValorCredito { get; set; }

        [Required(ErrorMessage = "Informe os Dados do Prestador de Serviço")]
        public DadosPrestadorRps PrestadorServico { get; set; }

        [Required(ErrorMessage = "Informe os Dados do Tormador")]
        public DadosTomadorRps TomadorServico { get; set; }
        public IdentificacaoIntermediarioServiceRps IntermediarioServico { get; set; }

        [Required(ErrorMessage = "Informe a Identificacao do Orgao Gerador")]
        public IdentificacaoOrgaoGeradorRps OrgaoGerador { get; set; }
        public DadosConstrucaoCivilRps ConstrucaoCivil { get; set; }

    }
}
