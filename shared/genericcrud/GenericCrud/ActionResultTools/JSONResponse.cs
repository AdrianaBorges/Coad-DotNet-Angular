using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using GenericCrud.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coad.GenericCrud.ActionResultTools
{
    /**
     * Classe para embrulhar uma resposta padrão em Json
     * Author: Diego Andrade da Silva
     */

    public class JSONResponse
    {
        /*
         * Como esse é um objeto para serealização direto no javascript em forma de json
         * os atributos foram colocados em letra minuscula para entrar em conformidade com 
         * a notação do json
         */
        public Dictionary<string, object> result { set; get; }
        public Message message { set; get; }
        public Dictionary<string, List<string>> validationMessage { set; get; }
        public bool ShowValidationErros { get; set; }
        public Pagina<object> page { set; get; }
        public bool success  {set; get;}

        public JSONResponse()
        {
            Init();
        }

        public JSONResponse(IEnumerable<Object> value)
        {
            Init();
            result.Add("model", value);
        }


        public JSONResponse(string key,IEnumerable<Object> value)
        {
            Init();
            result.Add(key, value);
        }

        public JSONResponse(object value)
        {
            Init();
            result.Add("model", value);
        }
        public JSONResponse(string key, object value)
        {
            Init();
            result.Add(key, value);
        }

        public JSONResponse(Message msg)
        {
            Init();            
        }

        public void Init()
        {
            result = new Dictionary<string, object>();
            validationMessage = new Dictionary<string, List<string>>();
            success = true;
        }

        /// <summary>
        /// Adiciona um objeto ao resultado da requisição Json
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            result.Add(key, value);
        }

        /// <summary>
        /// Adiciona a página a resposta da requisição em Json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pagina"></param>
        public void AddPage<T>(string key, Pagina<T> pagina) where T : class
        {
            var obj = pagina.lista;
            pagina.lista = null;
            result.Add(key, obj);

            Pagina<object> paginaNova = new Pagina<object>(){
            
                itensPorPagina = pagina.itensPorPagina,
                numeroPaginas = pagina.numeroPaginas,
                pagina = pagina.pagina,
                numeroRegistros = pagina.numeroRegistros

            };
            this.page = paginaNova;
        }

        public void Remove(string key)
        {
            result.Remove(key);
        }

        /// <summary>
        /// Adiciona mensagens de validação para determinada propriedade do model
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddValidationMessage(string key, List<string> value)
        {
            validationMessage.Add(key, value);
        }

        //public Dictionary<string, List<string>> AddMessageFromModelState(ModelStateDictionary modelState)
        //{
        //    if (!modelState.IsValid)
        //    {
        //        return modelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
        //    }
        //    return null;
        //}

        //public Dictionary<string, List<string>> AddMessageFromModelState(System.Web.ModelBinding.ModelStateDictionary modelState)
        //{
        //    if (!modelState.IsValid)
        //    {
        //        return modelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
        //    }
        //    return null;
        //}


        /// <summary>
        /// Checa os erros de validação do modelo e seta o dicionario de mensagens de validação
        /// 'validationMessage'
        /// </summary>
        /// <param name="modelState"></param>
        public void SetMessageFromModelState(ModelStateDictionary modelState, bool showDetalsInMessage = true)
        {   
            this.validationMessage = ExceptionFormatter.FormatModelState(modelState, this.validationMessage);
            if (this.validationMessage != null)
            {
              message = Message.Fail(new ValidacaoException("Existem erros de validação. Verifique os detalhes.", modelState));
            }
            
        }

        /// <summary>
        /// Adiciona mensagens de validação extra para o dicionário de validação padrão.
        /// Indicado para validações customizadas, fora do contexto da validação comum ou,
        /// se não é possível validar usando o mecanismo padrão.
        /// </summary>
        /// <param name="ex"></param>
        public void SetMessageFromValidacaoException(ValidacaoException ex, bool showMessageDetals = true, bool showDetailsOnResponse = false)
        {
            if (ex != null)
            {
                this.message = Message.Fail(ex);
            }
        }

    }   

}