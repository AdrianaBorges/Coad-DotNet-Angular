using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using COADSERVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COADCORP.Config.IOCContainer
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.AddFacility<WcfFacility>(x => x.CloseTimeout = TimeSpan.Zero)
                .Register(
                    Component
                    .For<ICoadService>()
                    .ImplementedBy<CoadService>()
                    .LifestylePerWcfOperation()
                    .AsWcfClient(new DefaultClientModel())
                );

        }
    }
}
