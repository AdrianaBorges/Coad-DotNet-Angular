using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Service;
using OpenPop.Mime;
using OpenPop.Mime.Decode;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace COAD.SEGURANCA.Service
{
    public class EmailSRV : IEmailSRV
    {
        public SmtpClient cliente { get; set; }        

        public EmailSRV()
        {
            cliente = new SmtpClient();
        }

        //public void EnviarEmail(
        //    string emaildest, 
        //    string emailassunto, 
        //    string emailmenssagem, 
        //    bool enviarAgendado = true, 
        //    string metodoOrigem = null,
        //    string servicoOrigem = null,
        //    string actionName = null,
        //    string actionArg = null)
        //{
        //    var filaEmailSRV = ServiceFactory.RetornarServico<FilaEmailSRV>();

        //    try
        //    {
        //        if (enviarAgendado == false || filaEmailSRV == null)
        //        {
        //            SmtpClient client = new SmtpClient();

        //            client.Host = "smtp.gmail.com";
        //            client.Port = 587;
        //            client.Timeout = 100000;
        //            client.EnableSsl = true;
        //            client.UseDefaultCredentials = false;
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            client.Credentials = new NetworkCredential("backup@coad.com.br", "Br@z1l!2014");
                    
        //            MailMessage Email = new MailMessage();
        //            Email.IsBodyHtml = true;
        //            Email.From = new MailAddress("coadcorp@coad.com.br");
        //            Email.Bcc.Add(new MailAddress("coadcorp@coad.com.br"));
        //            Email.To.Add(new MailAddress(emaildest));
        //            Email.Subject = emailassunto;
        //            Email.Body = emailmenssagem;

        //            client.Send(Email);
        //        }
        //        else
        //        {
        //            filaEmailSRV.InserirEmailNaFila(
        //                emaildest, 
        //                emailassunto, 
        //                emailmenssagem, 
        //                null, 
        //                null, 
        //                metodoOrigem, 
        //                servicoOrigem,
        //                actionName,
        //                actionArg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new EmailNaoEnviadoException("O email não pode ser enviado", ex);
        //    }
        //}

        public void EnviarEmail(EmailRequestDTO emailDTO)
        {
            var filaEmailSRV = ServiceFactory.RetornarServico<FilaEmailSRV>();


            if (emailDTO.EnviarAgendado == false || filaEmailSRV == null)
            {
                cliente.Host = emailDTO.Host;
                cliente.Port = emailDTO.Port;
                cliente.EnableSsl = emailDTO.EnableSsl;
                cliente.UseDefaultCredentials = false;
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;

                var usuario = emailDTO.User;
                var senha = emailDTO.Senha;
                var smtp = (emailDTO.codSMTP != null) ? emailDTO.codSMTP : 1;

                if (smtp != null)
                {
                    var contaSMTP = ServiceFactory.RetornarServico<PoolEmailSRV>().RetornarContaSMTP(smtp);
                    if (contaSMTP != null)
                    {
                        usuario = contaSMTP.Usuario;
                        senha = contaSMTP.Senha;
                    }
                }

                cliente.Credentials = new NetworkCredential(usuario, senha);
                MailMessage Email = new MailMessage();
                Email.IsBodyHtml = true;
                Email.From = new MailAddress(emailDTO.From);
                Email.To.Add(new MailAddress(emailDTO.EmailDestino));
                //Email.Bcc.Add(new MailAddress("coadcorp@coad.com.br"));
                Email.Subject = emailDTO.Assunto;
                Email.Body = emailDTO.CorpoEmail;

                if (emailDTO.AlternateView != null)
                {
                    Email.AlternateViews.Add(emailDTO.AlternateView);
                }

                if (!string.IsNullOrWhiteSpace(emailDTO.CC) && !emailDTO.EmailDestino.Equals(emailDTO.CC))
                {
                    Email.CC.Add(new MailAddress(emailDTO.CC));
                }

                if (!string.IsNullOrWhiteSpace(emailDTO.CCO) && !emailDTO.EmailDestino.Equals(emailDTO.CCO))
                {
                    Email.Bcc.Add(new MailAddress(emailDTO.CCO));
                }

                if(emailDTO.reporteDeErro == true)
                {
                    if (!string.IsNullOrWhiteSpace(emailDTO.CC_ERRO) && !emailDTO.EmailDestino.Equals(emailDTO.CC_ERRO))
                    {
                        Email.CC.Add(new MailAddress(emailDTO.CC_ERRO));
                    }

                    if (!string.IsNullOrWhiteSpace(emailDTO.CCO_ERRO) && !emailDTO.EmailDestino.Equals(emailDTO.CCO_ERRO))
                    {
                        Email.Bcc.Add(new MailAddress(emailDTO.CCO_ERRO));
                    }
                }

                if (emailDTO.Anexos != null && emailDTO.Anexos.Count() > 0)
                {
                    foreach (var anexo in emailDTO.Anexos)
                    {
                        Stream stream = new MemoryStream(anexo.Bytes);
                        var attachment = new Attachment(stream, anexo.NomeArquivo, anexo.MimeType);
                        Email.Attachments.Add(attachment);
                    }
                }
                                

                cliente.Send(Email);
            }
            else
            {
                filaEmailSRV.InserirEmailNaFila(emailDTO);
            }
        }

        /// <summary>
        /// Baixar todos os emails de uma determinada conta de email
        /// </summary>
        /// <param name="hostname">Hostname do servidor. For example: pop3.live.com</param>
        /// <param name="porta"> 110 ou 995 para SSL POP3</param>
        /// <param name="useSsl"></param>
        /// <param name="username">Username usuario</param>
        /// <param name="password">Password senha</param>
        /// <returns>Retorna todos os emails do servidor POP3 server</returns>
        public List<Message> BaixarEmails(string hostname, string username = null, string password = null, int porta = 995, bool useSsl = true)
        {
            using (Pop3Client _cliente = new Pop3Client())
            {
                //this.InsertCustomEncodings(Encoding.GetEncoding("iso-8859-1"));

                _cliente.Connect(hostname, porta, useSsl);
                _cliente.Authenticate(username, password);
                int _emails = _cliente.GetMessageCount();

                List<Message> _listaemails = new List<Message>(_emails);

                for (int i = _emails; i > 0; i--)
                {
                    _listaemails.Add(_cliente.GetMessage(i));
                }

                return _listaemails;
            }
        }

        public void InsertCustomEncodings(Encoding customEncoding)
        {
            // Lets say some email contains a characterSet of "iso-9999-9" which
            // is fictional, but is really just UTF-8.
            // Lets add that mapping to the class responsible for finding
            // the Encoding from the name of it
            EncodingFinder.AddMapping("iso-9999-9", Encoding.UTF8);

            // It is also possible to implement your own Encoding if
            // the framework does not provide what you need
            EncodingFinder.AddMapping("specialEncoding", customEncoding);

            // Now, if the EncodingFinder is not able to find an encoding, lets
            // see if we can find one ourselves
            EncodingFinder.FallbackDecoder = CustomFallbackDecoder;
        }

        public Encoding CustomFallbackDecoder(string characterSet)
        {
            // Is it a "foo" encoding?
            if (characterSet.StartsWith("foo"))
                return Encoding.ASCII; // then use ASCII

            // If no special encoding could be found, provide UTF8 as default.
            // You can also return null here, which would tell OpenPop that
            // no encoding could be found. This will then throw an exception.
            return Encoding.UTF8;
        }

        public string GerarTemplateCliente(string bodyMessage, string urlImagemCabecalho = null)
        {
            string template =
                @"<!DOCTYPE html>
                <html lang=""pt_br"">
                    <head>
                        <link rel='stylesheet' href='corp.coad.com.br/Content/themes/base/jquery-ui.css' type='text/css' />
                        <link rel='stylesheet' href='corp.coad.com.br/Content/themes/base/bootstrap-theme.css' type='text/css' />
                        <link rel='stylesheet' href='corp.coad.com.br/Content/themes/base/default.css' type='text/css' />
                        <link rel='stylesheet' href='corp.coad.com.br/Content/themes/base/bootstrap.css' type='text/css' />
                        <link rel='stylesheet' href='corp.coad.com.br/Content/themes/base/coadcorp-boostrap.css' type='text/css' />
                        <link rel='stylesheet' href='corp.coad.com.br/Content/themes/base/coadcorp-core.css' type='text/css' />
                    </head>
                    <body>
                            <table align=""center"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""border: none;"">
                            {1}
                            <tr>
                                <td colspan=""2"" valign=""top"" align=""center"" style=""padding: 25px 0 0 0;"">
                                    {0}
                                </td>
                            </tr>
                            <tr style=""background-color: #345e82;"">
                                  <td>
                                    <img src='http://www.coad.com.br/imagens/coadcorp/Rodape_GatewayAssinaturas.png'></img>
                                </div>
                            </tr>
                        </table>
            </body>
            </html>";

            string cabecalho = "";

            if (!string.IsNullOrWhiteSpace(urlImagemCabecalho))
            {
                cabecalho = @"
                            <tr>
                                <td colspan=""2""><img src=""{0}"" /></td>
                            </tr>";
                cabecalho = string.Format(cabecalho, urlImagemCabecalho);

            }
            template = string.Format(template, bodyMessage, cabecalho);
            return template;
        }

        public void EnviarEmailParaCliente(
            string emailDest, 
            string assunto, 
            string bodyMessage, 
            string urlImagemCabecalho = null, 
            string actionName = null,
            string actionArg = null,
            string emailCCErro = null,
            int? codSMTP = null)
        {
            var template = GerarTemplateCliente(bodyMessage, urlImagemCabecalho);
            EnviarEmail(new EmailRequestDTO() {
                EmailDestino = emailDest,
                Assunto = assunto,
                CorpoEmail = template,
                ActionName = actionName,
                ActionArg = actionArg,
                CC_ERRO = emailCCErro,
                codSMTP = codSMTP
            });
            //EnviarEmail(emailDest, assunto, template, true, null, null, actionName, actionArg);
        }

        public void LembrarSenha(string _login, string _email, string _sessionID, string _url)
        {

            Autenticado _autenticado = new Autenticado();

            Random randNum = new Random();

            try
            {
                if (_login.Length <= 0 && _email.Length <= 0)
                    throw new Exception("Login e/ou Email não informados !");

                var _usuario = new UsuarioSRV().ValidarLoginEmail(_login, _email);

                if (_usuario == null)
                    throw new Exception("Email não encontrado!!");

                _autenticado.USU_LOGIN = _login;
                _autenticado.IP_ACESSO = SessionContext.GetIp();
                _autenticado.PATH = _url;
                _autenticado.SESSION_ID = _sessionID;

                string _novasenha = randNum.Next(2113).ToString() + _login[1] + _login[2] + randNum.Next(2003).ToString();
                string _mensagem = "<DIV>Caro " + _login + ", este email é gerado automaticamente pelo sistema - COADCORP.  </DIV></p>" +
                                   "<DIV>Conforme a sua solicitação, o sistema gerou uma senha automatica, aleatória e provisória.</DIV></p>" +
                                   "<DIV>Voçe deve realizar o login com esta senha provisória e em seguida realizar o cadastramento da sua senha definitiva.</DIV>" +
                                   "<DIV>Senha Provisória => " + _novasenha + " </DIV>";

                _usuario.USU_NOVA_SENHA = 1;
                _usuario.USU_SENHA = _novasenha;

                new UsuarioSRV().AlterarSenha(_usuario);

                SessionContext.EnviarEmail(_email, "Nova Senha", _mensagem);

                SysException.RegistrarLog("Solicitação de Senha - Usuário  (" + _autenticado.USU_LOGIN + ")", "", _autenticado);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }
        


    }
}
