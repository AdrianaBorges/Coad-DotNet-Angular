using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace COAD.SEGURANCA.Util
{
    public static class AuthUtil
    {

        public static bool TryGetRepId(out int? REP_ID)
        {
            if (SessionContext.PossuiRepresentante())
            {
                REP_ID = SessionContext.GetIdRepresentante();
                return true;
            }

            REP_ID = null;

            return false;
        }

        public static FuncionalidadeSistemaDTO RetornarFuncionalidadeSistemaDaRequisicao()
        {
            if(HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains("funcionalidadeSistema"))
                {
                    var funcionalidade = HttpContext.Current.Items["funcionalidadeSistema"];
                    if(funcionalidade == null)
                    {
                        throw new FuncionalidadeSistemaException("A requisição não contém informações da funcionalidade de sistema. Essa ação no sistema depende dessa informação.");
                    }

                    if (funcionalidade is FuncionalidadeSistemaDTO)
                    return funcionalidade as FuncionalidadeSistemaDTO;
                }
            }
            throw new FuncionalidadeSistemaException("A requisição não contém informações da funcionalidade de sistema. Essa ação no sistema depende dessa informação.");

        }

    }
}
