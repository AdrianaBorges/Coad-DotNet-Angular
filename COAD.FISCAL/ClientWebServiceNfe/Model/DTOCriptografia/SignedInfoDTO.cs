using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTOCriptografia
{
    [Serializable]
    public class SignedInfoDTO
    {
        public CanonicalizationMethod CanonicalizationMethod { get; set; }
        public SignatureMethod SignatureMethod { get; set; }

        public Reference Reference { get; set; }


    }
}
