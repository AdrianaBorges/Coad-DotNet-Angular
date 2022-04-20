
using Castle.Core;
using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Dao.Base;
using GenericCrud.Interceptores;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Config.IOCContainer
{
    public class DbContextInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (IOCContainerProxy.ConfigDbContextInWindsor)
            {
                container.Register(
                    Component
                    .For<asteriskcdrdbEntities>()
                    .Named("URARJ")
                    .LifestylePerWebRequest()
                    ,
                    Component
                    .For<coadEntities>()
                    .Named("portalCoad")
                    .LifestylePerWebRequest()
                    ,
                    Component
                    .For<buscaEntities>()
                    .Named("portalBusca")
                    .LifestylePerWebRequest()
                    ,
                    Component
                    .For<COADIARIOEntities>()
                    .Named("portal")
                    .LifestylePerWebRequest()
                    ,
                    Component
                    .For<consultoriaEntities>()
                    .Named("portalConsultoria")
                    .LifestylePerWebRequest()
                );
            }
        }
    }
}
