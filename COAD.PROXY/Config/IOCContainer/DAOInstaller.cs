
using Castle.Core;
using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Dao.Base;
using GenericCrud.Interceptores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Config.IOCContainer
{

    public class DAOInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<CoadProxyConfig>()
                .BasedOn(typeof(AbstractGenericDao<,,>))
                .Configure(x =>
                {
                    x.OnCreate(c => ((IRepository)c).Init()); // Ao configurar, determino qual método é o método de inicialização.
                }
                ));

        }
    }
}
