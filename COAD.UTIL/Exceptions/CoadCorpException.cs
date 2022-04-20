using System;
using System.Runtime.Serialization;

namespace COAD.UTIL.Exceptions
{
    public class CoadCorpException : Exception
    {
        public CoadCorpException() { }
        public CoadCorpException(string message) : base(message) { }
        public CoadCorpException(string message, Exception inner) : base(message, inner) { }
        protected CoadCorpException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
    }
}
