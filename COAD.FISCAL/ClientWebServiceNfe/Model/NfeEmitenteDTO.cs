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
    /// Emitente
    /// </summary>
    public class NfeEmitenteDTO : NfeInformacoesPessoaisDTO
    {
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O campo NomeFantasia deve possuir no máximo 60 caracteres")]
        [XmlElement("xFant")]
        public string NomeFantasia { get; set; }


        [Required(ErrorMessage = "O objeto EnderecoEmitente é obrigatório")]
        [XmlElement("enderEmit")]
        public NfeEnderecoEmitenteDTO EnderecoEmitente { get; set; }

        /// <summary>
        /// Campo de informação obrigatória nos casos de emissão própria (procEmi = 0, 2 ou 3).
        /// </summary>
        [StringLength(13, MinimumLength = 2, ErrorMessage = "O campo [InscricaoEstadual] Inscrição estadual (emitente) deve possuir no mínimo 2 e no máximo 13 caracteres")]
        [TextValidator(ErrorMessage = "O Campo [InscricaoEstadual] Inscrição estadual (emitente) possui caracteres especiais.")]
        [XmlElement("IE")]
        public virtual string InscricaoEstadual { get; set; }

        public int CRT { get { return 3; } set { /*throw new InvalidOperationException("Esse campo não pode ser alterado. É somente para consulta"); */} }


    }
}
