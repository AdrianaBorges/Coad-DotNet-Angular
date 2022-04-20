using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.SEGURANCA.Config.CustomConvert;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Config
{
    public class CoadSegurancaConfig
    {
        public static void Configurar()
        {
            
            ProfileConfigurator.addConfig("coadsys", () =>
            {
                return new SEGURANCAContext();
            }, 
            x => x.AddProfile<AutoMapperProfile>(),
            "COAD.SEGURANCA.Model", 
            Assembly.GetExecutingAssembly(),
            typeof(SEGURANCAContext));

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadSegurancaConfig>();
            //
        }

       

    }
}
