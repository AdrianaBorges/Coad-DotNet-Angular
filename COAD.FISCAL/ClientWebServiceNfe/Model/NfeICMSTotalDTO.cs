using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    [Serializable]
    public class NFeICMSTotalDTO
    {
        /// <summary>
        /// Base de Cálculo do ICMS
        /// </summary>
        [Required(ErrorMessage = "O campo vBC é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vBC deve não pode exceder 999999999999999")]
        [XmlElement("vBC")]
        public decimal BaseCalculoICMS { get; set; }


        /// <summary>
        /// Valor Total do ICMS
        /// </summary>
        [Required(ErrorMessage = "O campo vICMS é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vICMS deve não pode exceder 999999999999999")]
        [XmlElement("vICMS")]
        public decimal ValorICMS { get; set; }

        [Required(ErrorMessage = "O campo ICMSDesoneracao é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ICMSDesoneracao deve não pode exceder 999999999999999")]
        [XmlElement("vICMSDeson")]
        public decimal ICMSDesoneracao { get; set; }

        [Required(ErrorMessage = "O campo ValorTotalFCP é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorTotalFCP deve não pode exceder 999999999999999")]
        [XmlElement("vFCP")]
        public decimal ValorTotalFCP { get; set; }

        /// <summary>
        /// Base de Cálculo do ICMS ST
        /// </summary>
        [Required(ErrorMessage = "O campo BaseCalculoST é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo BaseCalculoST deve não pode exceder 999999999999999")]
        [XmlElement("vBCST")]
        public decimal BaseCalculoST { get; set; }


        /// <summary>
        /// Valor Total do ICMS ST
        /// </summary>
        [Required(ErrorMessage = "O campo ValorST é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorST deve não pode exceder 999999999999999")]
        [XmlElement("vST")]
        public decimal ValorST { get; set; }

        [Required(ErrorMessage = "O campo ValorTotalFCPST é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorTotalFCPST deve não pode exceder 999999999999999")]
        [XmlElement("vFCPST")]
        public decimal ValorTotalFCPST { get; set; }

        [Required(ErrorMessage = "O campo ValorTotalFCPSTRetido é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorTotalFCPSTRetido deve não pode exceder 999999999999999")]
        [XmlElement("vFCPSTRet")]
        public decimal ValorTotalFCPSTRetido { get; set; }

        /// <summary>
        /// Valor Total dos produtos e serviços
        /// </summary>
        [Required(ErrorMessage = "O campo TotalProduto é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo TotalProduto deve não pode exceder 999999999999999")]
        [XmlElement("vProd")]
        public decimal TotalProduto { get; set; }


        /// <summary>
        /// Valor Total do Frete
        /// </summary>
        [Required(ErrorMessage = "O campo vFrete é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vFrete deve não pode exceder 999999999999999")]
        [XmlElement("vFrete")]
        public decimal ValorFrete { get; set; }


        /// <summary>
        /// Valor Total do Seguro
        /// </summary>
        [Required(ErrorMessage = "O campo vSeg é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vSeg deve não pode exceder 999999999999999")]
        [XmlElement("vSeg")]
        public decimal TotalSeguro { get; set; }


        /// <summary>
        /// Valor Total do Desconto
        /// </summary>
        [Required(ErrorMessage = "O campo TotalDesconto é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo TotalDesconto deve não pode exceder 999999999999999")]
        [XmlElement("vDesc")]
        public decimal TotalDesconto { get; set; }


        /// <summary>
        /// Valor Total do II
        /// </summary>
        [Required(ErrorMessage = "O campo vII é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vII deve não pode exceder 999999999999999")]
        [XmlElement("vII")]
        public decimal ValorII { get; set; }


        /// <summary>
        /// Valor Total do IPI
        /// </summary>
        [Required(ErrorMessage = "O campo vIPI é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vIPI deve não pode exceder 999999999999999")]
        [XmlElement("vIPI")]
        public decimal ValorIPI { get; set; }

        [Required(ErrorMessage = "O campo ValorIPIDevolvido é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorIPIDevolvido deve não pode exceder 999999999999999")]
        [XmlElement("vIPIDevol")]
        public decimal ValorIPIDevolvido { get; set; }

        /// <summary>
        /// Valor do PIS
        /// </summary>
        [Required(ErrorMessage = "O campo ValorPIS é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorPIS deve não pode exceder 999999999999999")]
        [XmlElement("vPIS")]
        public decimal ValorPIS { get; set; }


        /// <summary>
        /// Valor do COFINS
        /// </summary>
        [Required(ErrorMessage = "O campo ValorCOFINS é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo ValorCOFINS deve não pode exceder 999999999999999")]
        [XmlElement("vCOFINS")]
        public decimal ValorCOFINS { get; set; }


        /// <summary>
        /// Outras Despesas acessórias
        /// </summary>
        [Required(ErrorMessage = "O campo vOutro é obrigatório")]
        [Range(0, 999999999999999, ErrorMessage = "O campo vOutro deve não pode exceder 999999999999999")]
        [XmlElement("vOutro")]
        public decimal ValorOutrasDespesas { get; set; }


        /// <summary>
        /// Valor Total da NF-e
        /// (
        /// (+) vProd (id:W07)
        /// (-) vDesc (id:W10)
        /// (+) vICMSST (id:W06)
        /// (+) vFrete (id:W09)
        /// (+) vSeg (id:W10)
        /// (+) vOutro (id:W15)
        /// (+) vII (id:W11)
        /// (+) vIPI (id:W12)
        /// (+) vServ (id:W19) (NT 2011/004)
        /// )
        /// </summary>
        [Required(ErrorMessage = "O campo TotalNotaFiscal é obrigatório")]
        [Range(1, 999999999999999, ErrorMessage = "O campo qCom deve não pode exceder 999999999999999")]
        [XmlElement("vNF")]
        public decimal TotalNotaFiscal { get; set; }
    }
}
