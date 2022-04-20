using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.PORTAL.Config.Profile;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Config
{
    public class PortalConfig
    {
        public static void Configurar()
        {
            ProfileConfigurator.addConfig("URARJ", () =>
            {
                return new asteriskcdrdbEntities();
            },
            x => x.AddProfile<AutoMapperURAProfile>(),
            "COAD.PORTAL.Model.DTO.Uras",
            Assembly.GetExecutingAssembly(),
            typeof(asteriskcdrdbEntities));


            ProfileConfigurator.addConfig("portalCoad", () =>
            {
                return new coadEntities();
            },
            x => x.AddProfile<AutoMapperPortalCoadProfile>(),
            "COAD.PORTAL.Model.DTO.PortalCoad",
            Assembly.GetExecutingAssembly(),
            typeof(coadEntities));

            ProfileConfigurator.addConfig("portalBusca", () =>
            {
                return new buscaEntities();
            },
            x => x.AddProfile<AutoMapperPortalBuscaProfile>(),
            "COAD.PORTAL.Model.DTO.PortalBusca",
            Assembly.GetExecutingAssembly(),
            typeof(buscaEntities));

            ProfileConfigurator.addConfig("portal", () =>
            {
                return new COADIARIOEntities();
            },
            x => x.AddProfile<AutoMapperPortalProfile>(),
            "COAD.PORTAL.Model.DTO.CalendarioObrigacoes",
            Assembly.GetExecutingAssembly(),
            typeof(COADIARIOEntities));

            ProfileConfigurator.addConfig("portalConsultoria", () =>
            {
                return new consultoriaEntities();
            },
            x => x.AddProfile<AutoMapperPortaConsultoriaProfile>(),
            "COAD.PORTAL.Model.DTO.PortalConsultoria",
            Assembly.GetExecutingAssembly(),
            typeof(consultoriaEntities));

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<PortalConfig>();
            //
        }
    }
}
