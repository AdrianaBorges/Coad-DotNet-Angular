using Coad.GenericCrud.Exceptions;
using GenericCrud.Exceptions;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace Coad.GenericCrud.ActionResultTools
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string type { set; get; }
        [DataMember]
        public string message { set; get; }
        public string stackTrace { get; set; }
        public Dictionary<string, List<string>> ValidationErrors { get; set; }
        public ICollection<Message> subMessages { get; set; }


        public Message()
        {
            this.ValidationErrors = new Dictionary<string, List<string>>();
            subMessages = new List<Message>();
        }

        public Message(string tipo, string message)
        {
            type = tipo;
            this.ValidationErrors = new Dictionary<string, List<string>>();
            this.message = message;
            subMessages = new List<Message>();
        }

        public static Message Success(string msg)
        {
            return new Message("success", msg);
        }

        public static Message Success(string artigo, string entidade, string acao)
        {
            return Success(artigo +" "+ entidade + " foi " + acao + " com successo.");
        }

        public static Message Info(string msg)
        {
            return new Message("success", msg);
        }

        public static Message Info(string artigo, string entidade, string acao)
        {
            return Info(artigo + " " + entidade + " foi " + acao + " com successo. ");
        }

        public static Message Warning(string msg)
        {
            return new Message("warning", msg);
        }

        public static Message Fail(string msg)
        {
            return new Message("fail", msg);
        }

        public static Message Fail(string artigo, string entidade, string acao)
        {
            return Fail("Não foi possível " + acao + " " + artigo + " " + entidade + " ocorreu algum problema no processamento. Se o problema persistir contate o administrador do sistema");
        }

        public static Message Fail(Exception e, bool exibirErrosNaExcessao = true)
        {
            Message msg = new Message("fail", "Ocorreu ao processar essa ação.");
            String strMsg = ExceptionFormatter.RecursiveFindExceptionsMessage(e, msg);
            string stackTrace = ExceptionFormatter.RecursiveShowExceptionsStacktrace(e);

            if (!string.IsNullOrWhiteSpace(stackTrace))
            {
                msg.stackTrace = stackTrace;
            }

            if (exibirErrosNaExcessao == true)
            {
                msg.message = strMsg;
            }
            
            return msg;
        }
        public static string FindException(Exception ex)
        {
            try
            {
                string _mensagem = "";
                _mensagem  = FindFormattedDbEntityValidationException(ex);
                _mensagem += FindDbEntityValidationException(ex);
                _mensagem += FindDbUpdateException(ex);
                _mensagem += FindEntityException(ex);

                if (_mensagem == "")
                    _mensagem += FindInnerException(ex);

                return _mensagem;
            }
            catch
            {
                return ex.Message;
            }

        }
         
        private static string FindInnerException(Exception e)
        {
            string retorno = "";

            if (e.InnerException != null)
            {
                if (e.InnerException.InnerException != null)
                    retorno = e.InnerException.InnerException.Message;
                else
                    retorno = e.InnerException.Message;
            }
            else
                retorno = e.Message;

            return retorno;

        }

        private static string FindFormattedDbEntityValidationException(Exception ex)
        {
            try
            {
                var innerException = ex as FormattedDbEntityValidationException;

                return innerException.Message;
            }
            catch
            {
                return "";
            }
        }
        private static string FindDbEntityValidationException(Exception ex)
        {
              try
              {
                    var innerException = ex as DbEntityValidationException;

                    if (innerException != null)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine();
                        sb.AppendLine();
                        foreach (var eve in innerException.EntityValidationErrors)
                        {
                            sb.AppendLine(string.Format("- A Entidade \"{0}\" com o estado \"{1}\" contém os seguintes erros:",
                                eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                            foreach (var ve in eve.ValidationErrors)
                            {
                                sb.AppendLine(string.Format("-- Propriedade: \"{0}\", Valor: \"{1}\", Erro: \"{2}\"",
                                    ve.PropertyName,
                                    eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                    ve.ErrorMessage));
                            }
                        }
                        sb.AppendLine();

                        return sb.ToString();
                    }

                    return FindInnerException(innerException);
                    
              }
              catch
              {
                  return "";
              }
        }

        private static string FindDbUpdateException(Exception ex)
        {
            try
            {
                var innerException = ex as DbUpdateException;

                return FindInnerException(innerException);
            }
            catch
            {
                return "";
            }
        }

        private static string FindEntityException(Exception ex)
        {
            try
            {
                var innerException = ex as EntityException;

                return FindInnerException(innerException);
            }
            catch
            {
                return "";
            }

        }

        public void AddSubmessage(Message message)
        {
            if (this != message && !subMessages.Contains(message))
            {
                subMessages.Add(message);
            }
        }

        /// <summary>
        /// Retorna Todas as Validações Incluindo as submensagens
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> RetornarValidacoes()
        {
            var validacoes = ValidationErrors;

            if(subMessages != null && subMessages.Count > 0)
            {
                foreach(var me in subMessages)
                {
                    var subValidacoes = me.RetornarValidacoes();

                    if(subValidacoes != null && subValidacoes.Count > 0)
                    {
                        validacoes = validacoes.Union(subValidacoes).ToDictionary(k => k.Key, v => v.Value);
                    }
                }
            }
            return validacoes;
        }

    }
}