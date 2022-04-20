using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPop.Mime;
using OpenPop.Pop3;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Repositorios.Base;
using OpenPop.Mime.Header;
using System.Transactions;
using System.IO;
using COAD.SEGURANCA.Service.Interfaces;

namespace COAD.CORPORATIVO.Service
{
    public class EmailAtendSRV : ServiceAdapter<EMAIL_ATEND, EmailAtendDTO, int>
    {
        private EmailAtendDAO _dao;
        public IEmailSRV _emailSRV { get; set; }

        public EmailAtendSRV()
        {
            this._dao = new EmailAtendDAO();
            SetDao(_dao);
            SetKeys("EAT_ID");

        }

        public EmailAtendSRV(EmailAtendDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
            SetKeys("EAT_ID");

        }

        public Pagina<EmailAtendDTO> BuscarEmails(string username = null, string password = null)
        {
            try
            {
                var _listaemails = _emailSRV.BaixarEmails("pop.gmail.com", username, password);

                foreach (var e in _listaemails)
                {
                    EmailAtendDTO _email = new EmailAtendDTO();

                    _email.EAT_ASSUNTO = e.Headers.Subject;
                    _email.EAT_EMAIL_FROM = e.Headers.From.Address;
                    _email.EAT_FROM_NOME = e.Headers.From.DisplayName;
                    _email.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                    _email.EAT_DATA = e.Headers.DateSent;


                    foreach (var Cc in e.Headers.Cc)
                    {
                        _email.EAT_EMAIL_CC = Cc.Address + ";";
                    }

                    foreach (var Bcc in e.Headers.Bcc)
                    {
                        _email.EAT_EMAIL_BCC = Bcc.Address + ";";
                    }

                    foreach (MessagePart messagePart in e.MessagePart.MessageParts)
                    {
                        if (messagePart.IsAttachment == false)
                        {
                            if (messagePart.IsMultiPart == false)
                            {
                                if (messagePart.Body != null)
                                    _email.EAT_TEXTO_EMAIL = e.MessagePart.BodyEncoding.GetString(messagePart.Body);

                            }
                            else
                            {
                                if (messagePart.MessageParts[1].Body != null)
                                    _email.EAT_TEXTO_EMAIL = e.MessagePart.BodyEncoding.GetString(messagePart.MessageParts[1].Body);
                            }
                        }
                        else
                        {
                            EmailAtendAnexoDTO _anexo = new EmailAtendAnexoDTO();
                            string _filePath = System.Web.HttpContext.Current.Server.MapPath("~/temp/") + messagePart.FileName;
                            System.IO.File.WriteAllBytes(_filePath, messagePart.Body);
                            _anexo.ANX_ANEXO_NOMEARQ = messagePart.FileName;
                            _anexo.ANX_ANEXO = System.IO.File.ReadAllBytes(_filePath);
                            _email.EMAIL_ATEND_ANEXO.Add(_anexo);
                        }
                    }

                    this.SalvarEmaileAnexo(_email);

                }

                Pagina<EmailAtendDTO> _retorno = _dao.ListarEmail(SessionContext.autenticado.USU_LOGIN);

                return _retorno;
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                throw new Exception(ex.Message);
            }   

        }
        public Pagina<EmailAtendDTO> ListarEmail(string _usu_login = null, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarEmail(_usu_login, pagina, registroPorPagina);
        }
        public void SalvarEmaileAnexo(EmailAtendDTO _email)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    EmailAtendDTO _e = this.FindById(_email.EAT_ID);

                    if (_e == null)
                    {
                        _e = this.Save(_email);
                    }
                    else
                    {
                        _e = this.Merge(_email, "EAT_ID");
                    }
                    //-----------
                    //--- Realiza a inclusão dos itens na cofiguração das tabelas
                    //-----------

                    foreach (EmailAtendAnexoDTO _item in _email.EMAIL_ATEND_ANEXO)
                    {
                        _item.EAT_ID = _e.EAT_ID;
                        new EmailAtendAnexoSRV().Save(_item);

                    }

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
    }
}
