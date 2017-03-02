using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Exceptions
{
    /// <summary>
    /// 领域发生的异常。
    /// </summary>
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }
        protected DomainException(SerializationInfo
                    info, StreamingContext context)
            : base(info, context)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
