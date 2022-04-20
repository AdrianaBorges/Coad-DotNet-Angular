using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coad.GenericCrud.Exceptions
{
    /// <summary>
    /// Permite listar na exception Items que causaram algum conflito em uma validação para ser exibido na View
    /// </summary>
    public class ItemReferenciaWarningException : WarningException
    {
        public Dictionary<string, object> result { set; get; }

        public ItemReferenciaWarningException()
        {

        }

        public ItemReferenciaWarningException(string message) : base(message)
        {
            
        }

        public ItemReferenciaWarningException(string message, Dictionary<string, object> result) : base(message)
        {
            this.result = result;
        }
        public ItemReferenciaWarningException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}