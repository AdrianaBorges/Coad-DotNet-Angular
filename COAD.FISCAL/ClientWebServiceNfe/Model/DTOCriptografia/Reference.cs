using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.DTOCriptografia
{
    [Serializable]
    public class Reference
    {
        [XmlAttribute]
        public string URI { get; set; }
        public List<Transform> Transforms { get; set; }
        public DigestMethod DigestMethod { get; set; }
        public string DigestValue { get; set; }
    }
}
