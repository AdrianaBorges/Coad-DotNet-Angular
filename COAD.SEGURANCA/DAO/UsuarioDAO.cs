using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using System.Threading.Tasks;
using System.Runtime.Serialization.Diagnostics;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Exceptions;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class UsuarioDAO : DAOAdapter<USUARIO, UsuarioModel, string>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public UsuarioDAO()
        {
          //db = GetDb<COADSYSEntities>(false);
        }
        public USUARIO Buscar(string _login)
        {
            var _usuario = (from u in db.USUARIO
                            where u.USU_LOGIN == _login
                            select u).First();

            return _usuario;
        }
        public List<USUARIO> Listar(int _emp_id)
        {
            List<USUARIO> _usuario = (from u in db.PERFIL_USUARIO
                                      where u.EMP_ID == _emp_id
                                      select u.USUARIO).ToList();

            return _usuario;
        }
        public List<PERFIL_USUARIO> ListarPorPerfil(int? _emp_id, string _per_id, string _login)
        {
            if (_emp_id == null)
                _emp_id = 0;

            if (_per_id == null)
                _per_id = ""; 
            
            if (_login == null)
                _login = "";

            List<PERFIL_USUARIO> _usuario = (from u in db.PERFIL_USUARIO
                                             where ((_emp_id == 0) || (_emp_id > 0 && u.EMP_ID == _emp_id)) &&
                                                   ((_per_id == "") || (_per_id != "" && u.PER_ID == _per_id)) &&
                                                   ((_login == "") || (_login != "" && u.USU_LOGIN.StartsWith(_login)))
                                            select u).ToList();

            return _usuario;
        }
        public List<PERFIL_USUARIO> ListarPerfil(int? _emp_id, string _login, string _sis_id)
        {
            List<PERFIL_USUARIO> _perfis = (from u in db.PERFIL_USUARIO
                                            where (u.EMP_ID == _emp_id) &&
                                                  (u.USU_LOGIN == _login) &&
                                                  (u.PERFIL.SIS_ID == _sis_id)
                                            select u).ToList();
            
            return _perfis;
        }

        public bool ChecarLogin(string login, string senha, string sistema)
        {
            senha = SessionContext.HashMD5(senha);
            var qtde = (from p in db.PERFIL_USUARIO
                        where (p.USUARIO.USU_LOGIN == login) &&
                              (p.USUARIO.USU_SENHA == senha) &&
                              (p.PERFIL.SIS_ID == sistema)
                        select p).Count();

            return (qtde > 0);
        }

        public Autenticado ValidarLogin(string _login, string _senha, string _sis_id)
        {
            try
            {
     
                int horatu = DateTime.Now.Hour;
                int minatu = DateTime.Now.Minute;
                int secatu = DateTime.Now.Second;

                int horini = 0;
                int minini = 0;
                int horfim = 0;
                int minfim = 0;
                

                Autenticado a = null;
                _senha = SessionContext.HashMD5(_senha);
                var qtde = (from p in db.PERFIL_USUARIO
                            where (p.USUARIO.USU_LOGIN == _login) && 
                                  (p.USUARIO.USU_SENHA == _senha) &&
                                  (p.PERFIL.SIS_ID == _sis_id)
                            select p).Count();

                if (qtde > 0)
                {
                    USUARIO _usulogado = (from u in db.PERFIL_USUARIO
                                          where (u.USUARIO.USU_LOGIN == _login) &&
                                                (u.USUARIO.USU_SENHA == _senha)
                                          select u.USUARIO).First();

                    if (_usulogado.USU_ATIVO == 0)
                        throw new LoginInvalidoException(" Falha no login. Verifique com o administrador do sistema. (0)");

                    if (_usulogado.USU_DATA_EXPIRA < DateTime.Now )
                        throw new LoginInvalidoException(" Falha no login. Verifique com o administrador do sistema. (1)");
                    
                    PERFIL_USUARIO pu = _usulogado.PERFIL_USUARIO.Where(x => x.PUS_DEFAULT == 1 && x.PERFIL.SIS_ID == _sis_id).FirstOrDefault();

                    if (pu == null)
                        throw new Exception(" O usuário não possui perfil de acesso para esta aplicação. (2)");

                    string[] horaini = pu.PERFIL.PER_HORA_INI.Split(char.Parse(":"));
                    string[] horafim = pu.PERFIL.PER_HORA_FIM.Split(char.Parse(":"));

                    horini = int.Parse(horaini[0]);
                    minini = int.Parse(horaini[1]);
                    horfim = int.Parse(horafim[0]);
                    minfim = int.Parse(horafim[0]);

                    if (SessionContext.CompararHora(horatu, minatu, secatu, horini, minini, 0) &&
                        !SessionContext.CompararHora(horatu, minatu, secatu, horfim, minfim, 0))
                        throw new Exception(" Acesso fora do hoário permitido para este grupo (3)" + pu.PERFIL.PER_HORA_INI + " a " + pu.PERFIL.PER_HORA_FIM);
                    
                    EMPRESA _empresa = (from e in db.EMPRESA where e.EMP_ID == _usulogado.EMP_ID select e).First();

                    a = new Autenticado();
                    a.EMP_GRP_COAD = _empresa.EMP_GRP_COAD;
                    a.USU_LOGIN = _usulogado.USU_LOGIN;
                    a.EMP_ID = _usulogado.EMP_ID;
                    a.ADMIN = (_usulogado.USU_ADMIN == 1);
                    a.PER_ID = pu.PER_ID;
                    a.USU_NOME = pu.USUARIO.USU_NOME;
                    a.SIS_ID = pu.PERFIL.SIS_ID;
                    a.REP_ID = pu.USUARIO.REP_ID;
                    a.USU_CPF = pu.USUARIO.USU_CPF;
                    a.USU_NOVA_SENHA = (int)_usulogado.USU_NOVA_SENHA;
                    _usulogado.USU_DATA_ULTIMOACESSO = _usulogado.USU_DATA_LOGIN;
                    _usulogado.USU_DATA_LOGIN = DateTime.Now;
                    a.USU_ADMIN_LOGIN_PERFIL = _usulogado.USU_ADMIN_LOGIN_PERFIL;
                    a.EMAIL = _usulogado.USU_EMAIL;
                    a.EMAIL_SENHA = _usulogado.USU_EMAIL_SENHA;
                    a.DATA_LOGIN  = DateTime.Now;
                    a.MEIO_ACESSO = "COADCORP";

                    this.SalvarReg(_usulogado);

                }
              
                return a;
                
            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" +  SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex), ex);
               
            }
        }
        public USUARIO ValidarLoginEmail(string _login, string _email)
        {
            try
            {
                USUARIO _usuario = null;
                
                var qtde = (from u in db.USUARIO
                            where u.USU_LOGIN == _login && u.USU_EMAIL == _email
                            select u).Count();

                if (qtde > 0)
                   _usuario = (from u in db.USUARIO
                              where u.USU_LOGIN == _login && u.USU_EMAIL == _email
                             select u).First();

                return _usuario;

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));
            }
        }
        public List<Menu> MontaMenu(string _perfil, int _emp_id, string _sis_id, bool _admin)
        {
            return this.MontaMenuGeral(_perfil, _emp_id, _sis_id, _admin, false);
        }
        public List<Menu> MontaMenuManutencao(string _perfil, int _emp_id, string _sis_id, bool _admin)
        {
            return this.MontaMenuGeral(_perfil, _emp_id, _sis_id, _admin, true);
        }
        private List<Menu> MontaMenuGeral(string _perfil, int _emp_id, string _sis_id, bool _admin, bool todos )
        {
            var _item_menu = (from i in db.ITEM_MENU_PERFIL
                              where i.ITEM_MENU.ITM_ATIVO == 1 &&
                                    ((todos == true) || ( todos == false && i.NIV_ACESSO == 1)) &&
                                    i.PER_ID == _perfil &&
                                    i.EMP_ID == _emp_id &&
                                    i.ITEM_MENU.SIS_ID == _sis_id //&& ((_admin == true) || (_admin == false && i.ITEM_MENU.ADMIN != true)) 
                              orderby i.ITEM_MENU.ITM_MENU_NIVEL, i.ITEM_MENU.ITM_MENU_SEQ, i.ITEM_MENU.ITM_NODE_ID
                              select i).ToList();

            List<Menu> lista = new List<Menu>(); 

            foreach (var itm in _item_menu)
            {

                if (lista.Where(x => x.MenuEmpId != itm.EMP_ID && x.MenuPerid != itm.PER_ID &&  x.MenuId != itm.ITM_ID).Count() == 0)
                {
                    Menu itens = new Menu();
                    itens.MenuEmpId = itm.EMP_ID;
                    itens.MenuPerid = itm.PER_ID;
                    itens.MenuId = itm.ITM_ID;
                    itens.MenuNodeId = (itm.ITEM_MENU.ITM_NODE_ID != null) ? (int) itm.ITEM_MENU.ITM_NODE_ID : 0;
                    itens.MenuNivel = (itm.ITEM_MENU.ITM_MENU_NIVEL != null) ?  (int)itm.ITEM_MENU.ITM_MENU_NIVEL : 0;
                    itens.MenuOrden = (itm.ITEM_MENU.ITM_MENU_SEQ != null) ? (int)itm.ITEM_MENU.ITM_MENU_SEQ : 0;
                    itens.MenuText = itm.ITEM_MENU.ITM_DESCRICAO;
                    itens.MenuUrl = itm.ITEM_MENU.ITM_NOME_ARQUIVO;
                    itens.MenuValue = itm.ITEM_MENU.ITM_ARQUIVO;

                    itens.MenuNivAcesso = (int)itm.ITEM_MENU.ITEM_MENU_PERFIL.Where(s => s.PER_ID == itm.PER_ID &&
                                                                                         s.EMP_ID == itm.EMP_ID &&
                                                                                         s.ITM_ID == itm.ITM_ID).First().NIV_ACESSO;
                    itens.MenuNivInsert = (int)itm.ITEM_MENU.ITEM_MENU_PERFIL.Where(s => s.PER_ID == itm.PER_ID &&
                                                                                         s.EMP_ID == itm.EMP_ID &&
                                                                                         s.ITM_ID == itm.ITM_ID).First().NIV_INSERT;
                    itens.MenuNivEdit = (int)itm.ITEM_MENU.ITEM_MENU_PERFIL.Where(s => s.PER_ID == itm.PER_ID &&
                                                                                         s.EMP_ID == itm.EMP_ID &&
                                                                                         s.ITM_ID == itm.ITM_ID).First().NIV_EDIT;
                    itens.MenuNivDelete = (int)itm.ITEM_MENU.ITEM_MENU_PERFIL.Where(s => s.PER_ID == itm.PER_ID &&
                                                                                         s.EMP_ID == itm.EMP_ID &&
                                                                                         s.ITM_ID == itm.ITM_ID).First().NIV_DELETE;


                    if (itens.MenuNivel == 0)
                    {
                        if (itens != null)
                            lista.Add(itens);
                    }
                    else
                        this.MontaSubMenu(lista, itens);
                }

            }

            return lista;

        }
        private void MontaSubMenu(List<Menu> _mnu, Menu _submnu)
        {
            foreach (var _itm in _mnu)
            {
                if (_itm.MenuId == _submnu.MenuNodeId)
                    _itm.MenuItens.Add(_submnu);
                else
                {
                    if (_itm.MenuItens != null)
                    {
                        foreach (var _subitm in _itm.MenuItens)
                        {
                            if (_subitm.MenuId == _submnu.MenuNodeId)
                                _subitm.MenuItens.Add(_submnu);
                        }
                    }
                }

            }
        }
        public void AlterarSenha(USUARIO _usuario)
        {
            try
            {
                int _usu_nova_senha = (int)_usuario.USU_NOVA_SENHA;
                string _nsenha = SessionContext.HashMD5(_usuario.USU_SENHA);

                _usuario = this.Buscar(_usuario.USU_LOGIN);
                _usuario.USU_SENHA = _nsenha;
                _usuario.USU_NOVA_SENHA = _usu_nova_senha;
                
                this.Salvar(_usuario);

                SysException.RegistrarLog("Senha alterada com sucesso!!", "", SessionContext.autenticado);
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog("Erro ao alterar senhado usuário (" + SysException.Show(ex)  +")", SysException.ShowIdException(ex), SessionContext.autenticado);
                throw;
            }
        }
        public virtual void IncluirReg(USUARIO usuario)
        {
            try
            {
                // Envolvendo o processo por um TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    this.Incluir(usuario);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                throw;
            }
        }
        public virtual void IncluirReg(USUARIO usuario, List<PERFIL_USUARIO> ListaPerfil)
        {
            try
            {
                if (ListaPerfil.Count < 1)
                    throw new Exception("O usuário deve possuir no minimo 1 perfil.");

                // Envolvendo o processo por um TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    this.Incluir(usuario);

                    foreach (PERFIL_USUARIO item in ListaPerfil)
                    {
                        new PerfilUsuarioDAO().Incluir(item);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                throw;
            }
        }
        public virtual void SalvarReg(USUARIO usuario)
        {
            try
            {
                // Envolvendo o processo por um TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    this.Salvar(usuario);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                throw;
            }
        }
        public virtual void SalvarReg(USUARIO usuario, List<PERFIL_USUARIO> ListaPerfil)
        {
            try
            {
                if (ListaPerfil.Count < 1)
                    throw new Exception("O usuário deve possuir no minimo 1 perfil.");

                // Envolvendo o processo por um TransactionScope
                using (TransactionScope scope = new TransactionScope())
                {
                    this.Salvar(usuario);
                    
                    PerfilUsuarioDAO perfis = new PerfilUsuarioDAO();

                    perfis.ExcluirPorUsuario(usuario.USU_LOGIN);

                    foreach (PERFIL_USUARIO item in ListaPerfil)
                    {
                        perfis.Incluir(item);
                    } 

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                throw;
            }

        }

        /// <summary>
        /// Verifica se o usuário tem perfil
        /// </summary>
        /// <param name="perfil"></param>
        /// <param name="_emp_id"></param>
        /// <param name="_login"></param>
        /// <param name="_sis_id"></param>
        /// <returns></returns>
        public bool PossuiPerfil(string perfil ,int? _emp_id, string _login, string _sis_id)
        {
            int count = (from u in db.PERFIL_USUARIO
                        where (u.EMP_ID == _emp_id) &&
                              (u.USU_LOGIN == _login) &&
                              (u.PERFIL.SIS_ID == _sis_id) &&
                              (u.PERFIL.PER_ID == perfil)
                        select u).Count();

            return (count > 0) ? true : false;
        }
        public string GetSenhaFromUsuario(string USU_LOGIN)
        {
            var obj = FindById(USU_LOGIN);

            if (obj != null)
            {
                return obj.USU_SENHA;
            }

            return null;
        }
        public Pagina<UsuarioModel> Usuarios(int? _emp_id, string _per_id, string _usu_login, string loginGerenteDP = null, string nome = null, string cpf = null, int pagina = 1, int registrosPorPagina = 7)
        {
            IQueryable<USUARIO> query = GetDbSet();
            var db = GetDb<COADSYSEntities>(false);

            if (_emp_id != null)
            {
                query = query.Where(x => x.EMP_ID == _emp_id);
            }

            if (!string.IsNullOrWhiteSpace(_per_id))
            {
                query = query.Where(x => (from per_usu in db.PERFIL_USUARIO
                                  where per_usu.PER_ID ==  _per_id
                                  select per_usu.USU_LOGIN).Contains(x.USU_LOGIN));
            }

            if (!string.IsNullOrWhiteSpace(_usu_login))
            {
                query = query.Where(x => x.USU_LOGIN.Contains(_usu_login));
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.USU_NOME.Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                query = query.Where(x => x.USU_CPF.Contains(cpf));
            }

            if(!string.IsNullOrWhiteSpace(loginGerenteDP))
            {
                query = query.Where(x => (from per_usu in db.PERFIL_USUARIO
                                          where (from per_gerente in db.PERFIL_USUARIO
                                                 where (bool)per_gerente.PERFIL.GERENCIA && per_gerente.USU_LOGIN.Equals(loginGerenteDP)
                                                       select per_gerente.PERFIL.DP_ID).Contains(per_usu.PERFIL.DP_ID)
                                          select per_usu.USU_LOGIN).Contains(x.USU_LOGIN) 
                                          && x.USU_ADMIN != 1);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        /// <summary>
        /// Traz todos os usuário relazionados a um usuário
        /// </summary>
        /// <param name="lstRepIds"></param>
        /// <returns></returns>
        public IList<UsuarioModel> ListByRepIds(IEnumerable<int?> lstRepIds)
        {
            var query = GetDbSet().Where(x => lstRepIds.Contains(x.REP_ID));
            return ToDTO(query);
        }

        /// <summary>
        /// Traz todos os usuários que possuem o representante
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public IList<UsuarioModel> FindByRepId(int? REP_ID)
        {
            var query = GetDbSet().Where(x => x.REP_ID == REP_ID);
            return ToDTO(query);
        }

        /// <summary>
        /// Traz o primeiro usuário que possue o representante
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public UsuarioModel FindFirstByRepId(int? REP_ID)
        {
            var query = GetDbSet().Where(x => x.REP_ID == REP_ID);

            var obj = query.FirstOrDefault();
            return ToDTO(obj);
        }


        /// <summary>
        ///  Checa se o cpf já existe em outro usuário
        /// </summary>
        /// <param name="cpf">CPF a ser checado</param>
        /// <returns></returns>
        public bool ChecaCPFNaoDuplicado(string cpf)
        {
            return ChecaCPFNaoDuplicado(cpf, null);
        }

        /// <summary>
        /// Checa se o cpf já existe em outro usuário
        /// Essa checagem deve ser utilizado para operações de edição.
        /// O usu login é o login de quem está sendo editado. Dessa forma
        /// o usuário não será levando em conta na hora de checagem
        /// </summary>
        /// <param name="cpf">CPF a ser checado</param>
        /// <param name="usuLogin">Usuário que deve ser excluido da checagem</param>
        /// <returns></returns>
        public bool ChecaCPFNaoDuplicado(string cpf, string usuLogin)
        {

            if (!string.IsNullOrEmpty(cpf))
            {
                var query = GetDbSet().Where(x => x.USU_CPF == cpf);

                if (!string.IsNullOrEmpty(usuLogin))
                {
                    query = query.Where(x => x.USU_LOGIN != usuLogin);
                }

                var count = query.Count();
                return (count > 0);
            }

            return true;
                                   
        }

        public bool ChecaUsuarioDisponivel(string usuLogin)
        {
            return ChecaUsuarioDisponivel(usuLogin, null);
        }

        public bool ChecaUsuarioDisponivel(string usuLogin, int? REP_ID)
        {
            if (!string.IsNullOrEmpty(usuLogin))
            {
                var query = GetDbSet().Where(x => x.USU_LOGIN == usuLogin && 
                    (x.REP_ID != null && (REP_ID == null || x.REP_ID != REP_ID)));


                var count = query.Count();
                return (count > 0);
            }

            return true;
        }

        /// <summary>
        /// Traz uma lista de usuários para serem utilizados no autocomplete
        /// </summary>
        /// <param name="semRepresentente">Indica se os usuários na lista não deve estar associado a um representante</param>
        /// <returns></returns>
        public IList<AutoCompleteDTO> UsuarioAutoCompleteDTO(bool naoAssociadoAUmRepresentante = false)
        {
            IQueryable<USUARIO> query = GetDbSet();

            if (naoAssociadoAUmRepresentante)
            {
                query = query.Where(x => x.REP_ID == null);
            }            
    
            var lstUsuario = query.Select(sel => new AutoCompleteDTO(){
                
                label = sel.USU_LOGIN,
                value = sel.USU_LOGIN
            });

            return lstUsuario.ToList();            
        }

        public Pagina<UsuarioModel> BuscarUsuarios(
            string login,
            string nome,
            string cpf, 
            bool cpfExato,
            string email, 
            bool naoAssociadosARepresentante,
            int pagina = 1, 
            int registrosPorPagina = 15)
        {
            var query = (from usu in db.USUARIO
                         where
                            usu.USU_ATIVO == 1 &&
                            (login == null || usu.USU_LOGIN.Contains(login)) &&
                            (nome == null || usu.USU_LOGIN.Contains(nome)) &&
                            (cpf == null || ((cpfExato == true && usu.USU_CPF == cpf) || (cpfExato == false && usu.USU_CPF.Contains(cpf)))) &&
                            (email == null || usu.USU_EMAIL == email) &&
                            (naoAssociadosARepresentante == false || (naoAssociadosARepresentante == true && usu.REP_ID == null))
                         select usu);

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

        public IList<int> BuscarRepIdDosUsuarios(
            string login = null,
            string nome = null,
            string cpf = null,
            bool cpfExato = true,
            string email = null,
            string queryStr = null)
        {

            if (string.IsNullOrWhiteSpace(login) &&
                string.IsNullOrWhiteSpace(nome) &&
                string.IsNullOrWhiteSpace(cpf) &&
                string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(queryStr))
                return null;

            //if (string.IsNullOrWhiteSpace(login)) 
              //  if(string.IsNullOrWhiteSpace(nome))

             //   if(string.IsNullOrWhiteSpace(cpf))

             //   if (string.IsNullOrWhiteSpace(email))

                var query = (from usu in db.USUARIO
                         where
                            usu.USU_ATIVO == 1 &&
                            (login == null || usu.USU_LOGIN.Contains(login)) &&
                            (nome == null || usu.USU_NOME.Contains(nome)) &&
                            (cpf == null || ((cpfExato == true && usu.USU_CPF == cpf) || (cpfExato == false && usu.USU_CPF.Contains(cpf)))) &&
                            (email == null || usu.USU_EMAIL == email) &&
                            usu.REP_ID != null && 
                            (queryStr == null || 
                                (
                                    usu.USU_LOGIN.Contains(queryStr) ||
                                    usu.USU_NOME.Contains(queryStr) ||
                                    usu.USU_CPF.Contains(queryStr) ||
                                    usu.USU_EMAIL.Contains(queryStr)                                    
                                )
                            )
                         select usu.REP_ID.Value);

            return query.ToList();
        }

        public IList<UsuarioModel> ListarUsuariosPorPerfil(
            string perfil = null)
        {
            if (string.IsNullOrWhiteSpace(perfil))
                perfil = null;

            var query = (from 
                            usu in db.USUARIO join
                            per_usu in db.PERFIL_USUARIO on usu.USU_LOGIN equals per_usu.USU_LOGIN join
                            per in db.PERFIL on per_usu.PER_ID  equals per.PER_ID
                         where
                            usu.USU_ATIVO == 1 &&
                            per.PER_ID == perfil
                         select usu);

            return ToDTO(query);
        }

    }
}
