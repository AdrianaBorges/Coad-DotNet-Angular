using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Config.Email
{
    public static class EmailActionContainer
    {
        public delegate byte[] DelEmailRequest(string argumentoAcao);
        public delegate void SendingSuccessCallback(EmailContext emailContext);
        public delegate void SendingFailCallback(EmailContext emailContext);


        public static IDictionary<string, DelEmailRequest> Funcs = new Dictionary<string, DelEmailRequest>();
        public static IDictionary<string, SendingSuccessCallback> SendingCallback = new Dictionary<string, SendingSuccessCallback>();
        public static IDictionary<string, SendingFailCallback> FailCallback = new Dictionary<string, SendingFailCallback>();


        public static void AddActions(string key, DelEmailRequest function)
        {
            if (!string.IsNullOrWhiteSpace(key) && !Funcs.Keys.Contains(key))
            {
                Funcs.Add(key, function);
            }
        }

        public static byte[] ExecutarAcaoEmail(string key, string argumento)
        {
            if (Funcs.Keys.Contains(key))
            {
                var method = Funcs[key];

                if (method != null)
                {
                    return method(argumento);
                }
            }

            return null;
        }

        public static void AddSuccessCallback(string key, SendingSuccessCallback function)
        {
            if (!string.IsNullOrWhiteSpace(key) && !SendingCallback.Keys.Contains(key))
            {
                SendingCallback.Add(key, function);
            }
        }

        public static void ExecutarSuccessCallback(string key, FilaEmailDTO filaEmail)
        {
            if (SendingCallback.Keys.Contains(key))
            {
                var method = SendingCallback[key];

                if (method != null)
                {
                    if(filaEmail != null)
                    {
                        var emailContext = new EmailContext()
                        {
                            CodFila = filaEmail.FLE_ID,
                            Assunto = filaEmail.FLE_ASSUNTO,
                            ContextId = filaEmail.FLE_CALLBACK_CONTEXT_KEY,
                            ContextIdStr = filaEmail.FLE_CALLBACK_CONTEXT_KEY_STR,
                            Email = filaEmail.FLE_EMAIL,
                            RepId = filaEmail.REP_ID,
                            Usuario = filaEmail.USU_LOGIN
                        };
                        method(emailContext);
                    }
                }
            }
        }

        public static void AddFailCallback(string key, SendingFailCallback function)
        {
            if (!string.IsNullOrWhiteSpace(key) && !FailCallback.Keys.Contains(key))
            {
                FailCallback.Add(key, function);
            }
        }

        public static void ExecutarFailCallback(string key, FilaEmailDTO filaEmail)
        {
            if (FailCallback.Keys.Contains(key))
            {
                var method = FailCallback[key];

                if (method != null)
                {
                    if (filaEmail != null)
                    {
                        var emailContext = new EmailContext()
                        {
                            CodFila = filaEmail.FLE_ID,
                            Assunto = filaEmail.FLE_ASSUNTO,
                            ContextId = filaEmail.FLE_CALLBACK_CONTEXT_KEY,
                            ContextIdStr = filaEmail.FLE_CALLBACK_CONTEXT_KEY_STR,
                            Email = filaEmail.FLE_EMAIL,
                            RepId = filaEmail.REP_ID,
                            Usuario = filaEmail.USU_LOGIN
                        };
                        method(emailContext);
                    }
                }
            }
        }
    }
}
