using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Config;
using COAD.PORTAL.Config;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Config
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class COADGEDConfig
    {
        public static void Configurar()
        {
            // COADGED - SQL SERVER 2008
            ProfileConfigurator.addConfig("GED", () =>
            {
                return new COADGEDEntities();
            },
            x => x.AddProfile<AutoMapperDefaultProfile>(),
            "COAD.COADGED.Model.DTO",
            Assembly.GetExecutingAssembly(),
            typeof(COADGEDEntities));

            // PORTAL - MySQL
            PortalConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<COADGEDConfig>();
            //
        }
    }
}
