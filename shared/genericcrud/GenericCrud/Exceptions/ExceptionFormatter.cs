using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Metadatas;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace GenericCrud.Exceptions
{
    public static class ExceptionFormatter
    {
       

        public static Message RecursiveShowExceptionsMessage(Exception e)
        {

            Message msg = Message.Fail("Ocorreu um erro ao processar sua requisição.");
            RecursiveExceptionFormatting(e, msg);
            return msg;
        }

        public static Message RecursiveShowExceptionsMessage(string mensagem, Exception e)
        {

            Message msg = Message.Fail(mensagem);
            RecursiveExceptionFormatting(e, msg);
            return msg;
        }

        //private static void RecursiveShowMessage(Exception ex, StringBuilder sb)
        //{
        //    if (ex == null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        sb.Append("\n\r");
        //        sb.Append("---->");
        //        sb.Append(ex.Message);
        //        RecursiveShowMessage(ex.InnerException, sb);
        //    }
        //}

        private static string ExtractMessage(Exception ex)
        {
            if (ex is DbEntityValidationException)
            {
                var exception = new FormattedDbEntityValidationException(ex as DbEntityValidationException);
                return exception.Message;
            }
            return ex.Message;
        }

        private static void RecursiveExceptionFormatting(Exception ex, Message msg)
        {
            if (ex == null)
            {
                return;
            }
            else
            {
                var exceptionMessage = ExtractMessage(ex);
                var subMessage = Message.Fail(exceptionMessage);
                msg.AddSubmessage(subMessage);
                RecursiveExceptionFormatting(ex.InnerException, msg);
            }
        }

        /// <summary>
        /// Pega uma exceção busca todas as suas sub excessões e formata em uma string
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [MetodoAuxiliar]
        public static string RecursiveFindExceptionsMessage(Exception e, Message message = null)
        {

            StringBuilder sb = new StringBuilder();
            RecursiveShowMessage(e, sb, message);
            return sb.ToString();
        }

        public static Dictionary<string, List<string>> AddMessageFromModelState(System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (modelState != null && !modelState.IsValid)
            {
                return modelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
            }
            return null;
        }

        public static Dictionary<string, List<string>> AddMessageFromModelState(ModelStateDictionary modelState)
        {
            if (modelState != null && !modelState.IsValid)
            {
                return modelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
            }
            return null;
        }

        public static Dictionary<string, List<string>> FormatModelState(ModelStateDictionary modelState, Dictionary<string, List<string>> formatedValidations)
        {
            if (!modelState.IsValid)
            {
                var formated = AddMessageFromModelState(modelState).ToDictionary(x => x.Key, v => v.Value);
                if(formatedValidations != null)
                {   
                    formated = formatedValidations.Concat(formated).ToDictionary(x => x.Key, v => v.Value);
                }
                return formated;
            }

            return new Dictionary<string, List<string>>(formatedValidations);
        }

        public static Dictionary<string, List<string>> FormatModelState(System.Web.Mvc.ModelStateDictionary modelState, Dictionary<string, List<string>> formatedValidations)
        {
            if (!modelState.IsValid)
            {
                var formated = AddMessageFromModelState(modelState).ToDictionary(x => x.Key, v => v.Value);
                if (formatedValidations != null)
                {
                    formated = formatedValidations.Concat(formated).ToDictionary(x => x.Key, v => v.Value);
                }
                return formated;
            }
            return new Dictionary<string, List<string>>(formatedValidations);
        }

        public static void RecursiveShowMessage(Exception ex, StringBuilder sb, Message message = null, Dictionary<string, List<string>> ValidationErrorsCopy = null)
        {
            

            if (ex == null)
            {
                return;
            }
            else
            {
                sb.Append("\n\r ");
               // sb.Append("---->");
                sb.Append(ex.Message);

                if(message != null)
                {
                    Message msg = new Message("fail", ex.Message);
                    message.AddSubmessage(msg);

                    if (ex is ValidacaoException)
                    {
                        if((ex as ValidacaoException).Validations != null)
                        {
                            message.ValidationErrors = (ex as ValidacaoException).Validations;
                            msg.ValidationErrors = (ex as ValidacaoException).Validations;
                        }

                        if((ex as ValidacaoException).ModelState != null)
                        {
                            msg.ValidationErrors = FormatModelState((ex as ValidacaoException).ModelState, message.ValidationErrors);
                        }
                        else if((ex as ValidacaoException).ModelState2 != null)
                        {
                            msg.ValidationErrors = FormatModelState((ex as ValidacaoException).ModelState2, message.ValidationErrors);
                        }
                    }
                }
                RecursiveShowMessage(ex.InnerException, sb, message);
            }
        }

        public static string RecursiveShowExceptionsStacktrace(Exception e)
        {

            StringBuilder sb = new StringBuilder();
            RecursiveShowStacktrace(e, sb);
            return sb.ToString();
        }

        private static void RecursiveShowStacktrace(Exception ex, StringBuilder sb)
        {
            if (ex == null)
            {
                return;
            }
            else
            {
                sb.Append("\n\r");
                sb.Append(@"---->
                            
                            ");
                sb.Append(ex.StackTrace);
                RecursiveShowMessage(ex.InnerException, sb);
            }
        }
    }
}
