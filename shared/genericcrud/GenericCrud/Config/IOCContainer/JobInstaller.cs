using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Config;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Config;
using GenericCrud.Service.Base;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.IOCContainer
{
    public class JobInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(
                Classes
                .FromAssemblyContaining<GenericCrudConfig>()
                .BasedOn<IJob>());

        }
    }
}
