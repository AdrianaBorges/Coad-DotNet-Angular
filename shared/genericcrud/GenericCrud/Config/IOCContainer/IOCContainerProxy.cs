using Castle.MicroKernel.Registration;
using GenericCrud.Config.ConfigEnuns;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace GenericCrud.Config.IOCContainer
{
    public class IOCContainerProxy : IDisposable
    {
       //public static bool AmbienteWeb = true;
        public static bool UseIOCContainer = true;
        public static LifeStyleTypeEnum DbContextLifestyle = LifeStyleTypeEnum.PER_WEB_REQUEST;
        public static bool ConfigDbContextInWindsor { get; set; } = true;
        public static IOCContainerConfigurator containerConfig { get; set; }

        public static ComponentRegistration<T> SetarLifestyle<T>(ComponentRegistration<T> componentRegistration) where T : class
        {
            if(componentRegistration != null)
            {
                switch (DbContextLifestyle)
                {
                    case LifeStyleTypeEnum.PER_WEB_REQUEST:
                        {
                            return componentRegistration.LifestylePerWebRequest();
                        }
                    case LifeStyleTypeEnum.SCOPE:
                        {
                            return componentRegistration.LifestyleScoped();
                        }
                    case LifeStyleTypeEnum.SINGLETON:
                        {
                            return componentRegistration.LifestyleSingleton();
                        }
                    case LifeStyleTypeEnum.TRANSIENT:
                        {
                            return componentRegistration.LifestyleTransient();
                        }
                    default: 
                        {
                            return componentRegistration.LifestylePerThread();
                        }
                }
            }

            return null;
        }

        public static IOCContainerConfigurator GetIOCContainerConfiguratorSingletonInstance()
        {
            if (containerConfig == null)
                containerConfig = new IOCContainerConfigurator();

            return containerConfig;
        }

        public static void StaticDispose()
        {
            GetIOCContainerConfiguratorSingletonInstance().Container.Dispose();
            containerConfig.Container = null;
            containerConfig = null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
