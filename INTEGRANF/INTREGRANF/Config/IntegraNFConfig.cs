using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace INTREGRANF.Config
{
    public class IntegraNFConfig
    {
        public static void Configurar()
        {
            //IOCContainerProxy.AmbienteWeb = false;
            IOCContainerProxy.UseIOCContainer = false;
            IOCContainerProxy.DbContextLifestyle = GenericCrud.Config.ConfigEnuns.LifeStyleTypeEnum.PER_THREAD;

            GenericCrudConfig.Configurar();

            //// Configurando o Container de Injeção de Dependêcia
            //var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            //containerConfig.InstallFromAssembly<IntegraNFConfig>();
            

            CorporativoConfig.Configurar();
        }

        public static void FecharContainer()
        {
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.Container.Dispose();
        }
       

    }
}
