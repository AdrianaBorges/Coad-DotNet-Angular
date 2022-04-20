using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    public class ListaMensagemRetornoNfse
    {
        [XmlElement]
        public List<MensagemRetornoNfse> MensagemRetorno { get; set; }
    }
}
