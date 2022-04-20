﻿
using Castle.Core;
using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Dao.Base;
using GenericCrud.Dao.Base;
using GenericCrud.Interceptores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Config.IOCContainer
{

    public class DAOInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<CoadProspectadosConfig>()
                .BasedOn(typeof(AbstractGenericDao<,,>))
                .Configure(x =>
                {
                    x.OnCreate(c => ((IRepository)c).Init()); // Ao configurar, determino qual método é o método de inicialização.
                }
                ));

        }
    }
}
