using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COADCORP.Service
{
    public static class MapperWrapper
    {
        public static T Convert<T>(object val)
        {
            return Mapper.Map<T>(val);
        }

        public static T Convert<S,T>(S val)
        {
            return Mapper.Map<S,T>(val);
        }
    }
}