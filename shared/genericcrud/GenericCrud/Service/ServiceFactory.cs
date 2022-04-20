using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service
{
    public class ServiceFactory
    {
        public static TReturn RetornarServico<TReturn>()
        {
            var config = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();

            if (config != null && config.Container != null)
            {
                TReturn service = config.Container.Resolve<TReturn>();
                if (service != null)
                {
                    return service;
                }

            }

            return Activator.CreateInstance<TReturn>();
        }

        public static T RetornarServico<T>(Type type) where T : class
        {
            var config = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();

            if (config != null && config.Container != null)
            {
                object service = config.Container.Resolve(type);
                if (service != null && service is T)
                {
                    return service as T;
                }
            }

            return Activator.CreateInstance(type) as T;
        }
    }
}
