using AutoMapper;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Config.CustomConvert
{
    public class AutoMapperProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {

            // ---- Conta
            var conta = store.CreateMap<CONTA, ContaDTO>();
            conta.ForMember(obj => obj.EMPRESA, conf => conf.Ignore());
            conta.ForMember(obj => obj.BANCOS_REF, conf => conf.Ignore());

            // ---- Contabilista
            var mContabilista = store.CreateMap<CONTABILISTA,ContabilistaDTO>();
            mContabilista.ForMember(obj => obj.EMPRESA, conf => conf.Ignore());

            var mContabilistaR = mContabilista.ReverseMap();
            mContabilistaR.ForMember(obj => obj.EMPRESA, conf => conf.Ignore());

            // ---- Empresa
            var mEmpresa = store.CreateMap<EMPRESA,EmpresaModel>();
            mEmpresa.ForMember(obj => obj.CONTABILISTA, conf => conf.Ignore());
            mEmpresa.ForMember(obj => obj.PERFIL, conf => conf.Ignore());

            var mEmpresaR = mEmpresa.ReverseMap();
            mEmpresaR.ForMember(obj => obj.CONTABILISTA, conf => conf.Ignore());
            mEmpresaR.ForMember(obj => obj.PERFIL, conf => conf.Ignore());

            // ---- ItemMenu
            var mItemMenu = store.CreateMap<ITEM_MENU,ItemMenuModel>();
            mItemMenu.ForMember(obj => obj.ITEM_MENU_PERFIL, conf => conf.Ignore());
            mItemMenu.ForMember(obj => obj.ITEM_MENU1, conf => conf.Ignore());
            mItemMenu.ForMember(obj => obj.ITEM_MENU2, conf => conf.Ignore());
            mItemMenu.ForMember(obj => obj.SISTEMA, conf => conf.Ignore());

            var mItemMenuR = mItemMenu.ReverseMap();
            mItemMenuR.ForMember(obj => obj.ITEM_MENU_PERFIL, conf => conf.Ignore());
            mItemMenuR.ForMember(obj => obj.ITEM_MENU1, conf => conf.Ignore());
            mItemMenuR.ForMember(obj => obj.ITEM_MENU2, conf => conf.Ignore());
            mItemMenuR.ForMember(obj => obj.SISTEMA, conf => conf.Ignore());
            
            // ---- ItemMenuPerifl
            var mItemMenuPerfil = store.CreateMap<ITEM_MENU_PERFIL,ItemMenuPerfilModel>();
            //mItemMenuPerfil.ForMember(obj => obj.ITEM_MENU, conf => conf.Ignore());
            //mItemMenuPerfil.ForMember(obj => obj.PERFIL, conf => conf.Ignore());

            var mItemMenuPerfilR = mItemMenuPerfil.ReverseMap();
            mItemMenuPerfilR.ForMember(obj => obj.ITEM_MENU, conf => conf.Ignore());
            mItemMenuPerfilR.ForMember(obj => obj.PERFIL, conf => conf.Ignore());

            // ---- Log
            var mLogOCorrencia = store.CreateMap<LOG_OCORRENCIA,LogOcorrenciasModel>();
            //mLogOCorrencia.ForMember(obj => obj.USUARIO, conf => conf.Ignore());

            var mLogOCorrenciaR = mLogOCorrencia.ReverseMap();
            //mLogOCorrenciaR.ForMember(obj => obj.USUARIO, conf => conf.Ignore());

            // ---- Perfil
            var mPerfil = store.CreateMap<PERFIL,PerfilModel>();
            mPerfil.ForMember(obj => obj.ITEM_MENU_PERFIL, conf => conf.Ignore());
            mPerfil.ForMember(obj => obj.PERFIL_USUARIO, conf => conf.Ignore());
            
            var mPerfilR = mPerfil.ReverseMap();
            mPerfilR.ForMember(obj => obj.EMPRESA, conf => conf.Ignore());
            mPerfilR.ForMember(obj => obj.ITEM_MENU_PERFIL, conf => conf.Ignore());
            mPerfilR.ForMember(obj => obj.PERFIL_USUARIO, conf => conf.Ignore());
            mPerfilR.ForMember(obj => obj.SISTEMA, conf => conf.Ignore());
            mPerfilR.ForMember(obj => obj.DEPARTAMENTO, conf => conf.Ignore());

            // ---- Perfil Usuário
            var mPerfilUsuario = store.CreateMap<PERFIL_USUARIO,PerfilUsuarioModel>();
            //mPerfilUsuario.ForMember(obj => obj.PERFIL, conf => conf.Ignore());
            mPerfilUsuario.ForMember(obj => obj.USUARIO, conf => conf.Ignore());

            var mPerfilUsuarioR = mPerfilUsuario.ReverseMap();
            mPerfilUsuarioR.ForMember(obj => obj.PERFIL, conf => conf.Ignore());
            mPerfilUsuarioR.ForMember(obj => obj.USUARIO, conf => conf.Ignore());

            // ---- Sistema
            var mSistema = store.CreateMap<SISTEMA,SistemaModel>();
            mSistema.ForMember(obj => obj.ITEM_MENU, conf => conf.Ignore());
            mSistema.ForMember(obj => obj.PERFIL, conf => conf.Ignore());

            var mSistemaR = mSistema.ReverseMap();
            mSistemaR.ForMember(obj => obj.ITEM_MENU, conf => conf.Ignore());
            mSistemaR.ForMember(obj => obj.PERFIL, conf => conf.Ignore());

            var mparam = store.CreateMap<PARAMETROS, ParametrosDTO>();
            mparam.ForMember(obj => obj.PARAMETRO_GRUPO, conf => conf.Ignore());
            mparam.ForMember(obj => obj.USUARIO, conf => conf.Ignore());
            var mparamr = mparam.ReverseMap();
            mparam.ForMember(obj => obj.USUARIO, conf => conf.Ignore());
            mparamr.ForMember(obj => obj.PARAMETRO_GRUPO, conf => conf.Ignore());


            var mparamgrupo = store.CreateMap<PARAMETRO_GRUPO, ParametroGrupoDTO>();
            mparamgrupo.ForMember(obj => obj.PARAMETROS , conf => conf.Ignore());
            var mparamgrupor = mparamgrupo.ReverseMap();
            mparamgrupor.ForMember(obj => obj.PARAMETROS, conf => conf.Ignore());

            // ---- Usuário
            //var mUsuario = store.CreateMap<USUARIO, UsuarioModel>();
            //mUsuario.ForMember(obj => obj.EMPRESA, conf => conf.Ignore());
            //mUsuario.ForMember(obj => obj.LOG_OCORRENCIA, conf => conf.Ignore());
            //mUsuario.ForMember(obj => obj.PERFIL_USUARIO, conf => conf.Ignore());
            //mUsuario.ForMember(obj => obj.USU_SENHA, conf => conf.Ignore()); // questão de segurança
            //mUsuario.ForMember(obj => obj.USUARIO1, conf => conf.Ignore());
            //mUsuario.ForMember(obj => obj.USUARIO2, conf => conf.Ignore());

            //var mUsuarioR = mUsuario.ReverseMap();
            //mUsuarioR.ForMember(obj => obj.EMPRESA, conf => conf.Ignore());
            //mUsuarioR.ForMember(obj => obj.LOG_OCORRENCIA, conf => conf.Ignore());
            //mUsuarioR.ForMember(obj => obj.PERFIL_USUARIO, conf => conf.Ignore());

            //var mDepartamento = store.CreateMap<DEPARTAMENTO, DepartamentoDTO>();
            //mDepartamento.ForMember(obj => obj.PERFIL, conf => conf.Ignore());


            //var mDepartamentoR = mDepartamento.ReverseMap();

        }
    }
}
