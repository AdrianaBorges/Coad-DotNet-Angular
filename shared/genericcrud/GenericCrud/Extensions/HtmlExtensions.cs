using Coad.GenericCrud.ActionResultTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace Coad.GenericCrud.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, string validationMessageSourceName, Expression<Func<TModel, TProperty>> expression){


            var method = expression.Name;
           
            var model = htmlHelper.ViewData.Model;
            //var name = method.Method.GetMethodBody();
            var name = "";
            var result = htmlHelper.ValidationMessageFor<TModel, TProperty>(expression, null, new { app_validation_msg = "", @for = name });

            return result;            
        }


        /// <summary>
        /// Printa as mensagens vindas da ViewData ou ViewData
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString Messages(this HtmlHelper helper)
        {
            return Messages(helper, "message");
        }

        /// <summary>
        /// Printa as mensagens vindas da ViewData ou ViewData
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="messageName">Nome no objeto a ser pego a referencia da mensagem na TempData ou ViewBag</param>
        /// <returns></returns>
        public static MvcHtmlString Messages(this HtmlHelper helper, string messageName)
        {
            var tempData = helper.ViewContext.Controller.TempData;
            var viewData = helper.ViewContext.Controller.ViewData;
            var messageObj = tempData[messageName];

            if (messageObj != null && messageObj is Message)
            {
                return new MvcHtmlString(_BuildMessage(messageObj as Message));
            }

            messageObj = viewData[messageName];

            if (messageObj != null && messageObj is Message)
            {
                return new MvcHtmlString(_BuildMessage(messageObj as Message));
            }

            return null;
        }

        private static string _BuildMessage(Message messageObj)
        {
            Message messageCast = messageObj as Message;
            string message =
                "<div class=\"alert alert-{0} alert-dismissible\" role=\"alert\">" +
                @"<button type=""button"" class=""close"" ng-click=""fechar()""><span aria-hidden=""true"">&times;</span></button>" +
                    "<div>{1}</div>" +
                "</div>";

            string type = (messageCast.type == "fail") ? "danger" : messageCast.type;
            return String.Format(message, type, messageCast.message);

        }

        private static string _BuildMessageException(Message messageObj)
        {
            Message messageCast = messageObj as Message;
            string message = messageCast.message;

            return message;
        }

        public static MvcHtmlString ExceptionMessage(this HtmlHelper helper, string messageName, bool submessages = false)
        {
            var tempData = helper.ViewContext.Controller.TempData;
            var viewData = helper.ViewContext.Controller.ViewData;
            var messageObj = (tempData[messageName] != null) ? tempData[messageName] : viewData[messageName];

            if (messageObj != null && messageObj is Message)
            {
                Message msg = messageObj as Message;

                var messageString = msg.message;
                StringBuilder sb = new StringBuilder();
                
                if(submessages){

                    if (msg.subMessages != null)
                    {
                        foreach (var subMsg in msg.subMessages)
                        {
                            sb.Append("<li>");
                            sb.Append(subMsg.message);
                            sb.Append("</li>");
                        }
                    }
                    return new MvcHtmlString(sb.ToString());
                    
                }
                return new MvcHtmlString(messageString);
            }

            return null;
        }

    }
}
