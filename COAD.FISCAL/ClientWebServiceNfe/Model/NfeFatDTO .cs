using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeFatDTO
    {

        /// <summary>
        /// Numero do faturamento
        /// </summary>
        [StringLength(3, MinimumLength = 3, ErrorMessage = "O campo nFat deve possuir 3 caracteres")]
        [XmlElement("nFat")]
        public string NFat { get; set; }

        /// <summary>
        /// Valor original
        /// </summary>
        [Required(ErrorMessage = "O valor original é obrigatório")]
        [XmlElement("vOrig")]
        public decimal VOrig { get; set; }

        /// <summary>
        /// Valor de desconto
        /// </summary>
        [Required(ErrorMessage = "O valor do desconto é obrigatório")]
        [XmlElement("vDesc")]
        public decimal VDesc { get; set; }

        /// <summary>
        /// Valor liquido
        /// </summary>
        [Required(ErrorMessage = "O valor liquido é obrigatório")]
        [XmlElement("vLiq")]
        public decimal VLiq { get; set; }

    }
}
