using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web.UI;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.ActionResultTools;
using System.Transactions;
using Coad.GenericCrud.Models;
using GenericCrud.Exceptions;

namespace COAD.SEGURANCA.Repositorios.Base
{
     public static class SysException
     {
        public static string Show(Exception ex)
        {
            string _retorno = Message.FindException(ex);

            return _retorno;

        }
        public static string ShowIdException(Exception ex)
        {
            string retorno = "";
           
            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                    retorno = ex.InnerException.InnerException.HResult.ToString();
                else
                    retorno = ex.InnerException.HResult.ToString();
            }
            else
                retorno = ex.HResult.ToString();

            return retorno;

        }
        public static void ShowMessage(string _message, string _id_erro)
        {
           
        }
        public static void RegistrarLog(string _message, string _id_erro, Autenticado a, Boolean _cliente= false, Exception exception = null)
        {

            COADSYSEntities db = DbContextFactory.criarDbContext("coadsys") as COADSYSEntities;///new COADSYSEntities();

            LOG_OCORRENCIA log = new LOG_OCORRENCIA();
            string stackStrace = null;
            string fullMsg = null;

            if(exception != null)
            {
                fullMsg = ExceptionFormatter.RecursiveFindExceptionsMessage(exception);
                stackStrace = ExceptionFormatter.RecursiveShowExceptionsStacktrace(exception);
            }

            try
            {
                log.LOG_DATA = DateTime.Now;
                log.LOG_ID_ERRO = _id_erro;
                log.LOG_MESSAGE = _message;
                log.LOG_STACK_TRACE = stackStrace;
                log.LOG_FULL_MSG = fullMsg;

                if (a != null)
                {
                    if (!_cliente)
                        log.USU_LOGIN = a.USU_LOGIN;
                    else
                        log.USU_LOGIN = "COADSYS";

                    log.LOG_IP_ACESSO = a.IP_ACESSO;
                    log.ITM_PATH = a.PATH;
                }

                db.LOG_OCORRENCIA.Add(log);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }


    }
}
