using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using COAD.FISCAL.Model.DTO;

namespace COAD.FISCAL.Model
{
    [Serializable]
    public class InfoNfeDTO 
    {
        public InfoNfeDTO()
        {
            Detalhamento = new List<NFeDetalhamentoItem>();
        }

        /// <summary>
        /// Atributo
        /// </summary>
        [StringLength(4, MinimumLength = 1, ErrorMessage = "A versão devem conter 4 dígitos")]
        [RegularExpression(@"\d{1,2}.\d{2}", ErrorMessage = "O formato da versão deve ser [N.00]")]
        [Required(ErrorMessage = "O campo versão é obrigatório")]
        [XmlAttribute("versao")]
        public string Versao {get; set;}

        /// <summary>
        /// Atributo
        /// </summary>
        [Required(ErrorMessage = "O campo Id é obrigatório")]
        [RegularExpression(@"NFe[0-9]{44}", ErrorMessage = "Id da nfe está fora do padrão especificado pela Sefaz. Formato 'NFe[0-9]{44}'")]
        [XmlAttribute]
        public string Id { get; set; }

        /// <summary>
        /// Identificação
        /// </summary>
        [Required(ErrorMessage = "O objeto ide é obrigatório")]
        [XmlElement("ide")]
        public NfeIdentificacaoDTO Identificacao { get; set; }

        /// <summary>
        /// Emitente
        /// </summary>
        [Required(ErrorMessage = "O objeto emit é obrigatório")]
        [XmlElement("emit")]
        public NfeEmitenteDTO Emitente { get; set; }

        /// <summary>
        /// Destinatário
        /// </summary>
        [Required(ErrorMessage = "O objeto dest é obrigatório")]
        [XmlElement("dest")]
        public NfeDestinoDTO Destino { get; set; }

        [Required(ErrorMessage = "O det é obrigatório")]
        [XmlElement("det")]
        public List<NFeDetalhamentoItem> Detalhamento { get; set; }

        /// <summary>
        /// Grupo de Valores Totais da NF-e
        /// </summary>
        [Required(ErrorMessage = "O objeto total é obrigatório")]
        [XmlElement("total")]
        public NFeTotalDTO Total { get; set; }

        /// <summary>
        /// Grupo de Informações do Transporte da NF-e
        /// </summary>
        [Required(ErrorMessage = "O objeto total é obrigatório")]
        [XmlElement("transp")]
        public NfeInfoTransporteDTO Transporte { get; set; }

        [XmlElement("cobr")]
        public TNFeInfNFeCobr Cobranca { get; set; }

        [XmlElement("pag")]
        public InformacoesPagamento InformacoesPagamento { get; set; }
        
        /// <summary>
        /// Grupo de Informações Adicionais
        /// </summary>
        [XmlElement("infAdic")]
        public NfeInformacoesAdicionaisDTO InformacoesAdicionais { get; set; }

    }
}
