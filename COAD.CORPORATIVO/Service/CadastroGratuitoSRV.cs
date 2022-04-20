using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace COAD.CORPORATIVO.Service
{
    public class CadastroGratuitoSRV : ServiceAdapter<CADASTRO_GRATUITO, CadastroGratuitoDTO>
    {
        private CadastroGratuitoDAO _dao = new CadastroGratuitoDAO();

        public CadastroGratuitoSRV()
        {
            SetDao(_dao);
        }
        public Autenticado ValidarLogin(string _email)
        {
            return _dao.ValidarLogin(_email);
        }
        public CadastroGratuitoDTO ValidarLogin(string _email, string _senha)
        {
            return _dao.ValidarLogin( _email, _senha);
        }
        public CadastroGratuitoDTO RealizarLogin(string _login, string _senha, string _sistema, HttpContext _url)
        {
            string url = _url.Request.Url.AbsoluteUri;
            string path = _url.Request.Url.AbsolutePath;

            Autenticado _autenticado = new Autenticado();

            try
            {
                if (_login.Length == 0 || _senha.Length == 0)
                    throw new Exception("Login e/ou senha inválida !!");

                var _gratuito = this.ValidarLogin(_login, _senha);

                if (_gratuito == null)
                    throw new Exception(" Acesso não autorizado à esta funcionalidade. Em caso de novos cadastros, favor aguardar alguns minutos. Persistindo esta situação, favor entrar em contato com o serviço de atendimento ao cliente.");

                //_senha2 = SessionContext.HashMD5(_autenticado.USU_SENHA);

                //if (_senha != _senha2)
                //    throw new Exception("Login e/ou senha inválida !!");

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
                
                _autenticado.USU_NOME = _gratuito.CGR_NOME;
                _autenticado.USU_SENHA = _gratuito.CGR_SENHA;
                _autenticado.PER_ID = _gratuito.CGR_PERFIL;
                _autenticado.USU_LOGIN = "MOBILEUSER";

                SessionContext.autenticado = _autenticado;

                SessionContext.AddSessionGlobal(_url);

                FormsAuthentication.SetAuthCookie(_autenticado.USU_LOGIN, false);

                SysException.RegistrarLog("LogIn Usuário (" + _autenticado.EMAIL + ")", "", _autenticado);

                return _gratuito;

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

                var msgerro = "Tentativa de Login (" + _login + ")" + SysException.Show(ex);

                SysException.RegistrarLog(msgerro, SysException.ShowIdException(ex), _autenticado);

                throw new Exception(msgerro);
            }

        }
        public string Cadastrar(string _cliportaldto)
        {
            return "";
        }
    }
}
