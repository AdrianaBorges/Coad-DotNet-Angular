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
    public class NFeDetalhamentoItem 
    {
        [XmlElement("prod")]
        public NfeItemDTO Produto { get; set; }
        
        [Required(ErrorMessage = "O objeto imposto é obrigatório")]
        [XmlElement("imposto")]
        public NFeImposto Imposto { get; set; }

        [XmlAttribute("nItem")]
        public int NumeroItem { get; set; }
    }
}
