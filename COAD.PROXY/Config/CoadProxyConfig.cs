using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.COADGED.Config;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.LEGADO.Config;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Config
{
    public class CoadProxyConfig
    {
        public static void Configurar()
        {
            ProfileConfigurator.addConfig("proxy", () =>
            {
                return new COADCORPContext();
            },
            x => x.AddProfile<AutoMapperProfile>(),
            "COAD.PROXY.Model.DTO", 
            Assembly.GetExecutingAssembly(),
            typeof(COADCORPContext));

            CorporativoConfig.Configurar();
            COADGEDConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadProxyConfig>();
            //
        }



    }
}
