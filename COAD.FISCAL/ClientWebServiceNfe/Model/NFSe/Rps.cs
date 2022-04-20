using COAD.FISCAL.Model.DTOCriptografia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class Rps
    {
        [Required(ErrorMessage = "Informe as informações da Rps")]
        public InfRps InfRps { get; set; }
        public SignatureDTO Signature { get; set; }
    }
}
