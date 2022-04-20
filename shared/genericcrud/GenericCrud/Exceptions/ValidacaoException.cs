using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ModelBinding;

namespace Coad.GenericCrud.Exceptions
{
    public class ValidacaoException : Exception
    {
        public Dictionary<string, List<string>> Validations {get; set;}
        public ModelStateDictionary ModelState { get; set; }
        public System.Web.Mvc.ModelStateDictionary ModelState2 { get; set; }
        public string message { get; set; }

        public ValidationResume ValidationResume { get; set; }
        public override string Message
        {
            get
            {
                return message;
            }
        }

        public virtual void AddValidationError(string campo, params string[] msg)
        {
            var lstMsg = msg.ToList();
            _Init();
            Validations.Add(campo, lstMsg);
        }

        public void _Init()
        {
            this.ModelState = new ModelStateDictionary();
            this.Validations = new Dictionary<string, List<string>>();
        }

        public ValidacaoException()
        {
            _Init();
        }

        public ValidacaoException(string message)
        {
            this.message = message;
            _Init();
        }

        public ValidacaoException(string message, ValidationResume validationResume)
            : base(message)
        {
            this.message = message;
            _Init();
            this.ValidationResume = validationResume;
        }

        public ValidacaoException(string message, ModelStateDictionary modelState) : base(message)
        {
            this.message = message;
            this.ModelState = modelState;
            AdicionarValidacaoNaDescricao();
        }

        public ValidacaoException(string message, System.Web.Mvc.ModelStateDictionary modelState) : base(message)
        {
            this.message = message;
            this.ModelState2 = modelState;
            AdicionarValidacaoNaDescricao();
        }


        public ValidacaoException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
            _Init();
        }

        public ValidacaoException(string message, string campo, params string[] mensagens) : base(message)
        {
            this.message = message;
            _Init();
            AddValidationError(campo, mensagens);
        }

        public ValidacaoException(string campo, params string[] mensagens)
            : base("Existe erros de validação")
        {
            _Init();
            AddValidationError(campo, mensagens);
        }

        private void AdicionarValidacaoNaDescricao()
        {
        //    if(this.ModelState != null)
        //    {
        //        StringBuilder sb = new StringBuilder(this.Message);
        //        sb.Append("\n Detalhes: [ ");
        //        foreach(var key in ModelState.Keys)
        //        {
        //            int index = 0;

        //            if (index > 0)
        //                sb.Append(",");

        //            sb.Append(" Campo: ");
        //            sb.Append(key);
        //            sb.Append(" --> Mensagens ( ");

        //            var erros = ModelState[key];
        //            if(erros != null && erros.Errors != null)
        //            {
        //                int subIndex = 0;
        //                foreach(var erro in erros.Errors)
        //                {
        //                    if (subIndex > 0)
        //                        sb.Append(",");

        //                    if (erro.Exception != null)
        //                        sb.Append(erro.Exception.Message);
        //                    else
        //                        sb.Append(erro.ErrorMessage);

        //                    subIndex++;
        //                }
        //            }
        //            sb.Append(" )");
        //            index++;
        //        }
        //        sb.Append(" ]");
        //        this.message = sb.ToString();
        //    }
        }
    }
}