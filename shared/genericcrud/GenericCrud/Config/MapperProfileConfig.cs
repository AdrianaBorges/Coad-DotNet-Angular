using AutoMapper;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Config
{
    public class MapperProfileConfig
    {
        public MapperProfileConfig()
        {

        }

        public MapperProfileConfig(ConfigurationStore store)
        {
            this.store = store;
        }

        public ConfigurationStore store { get; set; }

        public void AddProfile<TProfile>() where TProfile : GenericProfile, new()
        {
            GenericProfile profile = Activator.CreateInstance<TProfile>();
            profile.Configure(store);
        }
    }
}
