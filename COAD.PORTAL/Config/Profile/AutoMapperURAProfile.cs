using AutoMapper;
using COAD.PORTAL.Model.DTO.Uras;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Config.Profile
{
    public class AutoMapperURAProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            var mCoad = store.CreateMap<coad, coadDTO>();
            var mCoadR = mCoad.ReverseMap();
        }
    }
}
