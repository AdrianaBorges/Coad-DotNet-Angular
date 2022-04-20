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
    public class NFeImpostoICMSGrupo 
    {
        [XmlElement("orig")]
        public int? Origem { get { return 0; } set { } }
        
        [Required(ErrorMessage = "O campo CST é obrigatório")]
        public int? CST { get; set; }
    }
}
