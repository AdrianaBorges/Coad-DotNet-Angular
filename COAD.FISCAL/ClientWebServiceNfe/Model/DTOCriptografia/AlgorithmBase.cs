using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.DTOCriptografia
{
    public class AlgorithmBase
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
}
