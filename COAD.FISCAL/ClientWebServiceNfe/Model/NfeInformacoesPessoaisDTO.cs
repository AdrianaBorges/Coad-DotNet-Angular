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
    /// Grupo de identificação do emitente da NF-e
    /// </summary>
    [Serializable]
    //[XmlInclude(typeof(NfeIdentificacaoDTO))]
    public class NfeInformacoesPessoaisDTO 
    {
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O campo CNPJ deve possuir 14 caracteres")]
        public virtual string CNPJ { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "O campo CPF deve possuir 11 caracteres")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo xNome é obrigatório")]
        [TextValidator(ErrorMessage = "O Nome do Destinatário é inválido")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O campo xNome deve possuir no máximo 60 caracteres")]
        public string xNome { get; set; }        



    }
}
