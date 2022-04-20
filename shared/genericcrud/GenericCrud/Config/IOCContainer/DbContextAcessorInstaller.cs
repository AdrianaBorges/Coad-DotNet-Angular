
using Castle.Core;
using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Config;
using Coad.GenericCrud.Dao.Base;
using GenericCrud.Dao.Base;
using GenericCrud.Interceptores;
using GenericCrud.IOCContainer.Proxies;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config.IOCContainer
{
    public class DAOInstalDbContextInstallerler : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (IOCContainerProxy.ConfigDbContextInWindsor)
            {
                container
                .Register(Classes
                .FromAssemblyContaining<GenericCrudConfig>()
                .BasedOn(typeof(IDbContextAcessor<>))
                .LifestyleTransient(),
                Classes.FromAssemblyContaining<GenericCrudConfig>()
                .BasedOn<DbContextAcessor>()
                .LifestyleTransient());
            }
        }
    }
}
