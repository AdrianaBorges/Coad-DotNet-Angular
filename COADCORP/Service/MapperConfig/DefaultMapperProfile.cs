using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COADCORP.MapperConfig.Service
{
    public class DefaultMapperProfile : Profile
    {
        protected override void Configure()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();

            //var mapConfig = Mapper.CreateMap<TIPO_PEDIDO, TiposDeNegocioModel>();
            //mapConfig.ForMember(x => x.id, opt => opt.MapFrom(source => source.TIPO_PED_ID));
            //mapConfig.ForMember(x => x.descricao, opt => opt.MapFrom(source => source.TIPO_PED_DESCRICAO));
            //
            //var mapConfigreverse = mapConfig.ReverseMap();
            //mapConfigreverse.ForMember(x => x.TIPO_PED_ID, opt => opt.MapFrom(source => source.id));
            //mapConfigreverse.ForMember(x => x.TIPO_PED_DESCRICAO, opt => opt.MapFrom(source => source.descricao));

            //mapConfig.ForMember(x => x.SAT_BAIRRO, opt => {opt.Ignore()});
            //mapConfig.ForMember(x => x.SAT_CIDADE, opt => opt.Ignore());
            //mapConfig.ReverseMap();
            //
            //Mapper.CreateMap<SAT_UF, UF>().ForMember(x => x.SAT_CIDADE, opt => opt.Ignore()).ReverseMap();
            //Mapper.CreateMap<SAT_CIDADE, Cidade>().ForMember(x => x.SAT_BAIRRO, opt => opt.Ignore()).ReverseMap();
            //Mapper.CreateMap<SAT_BAIRRO, Bairro>().ReverseMap();
            //Mapper.CreateMap<CidadeDto, Cidade>().ReverseMap();
            //
            //var suspectConfig = Mapper.CreateMap<SAT_SUSPECT, Suspect>();
            //suspectConfig.ForMember(x => x.ID, opt => opt.MapFrom(source => source.DocEntry));
            //suspectConfig.ForMember(x => x.IdStr, opt => opt.MapFrom(source => source.CardCode));
            //suspectConfig.ForMember(x => x.Nome, opt => opt.MapFrom(source => source.CardName));
            //
            //var suspectInverseConfig = suspectConfig.ReverseMap();
            //suspectInverseConfig.ForMember(x => x.DocEntry, opt => opt.MapFrom(source => source.ID));
            //suspectInverseConfig.ForMember(x => x.CardName, opt => opt.MapFrom(source => source.IdStr));
            //suspectInverseConfig.ForMember(x => x.CardName, opt => opt.MapFrom(source => source.Nome));
            //
            //Mapper.CreateMap<SAT_CARTEIRAMENTO, Carteiramento>().ReverseMap();
            //
            //var operadorConfig = Mapper.CreateMap<SAT_OPERADOR, Operador>();
            //operadorConfig.ForMember(x => x.SAT_CARTEIRAMENTO, opt => opt.Ignore());
            //operadorConfig.ForMember(x => x.SAT_CICLOPER, opt => opt.Ignore());
            //operadorConfig.ForMember(x => x.SAT_LOG, opt => opt.Ignore());
            //operadorConfig.ForMember(x => x.SAT_PRIORIDADE, opt => opt.Ignore());
            //operadorConfig.ForMember(x => x.SAT_SUSPECT, opt => opt.Ignore()).ReverseMap();
        }
    }
}