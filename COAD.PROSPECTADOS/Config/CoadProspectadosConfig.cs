using Coad.GenericCrud.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Config
{
    public class CoadProspectadosConfig
    {
        public static void Configurar()
        {
            ProfileConfigurator.addConfig("prospectados", () =>
            {
                return new prospectadosEntities();
            },
            x => x.AddProfile<AutoMapperProspectadosProfile>(),
            "COAD.PROSPECTADOS.Model.Dto",
            Assembly.GetExecutingAssembly(),
            typeof(prospectadosEntities));

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadProspectadosConfig>();
            //
        }
    }
}
