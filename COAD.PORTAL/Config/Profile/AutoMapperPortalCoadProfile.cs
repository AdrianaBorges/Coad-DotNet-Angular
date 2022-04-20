using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
//using COAD.PORTAL.Model.DTOPortal;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Mapper;

namespace COAD.PORTAL.Config.Profile
{
    public class AutoMapperPortalCoadProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            //#region cliente

            //var mCliente = store.CreateMap<clientes, ClienteDTO>();
            ////mAreas.ForMember(obj => obj.area_atuacao, conf => conf.Ignore());

            //var mClienteR = mCliente.ReverseMap();
            ////mClienteR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            //#endregion

            //#region noticia

            //var mNoticia = store.CreateMap<noticias, NoticiaDTO>();
            ////mAreas.ForMember(obj => obj.area_atuacao, conf => conf.Ignore());

            //var mNoticiaR = mNoticia.ReverseMap();
            ////mClienteR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            //var mNoticiaGrupo = store.CreateMap<noticias_grupos, NoticiaGrupoDTO>();
            //var mNoticiaGrupoR = mNoticiaGrupo.ReverseMap();

            //var mNoticiaConteudo = store.CreateMap<noticias_conteudo, NoticiaConteudoDTO>();
            //var mNoticiaConteudoR = mNoticiaConteudo.ReverseMap();

            //#endregion

            //#region indice

            //var mIndice = store.CreateMap<idc_agregado, IndiceDTO>();
            ////mAreas.ForMember(obj => obj.area_atuacao, conf => conf.Ignore());

            //var mIndiceR = mIndice.ReverseMap();
            ////mClienteR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            //#endregion

        }
    }
}
