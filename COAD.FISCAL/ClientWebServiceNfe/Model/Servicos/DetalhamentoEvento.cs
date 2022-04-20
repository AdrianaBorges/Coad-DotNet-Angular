using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos
{

    public class DetalhamentoEvento
    {

        [StringLength(4, MinimumLength = 1, ErrorMessage = "A versão devem conter 4 dígitos")]
        [RegularExpression(@"\d{1,2}.\d{2}", ErrorMessage = "O formato da versão deve ser [N.00]")]
        [Required(ErrorMessage = "O campo versão é obrigatório")]
        [XmlAttribute]
        public string versao { get; set; }

        public string descEvento { get; set; }

        /// <summary>
        /// Informar o número do Protocolo de Autorização da NF-e a ser Cancelada. (
        /// </summary>
        
        [XmlElement("nProt")]
        public string NumeroProtocolo { get; set; }

        [XmlElement("xJust")]
        public string Justificativa { get; set; }

        [XmlElement("xCorrecao")]
        public string DescricaoCorrecao { get; set; }

        [XmlElement("xCondUso")]
        public string CondicaoDeUso { get; set; }
    }
}
