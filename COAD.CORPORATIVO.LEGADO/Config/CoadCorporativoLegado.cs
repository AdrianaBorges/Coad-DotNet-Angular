using Coad.GenericCrud.Config;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.IOCContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Config
{
    public class CoadCorporativoLegado
    {
        public static void Configurar()
        {
            ProfileConfigurator.addConfig("corp_old", () =>
            {
                return new corporativo2Entities();
            },
            x => x.AddProfile<AutoMapperCorpLegadoProfile>(), 
            "COAD.CORPORATIVO.LEGADO.Model.Dto",
            Assembly.GetExecutingAssembly(),
            typeof(corporativo2Entities));

            // Configurando o Container de Injeção de Dependêcia
            var containerConfig = IOCContainerProxy.GetIOCContainerConfiguratorSingletonInstance();
            containerConfig.InstallFromAssembly<CoadCorporativoLegado>();
            //
        }
    }
}
