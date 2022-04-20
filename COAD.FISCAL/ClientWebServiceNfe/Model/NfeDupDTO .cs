using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeDupDTO
    {

        /// <summary>
        /// Numero da duplicata
        /// </summary>
        [StringLength(3, MinimumLength = 3, ErrorMessage = "O campo nDup deve possuir 3 caracteres")]
        [XmlElement("nDup")]
        public string NDup { get; set; }

        /// <summary>
        /// Data de vencimento da duplicata
        /// </summary>
        [Required(ErrorMessage = "O campo data de vencimento é obrigatório")]
        [XmlElement("dVenc")]
        public DateTime DVenc { get; set; }

        /// <summary>
        /// Valor duplicata
        /// </summary>
        [Required(ErrorMessage = "O valor da duplicata é obrigatório")]
        [XmlElement("vDup")]
        public decimal VDup { get; set; }

    }
}
