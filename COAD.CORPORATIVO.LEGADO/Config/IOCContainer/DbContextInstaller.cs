
using Castle.Core;
using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Dao.Base;
using GenericCrud.Interceptores;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Config.IOCContainer
{
    public class DbContextInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (IOCContainerProxy.ConfigDbContextInWindsor)
            {
                container.Register(IOCContainerProxy.SetarLifestyle(
                Component
                .For<corporativo2Entities>()
                .Named("corp_old"))
                );
            }
        }
    }
}
