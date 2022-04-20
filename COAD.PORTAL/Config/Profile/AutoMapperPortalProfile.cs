using AutoMapper;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Config.Profile
{
    public class AutoMapperPortalProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            #region areas

            var mAreas = store.CreateMap<CO_AREAS, CoAreasDTO>();
            mAreas.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            var mAreasR = mAreas.ReverseMap();
            mAreasR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            #endregion

            #region calendario
            var mCalendario = store.CreateMap<CO_CALENDARIO, CoCalendarioDTO>();
            var mCalendarioR = mCalendario.ReverseMap();
            #endregion

            #region estados

            var mEstados = store.CreateMap<CO_ESTADOS, CoEstadosDTO>();
            mEstados.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());
            mEstados.ForMember(obj => obj.CO_MUNICIPIOS, conf => conf.Ignore());

            var mEstadosR = mEstados.ReverseMap();
            mEstadosR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());
            mEstadosR.ForMember(obj => obj.CO_MUNICIPIOS, conf => conf.Ignore());

            #endregion

            #region municipios

            var mMunicipios = store.CreateMap<CO_MUNICIPIOS, CoMunicipiosDTO>();
            mMunicipios.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());
            
            var mMunicipiosR = mMunicipios.ReverseMap();
            mMunicipiosR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            #endregion

            #region obrigacoes

            var mObrigacoes = store.CreateMap<CO_OBRIGACOES, CoObrigacoesDTO>();
            mObrigacoes.ForMember(obj => obj.CO_CALENDARIO, conf => conf.Ignore());

            var mObrigacoesR = mObrigacoes.ReverseMap();
            mObrigacoesR.ForMember(obj => obj.CO_CALENDARIO, conf => conf.Ignore());

            #endregion

            #region tipos
            var mTipos = store.CreateMap<CO_TIPOS, CoTiposDTO>();
            mTipos.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());

            var mTiposR = mTipos.ReverseMap();
            mTiposR.ForMember(obj => obj.CO_OBRIGACOES, conf => conf.Ignore());
            #endregion
                        
            #region materias
            var mMateriasLista = store.CreateMap<BUSCAR_MATERIA_PORTAL_PROC_Result, MateriasDTO>();
            var mMateriasListaR = mMateriasLista.ReverseMap();

            var mMateria = store.CreateMap<BUSCAR_MATERIA_POR_ID_PORTAL_PROC_Result, MateriaDTO>();
            var mMateriaR = mMateria.ReverseMap();
            #endregion
            
        }
    }
}
