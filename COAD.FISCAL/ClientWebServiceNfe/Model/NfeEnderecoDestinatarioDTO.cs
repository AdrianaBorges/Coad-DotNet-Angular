using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeEnderecoDestinatarioDTO : NfeEnderecoDTO
    {

        /// <summary>
        /// Código do Pais
        /// </summary>
        [Range(1, 9999, ErrorMessage = "O campo cPais deve possuir até 4 dígitos")]
        [XmlElement("cPais")]
        public int? CodigoPais { get; set; }

        /// <summary>
        /// Nome do Pais
        /// </summary>[Required(ErrorMessage = "O campo UF é obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O campo xPais deve possuir no mínimo 2 caracteres e no máximo 60 caracteres")]
        [XmlElement("xPais")]
        public string Pais { get; set; }

        [StringLength(14, MinimumLength = 6, ErrorMessage = "O campo Telefone deve possuir no mínimo 6 e no máximo 14 caracteres")]
        [Required(ErrorMessage = "Informe o Telefone")]
        [XmlElement("fone")]
        public string Telefone { get; set; }

    }
}
