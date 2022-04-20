using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    public class EnderecoContribuinte
    {
        /// <summary>
        /// Logradouro
        /// </summary>
        [Required(ErrorMessage = "O campo Logradouro é obrigatório")]
        [XmlElement("xLgr")]
        public string Logradouro { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [Required(ErrorMessage = "O campo Numero é obrigatório")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo Numero deve possuir no máximo 60 caracteres")]
        [XmlElement("nro")]
        public string Numero { get; set; }

        /// <summary>
        /// Complemento
        /// </summary>
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo Complemento deve possuir no máximo 60 caracteres")]
        [XmlElement("xCpl")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo Bairro (Bairro) é obrigatório")]
        [XmlElement("xBairro")]
        public string Bairro { get; set; }

        /// <summary>
        /// Código do município do IBGE
        /// </summary>
        [Required(ErrorMessage = "O campo CodigoMunicipio é obrigatório")]
        [XmlElement("cMun")]
        public int? CodigoMunicipio { get; set; }

        /// <summary>
        /// Nome do município
        /// </summary>
        [Required(ErrorMessage = "O campo xMun é obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O campo xMun deve possuir no mínimo 2 e no máximo 60 caracteres")]
        [XmlElement("xMun")]
        public string Municipio { get; set; }

        public string CEP { get; set; }
    }
}
