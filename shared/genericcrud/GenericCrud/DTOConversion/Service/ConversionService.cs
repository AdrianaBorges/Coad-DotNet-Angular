using Coad.GenericCrud.Config;
using Coad.GenericCrud.Mapping;
using COAD.CORPORATIVO.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.DTOConversion.Service
{
    public class ConversionService
    {
        private string _profileName { get; set; }
        protected MapperEngineWrapper engine {get; set;}

        public ConversionService()
        {
            Init();
        }

        public ConversionService(string profileName)
        {
            _profileName = profileName;
            Init(profileName);
        }

        private void Init(string _profileName = null)
        {
            engine = MapperEngineFactory.criarMapperEngine(_profileName);
        }
        
        public TDestiny Convert<TSource, TDestiny>(TSource value)
        {
            return engine.Convert<TSource, TDestiny>(value);
        }


    }

    public class ConversionService<TSource, TDestiny> : ConversionService
    {
        public ConversionService() : base()
        {
           
        }

        public ConversionService(string profileName) : base(profileName)
        {
           
        }

        public TDestiny Convert(TSource value)
        {
            return engine.Convert<TSource, TDestiny>(value);
        }
    }
}
