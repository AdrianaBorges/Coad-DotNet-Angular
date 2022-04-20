using COAD.COBRANCA.Boleto.RegrasImpl.Bradesco;
using COAD.COBRANCA.Model.DTO.Banco;
using COAD.COBRANCA.Model.Enumerados;
using COAD.COBRANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COBRANCA.Config
{
    public static class ConfigBoleto
    {
        public static void Configurar()
        {
            ConfigBoletoSRV.RegistrarRegra<RegrasBradescoCarteira4>(new BancoBradesco(), "04");
        }
    }
}
