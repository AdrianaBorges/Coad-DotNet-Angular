using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using System.Web;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Web.Security;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Service.Base;
using COAD.SEGURANCA.Exceptions;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Constants;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("USU_LOGIN")]
    public class UsuarioSRV : ServiceAdapter<USUARIO, UsuarioModel, string>
    {
        private UsuarioDAO _dao { get; set; }
        public PerfilUsuarioSRV _perfilUsuarioSRV { get; set; }

        public UsuarioSRV()
        {
            _dao = new UsuarioDAO();
            _perfilUsuarioSRV = new PerfilUsuarioSRV();
            SetDao(_dao);
            //SetKeys("USU_LOGIN");
        }

        public UsuarioSRV(UsuarioDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
            //SetKeys("USU_LOGIN");
        }

        override 
        public void IncluirReg(USUARIO item)
        {
            item.USU_DATA_CAD = DateTime.Now;
            new UsuarioDAO().IncluirReg(item);
        }

        public USUARIO Buscar(string _login)
        {
            return new UsuarioDAO().Buscar(_login);
        }
        public List<USUARIO> Listar(int _emp_id)
        {
            return new UsuarioDAO().Listar(_emp_id);
        }
        public List<PERFIL_USUARIO> ListarPorPerfil(int? _emp_id, string _per_id, string _login)
        {
            return new UsuarioDAO().ListarPorPerfil(_emp_id, _per_id, _login);
        }
        public List<PERFIL_USUARIO> ListarPerfil(int _emp_id, string _login, string _sis_id)
        {
            return new UsuarioDAO().ListarPerfil(_emp_id, _login, _sis_id);
        }

        public bool ChecarLogin(string login, string senha, string sistema)
        {
            return _dao.ChecarLogin(login, senha, sistema);
        }

        public Autenticado ValidarLogin(string _login, string _senha, string _sis_id)
        {
            try
            {
                return _dao.ValidarLogin(_login, _senha, _sis_id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public USUARIO ValidarLoginEmail(string _login, string _email)
        {
            try
            {
                return new UsuarioDAO().ValidarLoginEmail(_login, _email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Menu> MontaMenu(string _perfil, int _emp_id, string _sis_id, bool _admin)
        {
            return new UsuarioDAO().MontaMenu(_perfil, _emp_id, _sis_id, _admin);
        }
        public List<Menu> MontaMenuManutencao(string _perfil, int _emp_id, string _sis_id, bool _admin)
        {
            return new UsuarioDAO().MontaMenuManutencao(_perfil, _emp_id, _sis_id, _admin);
        }
        public void AlterarSenha(USUARIO usuario)
        {
            new UsuarioDAO().AlterarSenha(usuario);
        }
        public void SalvarReg(USUARIO usuario, List<PERFIL_USUARIO> ListaPerfil)
        {
            new UsuarioDAO().SalvarReg(usuario, ListaPerfil);
        }
        public void IncluirReg(USUARIO usuario, List<PERFIL_USUARIO> ListaPerfil)
        {
            usuario.USU_DATA_CAD = DateTime.Now;
            new UsuarioDAO().IncluirReg(usuario, ListaPerfil);
        }
        public string RealizarLogin(string _login, string _senha, string _sistema)
        {
            string url = "";
     
            Autenticado _autenticado = new Autenticado();

            if (_login.Length > 0 && _senha.Length > 0)
            {
                try
                {
                    //UsuarioSRV _usuario = new UsuarioSRV();

                    //_autenticado = _usuario.ValidarLogin(_login, _senha, _sistema);

                    _autenticado = ValidarLogin(_login, _senha, _sistema);

                    if (_autenticado == null)
                        throw new Exception("Login e/ou senha inválida !!");

                    _autenticado.IP_ACESSO = SessionContext.GetIp();
                    _autenticado.PATH = url;
                    _autenticado.SESSION_ID = "";

                    //SessionContext.autenticado = _autenticado;
                    //SessionContext.perfis_usuario = _usuario.ListarPerfil(_autenticado.EMP_ID, _autenticado.USU_LOGIN, _autenticado.SIS_ID);
                    //SessionContext.menu_usuario = _usuario.MontaMenu(_autenticado.perId, _autenticado.EMP_ID, _autenticado.SIS_ID, _autenticado.ADMIN);
                    //SessionContext.sistemas_coad = new SistemaSRV().Listar();

                    SysException.RegistrarLog("LogIn Usuário (" + _autenticado.USU_LOGIN + ")", "", _autenticado);

                    if (_autenticado.USU_NOVA_SENHA == 1)
                        return "A sua senha é provisória e deve ser alterada.";

                    return "";

                }
                catch (DbEntityValidationException dbEx)
                {
                    string _erro = "";

                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            _erro += String.Format("Erro ao gravar a nota fiscal: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    SysException.RegistrarLog(_erro, "", SessionContext.autenticado);

                    throw new Exception(_erro);
                }
                catch (DbUpdateException e)
                {
                    SysException.RegistrarLog(SysException.Show(e), SysException.ShowIdException(e), SessionContext.autenticado);

                    throw new Exception(SysException.Show(e));
                }
                catch (EntityException ex)
                {
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                    throw new Exception(SysException.Show(ex));
                }
                catch (Exception ex)
                {
                    _autenticado = new Autenticado();

                    _autenticado.PATH = url;
                    _autenticado.IP_ACESSO = SessionContext.GetIp();

                    SysException.RegistrarLog("Tentativa de Login (" + _login + ")" + SysException.Show(ex), SysException.ShowIdException(ex), _autenticado);

                    throw new Exception(ex.Message);
                }
            }

            _autenticado = new Autenticado();

            _autenticado.PATH = url;
            _autenticado.IP_ACESSO = SessionContext.GetIp();

            SysException.RegistrarLog("Tentativa de Login (" + _login + ")", "", _autenticado);

            throw new Exception("Login e/ou Senha inválida!");

        }
        public string RealizarLogin(string _login, string _senha, string _sistema, HttpContext _url)
        {
            string url = _url.Request.Url.AbsoluteUri;
            string path = _url.Request.Url.AbsolutePath;

            Autenticado _autenticado = new Autenticado();
            
            if (_login.Length > 0 && _senha.Length > 0)
            {
                try
                {
                    UsuarioSRV _usuario = new UsuarioSRV();

                    _autenticado = ValidarLogin(_login, _senha, _sistema);

                    if (_autenticado == null)
                        throw new Exception("Login e/ou senha inválida !!");

                    var _outrasessao = SessionContext.FindSessionGlobal(_url, _login);


                    if (_login != "MOBILEUSER")
                        if (_outrasessao != null)
                            SessionContext.RemoveSessionGlobal(_url, _outrasessao.SESSION_ID);
                    
                    _url.Session.Timeout = 240;

                    _autenticado.IP_ACESSO = SessionContext.GetIp();
                    _autenticado.PATH = url;
                    _autenticado.SESSION_ID = _url.Session.SessionID;
                    _autenticado.SESSION_TIMEOUT = _url.Session.Timeout;
                    _autenticado.SESSION_TIMEOUT_RESTANTE = _url.Session.Timeout;
                    _autenticado.strSiteUrl = "http://" + _url.Request.Url.Authority.ToString(); 

                    SessionContext.autenticado = _autenticado;
                    SessionContext.empresa_do_grupo_coad = _autenticado.EMP_GRP_COAD;
                    SessionContext.perfis_usuario = _usuario.ListarPerfil(_autenticado.EMP_ID, _autenticado.USU_LOGIN, _autenticado.SIS_ID);
                    SessionContext.menu_usuario = _usuario.MontaMenu(_autenticado.PER_ID, _autenticado.EMP_ID, _autenticado.SIS_ID, _autenticado.ADMIN);
                    SessionContext.sistemas_coad = new SistemaSRV().Listar();

                    SessionContext.AddSessionGlobal(_url);

                    FormsAuthentication.SetAuthCookie(_autenticado.USU_LOGIN, false);

                    SysException.RegistrarLog("LogIn Usuário (" + _autenticado.USU_LOGIN + ")", "", _autenticado);

                    if (_autenticado.USU_NOVA_SENHA == 1)
                        return "A sua senha é provisória e deve ser alterada.";

                    return "";

                }
                catch (DbEntityValidationException dbEx)
                {
                    string _erro = "";

                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            _erro += String.Format("Erro ao realizar login: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    SysException.RegistrarLog(_erro, "", SessionContext.autenticado);

                    throw new Exception(_erro);
                }
                catch (DbUpdateException e)
                {
                    SysException.RegistrarLog(SysException.Show(e), SysException.ShowIdException(e), SessionContext.autenticado);

                    throw new Exception(SysException.Show(e));
                }
                catch (EntityException ex)
                {
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                    throw new Exception(SysException.Show(ex));
                }
                catch (Exception ex)
                {
                    _autenticado = new Autenticado();

                    _autenticado.PATH = url;
                    _autenticado.IP_ACESSO = SessionContext.GetIp();

                    var msgerro = "Tentativa de Login (" + _login + ")" + SysException.Show(ex);

                    SysException.RegistrarLog(msgerro, SysException.ShowIdException(ex), _autenticado);

                    throw new Exception(msgerro);
                }
            }

            _autenticado = new Autenticado();

            _autenticado.PATH = url;
            _autenticado.IP_ACESSO = SessionContext.GetIp();

            SysException.RegistrarLog("Tentativa de Login (" + _login + ")", "", _autenticado);

            throw new Exception("Login e/ou Senha inválida!");

        }

        /// <summary>
        /// Verifica se o usuário tem perfil
        /// </summary>
        /// <param name="perfil"></param>
        /// <param name="_emp_id"></param>
        /// <param name="_login"></param>
        /// <param name="_sis_id"></param>
        /// <returns></returns>
        public bool PossuiPerfil(string perfil, int? _emp_id, string _login, string _sis_id)
        {
            return new UsuarioDAO().PossuiPerfil(perfil, _emp_id, _login, _sis_id);
        }

        public UsuarioModel FindByIdFullLoaded(string USU_LOGIN, bool trazSenha = false)
        {
            var usuario = FindById(USU_LOGIN);

            if (usuario != null)
            {                
                _perfilUsuarioSRV.PreencherPerfisDoUsuario(usuario);
            }

            return usuario;
        }



        public void _validarCpfDuplicado(UsuarioModel usuario, bool update = false){


            var cpf = usuario.USU_CPF;
            bool error = false;

            if (!string.IsNullOrEmpty(cpf))
            {
                if (update)
                {
                    var usu_login = usuario.USU_LOGIN;
                    error = ChecaCPFNaoDuplicado(cpf, usu_login);
                }
                else
                {
                    error = ChecaCPFNaoDuplicado(cpf);
                }               
                
            }

            if (error)
            {
                throw new ValidacaoException("O CPF informado já pertence a outro usuário.");
            }
                                
        }

        /// <summary>
        /// Verifica se o usuário pode ser salvo para o representante
        /// Se não puder é disparada uma exception
        /// </summary>
        /// <param name="usuario"></param>
        public void ValidarUsuarioDisponivel(UsuarioModel usuario)
        {
            if (usuario != null)
            {
                var REP_ID = usuario.REP_ID;
                var USU_LOGIN = usuario.USU_LOGIN;
                bool error = false;

                if (REP_ID != null)
                {
                    error = ChecaUsuarioDisponivel(USU_LOGIN, REP_ID);
                }
                else
                {
                    error = ChecaUsuarioDisponivel(USU_LOGIN, REP_ID);
                }

                if (error)
                {
                    throw new ValidacaoException("Já existe um representante associado a esse usuário.");
                }
            }
        }

        private UsuarioModel _processarSalvamento(UsuarioModel usuario, SenhaDTO dto, bool update = true, bool franquia = false, string perId = "REPRESENTANTE")
        {
            if (usuario != null)
            {
                if (usuario.EMP_ID == null)
                {
                    usuario.EMP_ID = 1;
                }

                if (franquia && update)
                {
                    AdicionarPerfilDeAcessoERevogarPerfisQueNaoPertencemAoUsuario(usuario, perId);
                }

                var usu = FindById(usuario.USU_LOGIN); // pego o usuario do banco
                if (usu != null && update) // se ele existir e for um update
                {
                    _validarCpfDuplicado(usuario, true); // verifico o cpf
                    return Merge(usuario);
                }
                else if (usu != null && !update && !franquia) // se o usuário foi achado mas não é um update e nem uma operação da franquia dá erro
                {
                    throw new Exception("Não é possível criar esse usuário. O login já está sendo usado.");
                }
                else
                {

                    if (usu != null)
                    {
                        throw new Exception("Não é possível criar esse usuário. O login já está sendo usado.");
                    }
                    _validarCpfDuplicado(usuario, false);
                    if (franquia) // se for uma inclusão e tem como origem a franquia
                    {
                       
                        return _processarInclusaoDeUsuarioDaFranquia(usuario, dto, perId);
                    }
                    return _processarInclusaoDeUsuario(usuario, dto);
                }
            }

            return null;
        }

        private UsuarioModel _processarInclusaoDeUsuario(UsuarioModel usuario, SenhaDTO dto)
        {
            usuario.USU_DATA_CAD = DateTime.Now;
            usuario.USU_NOVA_SENHA = 1;           
            var usuarioSalvo = usuario;

            if (dto != null)
            {
                var senha = GerarSenha(usuario);
                dto.SenhaLiteral = senha;
            }

            // Verifico se o usuário está sendo cadastrado por um administrador de perfil.
            // Nesse caso o usuário cadastrado terá os mesmos perfis do usuário cadastrante.
            // Ou seja, o usuário cadastrante será seus perfis de template
            var cadastradoPor = usuario.CADASTRADO_POR;

            if (!string.IsNullOrWhiteSpace(cadastradoPor))
            {

                var usuarioCadastrante = FindByIdFullLoaded(cadastradoPor); // usuário administrador de perfil

                if (!usuarioCadastrante.USU_ADMIN_LOGIN_PERFIL)
                {
                    throw new Exception("O usuário está tentando cadastrar outro usuário, porém, o mesmo não possui permissão para isso.");
                }

                usuario.USU_ATIVO = 1;
                usuario.USU_ADMIN = 0;
                usuario.USU_ADMIN_LOGIN_PERFIL = false;
                usuario.EMP_ID = usuarioCadastrante.EMP_ID;

                var perfis = usuarioCadastrante.PERFIL_USUARIO;

                usuarioSalvo = Save(usuario);

                if (perfis != null)
                {
                    var perfisASerClonados = perfis.Where(x => x.PERFIL.SIS_ID != "CONFIGSIS" && x.PERFIL_CLONAVEL != null && (bool) x.PERFIL_CLONAVEL);
                    var perfisClonados = _perfilUsuarioSRV.ClonarPerfilUsuario(perfisASerClonados.AsQueryable(), usuarioSalvo.USU_LOGIN);
                    usuarioSalvo.PERFIL_USUARIO = perfisClonados;
                    usuario.PERFIL_USUARIO = perfisClonados;
                }
            }
            else
            {
                usuarioSalvo = Save(usuario);
            }
            return usuarioSalvo;
        }

        private UsuarioModel _processarInclusaoDeUsuarioDaFranquia(UsuarioModel usuario, SenhaDTO dto, string perId)
        {
            usuario.USU_DATA_CAD = DateTime.Now;
            usuario.USU_NOVA_SENHA = 1;
            var usuarioSalvo = usuario;

            if (dto != null)
            {
                var senha = GerarSenha(usuario);
                dto.SenhaLiteral = senha;
            }

            // Verifico se o usuário está sendo cadastrado por um administrador de perfil.
          

            usuario.USU_ATIVO = 1;
            usuario.USU_ADMIN = 0;
            usuario.USU_ADMIN_LOGIN_PERFIL = false;
            usuario.EMP_ID = 1;
            usuarioSalvo = Save(usuario);

            AdicionarPerfilDeAcessoERevogarPerfisQueNaoPertencemAoUsuario(usuario, perId);
            return usuarioSalvo;
        }

        public string GerarSenha(UsuarioModel usuario)
        {
            SenhaSRV _senhaSRV = new SenhaSRV();

             if (usuario != null)
             {
                 if (!string.IsNullOrWhiteSpace(usuario.USU_EMAIL))
                 {
                     var usuLogin = usuario.USU_LOGIN;
                     var senha = _senhaSRV.GerarSenhaAleatoria();
                     usuario.USU_SENHA = senha.SenhaHash;

                     var senhaLiteral = senha.SenhaLiteral;
                     return senhaLiteral;
                 }
             }
             return null;
        }

        public void EnviaEmail(string email, string usuLogin, string senhaLiteral)
        {

                if (!string.IsNullOrWhiteSpace(email) &&
                    !string.IsNullOrWhiteSpace(usuLogin) &&
                    !string.IsNullOrWhiteSpace(usuLogin)) {
                   

                    EmailSRV _emailSRV = new EmailSRV();

                    StringBuilder corpoEmail = new StringBuilder();
                    corpoEmail.Append("<div><strong><em>Bem Vindo ao COADCORP</em></strong></div>");
                    corpoEmail.Append("<div>Seu usuário do COADCORP foi gerado com sucesso.</div>");
                    corpoEmail.Append(string.Format("<div><strong>Usuário:</strong> {0}</div>", usuLogin));
                    corpoEmail.Append(string.Format("<div><strong>Senha:</strong> {0}</div>", senhaLiteral));

                    string strCorpoEmail = corpoEmail.ToString();
                    _emailSRV.EnviarEmail(new EmailRequestDTO()
                    {
                        EmailDestino = email,
                        Assunto = "Acesso do coadcorp",
                        CorpoEmail = strCorpoEmail
                    });
                    //email, "Acesso do coadcorp", strCorpoEmail);      
            }
        }

        public void SalvarUsuario(UsuarioModel usuario, bool update = true, bool franquia = false)
        {
            SenhaDTO senha = new SenhaDTO();
                if (usuario == null)
                {
                    return;
                }               
                _processarSalvamento(usuario, senha, update, franquia);               
                _perfilUsuarioSRV.ProcessarExclusaoEAtualizacaoPerfilUsuario(usuario);


            if (senha.SenhaLiteral != null)
            {
                EnviaEmail(usuario.USU_EMAIL, usuario.USU_LOGIN, senha.SenhaLiteral);
            }
        }

        /// <summary>
        /// Salva um usuário sem controle de transação. 
        /// Deve ser usuado para salvar usuários fora do configsis
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="update"></param>
        /// <param name="franquia"></param>
        public SenhaDTO SalvarUsuarioPorOutraAplicacao(UsuarioModel usuario, bool update = true, bool franquia = false, string perId = "REPRESENTANTE")
        {
            SenhaDTO senha = new SenhaDTO();
            if (usuario == null)
                {
                    return null;
                }
                _processarSalvamento(usuario, senha, update, franquia, perId);
                _perfilUsuarioSRV.ProcessarExclusaoEAtualizacaoPerfilUsuario(usuario);

                return senha;            
        }

        public string GetSenhaFromUsuario(string USU_LOGIN)
        {
            if (!string.IsNullOrWhiteSpace(USU_LOGIN))
            {
                return _dao.GetSenhaFromUsuario(USU_LOGIN);
            }
            return null;
        }

        public void PreencherSenhaUsuario(UsuarioModel usuario)
        {
            if (usuario != null && !string.IsNullOrWhiteSpace(usuario.USU_LOGIN))
            {
                var login = usuario.USU_LOGIN;
                usuario.USU_SENHA = GetSenhaFromUsuario(login);
            }
        }

        public override UsuarioModel Merge(UsuarioModel source, params string[] nameId)
        {
            PreencherSenhaUsuario(source);
            return base.Merge(source, nameId);
        }

        public override void MergeAll(IEnumerable<UsuarioModel> lstObj, bool saveChanges = true, params string[] nameId)
        {
            foreach (var obj in lstObj)
            {
                PreencherSenhaUsuario(obj);
            }

            base.MergeAll(lstObj, saveChanges, nameId);
        }

        public Pagina<UsuarioModel> Usuarios(int? _emp_id, string _per_id, string _usu_login, string loginGerenteDP = null, string nome = null, string cpf = null, int pagina = 1, int registrosPorPagina = 7, bool trazPerfilUsuario = false)
        {
           var paginaUsuario = _dao.Usuarios(_emp_id, _per_id, _usu_login, loginGerenteDP, nome, cpf, pagina, registrosPorPagina);
           var lstUsuario = paginaUsuario.lista;

           if (trazPerfilUsuario && lstUsuario != null)
           {
               foreach (var usuario in lstUsuario)
               {
                   if (usuario != null)
                   {
                       _perfilUsuarioSRV.PreencherPerfisDoUsuario(usuario);
                   }
               }
           }

           return paginaUsuario;
        }


        /// <summary>
        /// Traz todos os usuário relazionados a um usuário
        /// </summary>
        /// <param name="lstRepIds"></param>
        /// <returns></returns>
        public IList<UsuarioModel> ListByRepIds(IEnumerable<int?> lstRepIds)
        {
            var lstUsuarios = _dao.ListByRepIds(lstRepIds);
            return lstUsuarios;
        }


        /// <summary>
        /// Traz todos os usuários que possuem o representante
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public IList<UsuarioModel> FindByRepId(int? REP_ID)
        {
            var lstUsuarios = _dao.FindByRepId(REP_ID);
            return lstUsuarios;
        }

        /// <summary>
        /// Traz o primeiro usuário que possue o representante
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public UsuarioModel FindFirstByRepId(int? REP_ID)
        {
            var usuario = _dao.FindFirstByRepId(REP_ID);
            _perfilUsuarioSRV.PreencherPerfisDoUsuario(usuario);
            return usuario;
        }
      
        /// <summary>
        ///  Checa se o cpf já existe em outro usuário
        /// </summary>
        /// <param name="cpf">CPF a ser checado</param>
        /// <returns></returns>
        public bool ChecaCPFNaoDuplicado(string cpf)
        {
            return _dao.ChecaCPFNaoDuplicado(cpf);
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
            return _dao.ChecaCPFNaoDuplicado(cpf, usuLogin);
        }
        
        /// <summary>
        /// Verifica se o usuário passado já está associado a algum representante
        /// </summary>
        /// <param name="usuLogin"></param>
        /// <returns></returns>
        public bool ChecaRepresentanteDuplicado(string usuLogin)
        {
            return _dao.ChecaUsuarioDisponivel(usuLogin);
        }

        /// <summary>
        /// Verifica se o usuário passado já está associado a algum representante
        /// não levando em conta o id o representante passado
        /// </summary>
        /// <param name="usuLogin"></param>
        /// <param name="REP_ID">Id do representante que não será testado</param>
        /// <returns></returns>
        public bool ChecaUsuarioDisponivel(string usuLogin, int? REP_ID)
        {
            return _dao.ChecaUsuarioDisponivel(usuLogin, REP_ID);
        }

        /// <summary>
        /// Traz uma lista de usuários para serem utilizados no autocomplete
        /// </summary>
        /// <param name="semRepresentente">Indica se os usuários na lista não deve estar associado a um representante</param>
        /// <returns></returns>
        public IList<AutoCompleteDTO> UsuarioAutoCompleteDTO(bool naoAssociadoAUmRepresentante = false)
        {
            return _dao.UsuarioAutoCompleteDTO(naoAssociadoAUmRepresentante);
        }


        /// <summary>
        /// Adiciona a lista de perfil do usuário de acordo com os acessos e remove os perfis de franquia que não estão de acordo com os acessos.
        /// (Não realiza atualização no banco)
        /// </summary>
        /// <param name="usuario"></param>
        public void AdicionarPerfilDeAcessoERevogarPerfisQueNaoPertencemAoUsuario(UsuarioModel usuario, string perId = "REPRESENTANTE")
        {

            AdicionarPerfilUsuarioPorPerId(usuario, perId);
            RemoverPerfil(usuario, perId);
        }

        /// <summary>
        /// Adiciona a lista de perfil do usuário o perfil de representante
        /// (Não realiza atualização no banco)
        /// </summary>
        /// <param name="usuario"></param>
        public void AdicionarPerfilUsuarioPorPerId(UsuarioModel usuario, string perId = "REPRESENTANTE")
        {
            if (usuario != null)
            {
                int perfilEncontrados = 0;
              
                if (usuario.PERFIL_USUARIO != null && usuario.PERFIL_USUARIO.Count() > 0) // verifica se o usuário já possui esse perfil
                {
                    perfilEncontrados = usuario.PERFIL_USUARIO.Where(x => x.PER_ID == perId && x.EMP_ID == 1 && x.USU_LOGIN == usuario.USU_LOGIN).Count(); // pego a quantidade de ocorrências do perfil
                }

                if (perfilEncontrados <= 0) // se não for encontrado nenhum perfil de representante então eu o adiciono
                {
                    PerfilModel perfil = new PerfilSRV().FindById(1, perId);

                    if (perfil != null)
                    {
                        PerfilUsuarioModel perfilUsuario = new PerfilUsuarioModel()
                        {
                            EMP_ID = 1,
                            PER_ID = perfil.PER_ID,
                            PERFIL = perfil,
                            PERFIL_CLONAVEL = false,
                            PUS_ATIVO = 1,
                            PUS_DEFAULT = 1,
                            USU_LOGIN = usuario.USU_LOGIN,
                            USUARIO = usuario,
                            PUS_INSERIDO_EXTERNAMENTO = true
                        };

                        usuario.PERFIL_USUARIO.Add(perfilUsuario);
                    }
                    else
                    {
                        throw new Exception("Não é possível adicionar o perfil '" + perId + "' pois o mesmo não existe. Para continuar é necessário cria-lo");
                    }
                }
            }
        }


        /// <summary>
        /// Retira o REP_ID do usuário bem como o perfil de representante da lista de perfils
        /// </summary>
        /// <param name="usuario"></param>
        public void RemoverUsuarioDaFranquia(UsuarioModel usuario)
        {
            if(usuario != null){

                usuario.REP_ID = null;
                Merge(usuario);
                RemoverPerfil(usuario);
            }            
        }

        /// <summary>
        /// Retira o perfil de franquia da lista de perfis
        /// </summary>
        /// <param name="usuario"></param>
        public void RemoverPerfil(UsuarioModel usuario, string perId = null)
        {
            if (usuario != null)
            {
               
                if (usuario.PERFIL_USUARIO != null && usuario.PERFIL_USUARIO.Count() > 0) // verifico se o usuário possui usuario perfil
                {
                    // pego o perfil desejado para usa-lo na exclusão
                    var perfis = usuario.PERFIL_USUARIO.Where(x =>
                        ((x.PERFIL.DEPARTAMENTO != null) &&
                            (x.PERFIL.DEPARTAMENTO.DP_NOME != null && (x.PERFIL.DEPARTAMENTO.DP_NOME.ToUpper() == "FRANQUIADO") || (x.PERFIL.DEPARTAMENTO.DP_NOME.ToUpper() == "FRANQUIADOR")))
                        && x.EMP_ID == 1 && x.USU_LOGIN == usuario.USU_LOGIN
                        && x.PUS_INSERIDO_EXTERNAMENTO != null && ((bool)x.PUS_INSERIDO_EXTERNAMENTO)
                        && (perId == null || x.PER_ID != perId));

                    if (perfis != null) // se for encontrado indico que o perfil passado deve ser removido da lista.
                    {
                        foreach (var perfil in new List<PerfilUsuarioModel>(perfis))
                        {
                            usuario.PERFIL_USUARIO.Remove(perfil);

                        }
                        _perfilUsuarioSRV.ProcessarExclusaoEAtualizacaoPerfilUsuario(usuario);
                    }
                }
            }
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
            return _dao.BuscarUsuarios(login, nome, cpf, cpfExato, email, naoAssociadosARepresentante, pagina, registrosPorPagina);
        }

        public IList<int> BuscarRepIdDosUsuarios(
            string login = null,
            string nome = null,
            string cpf = null,
            bool cpfExato = true,
            string email = null,
            string queryStr = null)
        {
            return _dao.BuscarRepIdDosUsuarios(login, nome, cpf, cpfExato, email, queryStr);
        }

        public IList<UsuarioModel> ListarUsuariosPorPerfil(
            string perfil = null)
        {
            return _dao.ListarUsuariosPorPerfil(perfil);
        }
    }
}
