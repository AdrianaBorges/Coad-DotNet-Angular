using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons
{
    public class SmtpEmailDTO
    {
        public int? CodigoSmtpEmail { get; set; }

        public int? CodigoTipo { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

    }
}
