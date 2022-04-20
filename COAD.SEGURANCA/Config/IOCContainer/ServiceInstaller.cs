using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Service.Base;
using COAD.SEGURANCA.Interceptors;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Config.IOCContainer
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(
                Component
                .For<IEmailSRV>()
                .ImplementedBy<EmailSRV>()
                .Interceptors<EmailInterceptor>()
                ,
                Component.For<EmailInterceptor>()
                .LifestyleTransient()
                , Classes
                .FromAssemblyContaining<CoadSegurancaConfig>()
                .BasedOn(typeof(GenericService<,,>))
                .Configure(x =>
                {
                    x.OnCreate(c => ((IBaseService)c).Init()); // Ao configurar, determino qual método é o método de inicialização.
                }
                ),
                Classes
                .FromAssemblyContaining<CoadSegurancaConfig>()
                .Where(x => !x.IsSubclassOf(typeof(GenericService<,,>))
                    &&
                    x.Name.EndsWith("SRV"))
                );

        }
    }
}
