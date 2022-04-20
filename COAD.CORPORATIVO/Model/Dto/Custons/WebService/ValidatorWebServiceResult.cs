using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService
{
    [DataContract]
    public class ValidatorWebServiceResult : WebServiceResult
    {
        [DataMember]
        public Dictionary<string, List<string>> ValidationMessage { set; get; }
        
        public ValidatorWebServiceResult()
        {
            ValidationMessage = new Dictionary<string, List<string>>();
        }

        public Dictionary<string, List<string>> AddMessageFromModelState(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
            }
            return null;
        }

        /// <summary>
        /// Checa os erros de validação do modelo e seta o dicionario de mensagens de validação
        /// 'validationMessage'
        /// </summary>
        /// <param name="modelState"></param>
        public void SetMessageFromModelState(ModelStateDictionary modelState, bool showDetalsInMessage = true)
        {
            this.ValidationMessage = this.ValidationMessage.Concat(AddMessageFromModelState(modelState)).ToDictionary(x => x.Key, v => v.Value);
            if (this.ValidationMessage != null)
            {
                string _erro = "";

                if (showDetalsInMessage)
                {
                    foreach (var item in ValidationMessage)
                    {
                        _erro += " \\ " + item.Key + " " + item.Value[0];
                    }

                    this.Message = Coad.GenericCrud.ActionResultTools.Message.Fail("Existem erros de validação. Verifique os detalhes.(" + _erro + " )");
                }
                else
                {
                    this.Message = Coad.GenericCrud.ActionResultTools.Message.Fail("Existem erros de validação. Verifique os detalhes.");
                }
            }
        }

    }
}
