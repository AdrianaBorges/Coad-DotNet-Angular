using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Mapping
{
    public static class AutoMapperConfig
    {
        public static void Config(Action<IConfiguration> action)
        {
            Mapper.Initialize(action);
        }
    }
}