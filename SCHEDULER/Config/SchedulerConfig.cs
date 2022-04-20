using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.Jobs;
using COAD.CORPORATIVO.LEGADO.Config;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.PROXY.Config;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Jobs.Controles;
using COAD.SEGURANCA.Jobs.DataSource;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config;
using GenericCrud.Config.ConfigEnuns;
using GenericCrud.Config.IOCContainer;
using GenericCrud.Models;
using GenericCrud.Service;
using GenericCrud.Util;
using SCHEDULER.Jobs;
using SCHEDULER.Pumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCHEDULER.Config
{
    public class SchedulerConfig
    {
        public static string DefaultPath { get; set; }
        public static void Configurar()
        {
            IOCContainerProxy.DbContextLifestyle = LifeStyleTypeEnum.SCOPE;
            SysUtils.InitParams();
            GenericCrudConfig.Configurar();

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<SchedulerConfig>();
            //

            CoadProxyConfig.Configurar();
            
            // Nesse ponto a fábrica padrão de criação de controller é substituída por uma fábrica personalizada
            // do qual o Container de Injeção de Depêndencia irá ser responsável por criar os controllers
            // e injetar as dependências.(Serviços DAOS e etc)

            var container = containerConfig.Container;
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            SchedulerJobsRegister.RegistrarJobs();
            ServiceFactory.RetornarServico<SchedulerSRV>().Start(JobsRegister.Jobs);
        }


        public static void FecharContainer()
        {
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.Container.Dispose();
            ServiceFactory.RetornarServico<SchedulerSRV>().Stop();
        }
       

    }
}
