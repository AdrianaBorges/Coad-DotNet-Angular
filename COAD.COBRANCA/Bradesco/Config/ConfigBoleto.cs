
using COAD.COBRANCA.Boleto.Service;
using COAD.COBRANCA.Bradesco.Service;
using COAD.COBRANCA.Model.DTO;


namespace COAD.COBRANCA.Bradesco.Config
{
    public static class ConfigBoleto
    {
        public static void Configurar()
        {
            ConfigBoletoSRV.RegistrarRegra<RegrasBradescoCarteira4>(new BancoBradesco(), "04");
        }
    }
}
