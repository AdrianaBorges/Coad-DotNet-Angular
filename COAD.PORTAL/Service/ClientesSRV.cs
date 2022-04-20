using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO;
using COAD.PORTAL.Model.DTO;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.PORTAL.Utils;

namespace COAD.PORTAL.Service
{
    public class ClientesSRV : GenericService<CLIENTES_PORTAL_PROC_Result, ClientesDTO, string>
    {
        private ClientesDAO _dao = new ClientesDAO();

        public ClientesSRV()
        {
            Dao = _dao;           
        }

        public ClientesDTO LoginCliente(string login, string senha)
        {
            var usuario = new ClientesDTO();
            DateTime expiracao = DateTime.Parse("2014/01/21");

            if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
                throw new LoginInconpletoExcepetion();

            usuario = _dao.BuscarCliente(login, senha);

            if (usuario == null)
                throw new UsuarioNaoEncontradoExcepetion();

            if (usuario.status.ToUpper().Equals("B"))
                throw new UsuarioBloqueadoExcepetion();

            if (usuario.permissao.Equals("32"))
                throw new NotImplementedException();

            if (usuario.expiracao != null && usuario.expiracao < DateTime.Now && (usuario.tipo_usuario == "AVAL" || usuario.tipo_usuario == "ST" || usuario.tipo_usuario == "TMP" || usuario.tipo_usuario == "HOTSITE_LOJA_ATC" || usuario.tipo_usuario == "ASSINE_ADV") )
                throw new UsuarioExpiradoExcepetion();

            return usuario;
        }

        public bool CadastrarUsuario(ClientesDTO cliente)
        {
            UtilsPortal _util = new UtilsPortal();
            //tipoUsuario = 1 busca usuário atraves de email e cpf
            string tipoBusca = "1";
            var clienteBusca = _dao.BuscarCliente(cliente.usuario, cliente.senha, cliente.email, cliente.cpf, tipoBusca);
            
            string format = "yyyy-MM-dd HH:mm:ss";
            //DateTime dataAtual = DateTime.ParseExact(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day, format, null);
            DateTime dataAtual = DateTime.Parse(DateTime.Now.ToString(format));

            cliente.senha = new UtilsPortal().GeradorSenha();
            cliente.permissao = "99";
            cliente.empresa = cliente.nome + " " + cliente.sobrenome;
            cliente.status = "L"; 
            string dtaExpiracao = DateTime.Now.AddDays(10).Year + "-" + DateTime.Now.AddDays(10).Month + "-" + DateTime.Now.AddDays(10).Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            

            cliente.expiracao = null;
            cliente.endereco = "";
            cliente.tipo_usuario = "AVAL";

            if (clienteBusca != null)
            {
                if (cliente.expiracao != null && cliente.expiracao.Value < dataAtual.AddYears(-1))
                    _dao.AtualizarClienteExpirado(cliente, dtaExpiracao);
                else if (cliente.expiracao != null && (cliente.expiracao.Value < dataAtual))
                    throw new UsuarioExpiradoExcepetion();

                if (clienteBusca.cpf != null && clienteBusca.cpf == cliente.cpf)
                    throw new DocumentoDuplicadoExcepetion();
                else if (clienteBusca.email != null && clienteBusca.email == cliente.email)
                    throw new EmailDuplicadoExcepetion();

                throw new UsuarioCadastradoExcepetion();
            }

            if(cliente.cpf.Length != 11 && cliente.cpf.Length != 14)
                throw new CPFInvalidoExcepetion();

            if (cliente.cpf.Length == 11 && !_util.ValidaCPF(cliente.cpf))
                throw new CPFInvalidoExcepetion();

            if (cliente.cpf.Length == 14 && !_util.ValidaCnpj(cliente.cpf))
                throw new CNPJInvalidoExcepetion();

            try
            {
                var email = new System.Net.Mail.MailAddress(cliente.email);
            }
            catch
            {
                throw new EmailInvalidoExcepetion();
            }

            cliente.usuario = cliente.email;
            _dao.CadastrarCliente(cliente, dtaExpiracao);
            return true;


        }

        public class LoginInconpletoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Login ou senha não foi informado";
                }
            }
        }

        public class UsuarioNaoEncontradoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Login não encontrado.";
                }
            }
        }

        public class UsuarioBloqueadoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Usuário bloqueado.";
                }
            }
        }

        public class UsuarioExpiradoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Login expirado.";
                }
            }
        }

        public class UsuarioCadastradoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Usuário já cadastrado.";
                }
            }
        }

        public class DocumentoDuplicadoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Documento já cadastrado.";
                }
            }
        }

        public class EmailDuplicadoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Email já cadastrado.";
                }
            }
        }

        public class CPFInvalidoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "CPF inválido.";
                }
            }
        }

        public class CNPJInvalidoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "CNPJ inválido.";
                }
            }
        }

        public class EmailInvalidoExcepetion : Exception
        {
            public override string Message
            {
                get
                {
                    return "Email inválido.";
                }
            }
        }
    }
}
