using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.PROXY.Config;
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
using System.Web.Mvc;

namespace COADSERVICE.Config
{
    public class CoadServiceConfig
    {
        public static void Configurar()
        {

            GenericCrudConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadServiceConfig>();
            //

            CoadProxyConfig.Configurar();
        }


        public static void FecharContainer()
        {
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.Container.Dispose();
        }
       

    }
}
