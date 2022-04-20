
using COAD.SEGURANCA.Exceptions;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCAO.Util
{
    public static class BatchUtil
    {

        public static Dictionary<string, object> appData = new Dictionary<string, object>();

        public static void ArmazenarObjeto(string key, object value)
        {
            if (value != null)
            {
                if (appData.Keys.Contains(key))
                {
                    appData.Remove(key);
                }
                appData.Add(key, value);
            }

        }

        public static object RetornarObjeto(string key)
        {
            if (appData.Keys.Contains(key))
            {
                var obj = appData[key];
                return obj;
            }

            throw new UploadException("As informações de upload não estão disponíveis.");
        }

        public static T RetornarObjeto<T>(string key)
        {
            if (appData.Keys.Contains(key))
            {
                var obj = appData[key];

                if (obj is T)
                    return (T)obj;
                return default(T);
            }

            throw new UploadException("As informações de upload não estão disponíveis.");
        }

        public static void LimparObjeto(string key)
        {
            if (appData.Keys.Contains(key))
            {
                appData.Remove(key);
            }

        }

        public static void AdicionarBatchStatus(BatchStatus batchStatus, string sessionId, string key = "batchStatus")
        {
            if (batchStatus != null)
            {
                ArmazenarObjeto(sessionId + key, batchStatus);
            }
        }


        public static BatchStatus RetornarBatchStatus(string sessionId, string key = "batchStatus")
        {
            BatchStatus status = RetornarObjeto<BatchStatus>(sessionId + key);
            return status;           
        }

        public static void LimparBatchStatus(string sessionId, string key = "batchStatus")
        {
            LimparObjeto(sessionId + key);
        }
    }
}
