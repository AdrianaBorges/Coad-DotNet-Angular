using Castle.Windsor;
using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.LEGADO.Config;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using COAD.PROXY.Config;
using COAD.RM.Repositorios.Contexto;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using GenericCrud.IOCContainer.Proxies;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service.Custons;
using COADRESTSERVICE.Auth;
using Microsoft.AspNetCore.Http;

namespace COADRESTSERVICE.Config
{
    public class COADRESTSERVICEConfig
    {
        private static void ConfigDbContexts(IServiceCollection services)
        {
            if(services != null)
            {
                services.AddScoped<DbContextAcessorNetCore>();
                services.AddScoped(typeof(DbContextAcessorNetCore<>));
                services.AddScoped<COADCORPContext>();
                services.AddScoped<SEGURANCAContext>();
                services.AddScoped<CorporeRMEntities>();
                services.AddScoped<prospectadosEntities>();
                services.AddScoped<asteriskcdrdbEntities>();
                services.AddScoped<coadEntities>();
                services.AddScoped<buscaEntities>();
                services.AddScoped<consultoriaEntities>();
                services.AddScoped<COADIARIOEntities>();
                services.AddScoped<CorporeRMEntities>();
                services.AddScoped<corporativo2Entities>();
                services.AddScoped<COADGEDEntities>();
            }
        }

        public static WindsorContainer Configurar(IServiceCollection services)
        {
            IOCContainerProxy.ConfigDbContextInWindsor = false;
            ConfigDbContexts(services);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<UserContext>();

            SysUtils.InitParams();
            GenericCrudConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<COADRESTSERVICEConfig>();
            //
            RMConfig.Configurar();
            CoadProxyConfig.Configurar();
            
            // Nesse ponto a fábrica padrão de criação de controller é substituída por uma fábrica personalizada
            // do qual o Container de Injeção de Depêndencia irá ser responsável por criar os controllers
            // e injetar as dependências.(Serviços DAOS e etc)

            var container = containerConfig.Container;
            return container;
        }


        public static void FecharContainer()
        {
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.Container.Dispose();
        }
       

    }
}
