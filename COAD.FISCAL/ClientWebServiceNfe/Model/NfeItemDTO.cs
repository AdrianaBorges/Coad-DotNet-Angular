using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    [Serializable]
    public class NfeItemDTO 
    {
        /// <summary>
        /// Código do Produto
        /// </summary>
        [Required(ErrorMessage = "O campo CodigoProduto é obrigatório")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo CodigoProduto deve possuir no máximo 60 caracteres")]
        [XmlElement("cProd")]
        public string CodigoProduto { get; set; }

        [XmlIgnore]
        public string ean = "SEM GTIN";

        [MaxLength(14, ErrorMessage = "O campo eEan deve possuir no máximo 14 caracteres")]

        [XmlElement("cEAN")]
        public string CodigoEAN { get { return ean; } set { ean = value; } }

        public bool ShouldSerializeCodigoEAN()
        {
            if (ean == null)
            {
                ean = "SEM GTIN";
            }

            return true;
        }

        /// <summary>
        /// Descrição do Produto
        /// </summary>
        [Required(ErrorMessage = "O campo NomeProduto é obrigatório")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "O campo NomeProduto deve possuir no máximo 120 caracteres")]
        [XmlElement("xProd")]
        public string NomeProduto { get; set; }

        [Required(ErrorMessage = "O campo NCM é obrigatório")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "O campo NCM deve possuir no mínimo 2 no máximo 8 caracteres")]
        public string NCM { get; set; }

        /// <summary>
        /// Código Fiscal de Operações e Prestações
        /// </summary>
        [Required(ErrorMessage = "O campo CFOP é obrigatório")]
        [Range(0,9999,  ErrorMessage = "O campo CFOP deve possuir no máximo 4 dígitos")]
        public int? CFOP { get; set; }


        /// <summary>
        /// Unidade Comercial
        /// </summary>
        [Required(ErrorMessage = "O campo Unidade é obrigatório")]
        [StringLength(6,MinimumLength = 1, ErrorMessage = "O campo Unidade deve possuir no mínimo 1 e no máximo 4 caracteres")]
        [XmlElement("uCom")]
        public string Unidade { get; set; }

        /// <summary>
        /// Quantidade Comercial (Quantidade do produto)
        /// </summary>
        [Required(ErrorMessage = "O campo Quantidade é obrigatório")]
        [Range(1,999999999999999, ErrorMessage = "O campo Quantidade deve não pode exceder 999999999999999")]
        [XmlElement("qCom")]
        public decimal Quantidade { get; set; }

        /// <summary>
        /// Valor Unitário de comercialização (Valor unitário do produto)
        /// </summary>
        [Required(ErrorMessage = "O campo ValorUnitario (Valor unitário do produto) é obrigatório")]
        [Range(1, 999999999999999, ErrorMessage = "O campo ValorUnitario deve não pode exceder 999999999999999")]
        [XmlElement("vUnCom")]
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// Valor Total Bruto dos Produtos ou Serviços
        /// </summary>
        [Required(ErrorMessage = "O campo vProd é obrigatório")]
        [Range(1, 999999999999999, ErrorMessage = "O campo vProd deve não pode exceder 999999999999999")]
        [XmlElement("vProd")]
        public decimal ValorTotal { get; set; }

        [XmlIgnore]
        public string cEanTrib = "SEM GTIN";

        /// <summary>
        /// GTIN (Global Trade Item Number) da unidade tributável, antigo código EAN ou código de barras
        /// </summary>
        [StringLength(14, ErrorMessage = "O campo UnidadeTributavel deve possuir no máximo 14 caracteres")]
        [XmlElement("cEANTrib", IsNullable = true, Form = XmlSchemaForm.None)]
        public string TributacaoEAN { get { return cEanTrib; } set { cEanTrib = value; } }

        public bool ShouldSerializeTributacaoEAN()
        {
            if (cEanTrib == null)
            {
                cEanTrib = "SEM GTIN";
            }
            return true;
        }

        /// <summary>
        /// Unidade Tributável
        /// </summary>
        [Required(ErrorMessage = "O campo UnidadeTributavel é obrigatório")]
        [StringLength(6, ErrorMessage = "O campo UnidadeTributavel deve possuir no máximo 6 caracteres")]
        [XmlElement("uTrib")]
        public string UnidadeTributavel { get; set; }

        /// <summary>
        /// Quantidade Tributável (Repetir a quantidade)
        /// </summary>
        [Required(ErrorMessage = "O campo QuantidadeTributavel é obrigatório")]
        [Range(1, 999999999999999, ErrorMessage = "O campo QuantidadeTributavel deve não pode exceder 999999999999999")]
        [XmlElement("qTrib")]
        public decimal QuantidadeTributavel { get; set; }
        
        /// <summary>
        /// Valor Unitário de tributação (Repetir o valor unitário)
        /// Informar o valor unitário de tributação do produto, campo meramente informativo, o contribuinte pode utilizar a precisão desejada (0-10 decimais). 
        /// Para efeitos de cálculo, o valor unitário será obtido pela divisão do valor do produto pela quantidade tributável.
        /// </summary>
        [Required(ErrorMessage = "O campo vUnTrib é obrigatório")]
        [Range(1, 999999999999999, ErrorMessage = "O campo ValorUnitarioTributacao deve não pode exceder 999999999999999")]
        [XmlElement("vUnTrib")]
        public decimal ValorUnitarioTributacao { get; set; }


        /// <summary>
        /// Indica se valor do Item (vProd) entra no valor total da NF-e (vProd)
        /// 
        /// Este campo deverá ser preenchido com:
        /// 0 – o valor do item (vProd) não compõe o valor total da NF-e (vProd)
        /// 1 – o valor do item (vProd) compõe o valor total da NF-e (vProd) (v2.0)
        /// 
        /// </summary>
        [Required(ErrorMessage = "O campo indTot é obrigatório")]
        [Range(0,1 , ErrorMessage = "O campo indTot deve ter o valor 0 ou 1")]
        [XmlElement("indTot")]
        public short IndicacaoTotal { get; set; }

        [XmlIgnore]
        public decimal desconto { get; set; }

        [XmlElement("cBenef")]
        public string CodigoBeneficio { get; set; }


    }
}
