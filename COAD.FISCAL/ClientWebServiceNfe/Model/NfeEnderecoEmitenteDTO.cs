using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeEnderecoEmitenteDTO : NfeEnderecoDTO
    {
        /// <summary>
        /// Código do Pais, Código Fixo 1058.
        /// </summary>
        [XmlElement("cPais")]
        public int? CodigoPais { get { return 1058; } set { /*throw new InvalidOperationException("Esse campo não pode ser alterado. É somente para consulta"); */} }

        /// <summary>
        /// Nome do Pais. Nome Fixo "Brasil"
        /// </summary>
        [XmlElement("xPais")]
        public string Pais { get { return "Brasil"; } set { /*throw new InvalidOperationException("Esse campo não pode ser alterado. É somente para consulta"); */} }


        [StringLength(14, MinimumLength = 6, ErrorMessage = "O campo CEP deve no mínimo 6 e no máximo 14 caracteres")]
        [XmlElement("fone")]
        public string Telefone { get; set; }

    }
}
