using COAD.COBRANCA.Model.DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Model.DTO
{
    public class ContaDTO
    {
        public IBanco Banco { get; set; }
        public string CodigoAgencia { get; set; }
        public string NumeroConta { get; set; }
        public string CodigoConvenio { get; set; }
        public string CodigoCarteiraRemessa { get; set; }
        public string CodigoCarteiraBoleto { get; set; }
        public string CodigoCedenteRemessa { get; set; }
        public string CodigoCedenteBoleto { get; set; }

    }
}
