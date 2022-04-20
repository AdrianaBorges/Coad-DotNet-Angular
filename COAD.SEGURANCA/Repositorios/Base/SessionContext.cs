using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using COAD.SEGURANCA.Repositorios.Contexto;
using System.IO;
using COAD.SEGURANCA.Service;
using System.Security.Cryptography;
using System.Xml;

namespace COAD.SEGURANCA.Repositorios.Base
{
    public static class SessionContext
    {
        public static string RetornaStringDoCampo(XmlDocument _doc, string cNo, string cCampo, string cDefault = null)
        {
            // variável de retorno...
            string cRetorno = "";

            // foram informados os dois parâmetros?...
            if ((cNo != "") && (cCampo != ""))
            {
                // preparando a leitura do Nó no XML...
                XmlNodeList no = _doc.DocumentElement.GetElementsByTagName(cNo);

                // retornando com a informação do campo...
                if (no.Count > 0)
                {
                    if (((XmlElement)no[0]).GetElementsByTagName(cCampo).Count > 0)
                    {
                        cRetorno = ((XmlElement)no[0]).GetElementsByTagName(cCampo)[0].InnerText;
                    }
                }
            }

            if (cRetorno == "" && (cDefault != "" && cDefault != null))
            {
                cRetorno = cDefault;
            }

            return cRetorno;
        }
        public static Nullable<DateTime> RetornarDateTime(XmlDocument _doc, string cNo, string cCampo, string cDefault = null)
        {
            string cRetorno = RetornaStringDoCampo(_doc, cNo, cCampo, cDefault);

            Nullable<DateTime> cRetornoD = null;

            if (!String.IsNullOrWhiteSpace(cRetorno))
                cRetornoD = Convert.ToDateTime(cRetorno);

            return cRetornoD;

        }
        public static Autenticado autenticado
        {
            get {
                
                Page _page = new Page();

               if (_page.Session["Autenticado"] != null)
                    return (Autenticado)new Page().Session["Autenticado"];
                else
                    return null;
            }
            set {new Page().Session["Autenticado"] = value;} 
        }
        public static bool EhAutenticado(bool reloadUser = false)
        {
            Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

            if (autenticado != null)
                return true;
            else
                return false;
        }

        public static string ip
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.IP_ACESSO;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.IP_ACESSO = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static string path
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.PATH;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.PATH = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static string nomeoperedora
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.USU_NOME;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.USU_NOME = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }

        public static string login
        {
            get
            {                    
                if(HttpContext.Current != null)
                {
                    Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                    if (autenticado != null)
                        return autenticado.USU_LOGIN;
                    else
                        return "Anônimo";
                }
                return "Anônimo";

            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.USU_LOGIN = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static string sessao
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.SESSION_ID;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.SESSION_ID = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static int emp_id 
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.EMP_ID;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.EMP_ID = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }

        /// <summary>
        /// ALT: 21/03/2017 às 13h46m
        /// Empresa é do Grupo COAD? 
        /// Essa informação é muito útil, por exemplo, para permitir à COAD apenas um cadastro CNAB por banco.
        /// </summary>
        public static bool empresa_do_grupo_coad
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.EMP_GRP_COAD;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.EMP_GRP_COAD = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }

