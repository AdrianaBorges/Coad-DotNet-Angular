using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    [Serializable]
    public class NfeInformacoesAdicionaisDTO
    {

        /// <summary>
        /// Informações Adicionais de Interesse do Fisco
        /// </summary>
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "O campo Informações Adicionais de Interesse do Fisco deve possuir no máximo 2000 caracteres")]
        [XmlElement("infAdFisco")]
        public string infAdFisco { get; set; }

        /// <summary>
        /// Informações Complementares de interesse do Contribuinte
        /// </summary>
        [StringLength(5000, MinimumLength = 1, ErrorMessage = "O campo Informações Complementares de interesse do Contribuinte deve possuir no máximo 5000 caracteres")]
        [XmlElement("infCpl")]
        public string InformacoesComplementares { get; set; }


    }
}
