using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Service.Custons;
using COAD.SCHEDULER;
using GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.AGENDADOR.Config.IOCContainer
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(
                Component.For<JobScheduler>()
                .LifestyleTransient(),
            Classes
                .FromAssemblyContaining<AgendadorConfig>()
                .BasedOn(typeof(GenericService<,,>))
                .Configure(x =>
                {
                    x.OnCreate(c => ((IBaseService)c).Init()); // Ao configurar, determino qual método é o método de inicialização.
                }
                ),
                Classes
                .FromAssemblyContaining<AgendadorConfig>()
                .Where(x => !x.IsSubclassOf(typeof(GenericService<,,>))
                    && 
                    x.Name.EndsWith("SRV"))
                );

        }
    }
}
