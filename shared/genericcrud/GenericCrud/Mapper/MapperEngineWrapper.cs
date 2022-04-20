using AutoMapper;
using AutoMapper.Mappers;
using GenericCrud.Config;
using GenericCrud.Config.ClassScan;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config
{
    public class MapperEngineWrapper
    {
        private MappingEngine engine { get; set; }
        private Profile profile {get; set;}
               
        public MapperEngineWrapper()
        {
            Init();
        }
        
        public MapperEngineWrapper(Action<MapperProfileConfig> cfgProfileConfig = null)
        {
            Init(cfgProfileConfig);
        }

        public MapperEngineWrapper(string namespaces , Assembly assembly)
        {
            Init(null, namespaces, assembly);
        }

        public MapperEngineWrapper(Action<MapperProfileConfig> cfgProfileConfig = null, string namespaces = null, Assembly assembly = null)
        {
            Init(cfgProfileConfig, namespaces, assembly);
        }

        public void Init(Action<MapperProfileConfig> cfgProfileConfig = null, string namespaces = null, Assembly assembly = null)
        {
            ConfigurationStore store = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
            store.AssertConfigurationIsValid();

            MappingEngine engine = new MappingEngine(store);

            if (cfgProfileConfig != null)
            {
                MapperProfileConfig profileConfig = new MapperProfileConfig();
                profileConfig.store = store;
                cfgProfileConfig(profileConfig);               
                
               // config.store = store;
            }

            if (!string.IsNullOrWhiteSpace(namespaces) && assembly != null)
            {
                IEnumerable<Type> scannedClasses = ClassScanner.ScanNameSpaceForMapperAnnotations(assembly, namespaces);

                AnnotationConfigurationStore annotadedConfig = new AnnotationConfigurationStore();
                annotadedConfig.store = store;
                annotadedConfig.AddTypes(scannedClasses);
            }

            this.engine = engine;
        }

        public T Convert<T>(object val)
        {
            return engine.Map<T>(val);
        }

        public T Convert<S, T>(S val)
        {
            return engine.Map<S, T>(val);
        }
    }
}
