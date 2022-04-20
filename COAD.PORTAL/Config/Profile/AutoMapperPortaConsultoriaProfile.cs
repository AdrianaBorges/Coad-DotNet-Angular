using AutoMapper;
using COAD.PORTAL.Model.DTO.PortalConsultoria;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Config.Profile
{
    public class AutoMapperPortaConsultoriaProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            #region consultoria
            var mConsultoria = store.CreateMap<consultoria, ConsultoriaPortalDTO>();
            var mConsultoriaR = mConsultoria.ReverseMap();
            #endregion
        }
    }
}
