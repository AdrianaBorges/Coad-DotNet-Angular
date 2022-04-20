using COAD.PROXY.Config;
using COADWEBAPI.Pumbing;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using System.Web.Mvc;


namespace COADWEBAPI.Config
{
    public class CoadWebAPIConfig
    {
        public static void Configurar()
        {

            GenericCrudConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadWebAPIConfig>();
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
