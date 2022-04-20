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
    /// Grupo de Valores Totais da NF-e
    /// </summary>
    [Serializable]
    public class NFeTotalDTO
    {
        [Required(ErrorMessage = "O objeto ICMSTot é obrigatório")]
        [XmlElement("ICMSTot")]
        public NFeICMSTotalDTO ICMSTotal { get; set; }
    }
}
