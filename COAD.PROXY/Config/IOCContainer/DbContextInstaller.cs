
using Castle.Core;
using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Dao.Base;
using GenericCrud.Interceptores;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Config.IOCContainer
{
    public class DbContextInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            //container.Register(
            //    Component
            //    .For<COADCORPContext>()
            //    .Named("default")
            //    .LifestylePerWebRequest()
            //);
        }
    }
}
