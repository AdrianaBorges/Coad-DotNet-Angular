using COAD.PROXY.Config;
using COADMOBILE.Pumbing;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Util;
using System.Web.Mvc;

namespace COADMOBILE.Config
{
    public class CoadMobileConfig
    {
        public static void Configurar()
        {
            SysUtils.InitParams();
            GenericCrudConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadMobileConfig>();
            //

            CoadProxyConfig.Configurar();

            // Nesse ponto a fábrica padrão de criação de controller é substituída por uma fábrica personalizada
            // do qual o Container de Injeção de Depêndencia irá ser responsável por criar os controllers
            // e injetar as dependências.(Serviços DAOS e etc)

            var container = containerConfig.Container;
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }


        public static void FecharContainer()
        {
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.Container.Dispose();
        }
    }
}