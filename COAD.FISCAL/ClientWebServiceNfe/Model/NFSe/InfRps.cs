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
    public class InfRps
    {
        [XmlAttribute]
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe a Identificação da Rps")]
        public IdentificacaoRps IdentificacaoRps { get; set; }

        [Required(ErrorMessage = "Informe a Data de Emissão")]
        [XmlIgnore]
        public DateTime  DataEmissaoDateTime { get; set; }

        public string DataEmissao
        {

            get
            {

                if (DataEmissaoDateTime != null)
                {
                    return DataEmissaoDateTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DataEmissaoDateTime = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", provider);
                }
            }
        }

        [Required(ErrorMessage = "Informe a Natureza da Operação")]
        public TipoNaturezaOperacaoEnum NaturezaOperacao { get; set; }
        public RegimeEspecialTributacaoEnum? RegimeEspecialTributacao { get; set; }

        [Required(ErrorMessage = "Informe se o optante Simples Nacional ou não")]
        public IdenSimNaoEnum  OptanteSimplesNacional { get; set; }

        [Required(ErrorMessage = "Informe se é Incentivador a Cultura ou não")]
        public IdenSimNaoEnum IncentivadorCultural { get; set; }

        [Required(ErrorMessage = "Informe o Status")]
        public StatusRpsEnum Status { get; set; }

        public IdentificacaoRps RpsSubstituido { get; set; }

        [Required(ErrorMessage = "Informe o Serviço")]
        public DadosServicoRps Servico { get; set; }

        [Required(ErrorMessage = "Informe o Prestador")]
        public IdentificacaoPrestadorRps Prestador { get; set; }

        [Required(ErrorMessage = "Informe o Tomador")]
        public DadosTomadorRps Tomador { get; set; }
        public IdentificacaoIntermediarioServiceRps IntermediarioServico { get; set; }
        public DadosConstrucaoCivilRps ConstrucaoCivil { get; set; }

        public bool ShouldSerializeRegimeEspecialTributacao()
        {
            return (RegimeEspecialTributacao.HasValue);
        }


    }
}
