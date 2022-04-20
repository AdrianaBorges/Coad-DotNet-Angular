using Coad.GenericCrud.Security;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace COAD.SEGURANCA.Filter
{
    public enum NivelAcesso 
    {
        Acesso = 0, Editar = 1, Novo = 2, Excluir = 3
    }

    public class AutorizarAttribute : SecureAttribute
    {
        /// <summary>
        /// Limita o usuário a visualizar apenas as telas onde os há menus, que apontam para tela requerida, associados aos perfis do mesmo.
        /// </summary>
        public bool PorMenu 
        {    get 
            {
            return (_porMenu == null) ? !IsAjax : (bool)_porMenu;
            }
            set
            {
                _porMenu = value;
            }
        }
        private bool? _porMenu;

        /// <summary>
        /// Só Administradores podem ter acesso a funcionalidade
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Só Gerente de departamento podem ter acesso a funcionalidade
        /// </summary>
        public string GerenteDepartamento { get; set; }

        /// <summary>
        /// Se esse atributo for marcado como true faz com que a busca por nível de acesso (gerente, operador e etc) dê acesso a este nível e níveis com privilégios superiores.
        /// Ex: Se o nível informado foi 4 então os níveis com mais privilégios
        /// são aceitos, ou seja, níveis 3, 2, 1 e 0. 
        /// </summary>
        public bool PermitirNiveisPrivilegiosSuperiores { get; set; }

        /// <summary>
        /// Nivel de acesso destinado a usuários com poucos previlégios de acesso ao sistema, de um determinado departamento. 
        /// </summary>
        public string DepartamentoAcessoExterno { get; set; }


        /// <summary>
        /// Só quem está dentro do departamento podem ter acesso a funcionalidade
        /// </summary>
        public string Departamento { get; set; }
        
        /// <summary>
        /// Admimistradores de perfil podem ter acesso a funcionalidade
        /// </summary>
        public bool AdminDeLogins { get; set; }

        /// <summary>
        /// Ambos, administradores e administradores de perfil podem ter acesso a funcionalidade.
        /// </summary>
        public bool AdminOuAdminDeLogins { get; set; }


        /// <summary>
        /// Indica o nível de acesso que corresponde a funcionalidade que foi acionada. (Acesso,Editar,Novo,Delete)
        /// </summary>
        public string Acesso { get; set; }
        public NivelAcesso TipoAcesso { get; set; }
        

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var resp = true;
            var Raiz = "";

            if (SessionContext.autenticado != null)
            {   
                Raiz = '/'+httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
        
                Raiz += "/Index";

                SessionContext.autenticado.PATH = httpContext.Request.Url.AbsolutePath;

                //if (SessionContext.autenticado.USU_LOGIN == "esoliveira" ||
                //    SessionContext.autenticado.USU_LOGIN == "MOBILEUSER" ||
                //    SessionContext.autenticado.USU_LOGIN == "COADSYS" ||
                //    SessionContext.autenticado.USU_LOGIN == "GRATUITO" ||
                //    SessionContext.autenticado.USU_LOGIN == "MOBILEUSER" ||
                //    SessionContext.autenticado.USU_LOGIN == "ONLINE")
                //{
                //    httpContext.Response.Redirect("Benvindo");
                //}

            }
            
            var path = httpContext.Request.Path;

            //if (path != "/notificacoes/ListarQtdNotificacoesNaoLidas")
            //{
            //    path = path;
            //}

            if (path == "/Proposta/SalvarProposta")
            {
                path = path;
            }


            if (!SessionContext.EhAutenticado())
            {
                FormsAuthentication.SignOut();
                return false;
            }

            //-------Verfica se o usuário esta na sessão global. Caso esteja ele é removido da sessão.

            var _sessaoglobal = SessionContext.VerificaSessionGlobal(System.Web.HttpContext.Current, SessionContext.autenticado.USU_LOGIN);

            if (_sessaoglobal == null)
            {
                SessionContext.RemoveSession(System.Web.HttpContext.Current);
               
                SysException.RegistrarLog("Logout", "", SessionContext.autenticado);

                ErroMsg = @"Não é permitido login simultaneo. Seu usuário foi desconectado.";

                FormsAuthentication.SignOut();
            
                return false;
            }
            

            logado = true; 

            var autenticado = SessionContext.autenticado;
            
            // Verifico se é para restringir se for um admin ou admin de login
            if (AdminOuAdminDeLogins) // se for true verifico se ele possui pelo menos um dos atributos acima.
            {
                resp = (resp && (SessionContext.admin || SessionContext.administradorDeLogin));                
            }
            else // senão
            {
                if (Admin) // se só admins podem acessar
                {
                    resp = (resp && SessionContext.admin); // verifico se ele é admin
                   
                }

                if (AdminDeLogins) // se só admins de perfil podem ver
                {
                    resp = (resp && SessionContext.administradorDeLogin); // verifico se ele é um admin de perfil                   

                }
            }

            if (PorMenu && (Acesso==null && TipoAcesso == default(NivelAcesso)))
            {
                resp = (resp && RestrictByMenu(path));
            }

            if (Acesso != null || TipoAcesso != default(NivelAcesso))
            {
                if (Acesso != null)
                {
                    resp = (resp && RestrictByAcesso(Raiz));
                }
                else
                {
                    resp = (resp && RestrictByTipoAcesso(Raiz));
                }
            }



            if (!string.IsNullOrWhiteSpace(DepartamentoAcessoExterno))
            {
                resp = IsDepartamentoAcessoExterno(DepartamentoAcessoExterno);
            }

            if (!string.IsNullOrWhiteSpace(GerenteDepartamento))
            {
                resp = IsGerenteDepartamento(GerenteDepartamento);
            }
            if (!string.IsNullOrWhiteSpace(Departamento))
            {
                resp = HasDepartamento(Departamento);
            }
            if (!resp)
            {
                ErroMsg = @"Seu usuário não tem permissão para acessar essa funcionalidade.";
            }


            return resp;
        }

        private bool RestrictByMenu(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                Autenticado auth = SessionContext.autenticado;

                if (auth == null || !string.IsNullOrWhiteSpace(auth.USU_LOGIN))
                {
                    var service = new ItemMenuSRV();
                    var login = auth.USU_LOGIN;
                    var sisId = auth.SIS_ID;
                    var empId = auth.EMP_ID;

                    IList<ItemMenuModel> listMenu = service.GetByUserLogin(login, sisId, empId);

                    foreach (var menu in listMenu)
                    {

                        if (!string.IsNullOrWhiteSpace(menu.ITM_NOME_ARQUIVO) && 
                            path.ToLower().Equals(menu.ITM_NOME_ARQUIVO.ToLower())) 
                        {
                            return true;
                        }
                    }
                }

            }
          
            return false;
        }

        private bool RestrictByAcesso(string _raiz)
        {
            if (!string.IsNullOrWhiteSpace(_raiz))
            {
                Autenticado auth = SessionContext.autenticado;
               
                if (auth == null || !string.IsNullOrWhiteSpace(auth.USU_LOGIN))
                {
                    ItemMenuSRV _service = new ItemMenuSRV();
                    
                    var _sisId = auth.SIS_ID;
                    var _empId = auth.EMP_ID;
                    var _perid = auth.PER_ID;

                    ItemMenuPerfilModel _permissao = _service.TemAcessoFuncionalidade(_sisId, _empId, _perid, _raiz);

                    if (_permissao != null)
                    {
                        if (this.Acesso == "Acesso") return (_permissao.NIV_ACESSO > 0);
                        if (this.Acesso == "Editar") return (_permissao.NIV_EDIT > 0);
                        if (this.Acesso == "Excluir") return (_permissao.NIV_DELETE > 0);
                        if (this.Acesso == "Incluir") return (_permissao.NIV_INSERT > 0);
                      
                    }
                    
                }

            }

            return false;
        }

        private bool RestrictByTipoAcesso(string _raiz)
        {
            if (!string.IsNullOrWhiteSpace(_raiz))
            {
                Autenticado auth = SessionContext.autenticado;

                if (auth == null || !string.IsNullOrWhiteSpace(auth.USU_LOGIN))
                {
                    ItemMenuSRV _service = new ItemMenuSRV();

                    var _sisId = auth.SIS_ID;
                    var _empId = auth.EMP_ID;
                    var _perid = auth.PER_ID;

                    ItemMenuPerfilModel _permissao = _service.TemAcessoFuncionalidade(_sisId, _empId, _perid, _raiz);

                    if (_permissao != null && this.TipoAcesso != default(NivelAcesso))
                    {
                        if (this.TipoAcesso == NivelAcesso.Acesso) return (_permissao.NIV_ACESSO > 0);
                        if (this.TipoAcesso == NivelAcesso.Editar) return (_permissao.NIV_EDIT > 0);
                        if (this.TipoAcesso == NivelAcesso.Excluir) return (_permissao.NIV_DELETE > 0);
                    }
                    return false;
                }

            }

            return false;
        }

        private bool IsDepartamentoAcessoExterno(string departamento)
        {
            if (!string.IsNullOrWhiteSpace(departamento))
            {
                string[] deps = departamento.Split(',');

                if (deps != null && deps.Length > 0)
                {
                    foreach (var dep in deps)
                    {
                        var retorno = SessionContext.AcessoExterno(dep.Trim(), PermitirNiveisPrivilegiosSuperiores);
                        if (retorno)
                            return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool IsGerenteDepartamento(string departamento)
        {
            if (!string.IsNullOrWhiteSpace(departamento))
            {
                string[] deps = departamento.Split(',');

                if (deps != null && deps.Length > 0)
                {
                    foreach (var dep in deps)
                    {
                        var retorno = SessionContext.IsGerenteDepartamento(dep.Trim(), PermitirNiveisPrivilegiosSuperiores);
                        if (retorno)
                            return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsAdmDepartamento(string departamento)
        {
            if (!string.IsNullOrWhiteSpace(departamento))
            {
                string[] deps = departamento.Split(',');

                if (deps != null && deps.Length > 0)
                {
                    foreach (var dep in deps)
                    {
                        var retorno = SessionContext.IsAdmDepartamento(dep.Trim(), PermitirNiveisPrivilegiosSuperiores);
                        if (retorno)
                            return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool HasDepartamento(string departamento)
        {
            bool resp = true;
            if (!string.IsNullOrWhiteSpace(departamento))
            {
                string[] deps = departamento.Split(',');

                if (deps != null && deps.Length > 0)
                {
                    foreach (var dep in deps)
                    {
                        resp = (resp || SessionContext.HasDepartamento(dep));
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return resp;

        }
    }
}
