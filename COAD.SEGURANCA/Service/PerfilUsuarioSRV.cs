using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Service;

namespace COAD.SEGURANCA.Service
{
    public class PerfilUsuarioSRV : ServiceAdapter<PERFIL_USUARIO, PerfilUsuarioModel, object>
    {
        private PerfilUsuarioDAO _dao { get; set; }

        public PerfilUsuarioSRV()
        {
            _dao = new PerfilUsuarioDAO();
            SetDao(_dao);
            Comparator = new PerfilUsuarioComparator();
            SetKeys("EMP_ID", "PER_ID", "USU_LOGIN");
        }

        public PerfilUsuarioSRV(PerfilUsuarioDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
            Comparator = new PerfilUsuarioComparator();
            SetKeys("EMP_ID", "PER_ID", "USU_LOGIN");
        }

        public List<PerfilUsuarioModel> BuscarLista(string _usu_login)
        {
            return new PerfilUsuarioDAO().BuscarLista(_usu_login);
        }
        public void ExcluirPorUsuario(string _usu_login)
        {
            new PerfilUsuarioDAO().ExcluirPorUsuario(_usu_login);
        }

        public bool HasPerfilUsuario(int EMP_ID, string PER_ID, string USU_LOGIN)
        {
            return _dao.HasPerfilUsuario(EMP_ID, PER_ID, USU_LOGIN);
        }

        /// <summary>
        /// Pega todos os perfis do usuário
        /// </summary>
        /// <returns></returns>
        public IList<PerfilUsuarioModel> ListByUsuLogin(string USU_LOGIN)
        {
            return _dao.ListByUsuLogin(USU_LOGIN);
        }

        public void PreencherPerfisDoUsuario(UsuarioModel user)
        {
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(user.USU_LOGIN))
                {
                    var USU_LOGIN = user.USU_LOGIN;
                    var listPerfilUsuario = ListByUsuLogin(USU_LOGIN);

                    user.PERFIL_USUARIO = listPerfilUsuario;
                }
            }
        }

          /// <summary>
        ///
        /// </summary>
        /// <param name="regiaoTabPreco"></param>
        /// <param name="excecoes"></param>
        public void ExcluirPerfilUsuario(UsuarioModel usuario)
        {
            var USU_LOGIN = usuario.USU_LOGIN;
            var usuarioOriginal = ServiceFactory.RetornarServico<UsuarioSRV>().FindByIdFullLoaded(USU_LOGIN);

            if (usuarioOriginal != null)
            {
                ExcluirList<UsuarioModel>(usuario, usuarioOriginal, "PERFIL_USUARIO");
            }                           
        }

        public void SalvarPerfilUsuario(IEnumerable<PerfilUsuarioModel> lstPerfilUsuario) 
        {
            if (lstPerfilUsuario != null)
            {
               SaveOrUpdateNonIdentityKeyEntity(lstPerfilUsuario, "HasPerfilUsuario");
            }
        }

        private void _processarChaves(int? EMP_ID,string usu_login,  IEnumerable<PerfilUsuarioModel> lstPerfilUsuarioModel)
        {
            if (lstPerfilUsuarioModel != null)
            {
                foreach (var perfilUsuarioModel in lstPerfilUsuarioModel)
                {
                    if (perfilUsuarioModel.EMP_ID == null)
                    {
                        perfilUsuarioModel.EMP_ID = EMP_ID;
                    }

                    if (perfilUsuarioModel.PER_ID == null)
                    {
                        perfilUsuarioModel.PER_ID = perfilUsuarioModel.PERFIL.PER_ID;
                    }

                    if (perfilUsuarioModel.USU_LOGIN == null)
                    {
                        perfilUsuarioModel.USU_LOGIN = usu_login;
                    }
                }
            }
        }

        public void ProcessarExclusaoEAtualizacaoPerfilUsuario(UsuarioModel usuario)
        {
            if (usuario.USU_LOGIN != null)
            {
                _processarChaves((int) usuario.EMP_ID, usuario.USU_LOGIN, usuario.PERFIL_USUARIO);
                ExcluirPerfilUsuario(usuario);
                SalvarPerfilUsuario(usuario.PERFIL_USUARIO);
            }
        }

        /// <summary>
        ///  Faz uma clonagem (shadow copy)
        ///  porém substitui o usu_login passado
        /// </summary>
        /// <param name="lstUsuarioPerfil"></param>
        /// <param name="usu_login"></param>
        /// <returns>Lista clonada</returns>
        public IList<PerfilUsuarioModel> ClonarPerfilUsuario(IQueryable<PerfilUsuarioModel> lstUsuarioPerfil, string usu_login)
        {
            var shadowCopyLstUsuarioModel = new List<PerfilUsuarioModel>();

                        if (lstUsuarioPerfil != null && !string.IsNullOrWhiteSpace(usu_login))
            {
                foreach (var perUsu in lstUsuarioPerfil)
                {
                    var clonedPerUsu = perUsu.Clone();
                    clonedPerUsu.USUARIO = null;
                    clonedPerUsu.USU_LOGIN = usu_login;

                    shadowCopyLstUsuarioModel.Add(clonedPerUsu);
                }

            }

            var test = shadowCopyLstUsuarioModel.Where(x => x.PUS_DEFAULT == 1 && x.PERFIL.PER_ID != "CONFIGSIS");
            // verifica se existe algum perfil default
            if (test.Count() == 0)
            {
                var first = shadowCopyLstUsuarioModel.Where(x => x.PERFIL.PER_ID != "CONFIGSIS").FirstOrDefault();

                if (first != null)
                {
                    first.PUS_DEFAULT = 1;
                }                
            }
            return shadowCopyLstUsuarioModel;
        }
  }
    
}
