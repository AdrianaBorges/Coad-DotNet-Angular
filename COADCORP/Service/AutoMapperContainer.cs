using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COADCORP.MapperConfig.Service;

namespace COADCORP.Service
{
    public static class AutoMapperContainer
    {
       // private static bool _isConfigured;

        // adicionando todos os mapeamentos com os dtos
        public static void DefaultProfile()
        {
           Mapper.Initialize(cfg => cfg.AddProfile<DefaultMapperProfile>());
        }

        public static void EagerProfile()
        {
            ///Mapper.Initialize(cfg => cfg.AddProfile<EagerLoadingProfile>());

        }
    }
}