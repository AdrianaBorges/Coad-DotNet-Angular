using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeEnderecoDTO 
    {
        /// <summary>
        /// Logradouro
        /// </summary>
        [Required(ErrorMessage = "O campo Logradouro é obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O campo Logradouro deve possuir no mínimo 2 e no máximo 60 caracteres")]
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
        [TextValidator(ErrorMessage = "O Complemento não é válido. Verifique se existe algum caracter especial e remova-o.")]
        [XmlElement("xCpl")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo Bairro (Bairro) é obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O campo Bairro deve possuir no máximo 60 caracteres")]
        [XmlElement("xBairro")]
        public string Bairro { get; set; }

        /// <summary>
        /// Código do município do IBGE
        /// </summary>
        [Required(ErrorMessage = "O campo CodigoMunicipio é obrigatório")]
        [Range(1, 9999999, ErrorMessage = "O campo CodigoMunicipio deve possuir no máximo 7 dígitos")]
        [XmlElement("cMun")]
        public int? CodigoMunicipio { get; set; }

        /// <summary>
        /// Nome do município
        /// </summary>
        [Required(ErrorMessage = "O campo xMun é obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O campo xMun deve possuir no mínimo 2 e no máximo 60 caracteres")]
        [XmlElement("xMun")]
        public string Municipio { get; set; }

        [Required(ErrorMessage = "O campo UF é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo UF deve possuir 2 caracteres")]
        public string UF { get; set; }

        [Required(ErrorMessage = "O campo CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O campo CEP deve possuir 8 caracteres")]
        public string CEP { get; set; }

        public bool ShouldSerializeComplemento()
        {
            return (!string.IsNullOrWhiteSpace(Complemento));
        }
    }
}
