using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class PerfilUsuarioDAO : DAOAdapter<PERFIL_USUARIO, PerfilUsuarioModel, object>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public PerfilUsuarioDAO()
        {
            db = GetDb<COADSYSEntities>(false);
        }

        public List<PerfilUsuarioModel> BuscarLista(string _usu_login)
        {
            List<PerfilUsuarioModel> _perfil = (from p in db.PERFIL_USUARIO
                                                where (p.USU_LOGIN == _usu_login)
                                                select new PerfilUsuarioModel()
                                                {   EMP_ID = p.EMP_ID,
                                                    PER_ID = p.PER_ID,
                                                    USU_LOGIN = p.USU_LOGIN,
                                                    PUS_DEFAULT = p.PUS_DEFAULT,
                                                    PUS_ATIVO = p.PUS_ATIVO
                                                }).ToList();

            return _perfil;
        }
        public void ExcluirPorUsuario(string _usu_login)
        {
            try
            {
                List<PERFIL_USUARIO> listaperfil = (from p in db.PERFIL_USUARIO
                                                    where (p.USU_LOGIN == _usu_login)
                                                    select p).ToList();

                foreach (var perfil in listaperfil)
                {
                    this.Excluir(perfil);
                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(ex.InnerException.InnerException.Message, ex.InnerException.InnerException.HResult.ToString(), SessionContext.autenticado);
                throw;
            }

        }

        public bool HasPerfilUsuario(int EMP_ID, string PER_ID, string USU_LOGIN)
        {
            var query = GetDbSet().Where(x => x.EMP_ID == EMP_ID && 
                x.PER_ID == PER_ID && 
                x.USU_LOGIN == USU_LOGIN);

            return query.Count() > 0;
        }

        /// <summary>
        /// Pega todos os perfis do usuário
        /// </summary>
        /// <returns></returns>
        public IList<PerfilUsuarioModel> ListByUsuLogin(string USU_LOGIN)
        {
            var query = GetDbSet().Where(x => x.USU_LOGIN == USU_LOGIN);
            return ToDTO(query);
        }

    }
}
