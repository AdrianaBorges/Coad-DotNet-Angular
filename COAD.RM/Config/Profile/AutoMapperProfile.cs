using AutoMapper;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COAD.CORPORATIVO.Config.CustomConvert
{
    public class AutoMapperProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {

        }

        public void Ignore(IMemberConfigurationExpression tes)
        {
            tes.Ignore();
        }
    }
}
