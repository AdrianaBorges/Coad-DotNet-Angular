using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Config.IOCContainer
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes
                 .FromAssemblyContaining<COADGEDConfig>()
                 .BasedOn(typeof(GenericService<,,>))
                 .Configure(x =>
                 {
                     x.OnCreate(c => ((IBaseService)c).Init()); // Ao configurar, determino qual método é o método de inicialização.
                 }
                ));
        }
    }
}
