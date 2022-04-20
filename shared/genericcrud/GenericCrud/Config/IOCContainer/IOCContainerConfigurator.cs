using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.IOCContainer
{
    public class IOCContainerConfigurator
    {
        public WindsorContainer Container { get; set; }

        private void BuildNew()
        {
            Container = new WindsorContainer();
            
        }

        public void InstallFromAssembly<T>()
        {
            if (Container == null)
            {
                BuildNew();
            }
            Container.Install(FromAssembly.Containing<T>());
            
        }
    }
}
