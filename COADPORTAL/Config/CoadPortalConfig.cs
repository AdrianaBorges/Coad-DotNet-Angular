
using COAD.PROXY.Config;
using COADPORTAL.Pumbing;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using System.Web.Mvc;


namespace COADPORTAL.Config
{
    public class CoadPortalConfig
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
