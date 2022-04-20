using COAD.FISCAL.Model.Enumerados;
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
    /// <summary>
    /// Destinatário
    /// </summary>
    public class NfeDestinoDTO : NfeInformacoesPessoaisDTO
    {
        [StringLength(14,  ErrorMessage = "O campo CNPJ deve possuir no máximo 14 caracteres")]
        public override string CNPJ { get; set; }
        
        [Required(ErrorMessage = "O objeto Endereco é obrigatório")]
        [XmlElement("enderDest")]
        public NfeEnderecoDestinatarioDTO Endereco { get; set; }

        /// <summary>
        /// Indicador da IE do Destinatário
        /// </summary>
        [Required(ErrorMessage = "O objeto indIEDest é obrigatório")]
        [XmlElement("indIEDest")]
        public TipoIEDestinatarioEnum TipoIEDestinatario { get; set; }

        /// <summary>
        /// Campo de informação obrigatória nos casos de emissão própria (procEmi = 0, 2 ou 3).
        /// </summary>
        [StringLength(13, MinimumLength = 2, ErrorMessage = "O campo [IE] Inscrição estadual (destinatário) deve possuir no mínimo 2 e no máximo 13 caracteres")]
        [TextValidator(ErrorMessage = "O Campo [IE] Inscrição estadual (destinatário) possui caracteres especiais.")]
        [XmlElement("IE")]
        public string IncricaoEstadual { get; set; }

        public bool ShouldSerializeIncricaoEstadual()
        {
            return !string.IsNullOrEmpty(IncricaoEstadual);
        }

        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo email deve possuir no máximo 60 caracteres")]
        [Required(ErrorMessage = "O E-Mail é obrigatório")]
        [XmlElement("email")]
        public string Email { get; set; }

    }
}
