using Coad.GenericCrud.Security;
using COAD.CORPORATIVO.Filters.Interface;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.SessionUtils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Filters
{
    /// <summary>
    /// Permite informar um nome de método presente no SessionUtil para validar permissões
    /// </summary>
    public class AutorizarCustomAttribute : AutorizarAttribute
    {
        /// <summary>
        /// Indica um realização da interface com um método customizado.
        /// </summary>
        public Type IAccessControlMethodType { get; set; }
        
        /// <summary>
        /// Indica um nome de método contido em SessionUtil para testar acesso.
        /// </summary>
        public string SessionUtilMethodName { get; set; }

        public AutorizarCustomAttribute()
        {

        }

        public AutorizarCustomAttribute(Type iAccessControlMethodType)
        {
            this.IAccessControlMethodType = iAccessControlMethodType;
        }

        public AutorizarCustomAttribute(string SessionUtilMethodName)
        {
            this.SessionUtilMethodName = SessionUtilMethodName;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var resp = base.AuthorizeCore(httpContext);

            if (resp)
            {
                if (IAccessControlMethodType != null && typeof(IAccessControlMethod).Equals(IAccessControlMethodType))
                {
                    IAccessControlMethod obj = (IAccessControlMethod) Activator.CreateInstance(IAccessControlMethodType);

                    Autenticado autenticado = SessionContext.autenticado;
                    string perfilId = SessionContext.per_id;
                    int? repId = SessionContext.GetIdRepresentante();
                    int? rgId = SessionUtil.GetRegiao();
                    int? uenId = SessionUtil.GetUenId();

                    var retorno = obj.HasAccess(autenticado, perfilId, repId, rgId, uenId);
                    return retorno;
                }

                if (!string.IsNullOrWhiteSpace(SessionUtilMethodName))
                {
                    var method = typeof(SessionUtil).GetMethod(SessionUtilMethodName);
                    var methodReturn = method.Invoke(null, null);
                    return (bool) methodReturn;
                }

                return resp;
            }
            else
            {
                return false;
            }
        }
    }
}
