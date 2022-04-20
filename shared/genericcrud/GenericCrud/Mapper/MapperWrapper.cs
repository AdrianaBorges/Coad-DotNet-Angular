using AutoMapper;
using AutoMapper.Mappers;
using COAD.CORPORATIVO.Config;
using GenericCrud.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Coad.GenericCrud.Mapping
{
    public static class MapperWrapper
    {
        public static IDictionary<string, MappingEngine> automapperProfile = new Dictionary<string, MappingEngine>();

        public static T Convert<T>(object val)
        {
            return Mapper.Map<T>(val);
        }

        public static T Convert<S,T>(S val)
        {
            return Mapper.Map<S,T>(val);
        }

        public static MapperEngineWrapper BuildMapperEngineWrapper(Action<MapperProfileConfig> cfgProfileConfig = null, string namespaces = null, Assembly assembly = null)
        {

            return new MapperEngineWrapper(cfgProfileConfig, namespaces, assembly);
        }
    }
    
}