        /// <summary>
        /// ALT: 06/10/2015 às 10h59m
        /// CPF do usuário cadastrado pelo Departamento Pessoal usado para emissão individual do contracheque...
        /// </summary>
        /// <returns></returns>
        public static string cpf
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.USU_CPF;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.USU_CPF = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static int? rep_id
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.REP_ID;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.REP_ID = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static string per_id
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.PER_ID;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.PER_ID = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }

        public static bool admin 
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.ADMIN;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.ADMIN = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static bool administradorDeLogin
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.USU_ADMIN_LOGIN_PERFIL;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.USU_ADMIN_LOGIN_PERFIL = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }
        public static string sis_id
        {
            get
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                return autenticado.SIS_ID;
            }
            set
            {
                Autenticado autenticado = (Autenticado)new Page().Session["Autenticado"];

                autenticado.SIS_ID = value;

                new Page().Session["Autenticado"] = autenticado;
            }
        }



        public static string HashMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
        public static string profileName { get; set; }
        public static void EnviarEmail(string emaildest, string emailassunto, string emailmenssagem)
        {
            try
            {

                SmtpClient client = new SmtpClient();

                client.Host = "smtp.gmail.com";
                client.Port = int.Parse("587");
                client.Timeout = 100000;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("coadcorp@coad.com.br", "C04dc0rp@");

                MailMessage Email = new MailMessage();
                Email.IsBodyHtml = true;
                Email.From = new MailAddress("coadcorp@coad.com.br");
                Email.To.Add(new MailAddress(emaildest));
                Email.Subject = emailassunto;
                Email.Body = emailmenssagem;

                client.Send(Email);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string GetIp()
        {
            string strHostName = Dns.GetHostName();
            string _retorno = "";

            IPAddress[] ip = Dns.GetHostAddresses(strHostName);

            for (int i = 0; i <= ip.Count() - 1; i++)
            {
                if (ip[1].ToString().Length <= 17)
                {
                    _retorno = ip[1].ToString();
                    break;
                }
            }

            return _retorno;
        }
        public static List<PERFIL> perfis
        {
            get { return (List<PERFIL>)new Page().Session["perfis"]; }
            set { new Page().Session["perfis"] = value; }
        }
        public static List<PERFIL_USUARIO> perfis_usuario
        {
            get { return (List<PERFIL_USUARIO>)new Page().Session["perfis_usuario"]; }
            set { new Page().Session["perfis_usuario"] = value; } 
        }
        public static List<Menu> menu_usuario
        {
            get { return (List<Menu>)new Page().Session["menu_usuario"]; }
            set { new Page().Session["menu_usuario"] = value; }
        }
        public static List<SISTEMA> sistemas_coad
        {
            get { return (List<SISTEMA>)new Page().Session["sistemas_coad"]; }
            set { new Page().Session["sistemas_coad"] = value; }
        }
        public static string BuscarImagem(byte[] imagemByte, string strNomeArquivo)
        {
            try
            {
                string s = strNomeArquivo;
                FileStream fs = new FileStream(s, FileMode.CreateNew, FileAccess.Write);
                fs.Write(imagemByte, 0, imagemByte.Length);
                fs.Flush();
                fs.Close();
                return strNomeArquivo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static byte[] SalvarArquivo(Stream fs)
        {
            try
            {
                byte[] imagemByte = new byte[fs.Length];
                fs.Read(imagemByte, 0, imagemByte.Length);
                fs.Flush();
                fs.Close();
                return imagemByte;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string usu_login_desktop;
        /// <summary>
        /// Indica se o usuário possui um representanteId
        /// </summary>
        /// <returns></returns>
        public static bool PossuiRepresentante()
        {
            var resp = (autenticado != null && autenticado.REP_ID != null && autenticado.REP_ID > 0);
            return resp;
        }
        /// <summary>
        /// Pega o id de representante do usuário logado
        /// </summary>
        /// <returns></returns>
        public static int? GetIdRepresentante()
        {
            if (autenticado != null)
            {
                return autenticado.REP_ID;
            }
            return null;
        }
        public static bool HasPerfil(string perfil)
        {
            if (perfis_usuario != null && !string.IsNullOrWhiteSpace(perfil))
            {
                var resp = (perfis_usuario.Where(x => x.PER_ID.ToUpper() == perfil.ToUpper()).Count() > 0);
                return resp;
            }

            return false;
        }
        public static bool HasDepartamento(string departamento)
        {
            if (autenticado != null && !string.IsNullOrWhiteSpace(autenticado.PER_ID) && !string.IsNullOrWhiteSpace(departamento))
            {           
                var perfil =  new PerfilSRV(false).FindById(1, autenticado.PER_ID);
                return (perfil != null && perfil.DEPARTAMENTO != null
                    && perfil.DEPARTAMENTO.DP_NOME != null
                    && perfil.DEPARTAMENTO.DP_NOME.ToUpper().
                    Equals(departamento.ToUpper()));
            }
                      
            return false;
        }


        /// <summary>
        /// Recebe vários nomes de departamento para verificar se o usuário é um gerente de departamento ou não. 
        /// Aplica a lógida (or). Para retornar true ele deve pertencer e ser gerente a pelo menos um dos departamentos.
        /// </summary>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permitir administradores dos departamentos informados</param>
        /// <param name="departamentos">Departamentos</param>
        /// <returns></returns>
        public static bool IsAdmDepartamentoOR(params string[] departamentos)
        {

            bool test = false;

            if (departamentos != null)
            {
                foreach (var dep in departamentos)
                {

                    var resp = _TestarAcesso(dep, 0, true);

                    test = (test || resp);
                }
            }
            return test;
        }

        /// <summary>
        /// Recebe vários nomes de departamento para verificar se o usuário é um gerente de departamento ou não. 
        /// Aplica a lógida (and). Para retornar true ele deve pertencer e ser gerente de todos os departamentos.
        /// </summary>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permitir administradores dos departamentos informados</param>
        /// <param name="departamentos">Departamentos</param>
        /// <returns></returns>
        public static bool IsAdmDepartamentoAnd(params string[] departamentos)
        {
            bool test = true;

            if (departamentos != null)
            {
                foreach (var dep in departamentos)
                {
                    var resp = _TestarAcesso(dep, 0, false);
                    test = (test && resp);
                }
            }
            return test;
        }

        

        public static bool IsDepartamento(string departamento)
        {
            return _TestarAcesso(departamento, 3, true);
        }

        /// <summary>
        /// Recebe vários nomes de departamento para verificar se o usuário é um gerente de departamento ou não. 
        /// Aplica a lógida (or). Para retornar true ele deve pertencer e ser gerente a pelo menos um dos departamentos.
        /// </summary>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permitir administradores dos departamentos informados</param>
        /// <param name="departamentos">Departamentos</param>
        /// <returns></returns>
        public static bool IsGerenteDepartamentoOR(bool permitirNiveisPrivilegiosSuperiores, params string[] departamentos)
        {

            bool test = false;

            if (departamentos != null)
            {
                foreach(var dep in departamentos){

                    var resp  = _TestarAcesso(dep, 1, permitirNiveisPrivilegiosSuperiores);
                                   
                    test = (test || resp);
                }
            }
            return test;
        }

        /// <summary>
        /// Recebe vários nomes de departamento para verificar se o usuário é um gerente de departamento ou não. 
        /// Aplica a lógida (and). Para retornar true ele deve pertencer e ser gerente de todos os departamentos.
        /// </summary>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permitir administradores dos departamentos informados</param>
        /// <param name="departamentos">Departamentos</param>
        /// <returns></returns>
        public static bool IsGerenteDepartamentoAnd(bool permitirNiveisPrivilegiosSuperiores, params string[] departamentos)
        {
            bool test = true;

            if (departamentos != null)
            {
                foreach (var dep in departamentos)
                {
                    var resp = _TestarAcesso(dep, 1, permitirNiveisPrivilegiosSuperiores);
                    test = (test && resp);
                }
            }
            return test;
        }

        
        /// <summary>
        /// Recebe vários nomes de departamento para verificar se o usuário é operador no departamento ou não. 
        /// Aplica a lógida (or). Para retornar true ele deve pertencer e ser gerente a pelo menos um dos departamentos.
        /// </summary>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permitir administradores dos departamentos informados</param>
        /// <param name="departamentos">Departamentos</param>
        /// <returns></returns>
        public static bool HasDepartamentoOR(bool permitirNiveisPrivilegiosSuperiores, params string[] departamentos)
        {

            bool test = false;

            if (departamentos != null)
            {
                foreach (var dep in departamentos)
                {

                    var resp = _TestarAcesso(dep, 3, permitirNiveisPrivilegiosSuperiores);

                    test = (test || resp);
                }
            }
            return test;
        }

        /// <summary>
        /// Recebe vários nomes de departamento para verificar se o usuário é um operador no departamento ou não. 
        /// Aplica a lógida (and). Para retornar true ele deve pertencer e ser gerente de todos os departamentos.
        /// </summary>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permitir administradores dos departamentos informados</param>
        /// <param name="departamentos">Departamentos</param>
        /// <returns></returns>
        public static bool HasDepartamentoAnd(bool permitirNiveisPrivilegiosSuperiores, params string[] departamentos)
        {
            bool test = true;

            if (departamentos != null)
            {
                foreach (var dep in departamentos)
                {
                    var resp = _TestarAcesso(dep, 3, permitirNiveisPrivilegiosSuperiores);
                    test = (test && resp);
                }
            }
            return test;
        }
        public static bool IsAdmDepartamento(string departamento, bool permitirNiveisPrivilegiosSuperiores = false)
        {
            return _TestarAcesso(departamento, 0, permitirNiveisPrivilegiosSuperiores);
        }

        public static bool IsGerenteDepartamento(string departamento, bool permitirNiveisPrivilegiosSuperiores = false)
        {
            return _TestarAcesso(departamento, 1, permitirNiveisPrivilegiosSuperiores);
        }

        private static bool _TestarAcesso(string departamento, int _niv_ace_nivel, bool permitirNiveisPrivilegiosSuperiores = false)
        {   
            var _retorno = false;
            if (autenticado != null && !string.IsNullOrWhiteSpace(autenticado.PER_ID) && !string.IsNullOrWhiteSpace(departamento))
                    _retorno = new PerfilSRV(false).VerificarNivelAcesso(autenticado.PER_ID, departamento, _niv_ace_nivel, permitirNiveisPrivilegiosSuperiores);
            
            return _retorno;
        }


        public static bool AcessoExterno(string departamento, bool verificacaoAninhada = false)
        {
            return _TestarAcesso(departamento, 4, verificacaoAninhada);
        }

        public static bool AcessoGerente(string departamento, bool verificacaoAninhada = false)
        {
            return _TestarAcesso(departamento, 1, verificacaoAninhada);
        }


        public static List<Autenticado> autenticadoGlobal
        {
            get { return (List<Autenticado>)System.Web.HttpContext.Current.Application["USUARIO"]; }
            set { System.Web.HttpContext.Current.Application["USUARIO"] = value; }
        }
        public static void RemoveSession(System.Web.HttpContext context)
        {   
            RemoveSessionGlobal(context);
                    //----------
            context.Response.Expires = 0;
            context.Response.ExpiresAbsolute = DateTime.Now;
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "private");
            context.Response.CacheControl = "no-cache";
            context.Session.Contents.RemoveAll();
            context.Session.Abandon();
        }
        public static void RemoveSessionGlobal(System.Web.HttpContext context)
        {
            List<Autenticado> _Lista = new List<Autenticado>();

            if (context.Application["USUARIO"] != null)
                _Lista = autenticadoGlobal;

            _Lista.Remove(SessionContext.autenticado);

            autenticadoGlobal = _Lista;
        }
        public static void RemoveSessionGlobal(System.Web.HttpContext context, string _session_id)
        {
            List<Autenticado> _Lista = new List<Autenticado>();

            if (context.Application["USUARIO"] != null)
                _Lista = autenticadoGlobal;

            _Lista.RemoveAll(x => x.SESSION_ID.ToUpper() == _session_id.ToUpper());

            autenticadoGlobal = _Lista;
        }
        public static void AddSessionGlobal(System.Web.HttpContext context)
        {
            List<Autenticado> _Lista = new List<Autenticado>();

            if (context.Application["USUARIO"] != null)
                _Lista = autenticadoGlobal;

            _Lista.Add(autenticado);

            autenticadoGlobal = _Lista;
        }
        public static void ContadorTimeOut(System.Web.HttpContext context)
        {
            int tmprestante = 0;

            if (context.Application["USUARIO"] != null)
            {
                autenticadoGlobal.Find(x => x.USU_LOGIN == autenticado.USU_LOGIN).SESSION_TIMEOUT_RESTANTE -= 1;
                tmprestante = autenticadoGlobal.Find(x => x.USU_LOGIN == autenticado.USU_LOGIN).SESSION_TIMEOUT_RESTANTE;
            }

            if (tmprestante == 1)
                RemoveSession(context);

        }
        public static Autenticado FindSessionGlobal(System.Web.HttpContext context, string _usu_login)
        {
            List<Autenticado> _Lista = new List<Autenticado>();

            if (context.Application["USUARIO"] != null)
                _Lista = autenticadoGlobal;

            var autenticado = _Lista.Where(x => x.USU_LOGIN.ToUpper() == _usu_login.ToUpper()).FirstOrDefault();

            return autenticado;
        }
        public static Autenticado VerificaSessionGlobal(System.Web.HttpContext context, string _usu_login)
        {
            List<Autenticado> _Lista = new List<Autenticado>();

            if (context.Application["USUARIO"] != null)
                _Lista = autenticadoGlobal;

            var autenticado = _Lista.Where(x => x.USU_LOGIN.ToUpper() == _usu_login.ToUpper() && x.SESSION_ID == context.Session.SessionID).FirstOrDefault();

            return autenticado;
        }
        public static TReturn GetInSession<TReturn>(string key)
        {
            var sessionValue = new Page().Session[key];

            if(sessionValue != null)
            {
                TReturn value = (TReturn)sessionValue;
                return value;
            }
            else 
                return default(TReturn);
        }

        public static void PutInSession<TValue>(string key, TValue value)
        {
            new Page().Session[key] = value;
        }

        public static void RemoveFromSession(string key)
        {
            if (ExistsInSession(key))
            {
                new Page().Session.Remove(key);
            }            
        }

        public static bool ExistsInSession(string key)
        {
            object value = new Page().Session[key];
            return (value != null);
        }

        public static string GetHomePerfil()
        {
            if (autenticado != null && !string.IsNullOrWhiteSpace(autenticado.PER_ID))
            {
                
  
                var perfil = new PerfilSRV(false).FindById(1, autenticado.PER_ID);
                if (perfil != null)
                {
                    return perfil.PER_PATH_HOME;
                }
            }

            return null;
        }

        //------ O Bloco abaixo faz referencia a rotinas para Criptografar e Descriptografar a senha do email do usuário. Apenas nesse caso deve ser utilizado
        //------ Para as outras situações utilizar HashMD5
        const string senha = "E!09#x*&aTe$";
        public static string Criptografar(string Message)
        {

            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {

                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();

                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            }

            finally
            {

                TDESAlgorithm.Clear();

                HashProvider.Clear();

            }

            return Convert.ToBase64String(Results);

        }
        public static string Descriptografar(string Message)
        {

            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            try
            {

                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();

                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);

            }

            finally
            {

                TDESAlgorithm.Clear();

                HashProvider.Clear();

            }

            return UTF8.GetString(Results);

        }
        /// <summary>
        /// Verifica se a segunda hora informada é maior que a primeira
        /// _hi, _mi, _si => (Hora, Minuto e Segundo iniciais)
        /// _hf, _mf, _sf => (Hora, Minuto e Segundo finais)
        /// </summary>
        /// <returns>Retorna um boleano</returns>
        public static bool CompararHora (int _hi, int _mi, int _si, int _hf, int _mf, int _sf)
        {
            DateTime dataini = new DateTime(2009, 8, 1, _hi, _mi, _si);
            DateTime datafim = new DateTime(2009, 8, 1, _hf, _mf, _sf);
            
            int result = DateTime.Compare(dataini, datafim);

            if (result < 0)
                return true;
            else
                return false;

        }

        public static bool PossuiSessao()
        {
            return (HttpContext.Current != null && HttpContext.Current.Session != null);
        }


    }
}
