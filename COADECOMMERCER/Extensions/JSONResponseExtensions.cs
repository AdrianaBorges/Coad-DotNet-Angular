using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace COADRESTSERVICE.Extensions
{
    public static class JSONResponseExtensions
    {
        public static void SetMessageFromModelState(this JSONResponse jsonResponse, ModelStateDictionary modelState, bool showDetalsInMessage = true)
        {
            if (modelState != null && !modelState.IsValid)
            {
                jsonResponse.validationMessage = modelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());

                if (jsonResponse.validationMessage != null)
                {
                    jsonResponse.message = Message.Fail(new ValidacaoException("Existem erros de validação. Verifique os detalhes."));
                }
            }
        }
    }
}
