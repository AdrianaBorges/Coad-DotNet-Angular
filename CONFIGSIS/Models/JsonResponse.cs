using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CONFIGSIS.Models
{
    /// <summary>
    /// JsonResponse
    /// </summary>
    public class JsonResponse
    {
        public JsonResponse()
        { }
        public JsonResponse(bool _success, string _message)
        {
            this.Success = _success;
            this.Message = _message; 
        }
        public bool Success;
        public string Message;
    }
}