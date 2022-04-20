using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.DTOCriptografia
{
    [Serializable]
    public class SignatureDTO
    {
        public SignedInfoDTO SignedInfo { get; set; }
        public string SignatureValue { get; set; }
        public KeyInfo KeyInfo { get; set; }
    }
}
