using COAD.PROXY.Config;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COADCORPCORE.Config
{
    public class CoadCOREConfig
    {
        public static void Configurar()
        {

            GenericCrudConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<GenericCrudConfig>();
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