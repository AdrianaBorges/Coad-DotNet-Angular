using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.RM.Repositorios.Contexto;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config
{
    public class RMConfig
    {
        public static void Configurar()
        {
            ProfileConfigurator.addConfig("rm", () =>
            {
                return new CorporeRMEntities();
            },
            x => x.AddProfile<AutoMapperProfile>(), 
            "COAD.RM.Model.Dto", 
            Assembly.GetExecutingAssembly(),
            typeof(CorporeRMEntities));
           
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CorporeRMEntities>();

           // GenericCrudConfig.Configurar();
        }    

    }
}
