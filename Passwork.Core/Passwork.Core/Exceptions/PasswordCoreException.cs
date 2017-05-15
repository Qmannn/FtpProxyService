using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passwork.Core.Exceptions
{
    public class PasswordCoreException : Exception
    {
        public enum ErrorTypeEnum
        {
            BadPassword
        }

        public string ErrorCode { get; set; }

        public PasswordCoreException(string message, string code = null) : base(message)
        {
            ErrorCode = code;
        }
    }
}
