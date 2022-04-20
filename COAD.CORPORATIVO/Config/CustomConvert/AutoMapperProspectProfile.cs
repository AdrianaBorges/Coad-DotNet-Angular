using AutoMapper;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Prospect;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPSUSPECT.Repositorios.Base;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config.CustomConvert
{
    public class AutoMapperProspectProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            var mapeamentoProspect = store.CreateMap<PROSPECT, ProspectDTO>();
            //mapeamentoProspect.ForMember(s => s.PROSPECTS_TELEFONE, opt => opt.Ignore());

            var mapeamentoProspectReverso = mapeamentoProspect.ReverseMap();
            mapeamentoProspectReverso.ForMember(s => s.PROSPECTS_TELEFONE, opt => opt.Ignore());

            var mapeamentoTelefone = store.CreateMap<PROSPECTS_TELEFONE, ProspectsTelefoneDTO>();
            mapeamentoTelefone.ForMember(s => s.PROSPECT, opt => opt.Ignore());

            var mapeamentoTelefoneReverso = mapeamentoTelefone.ReverseMap();
            mapeamentoTelefoneReverso.ForMember(s => s.PROSPECT, opt => opt.Ignore());

            var mapeamentoTipoTelefone = store.CreateMap<COAD.CORPSUSPECT.Repositorios.Base.TIPO_TELEFONE, TipoTelefoneDTO>();
            mapeamentoTipoTelefone.ForMember(s => s.PROSPECTS_TELEFONE, opt => opt.Ignore());

            var mapeamentoTipoTelefoneReverso = mapeamentoTipoTelefone.ReverseMap();
           
        }
    }
}
