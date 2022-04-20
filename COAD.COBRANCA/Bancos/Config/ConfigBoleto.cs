
using COAD.COBRANCA.Boleto.Service;
using COAD.COBRANCA.Bancos.Service;
using COAD.COBRANCA.Model.DTO;
using System.Collections.Generic;
using COAD.COBRANCA.Bancos.Model.DTO.Interfaces;
using Coad.Reflection;
using System.Reflection;
using System.Linq;
using System;

namespace COAD.COBRANCA.Bancos.Config
{
    public static class ConfigBoleto
    {
        public static ICollection<IBanco> LstBancos { get; set; } = new List<IBanco>();
        public static void Configurar()
        {
            //ConfigBoletoSRV.RegistrarRegra<RegrasBradescoCarteira4>(new BancoBradesco(), "04");
            LstBancos.Clear();
            var lstRetorno = ReflectionProvider.FindInNamespaces<IBanco>
                (Assembly.GetExecutingAssembly(), "COAD.COBRANCA.Bancos.Service");

            if (lstRetorno != null)
            {
                foreach(var type in lstRetorno)
                {
                    var banco = Activator.CreateInstance(type);

                    if(banco is IBanco)
                        LstBancos.Add(banco as IBanco);
                }
            }
        }

        public static IBanco GetBanco(string banId)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(banId))
                {
                    var banco = LstBancos.Where(x => x.CodigoBanco == banId).FirstOrDefault();
                    return banco;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception($"Não foi possível obter as configurações/serviço do banco {banId}", e);
            }
        }
    }
}
