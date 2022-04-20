using AutoMapper;
using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config;
using GenericCrud.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coad.GenericCrud.Config
{
    public static class MapperEngineFactory
    {
        public static MapperEngineWrapper criarMapperEngine(string chave)
        {
            var config = ProfileConfigurator.getProfileConfig(chave);
            var engine = config.engine;

            var cfgProfileConfig = config.cfgProfileConfig;
            var namespaces = config.ScanNameSpaces;
            var assembly = config.assembly;

            if (engine != null)
            {
                return engine;
            }
            else
            if (cfgProfileConfig != null)
            {
                MapperEngineWrapper mapperEngine = null;               
                mapperEngine = MapperWrapper.BuildMapperEngineWrapper(cfgProfileConfig, namespaces, assembly);               

                config.engine = mapperEngine;

                return mapperEngine;
            }
            else
            {
                throw new Exception("O Profile de mapeamento do automapper não pode ser encontrado");
            }
                       
        }
    }
}
