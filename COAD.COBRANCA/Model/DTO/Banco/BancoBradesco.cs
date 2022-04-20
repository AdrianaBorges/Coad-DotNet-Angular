using COAD.COBRANCA.Model.DTO.Interfaces;
using COAD.COBRANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Model.DTO.Banco
{
    public class BancoBradesco : IBanco
    {
        public string CodigoBanco { get; set; }
        public string NomeBanco { get; set; }

        public BancoBradesco()
        {
            this.NomeBanco = "Banco Bradesco S.A.";
            this.CodigoBanco = "237";
        }

        public string CalcularDVCodigoBarras(string codigoBarras)
        {
            var modulo = MathUtil.CalcularModuloCrescente(codigoBarras, 43, 11, 9);

            if(modulo != null)
            {
                var digito = 11 - modulo;
                if (digito == 0 || digito == 1 || digito > 9)
                    return "1";
                else
                    return digito.ToString();
            }
            return null;
        }
    }
}
