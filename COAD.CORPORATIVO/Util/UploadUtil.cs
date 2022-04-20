using COAD.CORPORATIVO.Exceptions;
using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace COAD.CORPORATIVO.Util
{
    public class UploadUtil
    {
        public static Dictionary<int, object> appData = new Dictionary<int, object>();

        public static void ArmazenarArquivoTemporario(HttpPostedFileBase file)
        {
            if (file != null)
            {
                SessionContext.PutInSession<HttpPostedFileBase>("uploadedFile", file);
            }
        }

        public static HttpPostedFileBase RetornarArquivoDeUpload()
        {
            var file = SessionContext.GetInSession<HttpPostedFileBase>("uploadedFile");
            return file;
        }

        public static void ArmazenarObjetoTemporario(object value)
        {
            if(value != null)
            {
                SessionContext.PutInSession<object>("uploadObject", value);
            }

        }

        public static object RetornarObjetoDeUpload()
        {
            if (SessionContext.ExistsInSession("uploadObject"))
            {
                var obj = SessionContext.GetInSession<object>("uploadObject");
                return obj;
            }

            throw new UploadException("As informações de object não estão disponíveis.");
        }

        public static T RetornarObjetoDeUpload<T>()
        {
            if (SessionContext.ExistsInSession("uploadObject"))
            {
                var obj = SessionContext.GetInSession<T>("uploadObject");
                return obj;
            }

            throw new UploadException("As informações de object não estão disponíveis.");        
        }

        public static void LimparObjetoDeUpload()
        {
            if (SessionContext.ExistsInSession("uploadObject"))
            {
                SessionContext.RemoveFromSession("uploadObject");
            }

        }
    }
}
