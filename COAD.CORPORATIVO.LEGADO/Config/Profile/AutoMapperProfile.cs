using AutoMapper;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Model.Dto.CorporativoAntigo;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config.CustomConvert
{
    public class AutoMapperCorpLegadoProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {

            var mSeqProd = store.CreateMap<SEQ_PROD, SeqProdDTO>();
            var mSeqProdR = mSeqProd.ReverseMap();

            var mAgenda = store.CreateMap<AGENDA, AgendaDTO>();
            var mAgendaR = mAgenda.ReverseMap();

            var mTelefones2 = store.CreateMap<TELEFONES2, Telefones2DTO>();
            var mTelefones2R = mTelefones2.ReverseMap();

            var mEmails = store.CreateMap<EMAILS, EmailsDTO>();
            var mEmailsR = mEmails.ReverseMap();

            var mender_far = store.CreateMap<ender_fat, ender_fatDTO>();
            var mender_farR = mender_far.ReverseMap();

            var mcart_coad = store.CreateMap<cart_coad, cart_coadDTO>();
            var mcart_coadR = mcart_coad.ReverseMap();

            var mclienteLeg = store.CreateMap<CLIENTES, clienteLegDTO>();
            var mclienteLegR = mclienteLeg.ReverseMap();


        }
    }
}
