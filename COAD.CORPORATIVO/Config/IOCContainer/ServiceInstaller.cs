using Castle.MicroKernel.Registration;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Service.Custons;
using COAD.FISCAL.Config;
using COAD.FISCAL.Service.Integracoes;
using COAD.FISCAL.Service.Integracoes.Interfaces;
using GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config.IOCContainer
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes
                .FromAssemblyContaining<CorporativoConfig>()
                .BasedOn(typeof(GenericService<,,>))
                .Configure(x =>
                {
                    x.OnCreate(c => ((IBaseService)c).Init()); // Ao configurar, determino qual método é o método de inicialização.
                }
                ),
                Classes
                .FromAssemblyContaining<CorporativoConfig>()
                .Where(x => !x.IsSubclassOf(typeof(GenericService<,,>))
                    &&
                    x.Name.EndsWith("SRV")),
                Component.For<TipoDeClienteSRVProxy>(),
                Classes.FromAssemblyContaining<CorporativoConfig>()
                .BasedOn(typeof(IntegrLoteItemNFeSRV<>)),
                Component.For<ILoteNFeSRV>().ImplementedBy<LoteNFeSRVImpl>(),
                
                Component.For<IntegrNfeSRV>()
                    .DependsOn(Dependency.OnComponent<ILoteNFeSRV, LoteNFeSRVImpl>())
                );


        }
    }
}
