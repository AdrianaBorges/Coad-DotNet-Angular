using Coad.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace GenericCrud.Validations
{
    public class ValidatorProxy
    {
        //public static ValidationResume Validate<T>(T entity)
        //{            
        //    var validationsResult = new LinkedList<ValidationResult>();
        //    var vc = new ValidationContext(entity);

        //    var isValid = Validator.TryValidateObject(entity, vc, validationsResult, true);

        //    return new ValidationResume(isValid, validationsResult, vc);

        //}

        public static void PreencherModelState(ModelStateDictionary modelState, ICollection<ValidationResult> validationResult, string path)
        {
            if (validationResult != null)
            {
                string key = null;
                if (!string.IsNullOrWhiteSpace(path))
                    key = path + ".";

                foreach (var valResult in validationResult)
                {
                    if(valResult.MemberNames.Count() > 0)
                    {
                        foreach (var name in valResult.MemberNames)
                        {
                            modelState.AddModelError(key + name, valResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        modelState.AddModelError(key, valResult.ErrorMessage);                        
                    }

                    
                }
            }
        }

        //public static ValidationResume Validate<T>(ICollection<T> lstEntity)
        //{
        //    var validationsResult = new LinkedList<ValidationResult>();
        //    var lstValidationContext = new HashSet<ValidationContext>();
        //    bool isAllValid = true;

        //    foreach (var entity in lstEntity)
        //    {
        //        var vc = new ValidationContext(entity);
        //        var isValid = Validator.TryValidateObject(entity, vc, validationsResult, true);
        //        lstValidationContext.Add(vc);
        //        isAllValid = (isAllValid && isValid);
        //    }

        //    return new ValidationResume(isAllValid, validationsResult, lstValidationContext);
        //}
        
        
        /// <summary>
        /// Varre o objeto de modo recursivo e executa as validações em cada nó
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="modelState"></param>
        /// <param name="path"></param>
        private static void RecursiveFindObject(object obj, ModelStateDictionary modelState, string pathOriginal = null, int nivel = -1)
        {
            
            string pathPrincipal = null;
            string nomeEntidadeValidade = null;
            try
            {
                
             nivel++;
                

                if (obj != null && !(obj is string) && Convert.GetTypeCode(obj) == TypeCode.Object)
                {
                    nomeEntidadeValidade = obj.GetType().FullName;
                    ValidatorProxy.ValidateMember<object>(obj, modelState, pathOriginal);

                    IEnumerable<MemberInfo> lstMember = ReflectionProvider.GetMembers(obj, true, true);
                    foreach (var mem in lstMember)
                    {                       
                        var val = ReflectionProvider.GetMemberValue<object>(obj, mem);

                        if (Convert.GetTypeCode(val) == TypeCode.Object && !(val is string))
                        {
                            int index = 0;
                            if (val is IEnumerable)
                            {
                                foreach (var subValue in (IEnumerable)val)
                                {
                                    string path = pathOriginal;
                                    path += ((nivel > 0) ? "." + mem.Name : mem.Name) + "[" + index + "]";
                                    pathPrincipal = path;
                                    RecursiveFindObject(subValue, modelState, path, nivel);
                                    index++;
                                }
                            }
                            else
                            {
                                string path = pathOriginal;
                                path += (nivel > 0) ? "." + mem.Name : mem.Name;
                                pathPrincipal = path;
                                RecursiveFindObject(val, modelState, path, nivel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Ocorreu um erro ao tentar validar o path {0} da entidade {1}", pathOriginal,nomeEntidadeValidade), e);
            }
        }

      /// <summary>
      /// Executa a validação do Validator presente no C#
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="entity"></param>
      /// <param name="modelState"></param>
        private static void ValidateMember<T>(T entity, ModelStateDictionary modelState, string path)
        {
            if (path == null)
                path = "";

            if (entity != null && modelState != null)
            {
                var validationsResult = new LinkedList<ValidationResult>();
                var vc = new ValidationContext(entity);

                try
                {
                    var isValid = Validator.TryValidateObject(entity, vc, validationsResult, true);
                    ValidatorProxy.PreencherModelState(modelState, validationsResult, path);
                }
                catch(Exception e)
                {
                    modelState.AddModelError(path, e);
                }
            }
        }

        /// <summary>
        /// Valida de modo recursivo uma entidade e todas as suas propriedades.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ModelStateDictionary RecursiveValidate<T>(T entity)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();
            ValidatorProxy.RecursiveFindObject(entity, modelState);

            return modelState;
        }

        public static ModelStateDictionary RecursiveValidate<T>(IEnumerable<T> lstEntity)
        {
            ModelStateDictionary modelState = new ModelStateDictionary();

            foreach(var obj in lstEntity)
            {
                ValidatorProxy.RecursiveFindObject(obj, modelState);
            }
            return modelState;
        }


    }
}
