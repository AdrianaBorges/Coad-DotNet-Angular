using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.LEGADO.Config;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.FISCAL.Config;
using COAD.PROSPECTADOS.Config;
using COAD.SEGURANCA.Config;
using COAD.SEGURANCA.Config.Email;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config
{
    public class CorporativoConfig
    {
        public static void Configurar()
        {
            ProfileConfigurator.addConfig("default", () =>
            {
                return new COADCORPContext();
            },
            x => x.AddProfile<AutoMapperProfile>(), 
            "COAD.CORPORATIVO.Model.Dto", 
            Assembly.GetExecutingAssembly(),
            typeof(COADCORPContext));
           
            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CorporativoConfig>();           
            //

            //EmailActionContainer.AddActions("emailBoletoProposta", () => { 
            // TODO: Cadastrar Ação padrão para cada envio de email
                
            //});

            CoadSegurancaConfig.Configurar();
            CoadCorporativoLegado.Configurar();
            CoadProspectadosConfig.Configurar();
            CoadFiscalConfig.Configurar();
        }    

    }
}
